using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport
{
	public static class BindingService
	{
		private const bool ignoreCase = false;

		public static IUnresolvedMember GetCompatibleMemberInClass(ICompilation ctx, ITypeDefinition cls, CodeTypeMember member)
		{
			return null;
		}

		private static DomRegion GetValidRegion(IMember member)
		{
			if (member.BodyRegion.IsEmpty || (FilePath)member.DeclaringTypeDefinition.Region.FileName == FilePath.Null)
			{
				return member.DeclaringTypeDefinition.Region;
			}
			return member.BodyRegion;
		}

		private static DomRegion GetValidRegion(IUnresolvedMember member)
		{
			if (member.BodyRegion.IsEmpty || (FilePath)member.DeclaringTypeDefinition.Region.FileName == FilePath.Null)
			{
				return member.DeclaringTypeDefinition.Region;
			}
			return member.BodyRegion;
		}

		private static IType EnsureClassExists(ICompilation ctx, string className, DomRegion location)
		{
			int num = className.LastIndexOf(".");
			string namespaceName;
			string name;
			if (num < 0)
			{
				namespaceName = "";
				name = className;
			}
			else
			{
				namespaceName = className.Substring(0, num);
				name = className.Substring(num + 1);
			}
			ITypeDefinition typeDefinition = ctx.MainAssembly.GetTypeDefinition(namespaceName, name);
			if (typeDefinition == null)
			{
				throw new TypeNotFoundException(className, location, null);
			}
			return typeDefinition;
		}

		private static bool IsTypeCompatible(ICompilation ctx, string existingType, string checkType)
		{
			if (existingType == checkType)
			{
				return true;
			}
			IType type = EnsureClassExists(ctx, checkType, DomRegion.Empty);
			foreach (ITypeDefinition allBaseTypeDefinition in type.GetAllBaseTypeDefinitions())
			{
				if (IsTypeCompatible(ctx, existingType, allBaseTypeDefinition.FullName))
				{
					return true;
				}
			}
			return false;
		}

		public static INamedElement AddMemberToClass(Project project, ITypeDefinition cls, IUnresolvedTypeDefinition specificPartToAffect, CodeTypeMember member, bool throwIfExists)
		{
			bool flag = false;
			foreach (IUnresolvedTypeDefinition part in cls.Parts)
			{
				if (part == specificPartToAffect)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				throw new ArgumentException("Class specificPartToAffect is not a part of class cls");
			}
			ICompilation compilation = TypeSystemService.GetCompilation(project);
			IUnresolvedMember compatibleMemberInClass = GetCompatibleMemberInClass(compilation, cls, member);
			if (compatibleMemberInClass == null)
			{
				return CodeGenerationService.AddCodeDomMember(project, specificPartToAffect, member);
			}
			if (throwIfExists)
			{
				throw new MemberExistsException(cls.Name, member, MemberType.Method, compatibleMemberInClass.BodyRegion, cls.Region.FileName);
			}
			return compatibleMemberInClass;
		}

		public static IEnumerable<IMethod> GetCompatibleMethodsInClass(IType cls, IEvent eve)
		{
			IMethod methodSignature = GetMethodSignature(eve);
			if (methodSignature == null)
			{
				return new IMethod[0];
			}
			return GetCompatibleMethodsInClass(cls, methodSignature);
		}

		public static IEnumerable<IMethod> GetCompatibleMethodsInClass(IType cls, IMethod matchMeth)
		{
			IType[] pars = new IType[matchMeth.Parameters.Count];
			List<IType>[] baseTypes = new List<IType>[matchMeth.Parameters.Count];
			for (int i = 0; i < matchMeth.Parameters.Count; i++)
			{
				pars[i] = matchMeth.Parameters[i].Type;
				baseTypes[i] = new List<IType>(pars[i].GetAllBaseTypes());
			}
			IType matchMethType = matchMeth.ReturnType;
			foreach (IMethod method in cls.GetMethods())
			{
				if (method.IsPrivate || method.Parameters.Count != pars.Length || !method.ReturnType.Equals(matchMethType) || method.IsInternal)
				{
					continue;
				}
				bool allCompatible = true;
				for (int j = 0; j < pars.Length; j++)
				{
					IType pt = method.Parameters[j].Type;
					if (!pars[j].Equals(pt) && !baseTypes[j].Any((IType t) => t.Equals(pt)))
					{
						allCompatible = false;
						break;
					}
				}
				if (allCompatible)
				{
					yield return method;
				}
			}
		}

		public static bool IdentifierExistsInClass(IType cls, string identifier)
		{
			return cls.GetMembers().Any((IMember m) => m.Name == identifier);
		}

		public static string GenerateIdentifierUniqueInClass(IType cls, string trialIdentifier)
		{
			string text = trialIdentifier;
			for (int i = 1; i <= int.MaxValue; i++)
			{
				if (!IdentifierExistsInClass(cls, text))
				{
					return text;
				}
				text = trialIdentifier + i;
			}
			throw new Exception("Tried identifiers up to " + text + " and all already existed");
		}

		private static DomRegion GetRegion(INamedElement el)
		{
			if (el is IEntity)
			{
				return ((IEntity)el).BodyRegion;
			}
			return ((IUnresolvedEntity)el).BodyRegion;
		}

		public static void CreateAndShowMember(Project project, ITypeDefinition cls, IUnresolvedTypeDefinition specificPartToAffect, CodeTypeMember member)
		{
			INamedElement el = AddMemberToClass(project, cls, specificPartToAffect, member, throwIfExists: false);
			int beginLine = specificPartToAffect.BodyRegion.BeginLine;
			DomRegion region = GetRegion(el);
			if (!region.IsEmpty && region.BeginLine >= beginLine && region.BeginLine <= specificPartToAffect.BodyRegion.EndLine)
			{
				beginLine = region.BeginLine;
			}
			IdeApp.Workbench.OpenDocument(specificPartToAffect.Region.FileName, beginLine, 1);
		}

		public static CodeTypeMember ReflectionToCodeDomMember(MemberInfo memberInfo)
		{
			if (memberInfo is MethodInfo)
			{
				return ReflectionToCodeDomMethod((MethodInfo)memberInfo);
			}
			throw new NotImplementedException();
		}

		public static CodeMemberMethod ReflectionToCodeDomMethod(MethodInfo mi)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = mi.Name;
			codeMemberMethod.ReturnType = new CodeTypeReference(mi.ReturnType.FullName);
			codeMemberMethod.Attributes = MemberAttributes.Private;
			switch (mi.Attributes)
			{
			case MethodAttributes.Assembly:
				codeMemberMethod.Attributes |= MemberAttributes.Assembly;
				break;
			case MethodAttributes.FamANDAssem:
				codeMemberMethod.Attributes |= MemberAttributes.FamilyAndAssembly;
				break;
			case MethodAttributes.Family:
				codeMemberMethod.Attributes |= MemberAttributes.Family;
				break;
			case MethodAttributes.FamORAssem:
				codeMemberMethod.Attributes |= MemberAttributes.FamilyAndAssembly;
				break;
			case MethodAttributes.Public:
				codeMemberMethod.Attributes |= MemberAttributes.Public;
				break;
			case MethodAttributes.Static:
				codeMemberMethod.Attributes |= MemberAttributes.Static;
				break;
			}
			ParameterInfo[] parameters = mi.GetParameters();
			ParameterInfo[] array = parameters;
			foreach (ParameterInfo parameterInfo in array)
			{
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(parameterInfo.ParameterType.FullName, parameterInfo.Name);
				if (parameterInfo.IsIn)
				{
					codeParameterDeclarationExpression.Direction = FieldDirection.In;
				}
				else if (parameterInfo.IsOut)
				{
					codeParameterDeclarationExpression.Direction = FieldDirection.Out;
				}
				codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			}
			return codeMemberMethod;
		}

		public static IMethod GetMethodSignature(IEvent ev)
		{
			if (ev.ReturnType == null)
			{
				return null;
			}
			IType returnType = ev.ReturnType;
			if (returnType.Kind == TypeKind.Unknown)
			{
				return null;
			}
			foreach (IMethod method in returnType.GetMethods())
			{
				if (method.Name == "Invoke")
				{
					return method;
				}
			}
			return null;
		}

		public static IUnresolvedMethod CodeDomToMDDomMethod(CodeMemberMethod method)
		{
			DefaultUnresolvedMethod defaultUnresolvedMethod = new DefaultUnresolvedMethod(null, method.Name);
			defaultUnresolvedMethod.ReturnType = new DefaultUnresolvedTypeDefinition(method.ReturnType.BaseType);
			CodeDomModifiersToMDDom(defaultUnresolvedMethod, method.Attributes);
			foreach (CodeParameterDeclarationExpression parameter in method.Parameters)
			{
				DefaultUnresolvedTypeDefinition type = new DefaultUnresolvedTypeDefinition(parameter.Type.BaseType);
				DefaultUnresolvedParameter defaultUnresolvedParameter = new DefaultUnresolvedParameter(type, parameter.Name);
				if (parameter.Direction == FieldDirection.Ref)
				{
					defaultUnresolvedParameter.IsRef = true;
				}
				else if (parameter.Direction == FieldDirection.Out)
				{
					defaultUnresolvedParameter.IsOut = true;
				}
				defaultUnresolvedMethod.Parameters.Add(defaultUnresolvedParameter);
			}
			return defaultUnresolvedMethod;
		}

		public static CodeMemberMethod MDDomToCodeDomMethod(IEvent eve)
		{
			IMethod methodSignature = GetMethodSignature(eve);
			if (methodSignature == null)
			{
				return null;
			}
			return MDDomToCodeDomMethod(methodSignature);
		}

		private static void CodeDomModifiersToMDDom(DefaultUnresolvedMethod method, MemberAttributes modifiers)
		{
			if ((modifiers & MemberAttributes.FamilyOrAssembly) != 0)
			{
				method.Accessibility = Accessibility.ProtectedOrInternal;
			}
			else if ((modifiers & MemberAttributes.FamilyOrAssembly) != 0)
			{
				method.Accessibility = Accessibility.ProtectedAndInternal;
			}
			else if ((modifiers & MemberAttributes.Family) != 0)
			{
				method.Accessibility = Accessibility.Protected;
			}
			else if ((modifiers & MemberAttributes.Assembly) != 0)
			{
				method.Accessibility = Accessibility.Internal;
			}
			else if ((modifiers & MemberAttributes.Public) != 0)
			{
				method.Accessibility = Accessibility.Public;
			}
			else if ((modifiers & MemberAttributes.Private) != 0)
			{
				method.Accessibility = Accessibility.Private;
			}
			if ((modifiers & MemberAttributes.Abstract) != 0)
			{
				method.IsAbstract = true;
			}
			else if ((modifiers & MemberAttributes.Final) != 0)
			{
				method.IsSealed = true;
			}
			else if ((modifiers & MemberAttributes.Static) != 0)
			{
				method.IsStatic = true;
			}
			else if ((modifiers & MemberAttributes.Override) != 0)
			{
				method.IsOverride = true;
			}
		}

		private static MemberAttributes ApplyMDDomModifiersToCodeDom(IEntity entity, MemberAttributes initialState)
		{
			switch (entity.Accessibility)
			{
			case Accessibility.ProtectedOrInternal:
				initialState = (initialState & (MemberAttributes)(-61441)) | MemberAttributes.FamilyOrAssembly;
				break;
			case Accessibility.ProtectedAndInternal:
				initialState = (initialState & (MemberAttributes)(-61441)) | MemberAttributes.FamilyAndAssembly;
				break;
			case Accessibility.Protected:
				initialState = (initialState & (MemberAttributes)(-61441)) | MemberAttributes.Family;
				break;
			case Accessibility.Internal:
				initialState = (initialState & (MemberAttributes)(-61441)) | MemberAttributes.Assembly;
				break;
			case Accessibility.Public:
				initialState = (initialState & (MemberAttributes)(-61441)) | MemberAttributes.Public;
				break;
			case Accessibility.Private:
				initialState = (initialState & (MemberAttributes)(-61441)) | MemberAttributes.Private;
				break;
			}
			if (entity.IsAbstract)
			{
				initialState = (initialState & (MemberAttributes)(-16)) | MemberAttributes.Abstract;
			}
			else if (entity.IsSealed)
			{
				initialState = (initialState & (MemberAttributes)(-16)) | MemberAttributes.Final;
			}
			else if (entity.IsStatic)
			{
				initialState = (initialState & (MemberAttributes)(-16)) | MemberAttributes.Static;
			}
			else if (entity.IsShadowing)
			{
				initialState = (initialState & (MemberAttributes)(-16)) | MemberAttributes.Override;
			}
			if (entity is IField)
			{
				IField field = (IField)entity;
				if (field.IsReadOnly && field.IsStatic)
				{
					initialState = (initialState & (MemberAttributes)(-16)) | MemberAttributes.Const;
				}
			}
			return initialState;
		}

		public static CodeMemberMethod MDDomToCodeDomMethod(IMethod mi)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = mi.Name;
			string text = mi.ReturnType.ReflectionName ?? "System.Void";
			codeMemberMethod.ReturnType = new CodeTypeReference(text);
			codeMemberMethod.Attributes = ApplyMDDomModifiersToCodeDom(mi, codeMemberMethod.Attributes);
			foreach (IParameter parameter in mi.Parameters)
			{
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(text, parameter.Name);
				if (parameter.IsRef)
				{
					codeParameterDeclarationExpression.Direction = FieldDirection.Ref;
				}
				else if (parameter.IsOut)
				{
					codeParameterDeclarationExpression.Direction = FieldDirection.Out;
				}
				else
				{
					codeParameterDeclarationExpression.Direction = FieldDirection.In;
				}
				codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			}
			return codeMemberMethod;
		}
	}
}

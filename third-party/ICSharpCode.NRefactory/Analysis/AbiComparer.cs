using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Analysis
{
	/// <summary>
	/// The Abi comparer checks the public API of two compilation and determines the compatibility state.
	/// </summary>
	public class AbiComparer
	{
		public bool StopOnIncompatibility { get; set; }

		private void CheckContstraints(IType otype, ITypeParameter p1, ITypeParameter p2, ref AbiCompatibility compatibility)
		{
			if (p1.DirectBaseTypes.Count<IType>() != p2.DirectBaseTypes.Count<IType>() || p1.HasReferenceTypeConstraint != p2.HasReferenceTypeConstraint || p1.HasValueTypeConstraint != p2.HasValueTypeConstraint || p1.HasDefaultConstructorConstraint != p2.HasDefaultConstructorConstraint)
			{
				this.OnIncompatibilityFound(new AbiEventArgs(string.Format(this.TranslateString("Type parameter constraints of type {0} have changed."), otype.FullName)));
				compatibility = AbiCompatibility.Incompatible;
			}
		}

		private void CheckContstraints(IMethod omethod, ITypeParameter p1, ITypeParameter p2, ref AbiCompatibility compatibility)
		{
			if (p1.DirectBaseTypes.Count<IType>() != p2.DirectBaseTypes.Count<IType>() || p1.HasReferenceTypeConstraint != p2.HasReferenceTypeConstraint || p1.HasValueTypeConstraint != p2.HasValueTypeConstraint || p1.HasDefaultConstructorConstraint != p2.HasDefaultConstructorConstraint)
			{
				this.OnIncompatibilityFound(new AbiEventArgs(string.Format(this.TranslateString("Type parameter constraints of method {0} have changed."), omethod.FullName)));
				compatibility = AbiCompatibility.Incompatible;
			}
		}

		private void CheckTypes(ITypeDefinition oType, ITypeDefinition nType, ref AbiCompatibility compatibility)
		{
			int num = 0;
			Predicate<IUnresolvedMember> filter = null;
			if (oType.Kind == TypeKind.Class || oType.Kind == TypeKind.Struct)
			{
				filter = ((IUnresolvedMember m) => (m.IsPublic || m.IsProtected) && !m.IsOverride && !m.IsSynthetic);
			}
			for (int i = 0; i < oType.TypeParameterCount; i++)
			{
				this.CheckContstraints(oType, oType.TypeParameters[i], nType.TypeParameters[i], ref compatibility);
				if (compatibility == AbiCompatibility.Incompatible && this.StopOnIncompatibility)
				{
					return;
				}
			}
			using (IEnumerator<IMember> enumerator = oType.GetMembers(filter, GetMemberOptions.IgnoreInheritedMembers).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IMember member = enumerator.Current;
					IEnumerable<IMember> members = nType.GetMembers((IUnresolvedMember m) => member.UnresolvedMember.Name == m.Name && m.IsPublic == member.IsPublic && m.IsProtected == member.IsProtected, GetMemberOptions.None);
					IMember member2 = members.FirstOrDefault((IMember m) => SignatureComparer.Ordinal.Equals(member, m));
					if (member2 == null)
					{
						compatibility = AbiCompatibility.Incompatible;
						if (this.StopOnIncompatibility)
						{
							return;
						}
					}
					else
					{
						IMethod method = member as IMethod;
						if (method != null)
						{
							for (int j = 0; j < method.TypeParameters.Count; j++)
							{
								this.CheckContstraints(method, method.TypeParameters[j], ((IMethod)member2).TypeParameters[j], ref compatibility);
								if (compatibility == AbiCompatibility.Incompatible && this.StopOnIncompatibility)
								{
									return;
								}
							}
						}
						num++;
					}
				}
			}
			if (compatibility != AbiCompatibility.Bigger || oType.Kind == TypeKind.Interface)
			{
				if (num != nType.GetMembers(filter, GetMemberOptions.IgnoreInheritedMembers).Count<IMember>())
				{
					if (oType.Kind == TypeKind.Interface)
					{
						this.OnIncompatibilityFound(new AbiEventArgs(string.Format(this.TranslateString("Interafce {0} has changed."), oType.FullName)));
						compatibility = AbiCompatibility.Incompatible;
						return;
					}
					if (compatibility == AbiCompatibility.Equal)
					{
						compatibility = AbiCompatibility.Bigger;
					}
				}
				return;
			}
		}

		private void CheckNamespace(INamespace oNs, INamespace nNs, ref AbiCompatibility compatibility)
		{
			foreach (ITypeDefinition typeDefinition in oNs.Types)
			{
				if (typeDefinition.IsPublic || typeDefinition.IsProtected)
				{
					ITypeDefinition typeDefinition2 = nNs.GetTypeDefinition(typeDefinition.Name, typeDefinition.TypeParameterCount);
					if (typeDefinition2 == null)
					{
						this.OnIncompatibilityFound(new AbiEventArgs(string.Format(this.TranslateString("Type definition {0} is missing."), typeDefinition.FullName)));
						compatibility = AbiCompatibility.Incompatible;
						if (this.StopOnIncompatibility)
						{
							return;
						}
					}
					else
					{
						this.CheckTypes(typeDefinition, typeDefinition2, ref compatibility);
						if (compatibility == AbiCompatibility.Incompatible && this.StopOnIncompatibility)
						{
							return;
						}
					}
				}
			}
			if (compatibility != AbiCompatibility.Bigger)
			{
				foreach (ITypeDefinition typeDefinition3 in nNs.Types)
				{
					if ((typeDefinition3.IsPublic || typeDefinition3.IsProtected) && oNs.GetTypeDefinition(typeDefinition3.Name, typeDefinition3.TypeParameterCount) == null)
					{
						if (compatibility == AbiCompatibility.Equal)
						{
							compatibility = AbiCompatibility.Bigger;
						}
						return;
					}
				}
				return;
			}
		}

		private static bool ContainsPublicTypes(INamespace testNs)
		{
			Stack<INamespace> stack = new Stack<INamespace>();
			stack.Push(testNs);
			while (stack.Count > 0)
			{
				INamespace @namespace = stack.Pop();
				if (@namespace.Types.Any((ITypeDefinition t) => t.IsPublic))
				{
					return true;
				}
				foreach (INamespace item in @namespace.ChildNamespaces)
				{
					stack.Push(item);
				}
			}
			return false;
		}

		/// <summary>
		/// Check the specified oldProject and newProject if they're compatible.
		/// </summary>
		/// <param name="oldProject">Old project.</param>
		/// <param name="newProject">New project.</param>
		public AbiCompatibility Check(ICompilation oldProject, ICompilation newProject)
		{
			Stack<INamespace> stack = new Stack<INamespace>();
			Stack<INamespace> stack2 = new Stack<INamespace>();
			stack.Push(oldProject.MainAssembly.RootNamespace);
			stack2.Push(newProject.MainAssembly.RootNamespace);
			AbiCompatibility abiCompatibility = AbiCompatibility.Equal;
			while (stack.Count > 0)
			{
				INamespace @namespace = stack.Pop();
				INamespace namespace2 = stack2.Pop();
				this.CheckNamespace(@namespace, namespace2, ref abiCompatibility);
				if (abiCompatibility == AbiCompatibility.Incompatible && this.StopOnIncompatibility)
				{
					return AbiCompatibility.Incompatible;
				}
				foreach (INamespace namespace3 in @namespace.ChildNamespaces)
				{
					INamespace childNamespace = namespace2.GetChildNamespace(namespace3.Name);
					if (childNamespace == null)
					{
						this.OnIncompatibilityFound(new AbiEventArgs(string.Format(this.TranslateString("Namespace {0} is missing."), namespace3.FullName)));
						if (this.StopOnIncompatibility)
						{
							return AbiCompatibility.Incompatible;
						}
					}
					else
					{
						stack.Push(namespace3);
						stack2.Push(childNamespace);
					}
				}
				if (abiCompatibility != AbiCompatibility.Bigger)
				{
					foreach (INamespace namespace4 in namespace2.ChildNamespaces)
					{
						if (@namespace.GetChildNamespace(namespace4.Name) == null)
						{
							if (abiCompatibility == AbiCompatibility.Equal && AbiComparer.ContainsPublicTypes(namespace4))
							{
								abiCompatibility = AbiCompatibility.Bigger;
								break;
							}
							break;
						}
					}
					continue;
				}
			}
			return abiCompatibility;
		}

		public virtual string TranslateString(string str)
		{
			return str;
		}

		public event EventHandler<AbiEventArgs> IncompatibilityFound;

		protected virtual void OnIncompatibilityFound(AbiEventArgs e)
		{
			EventHandler<AbiEventArgs> incompatibilityFound = this.IncompatibilityFound;
			if (incompatibilityFound != null)
			{
				incompatibilityFound(this, e);
			}
		}
	}
}

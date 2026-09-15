using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Contains extension methods for the type system.
	/// </summary>
	public static class TypeSystemExtensions
	{
		/// <summary>
		/// Gets all base types.
		/// </summary>
		/// <remarks>This is the reflexive and transitive closure of <see cref="P:ICSharpCode.NRefactory.TypeSystem.IType.DirectBaseTypes" />.
		/// Note that this method does not return all supertypes - doing so is impossible due to contravariance
		/// (and undesirable for covariance as the list could become very large).
		///
		/// The output is ordered so that base types occur before derived types.
		/// </remarks>
		public static IEnumerable<IType> GetAllBaseTypes(this IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			BaseTypeCollector baseTypeCollector = new BaseTypeCollector();
			baseTypeCollector.CollectBaseTypes(type);
			return baseTypeCollector;
		}

		/// <summary>
		/// Gets all non-interface base types.
		/// </summary>
		/// <remarks>
		/// When <paramref name="type" /> is an interface, this method will also return base interfaces (return same output as GetAllBaseTypes()).
		///
		/// The output is ordered so that base types occur before derived types.
		/// </remarks>
		public static IEnumerable<IType> GetNonInterfaceBaseTypes(this IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			BaseTypeCollector baseTypeCollector = new BaseTypeCollector();
			baseTypeCollector.SkipImplementedInterfaces = true;
			baseTypeCollector.CollectBaseTypes(type);
			return baseTypeCollector;
		}

		/// <summary>
		/// Gets all base type definitions.
		/// The output is ordered so that base types occur before derived types.
		/// </summary>
		/// <remarks>
		/// This is equivalent to type.GetAllBaseTypes().Select(t =&gt; t.GetDefinition()).Where(d =&gt; d != null).Distinct().
		/// </remarks>
		public static IEnumerable<ITypeDefinition> GetAllBaseTypeDefinitions(this IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return (from t in type.GetAllBaseTypes()
			select t.GetDefinition() into d
			where d != null
			select d).Distinct<ITypeDefinition>();
		}

		/// <summary>
		/// Gets whether this type definition is derived from the base type definition.
		/// </summary>
		public static bool IsDerivedFrom(this ITypeDefinition type, ITypeDefinition baseType)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (baseType == null)
			{
				return false;
			}
			if (type.Compilation != baseType.Compilation)
			{
				throw new InvalidOperationException("Both arguments to IsDerivedFrom() must be from the same compilation.");
			}
			return type.GetAllBaseTypeDefinitions().Contains(baseType);
		}

		/// <summary>
		/// Gets whether this type definition is derived from a given known type.
		/// </summary>
		public static bool IsDerivedFrom(this ITypeDefinition type, KnownTypeCode baseType)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return baseType != KnownTypeCode.None && type.IsDerivedFrom(type.Compilation.FindType(baseType).GetDefinition());
		}

		/// <summary>
		/// Gets whether the type is an open type (contains type parameters).
		/// </summary>
		/// <example>
		/// <code>
		/// class X&lt;T&gt; {
		///   List&lt;T&gt; open;
		///   X&lt;X&lt;T[]&gt;&gt; open;
		///   X&lt;string&gt; closed;
		///   int closed;
		/// }
		/// </code>
		/// </example>
		public static bool IsOpen(this IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			TypeSystemExtensions.TypeClassificationVisitor typeClassificationVisitor = new TypeSystemExtensions.TypeClassificationVisitor();
			type.AcceptVisitor(typeClassificationVisitor);
			return typeClassificationVisitor.isOpen;
		}

		/// <summary>
		/// Gets the entity that owns the type parameters occurring in the specified type.
		/// If both class and method type parameters are present, the method is returned.
		/// Returns null if the specified type is closed.
		/// </summary>
		/// <seealso cref="M:ICSharpCode.NRefactory.TypeSystem.TypeSystemExtensions.IsOpen(ICSharpCode.NRefactory.TypeSystem.IType)" />
		private static IEntity GetTypeParameterOwner(IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			TypeSystemExtensions.TypeClassificationVisitor typeClassificationVisitor = new TypeSystemExtensions.TypeClassificationVisitor();
			type.AcceptVisitor(typeClassificationVisitor);
			return typeClassificationVisitor.typeParameterOwner;
		}

		/// <summary>
		/// Gets whether the type is unbound (is a generic type, but no type arguments were provided).
		/// </summary>
		/// <remarks>
		/// In "<c>typeof(List&lt;Dictionary&lt;,&gt;&gt;)</c>", only the Dictionary is unbound, the List is considered
		/// bound despite containing an unbound type.
		/// This method returns false for partially parameterized types (<c>Dictionary&lt;string, &gt;</c>).
		/// </remarks>
		public static bool IsUnbound(this IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type is ITypeDefinition && type.TypeParameterCount > 0;
		}

		/// <summary>
		/// Gets whether the type is the specified known type.
		/// For generic known types, this returns true any parameterization of the type (and also for the definition itself).
		/// </summary>
		public static bool IsKnownType(this IType type, KnownTypeCode knownType)
		{
			ITypeDefinition definition = type.GetDefinition();
			return definition != null && definition.KnownTypeCode == knownType;
		}

		/// <summary>
		/// Imports a symbol from another compilation.
		/// </summary>
		public static ISymbol Import(this ICompilation compilation, ISymbol symbol)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (symbol == null)
			{
				return null;
			}
			switch (symbol.SymbolKind)
			{
			case SymbolKind.Namespace:
				return compilation.Import((INamespace)symbol);
			case SymbolKind.Variable:
			{
				IVariable variable = (IVariable)symbol;
				return new DefaultVariable(compilation.Import(variable.Type), variable.Name, variable.Region, variable.IsConst, variable.ConstantValue);
			}
			case SymbolKind.Parameter:
			{
				IParameter parameter = (IParameter)symbol;
				if (parameter.Owner == null)
				{
					return new DefaultParameter(compilation.Import(parameter.Type), parameter.Name, null, parameter.Region, null, parameter.IsRef, parameter.IsOut, parameter.IsParams, false, null);
				}
				int num = parameter.Owner.Parameters.IndexOf(parameter);
				IParameterizedMember parameterizedMember = (IParameterizedMember)compilation.Import(parameter.Owner);
				if (parameterizedMember == null || num < 0 || num >= parameterizedMember.Parameters.Count)
				{
					return null;
				}
				return parameterizedMember.Parameters[num];
			}
			case SymbolKind.TypeParameter:
				return (ITypeParameter)compilation.Import((IType)symbol);
			default:
				if (symbol is IEntity)
				{
					return compilation.Import((IEntity)symbol);
				}
				throw new NotSupportedException("Unsupported symbol kind: " + symbol.SymbolKind);
			}
		}

		/// <summary>
		/// Imports a type from another compilation.
		/// </summary>
		public static IType Import(this ICompilation compilation, IType type)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (type == null)
			{
				return null;
			}
			ICompilationProvider compilationProvider = type as ICompilationProvider;
			if (compilationProvider != null && compilationProvider.Compilation == compilation)
			{
				return type;
			}
			IEntity typeParameterOwner = TypeSystemExtensions.GetTypeParameterOwner(type);
			IEntity entity = compilation.Import(typeParameterOwner);
			if (entity != null)
			{
				return type.ToTypeReference().Resolve(new SimpleTypeResolveContext(entity));
			}
			return type.ToTypeReference().Resolve(compilation.TypeResolveContext);
		}

		/// <summary>
		/// Imports a type from another compilation.
		/// </summary>
		public static ITypeDefinition Import(this ICompilation compilation, ITypeDefinition typeDefinition)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (typeDefinition == null)
			{
				return null;
			}
			if (typeDefinition.Compilation == compilation)
			{
				return typeDefinition;
			}
			return typeDefinition.ToTypeReference().Resolve(compilation.TypeResolveContext).GetDefinition();
		}

		/// <summary>
		/// Imports an entity from another compilation.
		/// </summary>
		public static IEntity Import(this ICompilation compilation, IEntity entity)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (entity == null)
			{
				return null;
			}
			if (entity.Compilation == compilation)
			{
				return entity;
			}
			if (entity is IMember)
			{
				return ((IMember)entity).ToReference().Resolve(compilation.TypeResolveContext);
			}
			if (entity is ITypeDefinition)
			{
				return ((ITypeDefinition)entity).ToTypeReference().Resolve(compilation.TypeResolveContext).GetDefinition();
			}
			throw new NotSupportedException("Unknown entity type");
		}

		/// <summary>
		/// Imports a member from another compilation.
		/// </summary>
		public static IMember Import(this ICompilation compilation, IMember member)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (member == null)
			{
				return null;
			}
			if (member.Compilation == compilation)
			{
				return member;
			}
			return member.ToReference().Resolve(compilation.TypeResolveContext);
		}

		/// <summary>
		/// Imports a member from another compilation.
		/// </summary>
		public static IMethod Import(this ICompilation compilation, IMethod method)
		{
			return (IMethod)compilation.Import(method);
		}

		/// <summary>
		/// Imports a member from another compilation.
		/// </summary>
		public static IField Import(this ICompilation compilation, IField field)
		{
			return (IField)compilation.Import(field);
		}

		/// <summary>
		/// Imports a member from another compilation.
		/// </summary>
		public static IEvent Import(this ICompilation compilation, IEvent ev)
		{
			return (IEvent)compilation.Import(ev);
		}

		/// <summary>
		/// Imports a member from another compilation.
		/// </summary>
		public static IProperty Import(this ICompilation compilation, IProperty property)
		{
			return (IProperty)compilation.Import(property);
		}

		/// <summary>
		/// Imports a namespace from another compilation.
		/// </summary>
		/// <remarks>
		/// This method may return null if the namespace does not exist in the target compilation.
		/// </remarks>
		public static INamespace Import(this ICompilation compilation, INamespace ns)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			if (ns == null)
			{
				return null;
			}
			if (ns.ParentNamespace == null)
			{
				return compilation.GetNamespaceForExternAlias(ns.ExternAlias);
			}
			INamespace @namespace = compilation.Import(ns.ParentNamespace);
			if (@namespace != null)
			{
				return @namespace.GetChildNamespace(ns.Name);
			}
			return null;
		}

		/// <summary>
		/// Gets the invoke method for a delegate type.
		/// </summary>
		/// <remarks>
		/// Returns null if the type is not a delegate type; or if the invoke method could not be found.
		/// </remarks>
		public static IMethod GetDelegateInvokeMethod(this IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.Kind == TypeKind.Delegate)
			{
				return type.GetMethods((IUnresolvedMethod m) => m.Name == "Invoke", GetMemberOptions.IgnoreInheritedMembers).FirstOrDefault<IMethod>();
			}
			return null;
		}

		/// <summary>
		/// Gets all unresolved type definitions from the file.
		/// For partial classes, each part is returned.
		/// </summary>
		public static IEnumerable<IUnresolvedTypeDefinition> GetAllTypeDefinitions(this IUnresolvedFile file)
		{
			return TreeTraversal.PreOrder<IUnresolvedTypeDefinition>(file.TopLevelTypeDefinitions, (IUnresolvedTypeDefinition t) => t.NestedTypes);
		}

		/// <summary>
		/// Gets all unresolved type definitions from the assembly.
		/// For partial classes, each part is returned.
		/// </summary>
		public static IEnumerable<IUnresolvedTypeDefinition> GetAllTypeDefinitions(this IUnresolvedAssembly assembly)
		{
			return TreeTraversal.PreOrder<IUnresolvedTypeDefinition>(assembly.TopLevelTypeDefinitions, (IUnresolvedTypeDefinition t) => t.NestedTypes);
		}

		public static IEnumerable<ITypeDefinition> GetAllTypeDefinitions(this IAssembly assembly)
		{
			return TreeTraversal.PreOrder<ITypeDefinition>(assembly.TopLevelTypeDefinitions, (ITypeDefinition t) => t.NestedTypes);
		}

		/// <summary>
		/// Gets all type definitions in the compilation.
		/// This may include types from referenced assemblies that are not accessible in the main assembly.
		/// </summary>
		public static IEnumerable<ITypeDefinition> GetAllTypeDefinitions(this ICompilation compilation)
		{
			return compilation.Assemblies.SelectMany((IAssembly a) => a.GetAllTypeDefinitions());
		}

		/// <summary>
		/// Gets all top level type definitions in the compilation.
		/// This may include types from referenced assemblies that are not accessible in the main assembly.
		/// </summary>
		public static IEnumerable<ITypeDefinition> GetTopLevelTypeDefinitons(this ICompilation compilation)
		{
			return compilation.Assemblies.SelectMany((IAssembly a) => a.TopLevelTypeDefinitions);
		}

		/// <summary>
		/// Gets the type (potentially a nested type) defined at the specified location.
		/// Returns null if no type is defined at that location.
		/// </summary>
		public static IUnresolvedTypeDefinition GetInnermostTypeDefinition(this IUnresolvedFile file, int line, int column)
		{
			return file.GetInnermostTypeDefinition(new TextLocation(line, column));
		}

		/// <summary>
		/// Gets the member defined at the specified location.
		/// Returns null if no member is defined at that location.
		/// </summary>
		public static IUnresolvedMember GetMember(this IUnresolvedFile file, int line, int column)
		{
			return file.GetMember(new TextLocation(line, column));
		}

		public static IList<IAttribute> CreateResolvedAttributes(this IList<IUnresolvedAttribute> attributes, ITypeResolveContext context)
		{
			if (attributes == null)
			{
				throw new ArgumentNullException("attributes");
			}
			if (attributes.Count == 0)
			{
				return EmptyList<IAttribute>.Instance;
			}
			return new ProjectedList<ITypeResolveContext, IUnresolvedAttribute, IAttribute>(context, attributes, (ITypeResolveContext c, IUnresolvedAttribute a) => a.CreateResolvedAttribute(c));
		}

		public static IList<ITypeParameter> CreateResolvedTypeParameters(this IList<IUnresolvedTypeParameter> typeParameters, ITypeResolveContext context)
		{
			if (typeParameters == null)
			{
				throw new ArgumentNullException("typeParameters");
			}
			if (typeParameters.Count == 0)
			{
				return EmptyList<ITypeParameter>.Instance;
			}
			return new ProjectedList<ITypeResolveContext, IUnresolvedTypeParameter, ITypeParameter>(context, typeParameters, (ITypeResolveContext c, IUnresolvedTypeParameter a) => a.CreateResolvedTypeParameter(c));
		}

		public static IList<IParameter> CreateResolvedParameters(this IList<IUnresolvedParameter> parameters, ITypeResolveContext context)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (parameters.Count == 0)
			{
				return EmptyList<IParameter>.Instance;
			}
			return new ProjectedList<ITypeResolveContext, IUnresolvedParameter, IParameter>(context, parameters, (ITypeResolveContext c, IUnresolvedParameter a) => a.CreateResolvedParameter(c));
		}

		public static IList<IType> Resolve(this IList<ITypeReference> typeReferences, ITypeResolveContext context)
		{
			if (typeReferences == null)
			{
				throw new ArgumentNullException("typeReferences");
			}
			if (typeReferences.Count == 0)
			{
				return EmptyList<IType>.Instance;
			}
			return new ProjectedList<ITypeResolveContext, ITypeReference, IType>(context, typeReferences, (ITypeResolveContext c, ITypeReference t) => t.Resolve(c));
		}

		public static IList<ResolveResult> Resolve(this IList<IConstantValue> constantValues, ITypeResolveContext context)
		{
			if (constantValues == null)
			{
				throw new ArgumentNullException("constantValues");
			}
			if (constantValues.Count == 0)
			{
				return EmptyList<ResolveResult>.Instance;
			}
			return new ProjectedList<ITypeResolveContext, IConstantValue, ResolveResult>(context, constantValues, (ITypeResolveContext c, IConstantValue t) => t.Resolve(c));
		}

		public static IEnumerable<ITypeDefinition> GetSubTypeDefinitions(this IType baseType)
		{
			if (baseType == null)
			{
				throw new ArgumentNullException("baseType");
			}
			ITypeDefinition definition = baseType.GetDefinition();
			if (definition == null)
			{
				return Enumerable.Empty<ITypeDefinition>();
			}
			return definition.GetSubTypeDefinitions();
		}

		/// <summary>
		/// Gets all sub type definitions defined in a context.
		/// </summary>
		public static IEnumerable<ITypeDefinition> GetSubTypeDefinitions(this ITypeDefinition baseType)
		{
			if (baseType == null)
			{
				throw new ArgumentNullException("baseType");
			}
			foreach (ITypeDefinition contextType in baseType.Compilation.GetAllTypeDefinitions())
			{
				if (contextType.IsDerivedFrom(baseType))
				{
					yield return contextType;
				}
			}
			yield break;
		}

		/// <summary>
		/// Retrieves the specified type in this compilation.
		/// Returns an <see cref="T:ICSharpCode.NRefactory.TypeSystem.Implementation.UnknownType" /> if the type cannot be found in this compilation.
		/// </summary>
		/// <remarks>
		/// There can be multiple types with the same full name in a compilation, as a
		/// full type name is only unique per assembly.
		/// If there are multiple possible matches, this method will return just one of them.
		/// When possible, use <see cref="M:ICSharpCode.NRefactory.TypeSystem.IAssembly.GetTypeDefinition(ICSharpCode.NRefactory.TypeSystem.TopLevelTypeName)" /> instead to
		/// retrieve a type from a specific assembly.
		/// </remarks>
		public static IType FindType(this ICompilation compilation, FullTypeName fullTypeName)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			foreach (IAssembly assembly in compilation.Assemblies)
			{
				ITypeDefinition typeDefinition = assembly.GetTypeDefinition(fullTypeName);
				if (typeDefinition != null)
				{
					return typeDefinition;
				}
			}
			return new UnknownType(fullTypeName);
		}

		/// <summary>
		/// Gets the type definition for the specified unresolved type.
		/// Returns null if the unresolved type does not belong to this assembly.
		/// </summary>
		public static ITypeDefinition GetTypeDefinition(this IAssembly assembly, FullTypeName fullTypeName)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			TopLevelTypeName topLevelTypeName = fullTypeName.TopLevelTypeName;
			ITypeDefinition typeDefinition = assembly.GetTypeDefinition(topLevelTypeName);
			if (typeDefinition == null)
			{
				return null;
			}
			int num = topLevelTypeName.TypeParameterCount;
			for (int i = 0; i < fullTypeName.NestingLevel; i++)
			{
				string nestedTypeName = fullTypeName.GetNestedTypeName(i);
				num += fullTypeName.GetNestedTypeAdditionalTypeParameterCount(i);
				typeDefinition = TypeSystemExtensions.FindNestedType(typeDefinition, nestedTypeName, num);
				if (typeDefinition == null)
				{
					break;
				}
			}
			return typeDefinition;
		}

		private static ITypeDefinition FindNestedType(ITypeDefinition typeDef, string name, int typeParameterCount)
		{
			foreach (ITypeDefinition typeDefinition in typeDef.NestedTypes)
			{
				if (typeDefinition.Name == name && typeDefinition.TypeParameterCount == typeParameterCount)
				{
					return typeDefinition;
				}
			}
			return null;
		}

		/// <summary>
		/// Resolves a type reference in the compilation's main type resolve context.
		/// Some type references require a more specific type resolve context and will not resolve using this method.
		/// </summary>
		/// <returns>
		/// Returns the resolved type.
		/// In case of an error, returns <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" />.
		/// Never returns null.
		/// </returns>
		public static IType Resolve(this ITypeReference reference, ICompilation compilation)
		{
			if (reference == null)
			{
				throw new ArgumentNullException("reference");
			}
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			return reference.Resolve(compilation.TypeResolveContext);
		}

		/// <summary>
		/// Gets the attribute of the specified attribute type (or derived attribute types).
		/// </summary>
		/// <param name="entity">The entity on which the attributes are declared.</param>
		/// <param name="attributeType">The attribute type to look for.</param>
		/// <param name="inherit">
		/// Specifies whether attributes inherited from base classes and base members (if the given <paramref name="entity" /> in an <c>override</c>)
		/// should be returned. The default is <c>true</c>.
		/// </param>
		/// <returns>
		/// Returns the attribute that was found; or <c>null</c> if none was found.
		/// If inherit is true, an from the entity itself will be returned if possible;
		/// and the base entity will only be searched if none exists.
		/// </returns>
		public static IAttribute GetAttribute(this IEntity entity, IType attributeType, bool inherit = true)
		{
			return entity.GetAttributes(attributeType, inherit).FirstOrDefault<IAttribute>();
		}

		/// <summary>
		/// Gets the attributes of the specified attribute type (or derived attribute types).
		/// </summary>
		/// <param name="entity">The entity on which the attributes are declared.</param>
		/// <param name="attributeType">The attribute type to look for.</param>
		/// <param name="inherit">
		/// Specifies whether attributes inherited from base classes and base members (if the given <paramref name="entity" /> in an <c>override</c>)
		/// should be returned. The default is <c>true</c>.
		/// </param>
		/// <returns>
		/// Returns the list of attributes that were found.
		/// If inherit is true, attributes from the entity itself are returned first; followed by attributes inherited from the base entity.
		/// </returns>
		public static IEnumerable<IAttribute> GetAttributes(this IEntity entity, IType attributeType, bool inherit = true)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			return TypeSystemExtensions.GetAttributes(entity, new Predicate<IType>(attributeType.Equals), inherit);
		}

		/// <summary>
		/// Gets the attribute of the specified attribute type (or derived attribute types).
		/// </summary>
		/// <param name="entity">The entity on which the attributes are declared.</param>
		/// <param name="attributeType">The attribute type to look for.</param>
		/// <param name="inherit">
		/// Specifies whether attributes inherited from base classes and base members (if the given <paramref name="entity" /> in an <c>override</c>)
		/// should be returned. The default is <c>true</c>.
		/// </param>
		/// <returns>
		/// Returns the attribute that was found; or <c>null</c> if none was found.
		/// If inherit is true, an from the entity itself will be returned if possible;
		/// and the base entity will only be searched if none exists.
		/// </returns>
		public static IAttribute GetAttribute(this IEntity entity, FullTypeName attributeType, bool inherit = true)
		{
			return entity.GetAttributes(attributeType, inherit).FirstOrDefault<IAttribute>();
		}

		/// <summary>
		/// Gets the attributes of the specified attribute type (or derived attribute types).
		/// </summary>
		/// <param name="entity">The entity on which the attributes are declared.</param>
		/// <param name="attributeType">The attribute type to look for.</param>
		/// <param name="inherit">
		/// Specifies whether attributes inherited from base classes and base members (if the given <paramref name="entity" /> in an <c>override</c>)
		/// should be returned. The default is <c>true</c>.
		/// </param>
		/// <returns>
		/// Returns the list of attributes that were found.
		/// If inherit is true, attributes from the entity itself are returned first; followed by attributes inherited from the base entity.
		/// </returns>
		public static IEnumerable<IAttribute> GetAttributes(this IEntity entity, FullTypeName attributeType, bool inherit = true)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			return TypeSystemExtensions.GetAttributes(entity, delegate(IType attrType)
			{
				ITypeDefinition definition = attrType.GetDefinition();
				return definition != null && definition.FullTypeName == attributeType;
			}, inherit);
		}

		/// <summary>
		/// Gets the attribute of the specified attribute type (or derived attribute types).
		/// </summary>
		/// <param name="entity">The entity on which the attributes are declared.</param>
		/// <param name="inherit">
		/// Specifies whether attributes inherited from base classes and base members (if the given <paramref name="entity" /> in an <c>override</c>)
		/// should be returned. The default is <c>true</c>.
		/// </param>
		/// <returns>
		/// Returns the attribute that was found; or <c>null</c> if none was found.
		/// If inherit is true, an from the entity itself will be returned if possible;
		/// and the base entity will only be searched if none exists.
		/// </returns>
		public static IEnumerable<IAttribute> GetAttributes(this IEntity entity, bool inherit = true)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			return TypeSystemExtensions.GetAttributes(entity, (IType a) => true, inherit);
		}

		private static IEnumerable<IAttribute> GetAttributes(IEntity entity, Predicate<IType> attributeTypePredicate, bool inherit)
		{
			if (!inherit)
			{
				foreach (IAttribute attr in entity.Attributes)
				{
					if (attributeTypePredicate(attr.AttributeType))
					{
						yield return attr;
					}
				}
			}
			else
			{
				ITypeDefinition typeDef = entity as ITypeDefinition;
				if (typeDef != null)
				{
					foreach (IType baseType in typeDef.GetNonInterfaceBaseTypes().Reverse<IType>())
					{
						ITypeDefinition baseTypeDef = baseType.GetDefinition();
						if (baseTypeDef != null)
						{
							foreach (IAttribute attr2 in baseTypeDef.Attributes)
							{
								if (attributeTypePredicate(attr2.AttributeType))
								{
									yield return attr2;
								}
							}
						}
					}
				}
				else
				{
					IMember member = entity as IMember;
					if (member == null)
					{
						throw new NotSupportedException("Unknown entity type");
					}
					HashSet<IMember> visitedMembers = new HashSet<IMember>();
					do
					{
						member = member.MemberDefinition;
						if (!visitedMembers.Add(member))
						{
							break;
						}
						foreach (IAttribute attr3 in member.Attributes)
						{
							if (attributeTypePredicate(attr3.AttributeType))
							{
								yield return attr3;
							}
						}
						if (!member.IsOverride)
						{
							break;
						}
					}
					while ((member = InheritanceHelper.GetBaseMember(member)) != null);
				}
			}
			yield break;
		}

		/// <summary>
		/// Gets the type definition for a top-level type.
		/// </summary>
		/// <remarks>This method uses ordinal name comparison, not the compilation's name comparer.</remarks>
		public static ITypeDefinition GetTypeDefinition(this IAssembly assembly, string namespaceName, string name, int typeParameterCount = 0)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			return assembly.GetTypeDefinition(new TopLevelTypeName(namespaceName, name, typeParameterCount));
		}

		public static ISymbol GetSymbol(this ResolveResult rr)
		{
			if (rr is LocalResolveResult)
			{
				return ((LocalResolveResult)rr).Variable;
			}
			if (rr is MemberResolveResult)
			{
				return ((MemberResolveResult)rr).Member;
			}
			if (rr is TypeResolveResult)
			{
				return ((TypeResolveResult)rr).Type.GetDefinition();
			}
			return null;
		}

		private sealed class TypeClassificationVisitor : TypeVisitor
		{
			public override IType VisitTypeParameter(ITypeParameter type)
			{
				this.isOpen = true;
				int nestingLevel = TypeSystemExtensions.TypeClassificationVisitor.GetNestingLevel(type.Owner);
				if (nestingLevel > this.typeParameterOwnerNestingLevel)
				{
					this.typeParameterOwner = type.Owner;
					this.typeParameterOwnerNestingLevel = nestingLevel;
				}
				return base.VisitTypeParameter(type);
			}

			private static int GetNestingLevel(IEntity entity)
			{
				int num = 0;
				while (entity != null)
				{
					num++;
					entity = entity.DeclaringTypeDefinition;
				}
				return num;
			}

			internal bool isOpen;

			internal IEntity typeParameterOwner;

			private int typeParameterOwnerNestingLevel;
		}
	}
}

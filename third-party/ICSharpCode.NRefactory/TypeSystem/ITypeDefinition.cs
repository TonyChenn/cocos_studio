using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a class, enum, interface, struct, delegate or VB module.
	/// For partial classes, this represents the whole class.
	/// </summary>
	public interface ITypeDefinition : IType, IEquatable<IType>, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		/// <summary>
		/// Returns all parts that contribute to this type definition.
		/// Non-partial classes have a single part that represents the whole class.
		/// </summary>
		IList<IUnresolvedTypeDefinition> Parts { get; }

		IList<ITypeParameter> TypeParameters { get; }

		IList<ITypeDefinition> NestedTypes { get; }

		IList<IMember> Members { get; }

		IEnumerable<IField> Fields { get; }

		IEnumerable<IMethod> Methods { get; }

		IEnumerable<IProperty> Properties { get; }

		IEnumerable<IEvent> Events { get; }

		/// <summary>
		/// Gets the known type code for this type definition.
		/// </summary>
		KnownTypeCode KnownTypeCode { get; }

		/// <summary>
		/// For enums: returns the underlying primitive type.
		/// For all other types: returns <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" />.
		/// </summary>
		IType EnumUnderlyingType { get; }

		/// <summary>
		/// Gets the full name of this type.
		/// </summary>
		FullTypeName FullTypeName { get; }

		/// <summary>
		/// Gets/Sets the declaring type (incl. type arguments, if any).
		/// This property never returns null -- for top-level entities, it returns SharedTypes.UnknownType.
		/// </summary>
		IType DeclaringType { get; }

		/// <summary>
		/// Gets whether this type contains extension methods.
		/// </summary>
		/// <remarks>This property is used to speed up the search for extension methods.</remarks>
		bool HasExtensionMethods { get; }

		/// <summary>
		/// Gets whether this type definition is made up of one or more partial classes.
		/// </summary>
		bool IsPartial { get; }

		/// <summary>
		/// Determines how this type is implementing the specified interface member.
		/// </summary>
		/// <returns>
		/// The method on this type that implements the interface member;
		/// or null if the type does not implement the interface.
		/// </returns>
		IMember GetInterfaceImplementation(IMember interfaceMember);

		/// <summary>
		/// Determines how this type is implementing the specified interface members.
		/// </summary>
		/// <returns>
		/// For each interface member, this method returns the class member 
		/// that implements the interface member.
		/// For interface members that are missing an implementation, the
		/// result collection will contain a null element.
		/// </returns>
		IList<IMember> GetInterfaceImplementation(IList<IMember> interfaceMembers);
	}
}

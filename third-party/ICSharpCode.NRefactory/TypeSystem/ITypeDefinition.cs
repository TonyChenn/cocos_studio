using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a class, enum, interface, struct, delegate or VB module.
	/// For partial classes, this represents the whole class.
	/// </summary>
	// Token: 0x020000B2 RID: 178
	public interface ITypeDefinition : IType, IEquatable<IType>, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		/// <summary>
		/// Returns all parts that contribute to this type definition.
		/// Non-partial classes have a single part that represents the whole class.
		/// </summary>
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060005CE RID: 1486
		IList<IUnresolvedTypeDefinition> Parts { get; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060005CF RID: 1487
		IList<ITypeParameter> TypeParameters { get; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060005D0 RID: 1488
		IList<ITypeDefinition> NestedTypes { get; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060005D1 RID: 1489
		IList<IMember> Members { get; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060005D2 RID: 1490
		IEnumerable<IField> Fields { get; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060005D3 RID: 1491
		IEnumerable<IMethod> Methods { get; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060005D4 RID: 1492
		IEnumerable<IProperty> Properties { get; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060005D5 RID: 1493
		IEnumerable<IEvent> Events { get; }

		/// <summary>
		/// Gets the known type code for this type definition.
		/// </summary>
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060005D6 RID: 1494
		KnownTypeCode KnownTypeCode { get; }

		/// <summary>
		/// For enums: returns the underlying primitive type.
		/// For all other types: returns <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" />.
		/// </summary>
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060005D7 RID: 1495
		IType EnumUnderlyingType { get; }

		/// <summary>
		/// Gets the full name of this type.
		/// </summary>
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060005D8 RID: 1496
		FullTypeName FullTypeName { get; }

		/// <summary>
		/// Gets/Sets the declaring type (incl. type arguments, if any).
		/// This property never returns null -- for top-level entities, it returns SharedTypes.UnknownType.
		/// </summary>
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060005D9 RID: 1497
		IType DeclaringType { get; }

		/// <summary>
		/// Gets whether this type contains extension methods.
		/// </summary>
		/// <remarks>This property is used to speed up the search for extension methods.</remarks>
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060005DA RID: 1498
		bool HasExtensionMethods { get; }

		/// <summary>
		/// Gets whether this type definition is made up of one or more partial classes.
		/// </summary>
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060005DB RID: 1499
		bool IsPartial { get; }

		/// <summary>
		/// Determines how this type is implementing the specified interface member.
		/// </summary>
		/// <returns>
		/// The method on this type that implements the interface member;
		/// or null if the type does not implement the interface.
		/// </returns>
		// Token: 0x060005DC RID: 1500
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
		// Token: 0x060005DD RID: 1501
		IList<IMember> GetInterfaceImplementation(IList<IMember> interfaceMembers);
	}
}

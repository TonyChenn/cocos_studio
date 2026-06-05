using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Special type definition for 'void'.
	/// </summary>
	// Token: 0x020000E7 RID: 231
	public class VoidTypeDefinition : DefaultResolvedTypeDefinition
	{
		// Token: 0x0600089F RID: 2207 RVA: 0x00016A16 File Offset: 0x00015A16
		public VoidTypeDefinition(ITypeResolveContext parentContext, params IUnresolvedTypeDefinition[] parts) : base(parentContext, parts)
		{
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00016A20 File Offset: 0x00015A20
		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Void;
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00016A23 File Offset: 0x00015A23
		public override IEnumerable<IMethod> GetConstructors(Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return EmptyList<IMethod>.Instance;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00016A2A File Offset: 0x00015A2A
		public override IEnumerable<IEvent> GetEvents(Predicate<IUnresolvedEvent> filter, GetMemberOptions options)
		{
			return EmptyList<IEvent>.Instance;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00016A31 File Offset: 0x00015A31
		public override IEnumerable<IField> GetFields(Predicate<IUnresolvedField> filter, GetMemberOptions options)
		{
			return EmptyList<IField>.Instance;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00016A38 File Offset: 0x00015A38
		public override IEnumerable<IMethod> GetMethods(Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return EmptyList<IMethod>.Instance;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00016A3F File Offset: 0x00015A3F
		public override IEnumerable<IMethod> GetMethods(IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter, GetMemberOptions options)
		{
			return EmptyList<IMethod>.Instance;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00016A46 File Offset: 0x00015A46
		public override IEnumerable<IProperty> GetProperties(Predicate<IUnresolvedProperty> filter, GetMemberOptions options)
		{
			return EmptyList<IProperty>.Instance;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00016A4D File Offset: 0x00015A4D
		public override IEnumerable<IMember> GetMembers(Predicate<IUnresolvedMember> filter, GetMemberOptions options)
		{
			return EmptyList<IMember>.Instance;
		}
	}
}

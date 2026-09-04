using System;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200006E RID: 110
	public sealed class ByReferenceType : TypeWithElementType
	{
		// Token: 0x0600038A RID: 906 RVA: 0x000089E2 File Offset: 0x000079E2
		public ByReferenceType(IType elementType) : base(elementType)
		{
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600038B RID: 907 RVA: 0x000089EB File Offset: 0x000079EB
		public override TypeKind Kind
		{
			get
			{
				return TypeKind.ByReference;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000089EF File Offset: 0x000079EF
		public override string NameSuffix
		{
			get
			{
				return "&";
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600038D RID: 909 RVA: 0x000089F8 File Offset: 0x000079F8
		public override bool? IsReferenceType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00008A0E File Offset: 0x00007A0E
		public override int GetHashCode()
		{
			return this.elementType.GetHashCode() ^ 91725813;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00008A24 File Offset: 0x00007A24
		public override bool Equals(IType other)
		{
			ByReferenceType byReferenceType = other as ByReferenceType;
			return byReferenceType != null && this.elementType.Equals(byReferenceType.elementType);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00008A4E File Offset: 0x00007A4E
		public override IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitByReferenceType(this);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00008A58 File Offset: 0x00007A58
		public override IType VisitChildren(TypeVisitor visitor)
		{
			IType type = this.elementType.AcceptVisitor(visitor);
			if (type == this.elementType)
			{
				return this;
			}
			return new ByReferenceType(type);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00008A83 File Offset: 0x00007A83
		public override ITypeReference ToTypeReference()
		{
			return new ByReferenceTypeReference(this.elementType.ToTypeReference());
		}
	}
}

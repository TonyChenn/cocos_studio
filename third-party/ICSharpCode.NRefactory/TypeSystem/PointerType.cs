using System;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x020000FD RID: 253
	public sealed class PointerType : TypeWithElementType
	{
		// Token: 0x06000947 RID: 2375 RVA: 0x00018D12 File Offset: 0x00017D12
		public PointerType(IType elementType) : base(elementType)
		{
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x00018D1B File Offset: 0x00017D1B
		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Pointer;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00018D1F File Offset: 0x00017D1F
		public override string NameSuffix
		{
			get
			{
				return "*";
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x00018D28 File Offset: 0x00017D28
		public override bool? IsReferenceType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00018D3E File Offset: 0x00017D3E
		public override int GetHashCode()
		{
			return this.elementType.GetHashCode() ^ 91725811;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00018D54 File Offset: 0x00017D54
		public override bool Equals(IType other)
		{
			PointerType pointerType = other as PointerType;
			return pointerType != null && this.elementType.Equals(pointerType.elementType);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00018D7E File Offset: 0x00017D7E
		public override IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitPointerType(this);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00018D88 File Offset: 0x00017D88
		public override IType VisitChildren(TypeVisitor visitor)
		{
			IType type = this.elementType.AcceptVisitor(visitor);
			if (type == this.elementType)
			{
				return this;
			}
			return new PointerType(type);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00018DB3 File Offset: 0x00017DB3
		public override ITypeReference ToTypeReference()
		{
			return new PointerTypeReference(this.elementType.ToTypeReference());
		}
	}
}

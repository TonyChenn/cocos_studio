using System;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public sealed class PointerType : TypeWithElementType
	{
		public PointerType(IType elementType) : base(elementType)
		{
		}

		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Pointer;
			}
		}

		public override string NameSuffix
		{
			get
			{
				return "*";
			}
		}

		public override bool? IsReferenceType
		{
			get
			{
				return null;
			}
		}

		public override int GetHashCode()
		{
			return this.elementType.GetHashCode() ^ 91725811;
		}

		public override bool Equals(IType other)
		{
			PointerType pointerType = other as PointerType;
			return pointerType != null && this.elementType.Equals(pointerType.elementType);
		}

		public override IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitPointerType(this);
		}

		public override IType VisitChildren(TypeVisitor visitor)
		{
			IType type = this.elementType.AcceptVisitor(visitor);
			if (type == this.elementType)
			{
				return this;
			}
			return new PointerType(type);
		}

		public override ITypeReference ToTypeReference()
		{
			return new PointerTypeReference(this.elementType.ToTypeReference());
		}
	}
}

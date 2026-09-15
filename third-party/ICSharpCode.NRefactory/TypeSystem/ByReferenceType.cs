using System;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public sealed class ByReferenceType : TypeWithElementType
	{
		public ByReferenceType(IType elementType) : base(elementType)
		{
		}

		public override TypeKind Kind
		{
			get
			{
				return TypeKind.ByReference;
			}
		}

		public override string NameSuffix
		{
			get
			{
				return "&";
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
			return this.elementType.GetHashCode() ^ 91725813;
		}

		public override bool Equals(IType other)
		{
			ByReferenceType byReferenceType = other as ByReferenceType;
			return byReferenceType != null && this.elementType.Equals(byReferenceType.elementType);
		}

		public override IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitByReferenceType(this);
		}

		public override IType VisitChildren(TypeVisitor visitor)
		{
			IType type = this.elementType.AcceptVisitor(visitor);
			if (type == this.elementType)
			{
				return this;
			}
			return new ByReferenceType(type);
		}

		public override ITypeReference ToTypeReference()
		{
			return new ByReferenceTypeReference(this.elementType.ToTypeReference());
		}
	}
}

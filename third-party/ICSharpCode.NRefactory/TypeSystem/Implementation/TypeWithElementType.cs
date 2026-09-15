using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public abstract class TypeWithElementType : AbstractType
	{
		protected TypeWithElementType(IType elementType)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			this.elementType = elementType;
		}

		public override string Name
		{
			get
			{
				return this.elementType.Name + this.NameSuffix;
			}
		}

		public override string Namespace
		{
			get
			{
				return this.elementType.Namespace;
			}
		}

		public override string FullName
		{
			get
			{
				return this.elementType.FullName + this.NameSuffix;
			}
		}

		public override string ReflectionName
		{
			get
			{
				return this.elementType.ReflectionName + this.NameSuffix;
			}
		}

		public abstract string NameSuffix { get; }

		public IType ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		public abstract override IType VisitChildren(TypeVisitor visitor);

		[CLSCompliant(false)]
		protected IType elementType;
	}
}

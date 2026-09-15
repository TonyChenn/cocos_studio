using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Base class for the visitor pattern on <see cref="T:ICSharpCode.NRefactory.TypeSystem.IType" />.
	/// </summary>
	public abstract class TypeVisitor
	{
		public virtual IType VisitTypeDefinition(ITypeDefinition type)
		{
			return type.VisitChildren(this);
		}

		public virtual IType VisitTypeParameter(ITypeParameter type)
		{
			return type.VisitChildren(this);
		}

		public virtual IType VisitParameterizedType(ParameterizedType type)
		{
			return type.VisitChildren(this);
		}

		public virtual IType VisitArrayType(ArrayType type)
		{
			return type.VisitChildren(this);
		}

		public virtual IType VisitPointerType(PointerType type)
		{
			return type.VisitChildren(this);
		}

		public virtual IType VisitByReferenceType(ByReferenceType type)
		{
			return type.VisitChildren(this);
		}

		public virtual IType VisitOtherType(IType type)
		{
			return type.VisitChildren(this);
		}
	}
}

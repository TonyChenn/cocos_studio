using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Base class for the visitor pattern on <see cref="T:ICSharpCode.NRefactory.TypeSystem.IType" />.
	/// </summary>
	// Token: 0x02000084 RID: 132
	public abstract class TypeVisitor
	{
		// Token: 0x06000415 RID: 1045 RVA: 0x0000A2F6 File Offset: 0x000092F6
		public virtual IType VisitTypeDefinition(ITypeDefinition type)
		{
			return type.VisitChildren(this);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000A2FF File Offset: 0x000092FF
		public virtual IType VisitTypeParameter(ITypeParameter type)
		{
			return type.VisitChildren(this);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000A308 File Offset: 0x00009308
		public virtual IType VisitParameterizedType(ParameterizedType type)
		{
			return type.VisitChildren(this);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000A311 File Offset: 0x00009311
		public virtual IType VisitArrayType(ArrayType type)
		{
			return type.VisitChildren(this);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000A31A File Offset: 0x0000931A
		public virtual IType VisitPointerType(PointerType type)
		{
			return type.VisitChildren(this);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000A323 File Offset: 0x00009323
		public virtual IType VisitByReferenceType(ByReferenceType type)
		{
			return type.VisitChildren(this);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000A32C File Offset: 0x0000932C
		public virtual IType VisitOtherType(IType type)
		{
			return type.VisitChildren(this);
		}
	}
}

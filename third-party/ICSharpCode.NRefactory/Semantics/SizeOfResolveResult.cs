using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the 'sizeof'.
	/// </summary>
	// Token: 0x0200003D RID: 61
	public class SizeOfResolveResult : ResolveResult
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x00005F3A File Offset: 0x00004F3A
		public SizeOfResolveResult(IType int32, IType referencedType, int? constantValue) : base(int32)
		{
			if (referencedType == null)
			{
				throw new ArgumentNullException("referencedType");
			}
			this.referencedType = referencedType;
			this.constantValue = constantValue;
		}

		/// <summary>
		/// The type referenced by the 'sizeof'.
		/// </summary>
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00005F5F File Offset: 0x00004F5F
		public IType ReferencedType
		{
			get
			{
				return this.referencedType;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00005F68 File Offset: 0x00004F68
		public override bool IsCompileTimeConstant
		{
			get
			{
				return this.constantValue != null;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00005F83 File Offset: 0x00004F83
		public override object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00005F90 File Offset: 0x00004F90
		public override bool IsError
		{
			get
			{
				return this.referencedType.IsReferenceType != false;
			}
		}

		// Token: 0x0400006B RID: 107
		private readonly IType referencedType;

		// Token: 0x0400006C RID: 108
		private readonly int? constantValue;
	}
}

using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the 'typeof'.
	/// </summary>
	// Token: 0x0200004E RID: 78
	public class TypeOfResolveResult : ResolveResult
	{
		// Token: 0x06000240 RID: 576 RVA: 0x00006A29 File Offset: 0x00005A29
		public TypeOfResolveResult(IType systemType, IType referencedType) : base(systemType)
		{
			if (referencedType == null)
			{
				throw new ArgumentNullException("referencedType");
			}
			this.referencedType = referencedType;
		}

		/// <summary>
		/// The type referenced by the 'typeof'.
		/// </summary>
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00006A47 File Offset: 0x00005A47
		public IType ReferencedType
		{
			get
			{
				return this.referencedType;
			}
		}

		// Token: 0x040000A8 RID: 168
		private readonly IType referencedType;
	}
}

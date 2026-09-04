using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Resolve result representing an array access.
	/// </summary>
	// Token: 0x02000038 RID: 56
	public class ArrayAccessResolveResult : ResolveResult
	{
		// Token: 0x060001AE RID: 430 RVA: 0x00005CE2 File Offset: 0x00004CE2
		public ArrayAccessResolveResult(IType elementType, ResolveResult array, IList<ResolveResult> indexes) : base(elementType)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (indexes == null)
			{
				throw new ArgumentNullException("indexes");
			}
			this.Array = array;
			this.Indexes = indexes;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00005D18 File Offset: 0x00004D18
		public override IEnumerable<ResolveResult> GetChildResults()
		{
			return new ResolveResult[]
			{
				this.Array
			}.Concat(this.Indexes);
		}

		// Token: 0x04000061 RID: 97
		public readonly ResolveResult Array;

		// Token: 0x04000062 RID: 98
		public readonly IList<ResolveResult> Indexes;
	}
}

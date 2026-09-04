using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an ambiguous type resolve result.
	/// </summary>
	// Token: 0x02000035 RID: 53
	public class AmbiguousTypeResolveResult : TypeResolveResult
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00005AB4 File Offset: 0x00004AB4
		public AmbiguousTypeResolveResult(IType type) : base(type)
		{
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00005ABD File Offset: 0x00004ABD
		public override bool IsError
		{
			get
			{
				return true;
			}
		}
	}
}

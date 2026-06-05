using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the 'this' reference.
	/// Also used for the 'base' reference.
	/// </summary>
	// Token: 0x0200004C RID: 76
	public class ThisResolveResult : ResolveResult
	{
		// Token: 0x0600023D RID: 573 RVA: 0x000069DE File Offset: 0x000059DE
		public ThisResolveResult(IType type, bool causesNonVirtualInvocation = false) : base(type)
		{
			this.causesNonVirtualInvocation = causesNonVirtualInvocation;
		}

		/// <summary>
		/// Gets whether this resolve result causes member invocations to be non-virtual.
		/// </summary>
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600023E RID: 574 RVA: 0x000069EE File Offset: 0x000059EE
		public bool CausesNonVirtualInvocation
		{
			get
			{
				return this.causesNonVirtualInvocation;
			}
		}

		// Token: 0x040000A5 RID: 165
		private bool causesNonVirtualInvocation;
	}
}

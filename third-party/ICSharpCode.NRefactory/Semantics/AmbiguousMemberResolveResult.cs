using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an ambiguous field/property/event access.
	/// </summary>
	// Token: 0x02000037 RID: 55
	public class AmbiguousMemberResolveResult : MemberResolveResult
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00005CD4 File Offset: 0x00004CD4
		public AmbiguousMemberResolveResult(ResolveResult targetResult, IMember member) : base(targetResult, member, null)
		{
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00005CDF File Offset: 0x00004CDF
		public override bool IsError
		{
			get
			{
				return true;
			}
		}
	}
}

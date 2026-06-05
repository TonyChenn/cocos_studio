using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x020000ED RID: 237
	[Flags]
	public enum GetMemberOptions
	{
		/// <summary>
		/// No options specified - this is the default.
		/// Members will be specialized, and inherited members will be included.
		/// </summary>
		// Token: 0x0400027A RID: 634
		None = 0,
		/// <summary>
		/// Do not specialize the returned members - directly return the definitions.
		/// </summary>
		// Token: 0x0400027B RID: 635
		ReturnMemberDefinitions = 1,
		/// <summary>
		/// Do not list inherited members - only list members defined directly on this type.
		/// </summary>
		// Token: 0x0400027C RID: 636
		IgnoreInheritedMembers = 2
	}
}

using System;

namespace ICSharpCode.NRefactory.Analysis
{
	/// <summary>
	/// Used to check the compatibility state of two compilations.
	/// </summary>
	// Token: 0x02000002 RID: 2
	public enum AbiCompatibility
	{
		/// <summary>
		/// The ABI is equal
		/// </summary>
		// Token: 0x04000002 RID: 2
		Equal,
		/// <summary>
		/// Some items got added, but the ABI remains to be compatible
		/// </summary>
		// Token: 0x04000003 RID: 3
		Bigger,
		/// <summary>
		/// The ABI has changed
		/// </summary>
		// Token: 0x04000004 RID: 4
		Incompatible
	}
}

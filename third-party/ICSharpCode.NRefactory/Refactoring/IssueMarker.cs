using System;

namespace ICSharpCode.NRefactory.Refactoring
{
	/// <summary>
	/// The issue marker is used to set how an issue should be marked inside the text editor.
	/// </summary>
	// Token: 0x0200013A RID: 314
	public enum IssueMarker
	{
		/// <summary>
		/// The issue is not shown inside the text editor. (But in the task bar)
		/// </summary>
		// Token: 0x040003A9 RID: 937
		None,
		/// <summary>
		/// The region is marked as underline in the severity color.
		/// </summary>
		// Token: 0x040003AA RID: 938
		WavedLine,
		/// <summary>
		/// The region is marked as dotted line in the severity color.
		/// </summary>
		// Token: 0x040003AB RID: 939
		DottedLine,
		/// <summary>
		/// The text is grayed out.
		/// </summary>
		// Token: 0x040003AC RID: 940
		GrayOut
	}
}

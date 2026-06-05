using System;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x020001F8 RID: 504
	public interface IEditableTextFile : ITextFile
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600132C RID: 4908
		// (set) Token: 0x0600132D RID: 4909
		string Text { get; set; }

		/// <returns>
		/// The length of the inserted text. The real text may differ in lenth because
		/// of some conversions (tabs -&gt; spaces, different line ends etc.)
		/// </returns>
		// Token: 0x0600132E RID: 4910
		int InsertText(int position, string text);

		// Token: 0x0600132F RID: 4911
		void DeleteText(int position, int length);
	}
}

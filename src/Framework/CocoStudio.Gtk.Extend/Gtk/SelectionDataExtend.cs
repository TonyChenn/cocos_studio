using System;
using System.Linq;
using System.Text;
using Gdk;

namespace Gtk
{
	// Token: 0x0200007B RID: 123
	public static class SelectionDataExtend
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000AD28 File Offset: 0x00008F28
		public static FileDropInfo GetFileArray(this SelectionData selectionData)
		{
			string @string = Encoding.UTF8.GetString(selectionData.Data);
			string[] filearray = (from u in Encoding.UTF8.GetString(selectionData.Data).Split(new string[]
			{
				"\r",
				"\0",
				"\0\n",
				"\n"
			}, StringSplitOptions.None)
			where !string.IsNullOrEmpty(u)
			select new Uri(u) into url
			select url.LocalPath).ToArray<string>();
			return new FileDropInfo(filearray);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000AE04 File Offset: 0x00009004
		public static Color? GetCurrentColor(this SelectionData selectionData)
		{
			Color? result;
			if (selectionData.Data.Length != 8)
			{
				result = null;
			}
			else
			{
				byte r = selectionData.Data[1];
				byte g = selectionData.Data[3];
				byte b = selectionData.Data[5];
				Color value = new Color(r, g, b);
				result = new Color?(value);
			}
			return result;
		}
	}
}

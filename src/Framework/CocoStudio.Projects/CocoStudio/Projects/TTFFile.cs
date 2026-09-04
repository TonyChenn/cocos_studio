using System;
using CocoStudio.Projects.Formates;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x0200004B RID: 75
	[DataItem(Name = "TTF")]
	public class TTFFile : ResourceFile
	{
		// Token: 0x0600021A RID: 538 RVA: 0x000084BA File Offset: 0x000066BA
		public TTFFile(FilePath fileName) : base(fileName)
		{
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000084C3 File Offset: 0x000066C3
		protected TTFFile()
		{
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000084CC File Offset: 0x000066CC
		protected override DataError OnCheckDataError()
		{
			DataError dataError = base.OnCheckDataError();
			if (dataError == null && !TTFFileFormat.CheckDataIsValid(this.FileName) && !TTCFileFormat.CheckDataIsValid(this.FileName))
			{
				dataError = new DataError(LanguageInfo.DataError12_FontIsBroke);
			}
			return dataError;
		}
	}
}

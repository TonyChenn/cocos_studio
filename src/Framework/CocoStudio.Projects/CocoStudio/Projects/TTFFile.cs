using System;
using CocoStudio.Projects.Formates;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem(Name = "TTF")]
	public class TTFFile : ResourceFile
	{
		public TTFFile(FilePath fileName) : base(fileName)
		{
		}

		protected TTFFile()
		{
		}

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

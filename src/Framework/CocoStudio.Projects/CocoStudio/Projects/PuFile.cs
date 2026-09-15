using System;
using System.Linq;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem(Name = "Pu")]
	public class PuFile : CompositeResourceFile
	{
		protected override bool IsDeleteComposite
		{
			get
			{
				return false;
			}
		}

		private PuFile()
		{
		}

		public PuFile(FilePath fileName) : base(fileName)
		{
		}

		public PuFile(ResourceData resourceData) : base(resourceData)
		{
		}

		internal override string PreviewImagePath
		{
			get
			{
				return this.FullPath;
			}
		}

		protected override ResourceData CreateResourceData(FilePath filePath)
		{
			return base.CreateResourceData(filePath);
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			base.OnDelete(monitor);
		}

		protected override DataError OnCheckDataError()
		{
			if (this.imageFiles != null)
			{
				this.imageFiles = (from n in this.imageFiles
				where !((FilePath)n).IsDirectory
				select n).ToList<string>();
			}
			DataError dataError = base.OnCheckDataError();
			if (dataError == null && !CSCocosHelp.CheckParticle3DFile(this.FullPath))
			{
				return new DataError(LanguageInfo.DataError14_Particle3DIsBroke);
			}
			return dataError;
		}
	}
}

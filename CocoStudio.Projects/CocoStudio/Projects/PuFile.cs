using System;
using System.Linq;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000057 RID: 87
	[DataItem(Name = "Pu")]
	public class PuFile : CompositeResourceFile
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00009420 File Offset: 0x00007620
		protected override bool IsDeleteComposite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009423 File Offset: 0x00007623
		private PuFile()
		{
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000942B File Offset: 0x0000762B
		public PuFile(FilePath fileName) : base(fileName)
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009434 File Offset: 0x00007634
		public PuFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000943D File Offset: 0x0000763D
		internal override string PreviewImagePath
		{
			get
			{
				return this.FullPath;
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009445 File Offset: 0x00007645
		protected override ResourceData CreateResourceData(FilePath filePath)
		{
			return base.CreateResourceData(filePath);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000944E File Offset: 0x0000764E
		protected override void OnRefresh()
		{
			base.OnRefresh();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009456 File Offset: 0x00007656
		protected override void OnDelete(IProgressMonitor monitor)
		{
			base.OnDelete(monitor);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009480 File Offset: 0x00007680
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

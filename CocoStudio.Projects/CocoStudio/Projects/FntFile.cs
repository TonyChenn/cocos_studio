using System;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000053 RID: 83
	[DataItem(Name = "Fnt")]
	public class FntFile : CompositeResourceFile
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00009074 File Offset: 0x00007274
		internal override string PreviewImagePath
		{
			get
			{
				if (base.IsDefault)
				{
					return Path.Combine(this.FileName.ParentDirectory, this.FileName.FileNameWithoutExtension + ".png");
				}
				if (this.imageFiles != null && this.imageFiles.Count > 0)
				{
					return this.imageFiles.FirstOrDefault<string>();
				}
				return string.Empty;
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x000090E3 File Offset: 0x000072E3
		private FntFile()
		{
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000090EB File Offset: 0x000072EB
		public FntFile(FilePath file) : base(file)
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000090F4 File Offset: 0x000072F4
		public FntFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009100 File Offset: 0x00007300
		protected override void OnMove(FilePath newMovePath)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(newMovePath);
				FilePath filePath = this.PreviewImagePath;
				string dstFile = Path.Combine(directoryName, filePath.FileName);
				FileService.MoveFile(filePath, dstFile);
				this.previewImagePath = dstFile;
			}
			catch (Exception ex)
			{
				LogConfig.Output.Error(ex.Message, ex);
			}
			base.OnMove(newMovePath);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009174 File Offset: 0x00007374
		protected override ICompositeResourceProcesser GetCompositeResourceProcesser()
		{
			if (FntFile.pairResourceProcesser == null)
			{
				FntFile.pairResourceProcesser = ProjectsService.Instance.GetCompositeResourceProcesser(this.FileName);
			}
			return FntFile.pairResourceProcesser;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000919C File Offset: 0x0000739C
		protected override DataError OnCheckDataError()
		{
			string text;
			this.imageFiles = FntFileFormat.ParseAndCheckFntFile(base.FullPath, out text);
			if (text != null)
			{
				return new DataError(text);
			}
			return base.OnCheckDataError();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000091CC File Offset: 0x000073CC
		protected override void OnRefresh()
		{
			if (File.Exists(this.FullPath))
			{
				CSCocosHelp.ReloadFntFileToCache(this.GetResourceData().Path);
			}
			base.OnRefresh();
		}

		// Token: 0x04000098 RID: 152
		public const string FileSuffix = ".fnt";

		// Token: 0x04000099 RID: 153
		private string previewImagePath;

		// Token: 0x0400009A RID: 154
		private static ICompositeResourceProcesser pairResourceProcesser;
	}
}

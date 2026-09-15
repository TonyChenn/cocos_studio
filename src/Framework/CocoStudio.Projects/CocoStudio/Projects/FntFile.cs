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
	[DataItem(Name = "Fnt")]
	public class FntFile : CompositeResourceFile
	{
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

		private FntFile()
		{
		}

		public FntFile(FilePath file) : base(file)
		{
		}

		public FntFile(ResourceData resourceData) : base(resourceData)
		{
		}

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

		protected override ICompositeResourceProcesser GetCompositeResourceProcesser()
		{
			if (FntFile.pairResourceProcesser == null)
			{
				FntFile.pairResourceProcesser = ProjectsService.Instance.GetCompositeResourceProcesser(this.FileName);
			}
			return FntFile.pairResourceProcesser;
		}

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

		protected override void OnRefresh()
		{
			if (File.Exists(this.FullPath))
			{
				CSCocosHelp.ReloadFntFileToCache(this.GetResourceData().Path);
			}
			base.OnRefresh();
		}

		public const string FileSuffix = ".fnt";

		private string previewImagePath;

		private static ICompositeResourceProcesser pairResourceProcesser;
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public class TmxFile : CompositeResourceFile
	{
		internal override string PreviewImagePath
		{
			get
			{
				if (this.imageFiles != null)
				{
					return this.imageFiles.FirstOrDefault<string>();
				}
				return null;
			}
		}

		private TmxFile()
		{
		}

		public TmxFile(FilePath filePath) : base(filePath)
		{
		}

		public TmxFile(ResourceData resourceData) : base(resourceData)
		{
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename)
		{
			if (this.imageFiles != null)
			{
				try
				{
					foreach (string text in this.imageFiles)
					{
						Path.GetFileName(text);
						FilePath filePath = text;
						FilePath filePath2 = filePath.ToRelative(this.FileName.ParentDirectory).ToAbsolute(newFilePath.ParentDirectory);
						string path = filePath2.ParentDirectory;
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						if (File.Exists(filePath))
						{
							if (File.Exists(filePath2))
							{
								filePath2.Delete();
							}
							FileService.RenameFile(text, filePath2);
						}
					}
				}
				catch (Exception ex)
				{
					LogConfig.Output.Error(ex.Message, ex);
				}
			}
			base.OnSetLocation(newFilePath, isRename);
			this.imageFiles = base.GetImageFiles();
		}

		protected override void OnMove(FilePath newMovePath)
		{
			if (this.imageFiles != null)
			{
				try
				{
					foreach (string text in this.imageFiles)
					{
						Path.GetFileName(text);
						FilePath filePath = text;
						FilePath filePath2 = filePath.ToRelative(this.FileName.ParentDirectory).ToAbsolute(newMovePath.ParentDirectory);
						string path = filePath2.ParentDirectory;
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						if (File.Exists(filePath))
						{
							if (File.Exists(filePath2))
							{
								filePath2.Delete();
							}
							FileService.MoveFile(text, filePath2);
						}
					}
				}
				catch (Exception ex)
				{
					LogConfig.Output.Error(ex.Message, ex);
				}
			}
			base.OnMove(newMovePath);
			this.imageFiles = base.GetImageFiles();
		}

		protected override DataError OnCheckDataError()
		{
			DataError dataError = base.OnCheckDataError();
			if (dataError == null)
			{
				this.imageFiles = base.GetImageFiles();
				if (!this.CheckFilesExists(this.imageFiles))
				{
					string message = "Tmx配对文件缺失";
					return new DataError(message);
				}
			}
			return dataError;
		}

		private bool CheckFilesExists(IEnumerable<string> files)
		{
			if (files == null)
			{
				return false;
			}
			foreach (string path in files)
			{
				if (!File.Exists(path))
				{
					return false;
				}
			}
			return true;
		}

		protected override void OnRefresh()
		{
			if (this.imageFiles != null)
			{
				foreach (string filePath in base.ImageFiles)
				{
					CSCocosHelp.RemovePngFileFromCache(filePath);
				}
			}
			base.OnRefresh();
		}

		public const string FileSuffix = ".tmx";
	}
}

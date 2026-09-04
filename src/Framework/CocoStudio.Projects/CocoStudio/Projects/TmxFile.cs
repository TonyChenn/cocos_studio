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
	// Token: 0x02000058 RID: 88
	public class TmxFile : CompositeResourceFile
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600026E RID: 622 RVA: 0x000094EB File Offset: 0x000076EB
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

		// Token: 0x0600026F RID: 623 RVA: 0x00009502 File Offset: 0x00007702
		private TmxFile()
		{
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000950A File Offset: 0x0000770A
		public TmxFile(FilePath filePath) : base(filePath)
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009513 File Offset: 0x00007713
		public TmxFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000951C File Offset: 0x0000771C
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

		// Token: 0x06000273 RID: 627 RVA: 0x00009634 File Offset: 0x00007834
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

		// Token: 0x06000274 RID: 628 RVA: 0x0000974C File Offset: 0x0000794C
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

		// Token: 0x06000275 RID: 629 RVA: 0x0000978C File Offset: 0x0000798C
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

		// Token: 0x06000276 RID: 630 RVA: 0x000097E4 File Offset: 0x000079E4
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

		// Token: 0x0400009D RID: 157
		public const string FileSuffix = ".tmx";
	}
}

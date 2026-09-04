using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x0200004A RID: 74
	public class Sprite3DFile : CompositeResourceFile, IInitialize
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00007F2F File Offset: 0x0000612F
		protected override bool IsDeleteComposite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00007F32 File Offset: 0x00006132
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00007F3A File Offset: 0x0000613A
		public List<ImageFile> CompositeFiles
		{
			get
			{
				return this.compositeFiles;
			}
			set
			{
				this.compositeFiles = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00007F43 File Offset: 0x00006143
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

		// Token: 0x0600020A RID: 522 RVA: 0x00007F5A File Offset: 0x0000615A
		private Sprite3DFile()
		{
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00007F6D File Offset: 0x0000616D
		public Sprite3DFile(FilePath filePath) : base(filePath)
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00007F81 File Offset: 0x00006181
		public Sprite3DFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00007F98 File Offset: 0x00006198
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

		// Token: 0x0600020E RID: 526 RVA: 0x000080B0 File Offset: 0x000062B0
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

		// Token: 0x0600020F RID: 527 RVA: 0x000081C8 File Offset: 0x000063C8
		protected override DataError OnCheckDataError()
		{
			DataError dataError = base.OnCheckDataError();
			if (dataError == null)
			{
				if (!CSCocosHelp.CheckSprite3DFile(this.FullPath))
				{
					return new DataError(LanguageInfo.DataError13_Sprite3DIsBroke);
				}
				this.imageFiles = base.GetImageFiles();
				if (!this.CheckFilesExists(this.imageFiles))
				{
					return new DataError(LanguageInfo.DataError1_PngNotExist);
				}
			}
			return dataError;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008220 File Offset: 0x00006420
		private bool CheckFilesExists(IEnumerable<string> files)
		{
			if (files == null)
			{
				return true;
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008278 File Offset: 0x00006478
		public bool IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000827C File Offset: 0x0000647C
		public void Initialize(IProgressMonitor monitor)
		{
			if (this.imageFiles == null)
			{
				this.imageFiles = base.GetImageFiles();
			}
			if (this.imageFiles != null)
			{
				foreach (string filePath in this.imageFiles)
				{
					ImageFile imageFile = ProjectsService.Instance.CurrentResourceGroup.FindResourceItem(filePath) as ImageFile;
					if (imageFile != null)
					{
						this.compositeFiles.Add(imageFile);
						this.RegisterImageEvent(imageFile);
					}
				}
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00008310 File Offset: 0x00006510
		private void RegisterImageEvent(ImageFile image)
		{
			image.ContentChanged += this.image_ContentChanged;
			image.FileMoved += this.image_ContentChanged;
			image.Deleted += this.image_ContentChanged;
			image.NameChanged += this.image_ContentChanged;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008368 File Offset: 0x00006568
		protected override void OnDelete(IProgressMonitor monitor)
		{
			if (this.compositeFiles != null)
			{
				foreach (ImageFile image in this.compositeFiles)
				{
					this.UnRegisterImageEvent(image);
				}
			}
			this.compositeFiles.Clear();
			base.OnDelete(monitor);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000083D8 File Offset: 0x000065D8
		private void UnRegisterImageEvent(ImageFile image)
		{
			image.ContentChanged -= this.image_ContentChanged;
			image.FileMoved -= this.image_ContentChanged;
			image.Deleted -= this.image_ContentChanged;
			image.NameChanged -= this.image_ContentChanged;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000842D File Offset: 0x0000662D
		private void image_ContentChanged(object sender, EventArgs e)
		{
			this.isReload = true;
			this.dataError = this.OnCheckDataError();
			this.OnContentChanged();
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008448 File Offset: 0x00006648
		protected internal override bool IsNeedRefresh()
		{
			bool flag = base.IsNeedRefresh();
			if (!flag)
			{
				flag = (this.dataError != null);
			}
			return flag;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000846D File Offset: 0x0000666D
		protected override void OnRefresh()
		{
			base.OnRefresh();
			this.Initialize(ProjectsService.Instance.DefaultMonitor);
		}

		// Token: 0x04000081 RID: 129
		public static readonly string[] FileSuffix = new string[]
		{
			".c3b",
			".c3t",
			".obj"
		};

		// Token: 0x04000082 RID: 130
		public bool isReload;

		// Token: 0x04000083 RID: 131
		private List<ImageFile> compositeFiles = new List<ImageFile>();
	}
}

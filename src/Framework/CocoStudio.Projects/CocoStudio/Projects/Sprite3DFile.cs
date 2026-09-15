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
	public class Sprite3DFile : CompositeResourceFile, IInitialize
	{
		protected override bool IsDeleteComposite
		{
			get
			{
				return false;
			}
		}

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

		private Sprite3DFile()
		{
		}

		public Sprite3DFile(FilePath filePath) : base(filePath)
		{
		}

		public Sprite3DFile(ResourceData resourceData) : base(resourceData)
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

		public bool IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

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

		private void RegisterImageEvent(ImageFile image)
		{
			image.ContentChanged += this.image_ContentChanged;
			image.FileMoved += this.image_ContentChanged;
			image.Deleted += this.image_ContentChanged;
			image.NameChanged += this.image_ContentChanged;
		}

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

		private void UnRegisterImageEvent(ImageFile image)
		{
			image.ContentChanged -= this.image_ContentChanged;
			image.FileMoved -= this.image_ContentChanged;
			image.Deleted -= this.image_ContentChanged;
			image.NameChanged -= this.image_ContentChanged;
		}

		private void image_ContentChanged(object sender, EventArgs e)
		{
			this.isReload = true;
			this.dataError = this.OnCheckDataError();
			this.OnContentChanged();
		}

		protected internal override bool IsNeedRefresh()
		{
			bool flag = base.IsNeedRefresh();
			if (!flag)
			{
				flag = (this.dataError != null);
			}
			return flag;
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();
			this.Initialize(ProjectsService.Instance.DefaultMonitor);
		}

		public static readonly string[] FileSuffix = new string[]
		{
			".c3b",
			".c3t",
			".obj"
		};

		public bool isReload;

		private List<ImageFile> compositeFiles = new List<ImageFile>();
	}
}

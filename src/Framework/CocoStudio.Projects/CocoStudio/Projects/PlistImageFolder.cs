using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Packer;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace CocoStudio.Projects
{
	public class PlistImageFolder : ResourceFolder
	{
		public override string Name
		{
			get
			{
				return this.plistFileInfo.FileName;
			}
			protected set
			{
				throw new InvalidOperationException("Can't set Name.");
			}
		}

		public override string FullPath
		{
			get
			{
				return this.plistFileInfo;
			}
		}

		internal override string PreviewImagePath
		{
			get
			{
				return this.imageFile;
			}
		}

		private PlistImageFolder()
		{
		}

		public PlistImageFolder(FilePath filePath) : base(filePath)
		{
			this.plistFileInfo = filePath;
			base.BaseDirectory = this.InitPlistDir(this.plistFileInfo);
		}

		protected override DataError OnCheckDataError()
		{
			if (!File.Exists(this.imageFile) || !File.Exists(this.plistFileInfo))
			{
				string message = "找不到对应的plist或png文件";
				return new DataError(message);
			}
			ICompositeResourceProcesser compositeResourceProcesser = ProjectsService.Instance.GetCompositeResourceProcesser(this.plistFileInfo);
			List<string> files = compositeResourceProcesser.GetFiles(this.plistFileInfo);
			if (files != null)
			{
				foreach (string text in files)
				{
					if (!File.Exists(text))
					{
						string message2 = string.Format("{0}不存在", text);
						return new DataError(message2);
					}
				}
			}
			return null;
		}

		private string InitPlistDir(FilePath fileInfo)
		{
			FilePath plistDirPath = PlistImageFolder.GetPListDirPath(this.plistFileInfo);
			DirectoryInfo directoryInfo = new DirectoryInfo(plistDirPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
				directoryInfo.Attributes = FileAttributes.Hidden;
			}
			return plistDirPath;
		}

		private PListImageReader AnalyzePlist(FilePath fileInfo)
		{
			PListImageReader result;
			try
			{
				lock (this.lockTag)
				{
					PListImageReader plistImageReader = new PListImageReader(this.plistFileInfo);
					plistImageReader.SaveAllSubImage(base.BaseDirectory);
					result = plistImageReader;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error(LanguageInfo.MessageBox_Content64, exception);
				result = null;
			}
			return result;
		}

		private static FilePath GetPListDirPath(FilePath plistFile)
		{
			string path = plistFile.ParentDirectory;
			return Path.Combine(path, "." + plistFile.FileNameWithoutExtension + "_PList.Dir");
		}

		private PListImageReader GetPlistImageReader(FilePath plistInfo)
		{
			PListImageReader plistImageReader = this.AnalyzePlist(plistInfo);
			if (plistImageReader == null)
			{
				if (Directory.Exists(base.BaseDirectory))
				{
					Directory.Delete(base.BaseDirectory, true);
				}
				return null;
			}
			return plistImageReader;
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			CSCocosHelp.RemovePlistFileFromCache(this.plistFileInfo);
			this.DeleteFile(this.imageFile);
			this.DeleteFile(base.BaseDirectory);
			this.DeleteFile(this.plistFileInfo);
			base.OnDelete(monitor);
		}

		protected override void OnInitialize(IProgressMonitor monitor)
		{
			if (string.IsNullOrEmpty(this.imageFile))
			{
				this.imageFile = PListImageReader.GetMatchImageFile(this.plistFileInfo);
			}
			this.ReadConfig();
			base.Refresh();
		}

		protected internal override bool IsNeedRefresh()
		{
			bool flag = false;
			if (!File.GetLastWriteTime(this.plistFileInfo).Equals(this.lastPlistWriteTime))
			{
				flag = true;
			}
			if (!flag && string.IsNullOrEmpty(this.imageFile))
			{
				this.imageFile = PListImageReader.GetMatchImageFile(this.plistFileInfo);
			}
			if (!flag && !string.IsNullOrEmpty(this.imageFile) && !File.GetLastWriteTime(this.imageFile).Equals(this.lastImageWriteTime))
			{
				flag = true;
			}
			return flag;
		}

		private void DeleteFile(FilePath filePath)
		{
			if (File.Exists(filePath))
			{
				DesktopService.PlatformService.DeleteToTrash(filePath.FullPath);
			}
		}

		private void SaveConfig()
		{
			try
			{
				if (Directory.Exists(base.BaseDirectory))
				{
					string fileName = base.BaseDirectory.Combine(new string[]
					{
						"InfoConfig.xml"
					});
					XElement xelement = new XElement("Configuration", new object[]
					{
						new XElement("LastPlistWriteTime", this.lastPlistWriteTime),
						new XElement("LastImageWriteTime", this.lastImageWriteTime)
					});
					xelement.Save(fileName);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("Save{0} config file failure!", this.FullPath), exception);
			}
		}

		private void ReadConfig()
		{
			try
			{
				string fileName = base.BaseDirectory.Combine(new string[]
				{
					"InfoConfig.xml"
				});
				FileInfo fileInfo = new FileInfo(fileName);
				if (fileInfo.Exists)
				{
					using (FileStream fileStream = fileInfo.OpenRead())
					{
						XElement xelement = XElement.Load(fileStream);
						XElement xelement2 = xelement.Element("LastPlistWriteTime");
						XElement xelement3 = xelement.Element("LastImageWriteTime");
						DateTime value;
						if (DateTime.TryParse(xelement2.Value, out value))
						{
							this.lastPlistWriteTime = new DateTime?(value);
						}
						if (DateTime.TryParse(xelement3.Value, out value))
						{
							this.lastImageWriteTime = new DateTime?(value);
						}
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("Read {0} config file failure!", this.FullPath), exception);
			}
		}

		protected override void OnRefresh()
		{
			bool flag = this.DataError == null;
			if (flag)
			{
				CSCocosHelp.ReloadPlistFileThreadSafe(this.FullPath);
				this.lastPlistWriteTime = new DateTime?(File.GetLastWriteTime(this.plistFileInfo));
				this.lastImageWriteTime = new DateTime?(File.GetLastWriteTime(this.imageFile));
				this.ReloadChildren();
			}
			else
			{
				for (int i = base.Items.Count - 1; i >= 0; i--)
				{
					base.Items[i].Delete(ProjectsService.Instance.DefaultMonitor);
				}
				this.lastPlistWriteTime = new DateTime?(DateTime.MinValue);
				this.lastImageWriteTime = new DateTime?(DateTime.MinValue);
			}
			this.imageFile = PListImageReader.GetMatchImageFile(this.plistFileInfo);
			this.UpdateChildrenCacheImage();
			this.SaveConfig();
		}

		public string GetPreviewFilePath()
		{
			if (!(this.imageFile != null))
			{
				return string.Empty;
			}
			return this.imageFile;
		}

		private void ReloadChildren()
		{
			PListImageReader plistImageReader = this.GetPlistImageReader(this.plistFileInfo);
			if (plistImageReader == null)
			{
				this.dataError = new DataError("不支持的Plist文件格式.");
				return;
			}
			string path = base.BaseDirectory;
			if (!Directory.Exists(base.BaseDirectory))
			{
				return;
			}
			List<ResourceItem> list = new List<ResourceItem>();
			foreach (ResourceItem resourceItem in base.Items)
			{
				bool flag = false;
				foreach (ImageInfo imageInfo in plistImageReader.ImageList)
				{
					string b = Path.Combine(path, imageInfo.FileName);
					if (resourceItem.PreviewImagePath == b)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(resourceItem);
				}
			}
			foreach (ResourceItem resourceItem2 in list)
			{
				resourceItem2.Delete(ProjectsService.Instance.DefaultMonitor);
				base.Items.Remove(resourceItem2);
			}
			List<ImageInfo> list2 = new List<ImageInfo>();
			foreach (ImageInfo item in plistImageReader.ImageList)
			{
				bool flag2 = false;
				string b2 = Path.Combine(path, item.FileName);
				foreach (ResourceItem resourceItem3 in base.Items)
				{
					if (resourceItem3.PreviewImagePath == b2)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					list2.Add(item);
				}
			}
			foreach (ImageInfo imageInfo2 in list2)
			{
				FilePath info = Path.Combine(path, imageInfo2.FileName);
				base.Items.Add(new PlistImageFile(info, imageInfo2.Name, this));
			}
		}

		public override ResourceData GetResourceData()
		{
			return this.CreateResourceData(this.plistFileInfo);
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename)
		{
			string text = base.BaseDirectory;
			if (newFilePath != this.plistFileInfo)
			{
				FilePath filePath = Path.GetDirectoryName(newFilePath);
				FilePath filePath2 = Path.GetFileName(base.BaseDirectory);
				text = Path.Combine(filePath, filePath2);
				FilePath filePath3 = Path.GetFileName(this.imageFile);
				FilePath filePath4 = Path.Combine(filePath, filePath3);
				FilePath filePath5 = filePath.Combine(new string[]
				{
					this.plistFileInfo.FileName
				});
				base.OnSetLocation(text, isRename);
				this.plistFileInfo = filePath5;
				this.imageFile = filePath4;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			directoryInfo.Attributes = FileAttributes.Hidden;
			this.OnRefresh();
		}

		protected override void OnMove(FilePath newMovePath)
		{
			string text = base.BaseDirectory;
			if (newMovePath != this.plistFileInfo)
			{
				FilePath filePath = Path.GetDirectoryName(newMovePath);
				FilePath filePath2 = Path.GetFileName(base.BaseDirectory);
				text = Path.Combine(filePath, filePath2);
				FilePath filePath3 = Path.GetFileName(this.imageFile);
				FilePath filePath4 = Path.Combine(filePath, filePath3);
				FilePath filePath5 = filePath.Combine(new string[]
				{
					this.plistFileInfo.FileName
				});
				try
				{
					if (!Directory.Exists(filePath))
					{
						Directory.CreateDirectory(filePath);
					}
					if (File.Exists(this.imageFile))
					{
						FileService.MoveFile(this.imageFile, filePath4);
					}
					if (File.Exists(this.plistFileInfo))
					{
						FileService.MoveFile(this.plistFileInfo, filePath5);
					}
				}
				catch (Exception ex)
				{
					LogConfig.Output.Error(ex.Message, ex);
				}
				base.OnMove(text);
				this.plistFileInfo = filePath5;
				this.imageFile = filePath4;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			directoryInfo.Attributes = FileAttributes.Hidden;
			this.OnRefresh();
		}

		private void UpdateChildrenCacheImage()
		{
			foreach (ResourceItem image in base.Items)
			{
				ProjectsService.Instance.PreviewImageService.UpdateCachedImage(image);
			}
		}

		public const string PlistFileDirExtention = "_PList.Dir";

		public const string FileSuffix = ".plist";

		private object lockTag = new object();

		[ResourcePathItemProperty(Name = "Image")]
		private FilePath imageFile;

		[ResourcePathItemProperty(Name = "PListFile")]
		private FilePath plistFileInfo;

		protected DateTime? lastPlistWriteTime;

		protected DateTime? lastImageWriteTime;
	}
}

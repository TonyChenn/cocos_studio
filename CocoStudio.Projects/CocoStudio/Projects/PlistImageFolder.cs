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
	// Token: 0x0200005B RID: 91
	public class PlistImageFolder : ResourceFolder
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000287 RID: 647 RVA: 0x000099F1 File Offset: 0x00007BF1
		// (set) Token: 0x06000288 RID: 648 RVA: 0x000099FE File Offset: 0x00007BFE
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00009A0A File Offset: 0x00007C0A
		public override string FullPath
		{
			get
			{
				return this.plistFileInfo;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00009A17 File Offset: 0x00007C17
		internal override string PreviewImagePath
		{
			get
			{
				return this.imageFile;
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00009A24 File Offset: 0x00007C24
		private PlistImageFolder()
		{
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00009A37 File Offset: 0x00007C37
		public PlistImageFolder(FilePath filePath) : base(filePath)
		{
			this.plistFileInfo = filePath;
			base.BaseDirectory = this.InitPlistDir(this.plistFileInfo);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00009A6C File Offset: 0x00007C6C
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

		// Token: 0x0600028E RID: 654 RVA: 0x00009B34 File Offset: 0x00007D34
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

		// Token: 0x0600028F RID: 655 RVA: 0x00009B74 File Offset: 0x00007D74
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

		// Token: 0x06000290 RID: 656 RVA: 0x00009BF8 File Offset: 0x00007DF8
		private static FilePath GetPListDirPath(FilePath plistFile)
		{
			string path = plistFile.ParentDirectory;
			return Path.Combine(path, "." + plistFile.FileNameWithoutExtension + "_PList.Dir");
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00009C34 File Offset: 0x00007E34
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

		// Token: 0x06000292 RID: 658 RVA: 0x00009C72 File Offset: 0x00007E72
		protected override void OnDelete(IProgressMonitor monitor)
		{
			CSCocosHelp.RemovePlistFileFromCache(this.plistFileInfo);
			this.DeleteFile(this.imageFile);
			this.DeleteFile(base.BaseDirectory);
			this.DeleteFile(this.plistFileInfo);
			base.OnDelete(monitor);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00009CAF File Offset: 0x00007EAF
		protected override void OnInitialize(IProgressMonitor monitor)
		{
			if (string.IsNullOrEmpty(this.imageFile))
			{
				this.imageFile = PListImageReader.GetMatchImageFile(this.plistFileInfo);
			}
			this.ReadConfig();
			base.Refresh();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00009CEC File Offset: 0x00007EEC
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

		// Token: 0x06000295 RID: 661 RVA: 0x00009D9B File Offset: 0x00007F9B
		private void DeleteFile(FilePath filePath)
		{
			if (File.Exists(filePath))
			{
				DesktopService.PlatformService.DeleteToTrash(filePath.FullPath);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00009DC0 File Offset: 0x00007FC0
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

		// Token: 0x06000297 RID: 663 RVA: 0x00009E98 File Offset: 0x00008098
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

		// Token: 0x06000298 RID: 664 RVA: 0x00009F98 File Offset: 0x00008198
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

		// Token: 0x06000299 RID: 665 RVA: 0x0000A075 File Offset: 0x00008275
		public string GetPreviewFilePath()
		{
			if (!(this.imageFile != null))
			{
				return string.Empty;
			}
			return this.imageFile;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A09C File Offset: 0x0000829C
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

		// Token: 0x0600029B RID: 667 RVA: 0x0000A30C File Offset: 0x0000850C
		public override ResourceData GetResourceData()
		{
			return this.CreateResourceData(this.plistFileInfo);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000A31C File Offset: 0x0000851C
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

		// Token: 0x0600029D RID: 669 RVA: 0x0000A404 File Offset: 0x00008604
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

		// Token: 0x0600029E RID: 670 RVA: 0x0000A580 File Offset: 0x00008780
		private void UpdateChildrenCacheImage()
		{
			foreach (ResourceItem image in base.Items)
			{
				ProjectsService.Instance.PreviewImageService.UpdateCachedImage(image);
			}
		}

		// Token: 0x0400009F RID: 159
		public const string PlistFileDirExtention = "_PList.Dir";

		// Token: 0x040000A0 RID: 160
		public const string FileSuffix = ".plist";

		// Token: 0x040000A1 RID: 161
		private object lockTag = new object();

		// Token: 0x040000A2 RID: 162
		[ResourcePathItemProperty(Name = "Image")]
		private FilePath imageFile;

		// Token: 0x040000A3 RID: 163
		[ResourcePathItemProperty(Name = "PListFile")]
		private FilePath plistFileInfo;

		// Token: 0x040000A4 RID: 164
		protected DateTime? lastPlistWriteTime;

		// Token: 0x040000A5 RID: 165
		protected DateTime? lastImageWriteTime;
	}
}

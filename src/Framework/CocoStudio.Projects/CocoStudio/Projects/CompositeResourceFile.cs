using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace CocoStudio.Projects
{
	public class CompositeResourceFile : ResourceFile
	{
		public IEnumerable<string> ImageFiles
		{
			get
			{
				return this.imageFiles;
			}
		}

		protected virtual bool IsDeleteComposite
		{
			get
			{
				return true;
			}
		}

		protected bool HasImageFiles
		{
			get
			{
				return this.imageFiles != null && this.imageFiles.Count > 0;
			}
		}

		protected CompositeResourceFile()
		{
		}

		public CompositeResourceFile(FilePath filePath) : base(filePath)
		{
		}

		public CompositeResourceFile(ResourceData resourceData) : base(resourceData)
		{
		}

		protected List<string> GetImageFiles()
		{
			ICompositeResourceProcesser compositeResourceProcesser = ProjectsService.Instance.GetCompositeResourceProcesser(this.FullPath);
			if (compositeResourceProcesser != null)
			{
				return compositeResourceProcesser.GetFiles(this.FullPath);
			}
			return null;
		}

		protected override void OnMove(FilePath newMovePath)
		{
			base.OnMove(newMovePath);
			this.imageFiles = this.GetImageFiles();
		}

		protected override void OnRefresh()
		{
			if (base.IsNeedRefresh())
			{
				this.imageFiles = this.GetImageFiles();
			}
			this.compositeFiles.Clear();
			base.OnRefresh();
			this.UpdateImageFileInfo();
		}

		protected internal override bool IsNeedRefresh()
		{
			bool flag = base.IsNeedRefresh();
			if (!flag && this.HasImageFiles)
			{
				if (this.imageFiles == null)
				{
					return true;
				}
				foreach (string filePath in this.imageFiles)
				{
					if (this.IsFileChanged(filePath))
					{
						return true;
					}
				}
				return flag;
			}
			return flag;
		}

		protected void UpdateImageFileInfo()
		{
			if (this.imageFiles != null)
			{
				foreach (string text in this.imageFiles)
				{
					if (this.compositeFiles.ContainsKey(text))
					{
						this.compositeFiles[text].Refresh();
					}
					else
					{
						this.compositeFiles[text] = new CompositeResourceFile.CompositeInfo(text);
					}
				}
			}
		}

		protected bool IsFileChanged(string filePath)
		{
			CompositeResourceFile.CompositeInfo obj = new CompositeResourceFile.CompositeInfo(filePath);
			return !this.compositeFiles.ContainsKey(filePath) || !this.compositeFiles[filePath].Equals(obj);
		}

		protected override ICompositeResourceProcesser GetCompositeResourceProcesser()
		{
			return ProjectsService.Instance.GetCompositeResourceProcesser(this.FileName);
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			try
			{
				if (this.imageFiles != null && this.IsDeleteComposite)
				{
					foreach (string text in this.imageFiles)
					{
						if (File.Exists(text))
						{
							DesktopService.PlatformService.DeleteToTrash(text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				monitor.ReportError(ex.Message, ex);
				LogConfig.Output.Error(ex);
			}
			finally
			{
				base.OnDelete(monitor);
				this.compositeFiles.Clear();
			}
		}

		protected override DataError OnCheckDataError()
		{
			DataError dataError = base.OnCheckDataError();
			if (dataError != null)
			{
				return dataError;
			}
			if (this.imageFiles != null)
			{
				foreach (string name in this.imageFiles)
				{
					FilePath filePath = name;
					if (!File.Exists(filePath))
					{
						string message = string.Format(LanguageInfo.DataError7_NotExist, filePath);
						return new DataError(message);
					}
				}
			}
			return null;
		}

		protected List<string> imageFiles;

		private Dictionary<string, CompositeResourceFile.CompositeInfo> compositeFiles = new Dictionary<string, CompositeResourceFile.CompositeInfo>();

		private class CompositeInfo
		{
			public DateTime? LasterWriterTime
			{
				get
				{
					return this.lasterWriterTime;
				}
				set
				{
					this.lasterWriterTime = value;
				}
			}

			public long? FileSize
			{
				get
				{
					return this.fileSize;
				}
				set
				{
					this.fileSize = value;
				}
			}

			public CompositeInfo(string filePath)
			{
				this.lasterWriterTime = ResourceItem.GetLastWriteTime(filePath);
				this.fileSize = ResourceItem.GetFileSize(filePath);
				this.filePath = filePath;
			}

			public void Refresh()
			{
				this.lasterWriterTime = ResourceItem.GetLastWriteTime(this.filePath);
				this.fileSize = ResourceItem.GetFileSize(this.filePath);
			}

			public override bool Equals(object obj)
			{
				CompositeResourceFile.CompositeInfo compositeInfo = obj as CompositeResourceFile.CompositeInfo;
				return compositeInfo != null && compositeInfo.LasterWriterTime == this.LasterWriterTime && compositeInfo.FileSize == this.FileSize;
			}

			public override int GetHashCode()
			{
				return this.LasterWriterTime.GetHashCode() + this.FileSize.GetHashCode();
			}

			private DateTime? lasterWriterTime;

			private long? fileSize;

			private string filePath;
		}
	}
}

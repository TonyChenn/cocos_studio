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
	// Token: 0x02000041 RID: 65
	public class CompositeResourceFile : ResourceFile
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00007078 File Offset: 0x00005278
		public IEnumerable<string> ImageFiles
		{
			get
			{
				return this.imageFiles;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00007080 File Offset: 0x00005280
		protected virtual bool IsDeleteComposite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00007083 File Offset: 0x00005283
		protected bool HasImageFiles
		{
			get
			{
				return this.imageFiles != null && this.imageFiles.Count > 0;
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000709D File Offset: 0x0000529D
		protected CompositeResourceFile()
		{
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000070B0 File Offset: 0x000052B0
		public CompositeResourceFile(FilePath filePath) : base(filePath)
		{
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000070C4 File Offset: 0x000052C4
		public CompositeResourceFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000070D8 File Offset: 0x000052D8
		protected List<string> GetImageFiles()
		{
			ICompositeResourceProcesser compositeResourceProcesser = ProjectsService.Instance.GetCompositeResourceProcesser(this.FullPath);
			if (compositeResourceProcesser != null)
			{
				return compositeResourceProcesser.GetFiles(this.FullPath);
			}
			return null;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00007107 File Offset: 0x00005307
		protected override void OnMove(FilePath newMovePath)
		{
			base.OnMove(newMovePath);
			this.imageFiles = this.GetImageFiles();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000711C File Offset: 0x0000531C
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

		// Token: 0x060001C8 RID: 456 RVA: 0x0000714C File Offset: 0x0000534C
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

		// Token: 0x060001C9 RID: 457 RVA: 0x000071C4 File Offset: 0x000053C4
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

		// Token: 0x060001CA RID: 458 RVA: 0x0000724C File Offset: 0x0000544C
		protected bool IsFileChanged(string filePath)
		{
			CompositeResourceFile.CompositeInfo obj = new CompositeResourceFile.CompositeInfo(filePath);
			return !this.compositeFiles.ContainsKey(filePath) || !this.compositeFiles[filePath].Equals(obj);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00007285 File Offset: 0x00005485
		protected override ICompositeResourceProcesser GetCompositeResourceProcesser()
		{
			return ProjectsService.Instance.GetCompositeResourceProcesser(this.FileName);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000729C File Offset: 0x0000549C
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

		// Token: 0x060001CD RID: 461 RVA: 0x00007358 File Offset: 0x00005558
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

		// Token: 0x04000076 RID: 118
		protected List<string> imageFiles;

		// Token: 0x04000077 RID: 119
		private Dictionary<string, CompositeResourceFile.CompositeInfo> compositeFiles = new Dictionary<string, CompositeResourceFile.CompositeInfo>();

		// Token: 0x02000042 RID: 66
		private class CompositeInfo
		{
			// Token: 0x17000039 RID: 57
			// (get) Token: 0x060001CE RID: 462 RVA: 0x000073E8 File Offset: 0x000055E8
			// (set) Token: 0x060001CF RID: 463 RVA: 0x000073F0 File Offset: 0x000055F0
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

			// Token: 0x1700003A RID: 58
			// (get) Token: 0x060001D0 RID: 464 RVA: 0x000073F9 File Offset: 0x000055F9
			// (set) Token: 0x060001D1 RID: 465 RVA: 0x00007401 File Offset: 0x00005601
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

			// Token: 0x060001D2 RID: 466 RVA: 0x0000740A File Offset: 0x0000560A
			public CompositeInfo(string filePath)
			{
				this.lasterWriterTime = ResourceItem.GetLastWriteTime(filePath);
				this.fileSize = ResourceItem.GetFileSize(filePath);
				this.filePath = filePath;
			}

			// Token: 0x060001D3 RID: 467 RVA: 0x00007436 File Offset: 0x00005636
			public void Refresh()
			{
				this.lasterWriterTime = ResourceItem.GetLastWriteTime(this.filePath);
				this.fileSize = ResourceItem.GetFileSize(this.filePath);
			}

			// Token: 0x060001D4 RID: 468 RVA: 0x00007460 File Offset: 0x00005660
			public override bool Equals(object obj)
			{
				CompositeResourceFile.CompositeInfo compositeInfo = obj as CompositeResourceFile.CompositeInfo;
				return compositeInfo != null && compositeInfo.LasterWriterTime == this.LasterWriterTime && compositeInfo.FileSize == this.FileSize;
			}

			// Token: 0x060001D5 RID: 469 RVA: 0x000074F0 File Offset: 0x000056F0
			public override int GetHashCode()
			{
				return this.LasterWriterTime.GetHashCode() + this.FileSize.GetHashCode();
			}

			// Token: 0x04000078 RID: 120
			private DateTime? lasterWriterTime;

			// Token: 0x04000079 RID: 121
			private long? fileSize;

			// Token: 0x0400007A RID: 122
			private string filePath;
		}
	}
}

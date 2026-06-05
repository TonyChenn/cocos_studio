using System;
using System.Collections;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x0200003D RID: 61
	[DataInclude(typeof(ResourceFile))]
	[DataInclude(typeof(ResourceFolder))]
	[DataItem("Item")]
	public abstract class ResourceItem : IResource, IExtendedDataItem
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000065A0 File Offset: 0x000047A0
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000065A8 File Offset: 0x000047A8
		public virtual string Name { get; protected set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600017E RID: 382
		public abstract string FullPath { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000065B1 File Offset: 0x000047B1
		internal virtual string PreviewImagePath
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000065B8 File Offset: 0x000047B8
		public virtual PreviewImageInfo PreviewImageInfo
		{
			get
			{
				return ProjectsService.Instance.PreviewImageService.GetImage(this);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000065CA File Offset: 0x000047CA
		public string RelativePath
		{
			get
			{
				return ResourceItem.GetRelativePath(this.FullPath);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000182 RID: 386 RVA: 0x000065E1 File Offset: 0x000047E1
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.hashtable == null)
				{
					this.hashtable = new Hashtable();
				}
				return this.hashtable;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000183 RID: 387 RVA: 0x000065FC File Offset: 0x000047FC
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00006604 File Offset: 0x00004804
		public virtual ResourceItem Parent { get; internal set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006610 File Offset: 0x00004810
		public virtual DataError DataError
		{
			get
			{
				if (!this.IsNeedRefresh())
				{
					return this.dataError;
				}
				return this.dataError = this.OnCheckDataError();
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000186 RID: 390 RVA: 0x0000663C File Offset: 0x0000483C
		// (remove) Token: 0x06000187 RID: 391 RVA: 0x00006674 File Offset: 0x00004874
		public event EventHandler<EventArgs> Deleted;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000188 RID: 392 RVA: 0x000066AC File Offset: 0x000048AC
		// (remove) Token: 0x06000189 RID: 393 RVA: 0x000066E4 File Offset: 0x000048E4
		public event EventHandler<EventArgs> NameChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600018A RID: 394 RVA: 0x0000671C File Offset: 0x0000491C
		// (remove) Token: 0x0600018B RID: 395 RVA: 0x00006754 File Offset: 0x00004954
		public event EventHandler<EventArgs> FileMoved;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600018C RID: 396 RVA: 0x0000678C File Offset: 0x0000498C
		// (remove) Token: 0x0600018D RID: 397 RVA: 0x000067C4 File Offset: 0x000049C4
		public event EventHandler<EventArgs> ContentChanged;

		// Token: 0x0600018E RID: 398
		protected abstract DataError OnCheckDataError();

		// Token: 0x0600018F RID: 399
		public abstract ResourceData GetResourceData();

		// Token: 0x06000190 RID: 400 RVA: 0x000067FC File Offset: 0x000049FC
		protected virtual ResourceData CreateResourceData(FilePath filePath)
		{
			FilePath relativePath = ResourceItem.GetRelativePath(filePath);
			return new ResourceData(relativePath);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006820 File Offset: 0x00004A20
		protected static FilePath GetRelativePath(FilePath fullPath)
		{
			if (ProjectsService.Instance.CurrentSolution == null)
			{
				return null;
			}
			FilePath itemDirectory = ProjectsService.Instance.CurrentSolution.ItemDirectory;
			FilePath filePath = fullPath.ToRelative(itemDirectory);
			return Option.ConvertToMacPath(filePath);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000686B File Offset: 0x00004A6B
		protected virtual void OnNameChanged()
		{
			if (this.NameChanged != null)
			{
				this.NameChanged(this, new EventArgs());
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006886 File Offset: 0x00004A86
		protected virtual void OnContentChanged()
		{
			if (this.ContentChanged != null)
			{
				this.ContentChanged(this, new EventArgs());
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000068A1 File Offset: 0x00004AA1
		internal void Delete(IProgressMonitor monitor)
		{
			ResourceChangeService.Instance.Register(this, true, null);
			this.OnDelete(monitor);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000068B7 File Offset: 0x00004AB7
		internal void Remove(IProgressMonitor monitor)
		{
			ResourceChangeService.Instance.Register(this, true, null);
			this.OnRemove(monitor);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000068CD File Offset: 0x00004ACD
		public void SetLocation(FilePath newFilePath, bool isRename = true)
		{
			ResourceChangeService.Instance.Register(this, false, null);
			this.OnSetLocation(newFilePath, isRename);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000068E4 File Offset: 0x00004AE4
		public void Move(FilePath newMovePath)
		{
			ResourceChangeService.Instance.Register(this, false, null);
			this.OnMove(newMovePath);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000068FA File Offset: 0x00004AFA
		protected virtual void OnMove(FilePath newMovePath)
		{
			if (this.FileMoved != null)
			{
				this.FileMoved(this, new EventArgs());
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006915 File Offset: 0x00004B15
		protected virtual void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			if (this.NameChanged != null)
			{
				this.NameChanged(this, new EventArgs());
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006930 File Offset: 0x00004B30
		protected virtual void OnDelete(IProgressMonitor monitor)
		{
			if (monitor.AsyncOperation.Success && this.Deleted != null)
			{
				this.Deleted(this, null);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006954 File Offset: 0x00004B54
		protected virtual void OnRemove(IProgressMonitor monitor)
		{
			if (monitor.AsyncOperation.Success && this.Deleted != null)
			{
				this.Deleted(this, null);
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006978 File Offset: 0x00004B78
		protected internal virtual bool IsNeedRefresh()
		{
			bool result;
			try
			{
				FileInfo fileInfo = new FileInfo(this.FullPath);
				DateTime? dateTime = null;
				long? num = null;
				if (fileInfo.Exists)
				{
					dateTime = new DateTime?(fileInfo.LastWriteTime);
					num = new long?(fileInfo.Length);
				}
				if (dateTime != this.lastWriteTime || this.fileSize != num)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
				result = false;
			}
			return result;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006A64 File Offset: 0x00004C64
		public static DateTime? GetLastWriteTime(string filePath)
		{
			DateTime? result;
			try
			{
				if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
				{
					result = null;
				}
				else
				{
					result = new DateTime?(File.GetLastWriteTime(filePath));
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006AC4 File Offset: 0x00004CC4
		public static long? GetFileSize(FilePath value)
		{
			long? result;
			try
			{
				FileInfo fileInfo = new FileInfo(value);
				if (!fileInfo.Exists)
				{
					result = null;
				}
				else
				{
					result = new long?(fileInfo.Length);
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006B2C File Offset: 0x00004D2C
		protected void UpdateFileInfo(string filePath)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(filePath);
				if (fileInfo.Exists)
				{
					this.lastWriteTime = new DateTime?(fileInfo.LastWriteTime);
					this.fileSize = new long?(fileInfo.Length);
					this.dataError = this.OnCheckDataError();
				}
				else
				{
					this.lastWriteTime = null;
					this.fileSize = null;
					string message = string.Format(LanguageInfo.DataError7_NotExist, this.Name);
					this.dataError = new DataError(message);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("UpdateFileInfo failed.", exception);
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006BD4 File Offset: 0x00004DD4
		public void Refresh()
		{
			if (this.IsNeedRefresh())
			{
				this.OnRefresh();
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006BE4 File Offset: 0x00004DE4
		protected virtual void OnRefresh()
		{
			this.UpdateFileInfo(this.FullPath);
			ProjectsService.Instance.PreviewImageService.UpdateCachedImage(this);
			this.OnContentChanged();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006C08 File Offset: 0x00004E08
		public override string ToString()
		{
			return this.FullPath;
		}

		// Token: 0x04000064 RID: 100
		internal const string PathPropertyName = "Name";

		// Token: 0x04000065 RID: 101
		internal const string DataItemName = "Item";

		// Token: 0x04000066 RID: 102
		protected Hashtable hashtable;

		// Token: 0x04000067 RID: 103
		protected DataError dataError;

		// Token: 0x0400006C RID: 108
		protected DateTime? lastWriteTime = new DateTime?(DateTime.MinValue);

		// Token: 0x0400006D RID: 109
		protected long? fileSize = new long?(-1L);
	}
}

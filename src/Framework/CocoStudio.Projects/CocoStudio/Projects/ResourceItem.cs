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
	[DataInclude(typeof(ResourceFile))]
	[DataInclude(typeof(ResourceFolder))]
	[DataItem("Item")]
	public abstract class ResourceItem : IResource, IExtendedDataItem
	{
		public virtual string Name { get; protected set; }

		public abstract string FullPath { get; }

		internal virtual string PreviewImagePath
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual PreviewImageInfo PreviewImageInfo
		{
			get
			{
				return ProjectsService.Instance.PreviewImageService.GetImage(this);
			}
		}

		public string RelativePath
		{
			get
			{
				return ResourceItem.GetRelativePath(this.FullPath);
			}
		}

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

		public virtual ResourceItem Parent { get; internal set; }

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

		public event EventHandler<EventArgs> Deleted;

		public event EventHandler<EventArgs> NameChanged;

		public event EventHandler<EventArgs> FileMoved;

		public event EventHandler<EventArgs> ContentChanged;

		protected abstract DataError OnCheckDataError();

		public abstract ResourceData GetResourceData();

		protected virtual ResourceData CreateResourceData(FilePath filePath)
		{
			FilePath relativePath = ResourceItem.GetRelativePath(filePath);
			return new ResourceData(relativePath);
		}

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

		protected virtual void OnNameChanged()
		{
			if (this.NameChanged != null)
			{
				this.NameChanged(this, new EventArgs());
			}
		}

		protected virtual void OnContentChanged()
		{
			if (this.ContentChanged != null)
			{
				this.ContentChanged(this, new EventArgs());
			}
		}

		internal void Delete(IProgressMonitor monitor)
		{
			ResourceChangeService.Instance.Register(this, true, null);
			this.OnDelete(monitor);
		}

		internal void Remove(IProgressMonitor monitor)
		{
			ResourceChangeService.Instance.Register(this, true, null);
			this.OnRemove(monitor);
		}

		public void SetLocation(FilePath newFilePath, bool isRename = true)
		{
			ResourceChangeService.Instance.Register(this, false, null);
			this.OnSetLocation(newFilePath, isRename);
		}

		public void Move(FilePath newMovePath)
		{
			ResourceChangeService.Instance.Register(this, false, null);
			this.OnMove(newMovePath);
		}

		protected virtual void OnMove(FilePath newMovePath)
		{
			if (this.FileMoved != null)
			{
				this.FileMoved(this, new EventArgs());
			}
		}

		protected virtual void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			if (this.NameChanged != null)
			{
				this.NameChanged(this, new EventArgs());
			}
		}

		protected virtual void OnDelete(IProgressMonitor monitor)
		{
			if (monitor.AsyncOperation.Success && this.Deleted != null)
			{
				this.Deleted(this, null);
			}
		}

		protected virtual void OnRemove(IProgressMonitor monitor)
		{
			if (monitor.AsyncOperation.Success && this.Deleted != null)
			{
				this.Deleted(this, null);
			}
		}

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

		public void Refresh()
		{
			if (this.IsNeedRefresh())
			{
				this.OnRefresh();
			}
		}

		protected virtual void OnRefresh()
		{
			this.UpdateFileInfo(this.FullPath);
			ProjectsService.Instance.PreviewImageService.UpdateCachedImage(this);
			this.OnContentChanged();
		}

		public override string ToString()
		{
			return this.FullPath;
		}

		internal const string PathPropertyName = "Name";

		internal const string DataItemName = "Item";

		protected Hashtable hashtable;

		protected DataError dataError;

		protected DateTime? lastWriteTime = new DateTime?(DateTime.MinValue);

		protected long? fileSize = new long?(-1L);
	}
}

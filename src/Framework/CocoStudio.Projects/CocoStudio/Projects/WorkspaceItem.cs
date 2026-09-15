using System;
using System.Collections;
using CocoStudio.Projects.Formates;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	public abstract class WorkspaceItem : IWorkspaceFileObject, IFileItem, IWorkspaceObject, IExtendedDataItem, IFoldeItem, IDisposable, ILoadController
	{
		public Workspace ParentWorkspace { get; internal set; }

		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.extendedProperties == null)
				{
					this.extendedProperties = new Hashtable();
				}
				return this.extendedProperties;
			}
		}

		public virtual string Name { get; set; }

		public virtual FilePath FileName { get; set; }

		public virtual FilePath ItemDirectory
		{
			get
			{
				return this.FileName.ParentDirectory.FullPath;
			}
		}

		public FilePath BaseDirectory
		{
			get
			{
				if (this.baseDirectory.IsNull)
				{
					return this.FileName.ParentDirectory.FullPath;
				}
				return this.baseDirectory;
			}
			set
			{
				if (!value.IsNull && !this.FileName.IsNull && this.FileName.ParentDirectory.FullPath == value.FullPath)
				{
					this.baseDirectory = null;
					return;
				}
				if (value.IsNullOrEmpty)
				{
					this.baseDirectory = null;
					return;
				}
				this.baseDirectory = value.FullPath;
			}
		}

		internal virtual FileFormat FileFormat
		{
			get
			{
				if (this._fileFormat == null)
				{
					this._fileFormat = ProjectsService.Instance.GetDefaultFormat(this);
				}
				return this._fileFormat;
			}
		}

		public void Save(IProgressMonitor monitor)
		{
			try
			{
				ProjectsService.Instance.Save(monitor, this);
				this.OnSaved(monitor);
			}
			catch (Exception exception)
			{
				monitor.ReportError("Save failed.", exception);
			}
		}

		public void Dispose()
		{
		}

		public virtual CocosItem GetProjectContainingFile(FilePath fileName)
		{
			return null;
		}

		protected internal virtual void OnSave(IProgressMonitor monitor)
		{
			ProjectsService.Instance.InternalWriteWorkspaceItem(monitor, this.FileName, this);
		}

		protected virtual void OnSaved(IProgressMonitor monitor)
		{
		}

		void ILoadController.BeginLoad()
		{
			throw new NotImplementedException();
		}

		void ILoadController.EndLoad()
		{
			throw new NotImplementedException();
		}

		private FilePath baseDirectory;

		private Hashtable extendedProperties;

		private FileFormat _fileFormat;
	}
}

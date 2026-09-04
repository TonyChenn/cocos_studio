using System;
using System.Collections;
using CocoStudio.Projects.Formates;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000081 RID: 129
	public abstract class WorkspaceItem : IWorkspaceFileObject, IFileItem, IWorkspaceObject, IExtendedDataItem, IFoldeItem, IDisposable, ILoadController
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0000D411 File Offset: 0x0000B611
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x0000D419 File Offset: 0x0000B619
		public Workspace ParentWorkspace { get; internal set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000D422 File Offset: 0x0000B622
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000D43D File Offset: 0x0000B63D
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0000D445 File Offset: 0x0000B645
		public virtual string Name { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000D44E File Offset: 0x0000B64E
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x0000D456 File Offset: 0x0000B656
		public virtual FilePath FileName { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000D460 File Offset: 0x0000B660
		public virtual FilePath ItemDirectory
		{
			get
			{
				return this.FileName.ParentDirectory.FullPath;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000D484 File Offset: 0x0000B684
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x0000D4BC File Offset: 0x0000B6BC
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

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0000D536 File Offset: 0x0000B736
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

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000D558 File Offset: 0x0000B758
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

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000D59C File Offset: 0x0000B79C
		public void Dispose()
		{
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000D59E File Offset: 0x0000B79E
		public virtual CocosItem GetProjectContainingFile(FilePath fileName)
		{
			return null;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000D5A1 File Offset: 0x0000B7A1
		protected internal virtual void OnSave(IProgressMonitor monitor)
		{
			ProjectsService.Instance.InternalWriteWorkspaceItem(monitor, this.FileName, this);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000D5B5 File Offset: 0x0000B7B5
		protected virtual void OnSaved(IProgressMonitor monitor)
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000D5B7 File Offset: 0x0000B7B7
		void ILoadController.BeginLoad()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000D5BE File Offset: 0x0000B7BE
		void ILoadController.EndLoad()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000109 RID: 265
		private FilePath baseDirectory;

		// Token: 0x0400010A RID: 266
		private Hashtable extendedProperties;

		// Token: 0x0400010B RID: 267
		private FileFormat _fileFormat;
	}
}

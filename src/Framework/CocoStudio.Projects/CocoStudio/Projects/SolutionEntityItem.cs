using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000084 RID: 132
	public abstract class SolutionEntityItem : SolutionItem, IWorkspaceObject, IExtendedDataItem, IFoldeItem, IDisposable
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000DB5C File Offset: 0x0000BD5C
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x0000DB63 File Offset: 0x0000BD63
		public override string Name
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000DB6A File Offset: 0x0000BD6A
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x0000DB72 File Offset: 0x0000BD72
		public FilePath ItemDirectory { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000DB7B File Offset: 0x0000BD7B
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x0000DB83 File Offset: 0x0000BD83
		public FilePath BaseDirectory { get; set; }

		// Token: 0x06000429 RID: 1065 RVA: 0x0000DB8C File Offset: 0x0000BD8C
		public void Save(IProgressMonitor monitor)
		{
			try
			{
				this.OnSave(monitor);
			}
			catch (Exception exception)
			{
				monitor.ReportError("Save failed.", exception);
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000DBC4 File Offset: 0x0000BDC4
		protected virtual void OnSave(IProgressMonitor monitor)
		{
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000DBC6 File Offset: 0x0000BDC6
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}

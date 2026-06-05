using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200015D RID: 349
	[ProjectModelDataItem]
	public class Workspace : WorkspaceItem, ICustomDataItem
	{
		// Token: 0x06000D25 RID: 3365 RVA: 0x0002FC4C File Offset: 0x0002DE4C
		public override void Dispose()
		{
			base.Dispose();
			foreach (WorkspaceItem workspaceItem in this.Items)
			{
				workspaceItem.Dispose();
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0002FCA0 File Offset: 0x0002DEA0
		public override ReadOnlyCollection<string> GetConfigurations()
		{
			List<string> list = new List<string>();
			foreach (WorkspaceItem workspaceItem in this.Items)
			{
				foreach (string item in workspaceItem.GetConfigurations())
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			return list.AsReadOnly();
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0002FD3C File Offset: 0x0002DF3C
		public WorkspaceItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new WorkspaceItemCollection(this);
				}
				return this.items;
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0002FD58 File Offset: 0x0002DF58
		public override ReadOnlyCollection<T> GetAllItems<T>()
		{
			List<T> list = new List<T>();
			this.GetAllItems<T>(list, this);
			return list.AsReadOnly();
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0002FD7C File Offset: 0x0002DF7C
		private void GetAllItems<T>(List<T> list, WorkspaceItem item) where T : WorkspaceItem
		{
			if (item is T)
			{
				list.Add((T)((object)item));
			}
			if (item is Workspace)
			{
				foreach (WorkspaceItem item2 in ((Workspace)item).Items)
				{
					this.GetAllItems<T>(list, item2);
				}
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0002FDEC File Offset: 0x0002DFEC
		public override SolutionEntityItem FindSolutionItem(string fileName)
		{
			foreach (WorkspaceItem workspaceItem in this.Items)
			{
				SolutionEntityItem solutionEntityItem = workspaceItem.FindSolutionItem(fileName);
				if (solutionEntityItem != null)
				{
					return solutionEntityItem;
				}
			}
			return null;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0002FE44 File Offset: 0x0002E044
		[Obsolete("Use GetProjectsContainingFile() (plural) instead")]
		public override Project GetProjectContainingFile(FilePath fileName)
		{
			foreach (WorkspaceItem workspaceItem in this.Items)
			{
				Project projectContainingFile = workspaceItem.GetProjectContainingFile(fileName);
				if (projectContainingFile != null)
				{
					return projectContainingFile;
				}
			}
			return null;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x000300C4 File Offset: 0x0002E2C4
		public override IEnumerable<Project> GetProjectsContainingFile(FilePath fileName)
		{
			foreach (WorkspaceItem it in this.Items)
			{
				foreach (Project p in it.GetProjectsContainingFile(fileName))
				{
					yield return p;
				}
			}
			yield break;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x000300E8 File Offset: 0x0002E2E8
		public override bool ContainsItem(IWorkspaceObject obj)
		{
			if (base.ContainsItem(obj))
			{
				return true;
			}
			foreach (WorkspaceItem workspaceItem in this.Items)
			{
				if (workspaceItem.ContainsItem(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0003014C File Offset: 0x0002E34C
		public override ReadOnlyCollection<T> GetAllSolutionItems<T>()
		{
			List<T> list = new List<T>();
			foreach (WorkspaceItem workspaceItem in this.Items)
			{
				list.AddRange(workspaceItem.GetAllSolutionItems<T>());
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x000301AC File Offset: 0x0002E3AC
		public override void ConvertToFormat(FileFormat format, bool convertChildren)
		{
			base.ConvertToFormat(format, convertChildren);
			if (convertChildren)
			{
				foreach (WorkspaceItem workspaceItem in this.Items)
				{
					workspaceItem.ConvertToFormat(format, true);
				}
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00030208 File Offset: 0x0002E408
		protected internal override BuildResult OnRunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			BuildResult buildResult = null;
			monitor.BeginTask(null, this.Items.Count);
			try
			{
				foreach (WorkspaceItem workspaceItem in this.Items)
				{
					BuildResult buildResult2 = workspaceItem.RunTarget(monitor, target, configuration);
					if (buildResult2 != null)
					{
						if (buildResult == null)
						{
							buildResult = new BuildResult();
							buildResult.BuildCount = 0;
						}
						buildResult.Append(buildResult2);
					}
					monitor.Step(1);
				}
			}
			finally
			{
				monitor.EndTask();
			}
			return buildResult;
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000302A4 File Offset: 0x0002E4A4
		protected internal override void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x000302AC File Offset: 0x0002E4AC
		public WorkspaceItem ReloadItem(IProgressMonitor monitor, WorkspaceItem item)
		{
			if (this.Items.IndexOf(item) == -1)
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"Item '",
					item.Name,
					"' does not belong to workspace '",
					this.Name,
					"'"
				}));
			}
			WorkspaceItem workspaceItem;
			try
			{
				workspaceItem = Services.ProjectService.ReadWorkspaceItem(monitor, item.FileName);
			}
			catch (Exception ex)
			{
				workspaceItem = new UnknownWorkspaceItem
				{
					LoadError = ex.Message,
					FileName = item.FileName
				};
			}
			this.Items.Replace(item, workspaceItem);
			base.NotifyModified();
			this.NotifyItemRemoved(new WorkspaceItemChangeEventArgs(item, true));
			this.NotifyItemAdded(new WorkspaceItemChangeEventArgs(workspaceItem, true));
			item.Dispose();
			return workspaceItem;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00030388 File Offset: 0x0002E588
		public override List<FilePath> GetItemFiles(bool includeReferencedFiles)
		{
			List<FilePath> itemFiles = base.GetItemFiles(includeReferencedFiles);
			if (includeReferencedFiles)
			{
				foreach (WorkspaceItem workspaceItem in this.Items)
				{
					itemFiles.AddRange(workspaceItem.GetItemFiles(true));
				}
			}
			return itemFiles;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x000303E8 File Offset: 0x0002E5E8
		internal void NotifyItemAdded(WorkspaceItemChangeEventArgs args)
		{
			this.OnItemAdded(args);
			this.OnConfigurationsChanged();
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x000303F7 File Offset: 0x0002E5F7
		internal void NotifyItemRemoved(WorkspaceItemChangeEventArgs args)
		{
			this.OnItemRemoved(args);
			this.OnConfigurationsChanged();
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00030406 File Offset: 0x0002E606
		protected virtual void OnItemAdded(WorkspaceItemChangeEventArgs args)
		{
			if (this.ItemAdded != null)
			{
				this.ItemAdded(this, args);
			}
			this.OnDescendantItemAdded(args);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00030424 File Offset: 0x0002E624
		protected virtual void OnItemRemoved(WorkspaceItemChangeEventArgs args)
		{
			if (this.ItemRemoved != null)
			{
				this.ItemRemoved(this, args);
			}
			this.OnDescendantItemRemoved(args);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00030442 File Offset: 0x0002E642
		protected virtual void OnDescendantItemAdded(WorkspaceItemChangeEventArgs args)
		{
			if (this.DescendantItemAdded != null)
			{
				this.DescendantItemAdded(this, args);
			}
			if (base.ParentWorkspace != null)
			{
				base.ParentWorkspace.OnDescendantItemAdded(args);
			}
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0003046D File Offset: 0x0002E66D
		protected virtual void OnDescendantItemRemoved(WorkspaceItemChangeEventArgs args)
		{
			if (this.DescendantItemRemoved != null)
			{
				this.DescendantItemRemoved(this, args);
			}
			if (base.ParentWorkspace != null)
			{
				base.ParentWorkspace.OnDescendantItemRemoved(args);
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00030498 File Offset: 0x0002E698
		DataCollection ICustomDataItem.Serialize(ITypeSerializer handler)
		{
			DataCollection dataCollection = handler.Serialize(this);
			DataItem dataItem = new DataItem();
			dataItem.Name = "Items";
			dataItem.UniqueNames = false;
			string directoryName = Path.GetDirectoryName(handler.SerializationContext.BaseFile);
			foreach (WorkspaceItem workspaceItem in this.Items)
			{
				DataValue entry = new DataValue("Item", FileService.AbsoluteToRelativePath(directoryName, workspaceItem.FileName));
				dataItem.ItemData.Add(entry);
			}
			dataCollection.Add(dataItem);
			return dataCollection;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00030548 File Offset: 0x0002E748
		void ICustomDataItem.Deserialize(ITypeSerializer handler, DataCollection data)
		{
			DataItem dataItem = (DataItem)data.Extract("Items");
			handler.Deserialize(this, data);
			IProgressMonitor progressMonitor = handler.SerializationContext.ProgressMonitor;
			if (progressMonitor == null)
			{
				progressMonitor = new NullProgressMonitor();
			}
			if (dataItem != null)
			{
				string directoryName = Path.GetDirectoryName(handler.SerializationContext.BaseFile);
				progressMonitor.BeginTask(null, dataItem.ItemData.Count);
				try
				{
					foreach (object obj in dataItem.ItemData)
					{
						DataValue dataValue = (DataValue)obj;
						string file = Path.Combine(directoryName, dataValue.Value);
						WorkspaceItem workspaceItem = Services.ProjectService.ReadWorkspaceItem(progressMonitor, file);
						if (workspaceItem != null)
						{
							this.Items.Add(workspaceItem);
						}
						progressMonitor.Step(1);
					}
				}
				finally
				{
					progressMonitor.EndTask();
				}
			}
		}

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06000D3C RID: 3388 RVA: 0x00030644 File Offset: 0x0002E844
		// (remove) Token: 0x06000D3D RID: 3389 RVA: 0x0003067C File Offset: 0x0002E87C
		public event EventHandler<WorkspaceItemChangeEventArgs> ItemAdded;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06000D3E RID: 3390 RVA: 0x000306B4 File Offset: 0x0002E8B4
		// (remove) Token: 0x06000D3F RID: 3391 RVA: 0x000306EC File Offset: 0x0002E8EC
		public event EventHandler<WorkspaceItemChangeEventArgs> ItemRemoved;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06000D40 RID: 3392 RVA: 0x00030724 File Offset: 0x0002E924
		// (remove) Token: 0x06000D41 RID: 3393 RVA: 0x0003075C File Offset: 0x0002E95C
		public event EventHandler<WorkspaceItemChangeEventArgs> DescendantItemAdded;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06000D42 RID: 3394 RVA: 0x00030794 File Offset: 0x0002E994
		// (remove) Token: 0x06000D43 RID: 3395 RVA: 0x000307CC File Offset: 0x0002E9CC
		public event EventHandler<WorkspaceItemChangeEventArgs> DescendantItemRemoved;

		// Token: 0x040003DF RID: 991
		private WorkspaceItemCollection items;
	}
}

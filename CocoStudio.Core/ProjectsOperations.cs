using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core.Events;
using CocoStudio.Core.ProgressMonitors;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using CocoStudio.UserStatistics;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace CocoStudio.Core
{
	// Token: 0x02000046 RID: 70
	public class ProjectsOperations
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000A610 File Offset: 0x00008810
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000A627 File Offset: 0x00008827
		public WorkspaceItem CurrentSelectedWorkspaceItem { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000A630 File Offset: 0x00008830
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000A648 File Offset: 0x00008848
		public CocosItem CurrentSelectedProject
		{
			get
			{
				return this.currentSelectedProject;
			}
			set
			{
				this.currentSelectedProject = value;
				if (this.CurrentProjectChanged != null)
				{
					this.CurrentProjectChanged(this, new ProjectsOperations.ProjectEventArgs(value));
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000A680 File Offset: 0x00008880
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000A69C File Offset: 0x0000889C
		public Solution CurrentSelectedSolution
		{
			get
			{
				return ProjectsService.Instance.CurrentSolution;
			}
			set
			{
				if (ProjectsService.Instance.CurrentSolution != value)
				{
					ProjectsService.Instance.CurrentSolution = value;
					this.OnCurrentSolutionChanged();
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000A6D0 File Offset: 0x000088D0
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000A6E7 File Offset: 0x000088E7
		public SolutionItem CurrentSelectedSolutionItem { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000A6F0 File Offset: 0x000088F0
		public ResourceGroup CurrentResourceGroup
		{
			get
			{
				return Services.ProjectsService.CurrentResourceGroup;
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600025F RID: 607 RVA: 0x0000A70C File Offset: 0x0000890C
		// (remove) Token: 0x06000260 RID: 608 RVA: 0x0000A748 File Offset: 0x00008948
		public event EventHandler<SolutionEventArgs> CurrentSelectedSolutionClosed;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000261 RID: 609 RVA: 0x0000A784 File Offset: 0x00008984
		// (remove) Token: 0x06000262 RID: 610 RVA: 0x0000A7C0 File Offset: 0x000089C0
		public event EventHandler<SolutionEventArgs> CurrentSelectedSolutionClosing;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000263 RID: 611 RVA: 0x0000A7FC File Offset: 0x000089FC
		// (remove) Token: 0x06000264 RID: 612 RVA: 0x0000A838 File Offset: 0x00008A38
		public event EventHandler<SolutionEventArgs> CurrentSelectedSolutionChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000265 RID: 613 RVA: 0x0000A874 File Offset: 0x00008A74
		// (remove) Token: 0x06000266 RID: 614 RVA: 0x0000A8B0 File Offset: 0x00008AB0
		public event EventHandler<ProjectsOperations.ProjectEventArgs> CurrentProjectChanged;

		// Token: 0x06000267 RID: 615 RVA: 0x0000A8EC File Offset: 0x00008AEC
		internal ProjectsOperations()
		{
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000A8F8 File Offset: 0x00008AF8
		public CocosItem AddNewFile(ResourceFolder parentResourceItem, CocosItemCreateInfo info)
		{
			CocosItem cocosItem = Services.ProjectsService.CreateCocosItem(info.ContentType, info);
			cocosItem.ContentType = info.ContentType;
			parentResourceItem.Items.Add(cocosItem);
			IProgressMonitor @default = Services.ProgressMonitors.Default;
			cocosItem.Save(@default);
			cocosItem.Initialize(@default);
			this.CurrentSelectedSolution.Save(@default);
			return cocosItem;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000AB0C File Offset: 0x00008D0C
		public async Task<List<ResourceItem>> ImportResourcesAsync(ResourceFolder parent, IEnumerable<string> pathes, Action<IProgressMonitor> continueAction = null)
		{
			MessageDialogProgressMonitor monitor = Services.ProgressMonitors.GetMessageDialogProgreeMonitor();
			List<ResourceItem> result = await ImportFileService.MainProcess(parent, pathes, monitor);
			Services.ProjectOperations.CurrentSelectedSolution.Save(Services.ProgressMonitors.Default);
			if (continueAction != null)
			{
				continueAction(monitor);
			}
			monitor.Dispose();
			if (monitor != null && monitor.Errors.Count == 0 && monitor.Warnings.Count == 0)
			{
				monitor.CloseDialogs();
			}
			GC.Collect();
			return result;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000AB70 File Offset: 0x00008D70
		public ResourceItem AddResourceItem(ResourceFolder parentResourceItem, FilePath itemFileName, IProgressMonitor monitor)
		{
			ResourceItem result;
			try
			{
				ResourceItem resourceItem = this.CurrentResourceGroup.FindResourceItem(parentResourceItem, itemFileName);
				if (resourceItem == null)
				{
					resourceItem = Services.ProjectsService.ReadResourceItem(monitor, itemFileName);
					if (resourceItem is ICocosFile)
					{
						((ICocosFile)resourceItem).Initialize(monitor);
					}
					parentResourceItem.Items.Add(resourceItem);
				}
				monitor.Step(1);
				result = resourceItem;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Failed to create ResourceItem", exception);
				result = null;
			}
			return result;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000AC10 File Offset: 0x00008E10
		public ResourceItem FindResourceItem(ResourceData resourceData)
		{
			return this.CurrentResourceGroup.FindResourceItem(resourceData);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000AC30 File Offset: 0x00008E30
		public ResourceItem FindResourceItem(string filePath)
		{
			return this.CurrentResourceGroup.FindResourceItem(filePath);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000AC50 File Offset: 0x00008E50
		public Solution OpenSolution(IProgressMonitor monitor, string filePath)
		{
			Solution result;
			try
			{
				Solution wrapperSolution = Services.ProjectsService.GetWrapperSolution(monitor, filePath);
				if (!ProjectsOperations.CheckFileVersion(wrapperSolution.Version, filePath))
				{
					Tracker.Add(ViewRegions.None, "OpenProjectFailed", "", "");
					result = null;
				}
				else
				{
					ProjectsService.Instance.CurrentSolution = wrapperSolution;
					wrapperSolution.Initialize(monitor);
					this.CurrentSelectedWorkspaceItem = wrapperSolution;
					using (TaskServiceLock.Lock())
					{
						this.OnCurrentSolutionChanged();
					}
					ProjectsService.Instance.CurrentResourceGroup.RootFolder.Refresh();
					Services.RecentFileService.AddProject(this.CurrentSelectedSolution.FileName);
					Tracker.Add(ViewRegions.None, "OpenProject", "", "");
					result = wrapperSolution;
				}
			}
			catch (Exception exception)
			{
				Tracker.Add(ViewRegions.None, "OpenProjectFailed", "", "");
				LogConfig.Output.Error(LanguageInfo.OpenProjectFail, exception);
				result = null;
			}
			return result;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000AD70 File Offset: 0x00008F70
		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
			Services.ProjectsService.Publish(monitor, info);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000AD80 File Offset: 0x00008F80
		public bool CloseSolution()
		{
			if (this.CurrentSelectedSolutionClosing != null)
			{
				SolutionEventArgs e = new SolutionEventArgs(this.CurrentSelectedSolution);
				this.CurrentSelectedSolutionClosing(this, e);
			}
			bool result;
			if (Services.ProjectOperations.CurrentSelectedSolution != null)
			{
				if (!Services.Workbench.CloseAll(false))
				{
					result = false;
				}
				else
				{
					if (Services.Workspace.Items.Count != 0)
					{
						Services.Workspace.CloseWorkspaceItem(Services.Workspace.Items[0]);
					}
					if (this.CurrentSelectedSolutionClosed != null)
					{
						SolutionEventArgs e = new SolutionEventArgs(this.CurrentSelectedSolution);
						this.CurrentSelectedSolutionClosed(this, e);
					}
					try
					{
						CSCocosHelp.ClearResurceCache();
						CSCocosHelp.StopAllEffects();
					}
					catch (Exception exception)
					{
						LogConfig.Logger.Error("Clear resource cache failed.", exception);
					}
					this.CurrentSelectedSolution = null;
					this.CurrentSelectedWorkspaceItem = null;
					this.CurrentSelectedProject = null;
					Services.TaskService.Clear(null);
					GC.Collect();
					result = true;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public void Save(ICocosFile cocosFile)
		{
			IProgressMonitor @default = Services.ProgressMonitors.Default;
			try
			{
				cocosFile.Save(@default);
			}
			catch (Exception exception)
			{
				@default.ReportError("Save failed.", exception);
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000AEF0 File Offset: 0x000090F0
		private void OnCurrentSolutionChanged()
		{
			if (this.CurrentSelectedSolutionChanged != null)
			{
				this.CurrentSelectedSolutionChanged(this, new SolutionEventArgs(this.CurrentSelectedSolution));
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000AF28 File Offset: 0x00009128
		public void MarkFileDirty(string filename)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(filename);
				if (fileInfo.Exists)
				{
					fileInfo.LastWriteTime = DateTime.Now;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Error while marking file as dirty", exception);
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000AF84 File Offset: 0x00009184
		private static bool CheckFileVersion(Version fileVersion, string filePath)
		{
			bool result;
			if (fileVersion == Option.EditorVersion)
			{
				result = true;
			}
			else if (fileVersion.CompareTo(Option.EditorVersion) < 0)
			{
				result = true;
			}
			else
			{
				MessageBox.Show(LanguageInfo.MessageBox_Content71, MessageBoxImage.Other, null, null);
				result = false;
			}
			return result;
		}

		// Token: 0x0400012D RID: 301
		private CocosItem currentSelectedProject;

		// Token: 0x02000047 RID: 71
		public class ProjectEventArgs : EventArgs
		{
			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x06000274 RID: 628 RVA: 0x0000AFD8 File Offset: 0x000091D8
			// (set) Token: 0x06000275 RID: 629 RVA: 0x0000AFEF File Offset: 0x000091EF
			public CocosItem Project { get; private set; }

			// Token: 0x06000276 RID: 630 RVA: 0x0000AFF8 File Offset: 0x000091F8
			public ProjectEventArgs(CocosItem project)
			{
				this.Project = project;
			}
		}
	}
}

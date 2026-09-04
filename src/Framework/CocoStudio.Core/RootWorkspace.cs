using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;

namespace CocoStudio.Core
{
	// Token: 0x02000048 RID: 72
	public class RootWorkspace
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000B00C File Offset: 0x0000920C
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000B023 File Offset: 0x00009223
		public ObservableCollection<WorkspaceItem> Items { get; private set; }

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000279 RID: 633 RVA: 0x0000B02C File Offset: 0x0000922C
		// (remove) Token: 0x0600027A RID: 634 RVA: 0x0000B068 File Offset: 0x00009268
		public event EventHandler<EventArgs> LoadWorkspaceItemSuccessedEvent;

		// Token: 0x0600027B RID: 635 RVA: 0x0000B0A4 File Offset: 0x000092A4
		public RootWorkspace()
		{
			this.Items = new ObservableCollection<WorkspaceItem>();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000B0BC File Offset: 0x000092BC
		public void Save(IProgressMonitor monitor)
		{
			monitor.BeginTask("Saving workspace...", this.Items.Count);
			Services.Workbench.SaveAll();
			foreach (WorkspaceItem workspaceItem in this.Items)
			{
				workspaceItem.Save(monitor);
				monitor.Step(1);
			}
			monitor.EndTask();
			LogConfig.Output.Info(LanguageInfo.Output_Saved, true);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000B15C File Offset: 0x0000935C
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000B164 File Offset: 0x00009364
		public IAsyncOperation OpenWorkspaceItem(string filePath)
		{
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			if (currentSelectedSolution != null)
			{
				string fullPath = Path.GetFullPath(currentSelectedSolution.FileName);
				string fullPath2 = Path.GetFullPath(filePath);
				if (fullPath.Equals(fullPath2))
				{
					return null;
				}
			}
			IProgressMonitor progressMonitor;
			if (Services.MainWindow == null)
			{
				progressMonitor = new SimpleProgressMonitor();
			}
			else
			{
				progressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
			}
			if (this.Items.Count > 0)
			{
				MessageBoxResult messageBoxResult = MessageBox.Show(LanguageInfo.MessageBox172_AskCloseCurSln, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
				if (messageBoxResult != MessageBoxResult.Yes)
				{
					return progressMonitor.AsyncOperation;
				}
				this.CloseWorkspaceItem(this.Items[0]);
			}
			this.LoadWorkspaceItem(progressMonitor, filePath);
			return progressMonitor.AsyncOperation;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000B284 File Offset: 0x00009484
		private void LoadWorkspaceItem(IProgressMonitor monitor, string filePath)
		{
			if (!File.Exists(filePath))
			{
				monitor.ReportError("File not found." + filePath, null);
			}
			else
			{
				Solution solution = Services.ProjectOperations.OpenSolution(monitor, filePath);
				if (solution != null)
				{
					SolutionLockHandler.Instance.TryLockSolution(filePath);
					this.Items.Add(solution);
					GLib.Timeout.Add(100U, delegate
					{
						if (this.LoadWorkspaceItemSuccessedEvent != null)
						{
							this.LoadWorkspaceItemSuccessedEvent(this, EventArgs.Empty);
						}
						return false;
					});
				}
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000B2FC File Offset: 0x000094FC
		public void CloseWorkspaceItem(WorkspaceItem item)
		{
			if (item != null)
			{
				IProgressMonitor consoleProgressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
				item.Save(consoleProgressMonitor);
				foreach (DocumentExtend documentExtend in Services.Workbench.Documents.ToArray<DocumentExtend>())
				{
					documentExtend.Close(true);
				}
				SolutionLockHandler.Instance.ReleaseLock();
				this.Items.Remove(item);
				item.Dispose();
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000B381 File Offset: 0x00009581
		public void SaveCurrentSolution()
		{
			Services.ProjectOperations.CurrentSelectedSolution.Save(Services.ProgressMonitors.Default);
		}
	}
}

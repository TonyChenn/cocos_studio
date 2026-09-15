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
	public class RootWorkspace
	{
		public ObservableCollection<WorkspaceItem> Items { get; private set; }

		public event EventHandler<EventArgs> LoadWorkspaceItemSuccessedEvent;

		public RootWorkspace()
		{
			this.Items = new ObservableCollection<WorkspaceItem>();
		}

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

		public void Dispose()
		{
			throw new NotImplementedException();
		}

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

		public void SaveCurrentSolution()
		{
			Services.ProjectOperations.CurrentSelectedSolution.Save(Services.ProgressMonitors.Default);
		}
	}
}

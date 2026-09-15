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
	public class ProjectsOperations
	{
		public WorkspaceItem CurrentSelectedWorkspaceItem { get; set; }

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

		public SolutionItem CurrentSelectedSolutionItem { get; set; }

		public ResourceGroup CurrentResourceGroup
		{
			get
			{
				return Services.ProjectsService.CurrentResourceGroup;
			}
		}

		public event EventHandler<SolutionEventArgs> CurrentSelectedSolutionClosed;

		public event EventHandler<SolutionEventArgs> CurrentSelectedSolutionClosing;

		public event EventHandler<SolutionEventArgs> CurrentSelectedSolutionChanged;

		public event EventHandler<ProjectsOperations.ProjectEventArgs> CurrentProjectChanged;

		internal ProjectsOperations()
		{
		}

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

		public ResourceItem FindResourceItem(ResourceData resourceData)
		{
			return this.CurrentResourceGroup.FindResourceItem(resourceData);
		}

		public ResourceItem FindResourceItem(string filePath)
		{
			return this.CurrentResourceGroup.FindResourceItem(filePath);
		}

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

		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
			Services.ProjectsService.Publish(monitor, info);
		}

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

		private void OnCurrentSolutionChanged()
		{
			if (this.CurrentSelectedSolutionChanged != null)
			{
				this.CurrentSelectedSolutionChanged(this, new SolutionEventArgs(this.CurrentSelectedSolution));
			}
		}

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

		private CocosItem currentSelectedProject;

		public class ProjectEventArgs : EventArgs
		{
			public CocosItem Project { get; private set; }

			public ProjectEventArgs(CocosItem project)
			{
				this.Project = project;
			}
		}
	}
}

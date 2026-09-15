using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel.Upgrade;
using CocoStudio.Projects.Visiter;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace Modules.UI.MainTool
{
	internal class CanvasViewModel : NotificationObject
	{
		public List<ResolutionConfig> CanvasSizeList { get; set; }

		public CanvasViewModel(IEventAggregator eventAggregator, CanvasWidget widget)
		{
			this.eventAggregator = eventAggregator;
			this.canvasWidget = widget;
			this.eventAggregator.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.OnCanvasSizeChangeEvent));
			this.CanvasSizeList = new List<ResolutionConfig>();
		}

		public void RaiseCanvasSizeChange(SizeF size)
		{
			if (!this.isReceiveMessage)
			{
				if (Services.ProjectOperations.CurrentSelectedSolution != null)
				{
					GameWindow gameWindow = GameWindow.Current;
					if (gameWindow != null)
					{
						using (CompositeTask.Run("Canvas size change.", null))
						{
							this.UpdateGameFileSize(Services.ProjectOperations.CurrentResourceGroup.RootFolder, size);
							this.eventAggregator.GetEvent<CanvasSizeChangeEvent>().Unsubscribe(new Action<CanvasSizeChangeEventArgs>(this.OnCanvasSizeChangeEvent));
							CanvasObject canvasObject = gameWindow.GetCanvasObject();
							string fileType = Services.ProjectOperations.CurrentSelectedProject.GetFileType();
							if (fileType == NodeType.Scene.ToString() || fileType == NodeType.Scene3D.ToString())
							{
								canvasObject.SetSceneSize(size, true);
								this.eventAggregator.GetEvent<CanvasSizeChangeEvent>().Publish(new CanvasSizeChangeEventArgs(string.Empty, size));
							}
							else
							{
								canvasObject.SetSceneSize(size, false);
							}
							this.eventAggregator.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.OnCanvasSizeChangeEvent));
						}
					}
					this.SaveCanvasSize();
					IPropertyGrid service = Services.GetService<IPropertyGrid>();
					service.ForceRefresh();
				}
			}
		}

		private void UpdateGameFileSize(ResourceFolder resFolder, SizeF size)
		{
			if (resFolder != null)
			{
				foreach (ResourceItem resourceItem in resFolder.Items)
				{
					if (resourceItem is ResourceFolder)
					{
						this.UpdateGameFileSize(resourceItem as ResourceFolder, size);
					}
					else if (resourceItem is CocosItem)
					{
						CocosItem cocosItem = resourceItem as CocosItem;
						if (cocosItem.GetFileType() == NodeType.Scene.ToString())
						{
							if (cocosItem.IsLoaded)
							{
								cocosItem.GetRootNode().Size = size;
							}
							if (cocosItem.GetRootNodeData().Size != size)
							{
								cocosItem.GetRootNodeData().Size = size;
								cocosItem.CocosFile.WriteFile(ProjectsService.Instance.DefaultMonitor);
							}
						}
					}
				}
			}
		}

		private void UpdateCanvasResolution(SizeF newSize, string newName)
		{
			List<ResolutionConfig> list = Option.UserConfig.ResolutionList.FindAll((ResolutionConfig w) => (float)w.Width == newSize.Width && (float)w.Height == newSize.Height);
			ResolutionConfig item;
			if (list.Count == 0)
			{
				string size = newSize.FormatString();
				if (string.IsNullOrEmpty(newName))
				{
					newName = "Default";
				}
				item = new ResolutionConfig(size)
				{
					Name = newName,
					IsSelected = true,
					Order = 0
				};
			}
			else
			{
				item = list.Find((ResolutionConfig w) => w.Name.Equals(newName));
				if (item == null)
				{
					item = list[0];
				}
			}
			this.canvasWidget.SelectResolution(item);
			this.RaisePropertyChanged<string>(() => item.Size);
		}

		private void SaveCanvasSize()
		{
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			if (currentSelectedSolution != null)
			{
				ResolutionConfig currentResolution = this.canvasWidget.CurrentResolution;
				if (currentResolution != null)
				{
					if (currentSelectedSolution.GetSceneSizeString() != currentResolution.Size || currentSelectedSolution.GetResolutionName() != currentResolution.Name)
					{
						SizeF sceneSize = currentSelectedSolution.GetSceneSize();
						currentSelectedSolution.SetSceneSize(currentResolution.Size);
						currentSelectedSolution.SetResolutionName(currentResolution.Name);
						currentSelectedSolution.Config.Save();
						this.UpdateConfigJson(currentSelectedSolution, currentResolution.Width, currentResolution.Height, sceneSize);
					}
				}
			}
		}

		private void OnCanvasSizeChangeEvent(CanvasSizeChangeEventArgs args)
		{
			this.isReceiveMessage = true;
			this.UpdateCanvasResolution(args.NewSize, Services.ProjectOperations.CurrentSelectedSolution.GetResolutionName());
			this.isReceiveMessage = false;
			this.UpdateGameFileSize(Services.ProjectOperations.CurrentResourceGroup.RootFolder, args.NewSize);
		}

		public void OnProjectChanged(ProjectsOperations.ProjectEventArgs e)
		{
			this.SaveCanvasSize();
		}

		public void OnSolutionChanged(SolutionEventArgs e)
		{
			if (e.Solution != null)
			{
				SizeF sceneSize = e.Solution.GetSceneSize();
				if (sceneSize != SizeF.Empty)
				{
					this.UpdateCanvasResolution(sceneSize, e.Solution.GetResolutionName());
				}
				this.SaveCanvasSize();
			}
		}

		public void OnSolutionClosed(SolutionEventArgs e)
		{
			this.SaveCanvasSize();
		}

		private void UpdateConfigJson(Solution solution, int newWidth, int newHeight, SizeF oldSize)
		{
			bool flag = oldSize.Width > oldSize.Height;
			bool flag2 = newWidth > newHeight;
			if (flag != flag2)
			{
				SolutionUpgraderHelper.UpdateConfigJson(Path.GetDirectoryName(solution.ItemDirectory), flag2);
			}
		}

		private IEventAggregator eventAggregator;

		private bool isReceiveMessage;

		private CanvasWidget canvasWidget;
	}
}

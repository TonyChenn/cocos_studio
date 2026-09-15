using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;
using Modules.Communal.Render.Model;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;

namespace Modules.Communal.Render.View
{
	[Extension(Path = "/CocoStudio/Ide/Render")]
	internal class GameCanvasContent : ViewContent, IMainRender, IViewContentExtend, IViewContent, IBaseViewContent, IDisposable
	{
		public override CocosItem File
		{
			get
			{
				return this.cocosItem;
			}
			set
			{
				this.cocosItem = value;
				if (value != null)
				{
					this.ContentName = this.cocosItem.FileName;
				}
			}
		}

		public IEnumerable<VisualObject> SelectedObject
		{
			get
			{
				return this.selectedObject;
			}
		}

		public IEnumerable<VisualObject> SelectedParentObject
		{
			get
			{
				return this.selectedParentObject;
			}
		}

		public GameCanvasContent()
		{
			this.widget = GameCanvasContent.gameCanvas;
		}

		private bool LoadCocosItem(CocosItem newCocosItem, bool isReload = false)
		{
			bool result;
			if (newCocosItem == null)
			{
				result = false;
			}
			else
			{
				IProgressMonitor @default = Services.ProgressMonitors.Default;
				if (!newCocosItem.IsLoaded)
				{
					using (TaskServiceLock.Lock())
					{
						newCocosItem.Load(@default);
						this.rootNode = newCocosItem.GetRootNode();
						if (this.rootNode == null)
						{
							LogConfig.Output.Error(LanguageInfo.FailedToLoadFile);
							return false;
						}
						if (isReload && !base.IsActivated)
						{
							this.rootNode.Visible = false;
						}
						else
						{
							this.rootObjectVisible = this.rootNode.Visible;
						}
						if (GameCanvasContent.gameCanvas.GameWindow != null)
						{
							GameCanvasContent.gameCanvas.GameWindow.GetCanvasObject().Children.Add(this.rootNode);
						}
					}
				}
				else
				{
					using (TaskServiceLock.Lock())
					{
						newCocosItem.ReloadReferencedItem(@default);
					}
				}
				result = true;
			}
			return result;
		}

		protected override void OnActivated()
		{
			if (GameCanvasContent.gameCanvas.CheckInitialized())
			{
				GameCanvasContent.gameCanvas.OnCocosItemChanged(this.File);
				if (!this.LoadCocosItem(this.File, false))
				{
					this.WorkbenchWindow.CloseWindow(true);
				}
				else
				{
					if (this.rootNode != null)
					{
						if (this.rootNode.Recorder != null)
						{
							this.rootNode.Recorder.Stop(false);
						}
						this.rootNode.Visible = (this.rootObjectVisible && this.rootNode.VisibleForFrame);
						if (this.rootNode.Recorder != null)
						{
							this.rootNode.Recorder.Start(false, false);
						}
					}
					ViewModeManager.Instance.OnDocumentChanged(this.File);
					DragOperationManager.OnProjectChanged(this.File);
					GameCanvasContent.gameCanvas.SwitchView(true);
				}
			}
		}

		public override void AfterActivated()
		{
			if (GameCanvasContent.gameCanvas.CheckInitialized())
			{
				SelectService.Instance.IsActived = true;
				SelectService.Instance.OnDocumentChanged(this);
			}
		}

		protected override void OnDeactivated()
		{
			this.ClearPropertyUC();
			if (this.rootNode != null)
			{
				this.rootObjectVisible = this.rootNode.Visible;
				if (this.rootNode.Recorder != null)
				{
					this.rootNode.Recorder.Stop(false);
				}
				this.rootNode.Visible = false;
				if (this.rootNode.Recorder != null)
				{
					this.rootNode.Recorder.Start(false, false);
				}
			}
			this.selectedObject = SelectService.Instance.SelectedObjectList;
			this.selectedParentObject = SelectService.Instance.SelectedParentObjectList;
			GameCanvasContent.gameCanvas.SwitchView(false);
			SelectService.Instance.IsActived = false;
		}

		protected override void OnClosing()
		{
		}

		protected override void OnClosed()
		{
			if (GameCanvasContent.gameCanvas.GameWindow != null)
			{
				CanvasObject canvasObject = GameCanvasContent.gameCanvas.GameWindow.GetCanvasObject();
				if (canvasObject != null)
				{
					canvasObject.Children.Remove(this.rootNode);
				}
				this.File.UnLoad(null);
				this.rootNode = null;
				ViewModeManager.Instance.OnDocumentClosed(this.File);
			}
		}

		public override void OnBeforeSave(EventArgs e)
		{
			base.OnBeforeSave(e);
			if (this.cocosItem != null)
			{
				ViewModeManager.Instance.OnDocumentBeforeSave(this.File);
				if (this.cocosItem != Services.Workbench.ActiveDocument.File)
				{
					if (this.rootNode != null && this.rootNode.Recorder != null)
					{
						this.rootNode.Recorder.Stop(false);
						this.rootNode.Visible = this.rootObjectVisible;
						this.rootNode.Recorder.Start(false, false);
					}
				}
			}
		}

		protected override void OnSaved()
		{
			base.OnSaved();
			if (this.cocosItem != null)
			{
				ViewModeManager.Instance.OnDocumentSaved(this.cocosItem);
				if (this.cocosItem != Services.Workbench.ActiveDocument.File)
				{
					if (this.rootNode != null && this.rootNode.Recorder != null)
					{
						this.rootNode.Recorder.Stop(false);
						this.rootNode.Visible = false;
						this.rootNode.Recorder.Start(false, false);
					}
				}
			}
		}

		public override void Reload()
		{
			CocosItem file = this.File;
			this.OnClosed();
			this.File = file;
			this.LoadCocosItem(file, true);
			this.ClearPropertyUC();
		}

		public override Widget GetToolbar()
		{
			IViewMode viewMode = ViewModeManager.Instance.Current;
			Widget result;
			if (viewMode != null)
			{
				result = viewMode.GetToolbar();
			}
			else
			{
				result = null;
			}
			return result;
		}

		private void ClearPropertyUC()
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			service.SelectedObjects = null;
		}

		void IMainRender.SwitchView()
		{
			if (GameCanvasContent.gameCanvas != null)
			{
				GameCanvasContent.gameCanvas.SwitchView(true);
			}
		}

		private static GameCanvas gameCanvas = new GameCanvas();

		private CocosItem cocosItem;

		private bool rootObjectVisible = false;

		private AbstractNodeObject rootNode;

		private IEnumerable<VisualObject> selectedObject;

		private IEnumerable<VisualObject> selectedParentObject;
	}
}

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
	// Token: 0x02000037 RID: 55
	[Extension(Path = "/CocoStudio/Ide/Render")]
	internal class GameCanvasContent : ViewContent, IMainRender, IViewContentExtend, IViewContent, IBaseViewContent, IDisposable
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000DD3C File Offset: 0x0000BF3C
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0000DD54 File Offset: 0x0000BF54
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000DD88 File Offset: 0x0000BF88
		public IEnumerable<VisualObject> SelectedObject
		{
			get
			{
				return this.selectedObject;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		public IEnumerable<VisualObject> SelectedParentObject
		{
			get
			{
				return this.selectedParentObject;
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000DDC5 File Offset: 0x0000BFC5
		public GameCanvasContent()
		{
			this.widget = GameCanvasContent.gameCanvas;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000DDE4 File Offset: 0x0000BFE4
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

		// Token: 0x0600028E RID: 654 RVA: 0x0000DF24 File Offset: 0x0000C124
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

		// Token: 0x0600028F RID: 655 RVA: 0x0000E020 File Offset: 0x0000C220
		public override void AfterActivated()
		{
			if (GameCanvasContent.gameCanvas.CheckInitialized())
			{
				SelectService.Instance.IsActived = true;
				SelectService.Instance.OnDocumentChanged(this);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000E058 File Offset: 0x0000C258
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

		// Token: 0x06000291 RID: 657 RVA: 0x0000E11B File Offset: 0x0000C31B
		protected override void OnClosing()
		{
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000E120 File Offset: 0x0000C320
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

		// Token: 0x06000293 RID: 659 RVA: 0x0000E194 File Offset: 0x0000C394
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

		// Token: 0x06000294 RID: 660 RVA: 0x0000E24C File Offset: 0x0000C44C
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

		// Token: 0x06000295 RID: 661 RVA: 0x0000E2FC File Offset: 0x0000C4FC
		public override void Reload()
		{
			CocosItem file = this.File;
			this.OnClosed();
			this.File = file;
			this.LoadCocosItem(file, true);
			this.ClearPropertyUC();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000E330 File Offset: 0x0000C530
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

		// Token: 0x06000297 RID: 663 RVA: 0x0000E360 File Offset: 0x0000C560
		private void ClearPropertyUC()
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			service.SelectedObjects = null;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000E37C File Offset: 0x0000C57C
		void IMainRender.SwitchView()
		{
			if (GameCanvasContent.gameCanvas != null)
			{
				GameCanvasContent.gameCanvas.SwitchView(true);
			}
		}

		// Token: 0x040000BA RID: 186
		private static GameCanvas gameCanvas = new GameCanvas();

		// Token: 0x040000BB RID: 187
		private CocosItem cocosItem;

		// Token: 0x040000BC RID: 188
		private bool rootObjectVisible = false;

		// Token: 0x040000BD RID: 189
		private AbstractNodeObject rootNode;

		// Token: 0x040000BE RID: 190
		private IEnumerable<VisualObject> selectedObject;

		// Token: 0x040000BF RID: 191
		private IEnumerable<VisualObject> selectedParentObject;
	}
}

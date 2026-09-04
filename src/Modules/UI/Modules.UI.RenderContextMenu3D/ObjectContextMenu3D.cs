using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.ExtensionModel;
using Modules.Communal.Render.Model;
using Mono.Addins;
using MonoDevelop.Components.Commands;

namespace Modules.UI.RenderContextMenu3D
{
	// Token: 0x02000002 RID: 2
	[Extension(typeof(IObjectContextMenu))]
	public class ObjectContextMenu3D : CommandMenu, IObjectContextMenu, IActivateControl
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public bool IsRenderArea { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000207C File Offset: 0x0000027C
		public ObjectContextMenuModel3D DataModel
		{
			get
			{
				if (this.dataModel == null)
				{
					this.dataModel = new ObjectContextMenuModel3D();
				}
				return this.dataModel;
			}
			set
			{
				this.dataModel = value;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002088 File Offset: 0x00000288
		public ObjectContextMenu3D() : base(Services.CommandService)
		{
			this.taskService = Services.TaskService;
			this.eventAggregator = Services.EventsService;
			base.Shown += this.ObjectContextMenuWPF_Shown;
			this.InitMenuItem();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020D9 File Offset: 0x000002D9
		private void ObjectContextMenuWPF_Shown(object sender, EventArgs e)
		{
			this.OnOpened(e);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020E4 File Offset: 0x000002E4
		private MenuItem CreatMenuItem(string header)
		{
			MenuItem menuItem = new MenuItem(header);
			base.Add(menuItem);
			return menuItem;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002100 File Offset: 0x00000300
		private void InitMenuItem()
		{
			this.menuItemCutComponent = MenuCreator.CreateMenuItem(GlobalCommand.CutCmd, false, LanguageInfo.Command_Cut);
			this.menuItemCopyComponent = MenuCreator.CreateMenuItem(GlobalCommand.CopyCmd, false, LanguageInfo.Command_Copy);
			this.menuItemPasteComponent = MenuCreator.CreateDelayCloseMenuItem(GlobalCommand.PasteCmd, false, null);
			this.menuItemDeleteObject = MenuCreator.CreateMenuItem(GlobalCommand.DeleteCmd, false, null);
			this.menuItemRenameObject = MenuCreator.CreateMenuItem(GlobalCommand.RenameCmd, true, null);
			base.Append(this.menuItemCutComponent);
			base.Append(this.menuItemCopyComponent);
			base.Append(this.menuItemPasteComponent);
			base.Append(this.menuItemDeleteObject);
			base.Append(this.menuItemRenameObject);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021AB File Offset: 0x000003AB
		private void RenderEngineInitCompletedEventHandled(RenderEngineLoadedEventArgs args)
		{
			this.eventAggregator.GetEvent<RenderEngineLoadedEvent>().Unsubscribe(new Action<RenderEngineLoadedEventArgs>(this.RenderEngineInitCompletedEventHandled));
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021CC File Offset: 0x000003CC
		public void CanShow(ContextMenuShowingArgs args)
		{
			if (args != null && args.IsShowOnRenderArea)
			{
				args.Enable = false;
			}
			else
			{
				if (!base.Children.Contains(this.menuItemRenameObject))
				{
					base.Append(this.menuItemRenameObject);
				}
				this.clickPoint.X = 0;
				this.clickPoint.Y = 0;
			}
			base.ShowAll();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000222A File Offset: 0x0000042A
		protected void OnOpened(EventArgs e)
		{
			this.UpdateOperationSensitive(true);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002234 File Offset: 0x00000434
		private void UpdateOperationSensitive(bool hasSelected = true)
		{
			bool flag = false;
			if (this.DataModel.SelectedObjectList != null && this.DataModel.SelectedObjectList.Count >= 1)
			{
				flag = true;
			}
			this.menuItemCutComponent.Sensitive = (flag && hasSelected);
			this.menuItemDeleteObject.Sensitive = (flag && hasSelected);
			this.menuItemCopyComponent.Sensitive = (flag && hasSelected);
			this.menuItemRenameObject.Sensitive = (flag && hasSelected);
			this.menuItemPasteComponent.Sensitive = (this.DataModel.PasteMenuIsAction(this.DataModel.CopyObjectList) && hasSelected);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022C0 File Offset: 0x000004C0
		private bool PasteMenuIsAction(IEnumerable<VisualObject> pasteList)
		{
			VisualObject rootNode = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
			IEnumerable<VisualObject> allChildeNode = this.GetAllChildeNode(rootNode);
			return pasteList != null && pasteList.Count<VisualObject>() != 0 && allChildeNode.Intersect(pasteList).Count<VisualObject>() == pasteList.Count<VisualObject>();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000230C File Offset: 0x0000050C
		private IEnumerable<VisualObject> GetAllChildeNode(VisualObject rootNode)
		{
			List<VisualObject> list = new List<VisualObject>();
			IEnumerable<VisualObject> visualChildren = rootNode.GetVisualChildren();
			if (visualChildren != null && visualChildren.Count<VisualObject>() > 0)
			{
				foreach (VisualObject rootNode2 in visualChildren)
				{
					IEnumerable<VisualObject> allChildeNode = this.GetAllChildeNode(rootNode2);
					list.AddRange(allChildeNode);
				}
				list.AddRange(visualChildren);
			}
			return list;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002384 File Offset: 0x00000584
		public void Activated(CocosItem project)
		{
			this.DataModel.RegisterEvent();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002391 File Offset: 0x00000591
		public new void Deactivated()
		{
			this.DataModel.UnregisterEvent();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000239E File Offset: 0x0000059E
		public string Type
		{
			get
			{
				return NodeType.Scene3D.ToString();
			}
		}

		// Token: 0x04000001 RID: 1
		private MenuItem menuItemDeleteObject;

		// Token: 0x04000002 RID: 2
		private MenuItem menuItemCopyComponent;

		// Token: 0x04000003 RID: 3
		private MenuItem menuItemPasteComponent;

		// Token: 0x04000004 RID: 4
		private MenuItem menuItemCutComponent;

		// Token: 0x04000005 RID: 5
		private MenuItem menuItemRenameObject;

		// Token: 0x04000006 RID: 6
		private IUndoManager taskService;

		// Token: 0x04000007 RID: 7
		private IEventAggregator eventAggregator;

		// Token: 0x04000008 RID: 8
		private static Point zeroPoint = new Point(0, 0);

		// Token: 0x04000009 RID: 9
		private ObjectContextMenuModel3D dataModel = new ObjectContextMenuModel3D();

		// Token: 0x0400000A RID: 10
		private Point clickPoint;
	}
}

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
	[Extension(typeof(IObjectContextMenu))]
	public class ObjectContextMenu3D : CommandMenu, IObjectContextMenu, IActivateControl
	{
		public bool IsRenderArea { get; set; }

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

		public ObjectContextMenu3D() : base(Services.CommandService)
		{
			this.taskService = Services.TaskService;
			this.eventAggregator = Services.EventsService;
			base.Shown += this.ObjectContextMenuWPF_Shown;
			this.InitMenuItem();
		}

		private void ObjectContextMenuWPF_Shown(object sender, EventArgs e)
		{
			this.OnOpened(e);
		}

		private MenuItem CreatMenuItem(string header)
		{
			MenuItem menuItem = new MenuItem(header);
			base.Add(menuItem);
			return menuItem;
		}

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

		private void RenderEngineInitCompletedEventHandled(RenderEngineLoadedEventArgs args)
		{
			this.eventAggregator.GetEvent<RenderEngineLoadedEvent>().Unsubscribe(new Action<RenderEngineLoadedEventArgs>(this.RenderEngineInitCompletedEventHandled));
		}

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

		protected void OnOpened(EventArgs e)
		{
			this.UpdateOperationSensitive(true);
		}

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

		private bool PasteMenuIsAction(IEnumerable<VisualObject> pasteList)
		{
			VisualObject rootNode = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
			IEnumerable<VisualObject> allChildeNode = this.GetAllChildeNode(rootNode);
			return pasteList != null && pasteList.Count<VisualObject>() != 0 && allChildeNode.Intersect(pasteList).Count<VisualObject>() == pasteList.Count<VisualObject>();
		}

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

		public void Activated(CocosItem project)
		{
			this.DataModel.RegisterEvent();
		}

		public new void Deactivated()
		{
			this.DataModel.UnregisterEvent();
		}

		public string Type
		{
			get
			{
				return NodeType.Scene3D.ToString();
			}
		}

		private MenuItem menuItemDeleteObject;

		private MenuItem menuItemCopyComponent;

		private MenuItem menuItemPasteComponent;

		private MenuItem menuItemCutComponent;

		private MenuItem menuItemRenameObject;

		private IUndoManager taskService;

		private IEventAggregator eventAggregator;

		private static Point zeroPoint = new Point(0, 0);

		private ObjectContextMenuModel3D dataModel = new ObjectContextMenuModel3D();

		private Point clickPoint;
	}
}

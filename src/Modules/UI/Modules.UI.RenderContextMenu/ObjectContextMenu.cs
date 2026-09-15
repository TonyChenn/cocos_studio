using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.ControlLib;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
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
using Mono.Addins;
using MonoDevelop.Components.Commands;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(IObjectContextMenu))]
	public class ObjectContextMenu : BaseMenu
	{
		public ObjectContextMenuModel DataModel
		{
			get
			{
				if (this.dataModel == null)
				{
					this.dataModel = new ObjectContextMenuModel(this);
				}
				return this.dataModel;
			}
			set
			{
				this.dataModel = value;
			}
		}

		public ObjectContextMenu()
		{
			this.dataModel = new ObjectContextMenuModel(this);
		}

		private MenuItem CreatMenuItem(string header)
		{
			MenuItem menuItem = new MenuItem(header);
			base.Add(menuItem);
			return menuItem;
		}

		protected override void InitMenuItem()
		{
			base.InitMenuItem();
			this.menuItemRenameObject = MenuCreator.CreateMenuItem(GlobalCommand.RenameCmd, true, null);
			base.Append(this.menuItemRenameObject);
			this.InitMoveNodeOrderMenuItem();
			this.menuItemAlignComponent = this.CreatMenuItem(LanguageInfo.UI_ProjectContexMenu_LayoutAlign);
			this.InsertAddPageItem();
			this.InitAddMenuItem();
			this.InitAddWidgetMenuItem();
		}

		[CommandUpdateHandler(CmdEnum.MoveDownCmd)]
		private void MoveDownCmdCanExecute(CommandInfo info)
		{
			info.Enabled = this.dataModel.IsCanMoveIndex(MoveOrderType.Down);
		}

		[CommandUpdateHandler(CmdEnum.MoveUpCmd)]
		private void MoveUpCmdCanExecute(CommandInfo info)
		{
			info.Enabled = this.dataModel.IsCanMoveIndex(MoveOrderType.Up);
		}

		[CommandUpdateHandler(CmdEnum.MoveToTopCmd)]
		private void MoveToTopCmdCanExecute(CommandInfo info)
		{
			info.Enabled = this.dataModel.IsCanMoveIndex(MoveOrderType.Top);
		}

		[CommandUpdateHandler(CmdEnum.MoveToBottomCmd)]
		private void MoveToBottomCmdExecute(CommandInfo info)
		{
			info.Enabled = this.dataModel.IsCanMoveIndex(MoveOrderType.Bottom);
		}

		[CommandHandler(CmdEnum.MoveDownCmd)]
		private void MoveDownCmdExecute()
		{
			this.dataModel.MoveNodeOrder(MoveOrderType.Down);
		}

		[CommandHandler(CmdEnum.MoveUpCmd)]
		private void MoveUpCmdExecute()
		{
			this.dataModel.MoveNodeOrder(MoveOrderType.Up);
		}

		[CommandHandler(CmdEnum.MoveToTopCmd)]
		private void MoveToTopCmdExecute()
		{
			this.dataModel.MoveNodeOrder(MoveOrderType.Top);
		}

		[CommandHandler(CmdEnum.MoveToBottomCmd)]
		private void MoveToBottomExecute()
		{
			this.dataModel.MoveNodeOrder(MoveOrderType.Bottom);
		}

		private void InitMoveNodeOrderMenuItem()
		{
			this.menuItemNodeRanking = this.CreatMenuItem(LanguageInfo.ContextMenu_Ranking);
			this.moveUp = MenuCreator.CreateMenuItem(GlobalCommand.MoveUpCmd, false, LanguageInfo.Animation_BoneTreetMenu_NodeMoveUp);
			this.moveDown = MenuCreator.CreateMenuItem(GlobalCommand.MoveDownCmd, false, LanguageInfo.Animation_BoneTreetMenu_NodeMoveDown);
			this.moveTop = MenuCreator.CreateMenuItem(GlobalCommand.MoveToTopCmd, false, LanguageInfo.Animation_BoneTreetMenu_NodeMoveTop);
			this.moveBottom = MenuCreator.CreateMenuItem(GlobalCommand.MoveToBottomCmd, false, LanguageInfo.Animation_BoneTreetMenu_NodeMoveBottom);
			Menu menu = MenuCreator.CreatePopupMenu();
			menu.Add(this.moveUp);
			menu.Add(this.moveDown);
			menu.Add(this.moveTop);
			menu.Add(this.moveBottom);
			this.menuItemNodeRanking.Submenu = menu;
		}

		private void InitAddWidgetMenuItem()
		{
			this.menuItemAddWidget = this.CreatMenuItem(LanguageInfo.ContexMenu_AddSubComponent);
			IEnumerable<ModelMetaData> modelCollection = ModelManager.Instance.ModelCollection;
			Menu menu = new Menu();
			foreach (ModelMetaData modelMetaData in modelCollection)
			{
				if (typeof(WidgetObject).IsAssignableFrom(modelMetaData.Type))
				{
					string valueBykey = LanguageOption.GetValueBykey(modelMetaData.DisplayName);
					StudioMenuItem studioMenuItem = new StudioMenuItem(valueBykey);
					studioMenuItem.Tag = modelMetaData;
					studioMenuItem.ButtonReleaseEvent += this.menuItemAddObject_Click;
					menu.Add(studioMenuItem);
				}
			}
			this.menuItemAddWidget.Submenu = menu;
			this.menuItemAddWidget.ShowAll();
		}

		private void InitAddMenuItem()
		{
			this.menuItemAddObject = this.CreatMenuItem(LanguageInfo.ContexMenu_AddSubComponent);
			IEnumerable<ModelMetaData> modelCollection = ModelManager.Instance.ModelCollection;
			Menu menu = new Menu();
			int num = 0;
			foreach (ModelMetaData modelMetaData in modelCollection)
			{
				if (modelMetaData.ModelType != EnumModelType.ThreeDimensional)
				{
					string valueBykey = LanguageOption.GetValueBykey(modelMetaData.DisplayName);
					StudioMenuItem studioMenuItem = new StudioMenuItem(valueBykey);
					studioMenuItem.Tag = modelMetaData;
					studioMenuItem.ButtonReleaseEvent += this.menuItemAddObject_Click;
					menu.Add(studioMenuItem);
					if (modelMetaData.Type.Equals(typeof(TextAtlasObject)))
					{
						this.menuItemAddTextAtlasMenu = studioMenuItem;
						this.textAtlasPosition = num;
					}
					num++;
				}
			}
			this.menuItemAddObject.Submenu = menu;
			this.menuItemAddObject.ShowAll();
		}

		private void InitAlignMenuItem()
		{
			if (base.SelectedParentObjectList.Count <= 1)
			{
				this.menuItemAlignComponent.Sensitive = false;
			}
			else
			{
				this.menuItemAlignComponent.Sensitive = true;
			}
			base.Add(this.menuItemAlignComponent);
			this.menuItemAlignComponent.Submenu = null;
			Array values = Enum.GetValues(typeof(AlignType));
			Menu menu = new Menu();
			foreach (object obj in values)
			{
				string valueBykey = LanguageOption.GetValueBykey(obj.ToString());
				StudioMenuItem studioMenuItem = new StudioMenuItem(valueBykey);
				studioMenuItem.Tag = (int)obj;
				if (obj.ToString() == AlignType.Horizontal_Equidistance.ToString() || obj.ToString() == AlignType.Vertical_Equidistance.ToString())
				{
					studioMenuItem.Sensitive = (base.SelectedParentObjectList.Count > 2);
				}
				studioMenuItem.ButtonReleaseEvent += new ButtonReleaseEventHandler(this.menuItemAlignObject_Click);
				if (obj.ToString() == AlignType.Align_Top.ToString() || obj.ToString() == AlignType.Horizontal_Equidistance.ToString() || obj.ToString() == AlignType.Align_Left.ToString())
				{
					menu.Add(new SeparatorMenuItem());
				}
				menu.Add(studioMenuItem);
			}
			this.menuItemAlignComponent.Submenu = menu;
			this.menuItemAlignComponent.ShowAll();
		}

		private void RenderEngineInitCompletedEventHandled(RenderEngineLoadedEventArgs args)
		{
			this.eventAggregator.GetEvent<RenderEngineLoadedEvent>().Unsubscribe(new Action<RenderEngineLoadedEventArgs>(this.RenderEngineInitCompletedEventHandled));
		}

		private void menuItemAddPage_Click(object sender, EventArgs e)
		{
			this.AddSubObject(typeof(PanelObject));
		}

		private void AddSubObject(Type type)
		{
			AbstractNodeObject rootNode = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
			PointF pointF = ObjectContextMenu.zeroPoint;
			if (this.contextMenuArgs != null && this.contextMenuArgs.IsShowOnRenderArea)
			{
				pointF = this.clickPoint;
			}
			else
			{
				pointF = rootNode.TransformToScene(pointF);
			}
			AbstractNodeObject abstractNodeObject = Activator.CreateInstance(type) as AbstractNodeObject;
			abstractNodeObject.Name = Services.ProjectOperations.CurrentSelectedProject.CreateObjectName(abstractNodeObject, "");
			this.DataModel.AddObject(abstractNodeObject, pointF);
		}

		private void menuItemAddObject_Click(object sender, ButtonReleaseEventArgs e)
		{
			StudioMenuItem studioMenuItem = sender as StudioMenuItem;
			if (studioMenuItem.Tag != null)
			{
				PointF pointF = ObjectContextMenu.zeroPoint;
				if (this.contextMenuArgs != null && this.contextMenuArgs.IsShowOnRenderArea)
				{
					pointF = this.clickPoint;
				}
				else
				{
					AbstractNodeObject rootNode = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
					pointF = rootNode.TransformToScene(pointF);
				}
				ModelMetaData modelMetaData = studioMenuItem.Tag as ModelMetaData;
				AbstractNodeObject abstractNodeObject = modelMetaData.CreateObject();
				if (abstractNodeObject != null)
				{
					this.DataModel.AddObject(abstractNodeObject, pointF);
				}
			}
		}

		private void menuItemAlignObject_Click(object sender, EventArgs e)
		{
			StudioMenuItem studioMenuItem = sender as StudioMenuItem;
			if (studioMenuItem.Tag != null)
			{
				if (base.SelectedParentObjectList.Count > 1)
				{
					this.DataModel.AlignObject((AlignType)studioMenuItem.Tag);
				}
			}
		}

		private void UpdateCustomItemStat()
		{
			foreach (object obj in this)
			{
				MenuItem menuItem = (MenuItem)obj;
				IObjectMenuItem objectMenuItem = menuItem as IObjectMenuItem;
				if (null != objectMenuItem)
				{
					objectMenuItem.UpdateMenuItemState();
				}
			}
		}

		private void UpdateMenuItems()
		{
			this.RemoveCustomMenu();
			if (Services.ProjectOperations.CurrentSelectedProject == null)
			{
				this.UpateNoProjectObject();
			}
			else
			{
				base.Add(new SeparatorMenuItem());
				if (base.SelectedParentObjectList == null || base.SelectedParentObjectList.Count == 0)
				{
					this.UpdateNullObject();
				}
				else
				{
					bool canShow = true;
					foreach (VisualObject visualObject in base.SelectedParentObjectList)
					{
						AbstractNodeObject abstractNodeObject = visualObject as AbstractNodeObject;
						if (abstractNodeObject != null && !abstractNodeObject.OperationFlag.HasFlag(OperationMask.AlignFlag))
						{
							canShow = false;
							break;
						}
					}
					if (base.SelectedParentObjectList.Count > 1)
					{
						this.UpdateMultiObject(canShow);
					}
					else
					{
						this.UpdateOneObject();
					}
					AbstractNodeObject rootNode = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
					if (base.SelectedParentObjectList.Contains(rootNode))
					{
						this.menuItemCopyComponent.Sensitive = (this.menuItemDeleteObject.Sensitive = false);
					}
				}
			}
			this.UpdateCustomItemStat();
		}

		private void UpateNoProjectObject()
		{
			this.DeleteAddPageItem();
		}

		private void UpdateNullObject()
		{
			base.Add(this.menuItemAddObject);
		}

		private void UpdateMultiObject(bool canShow = true)
		{
			if (!canShow)
			{
				if (base.Children.Count<Widget>() > 0)
				{
					base.Remove(base.Children[base.Children.Count<Widget>() - 1]);
				}
			}
			else
			{
				this.InitAlignMenuItem();
			}
		}

		private void UpdateOneObject()
		{
			this.UpdateComMenu();
		}

		private void UpdateComMenu()
		{
			AbstractNodeObject abstractNodeObject = base.SelectedObject as AbstractNodeObject;
			if (abstractNodeObject != null)
			{
				if (null != abstractNodeObject as IPlayControl)
				{
					this.InsertItem(new ObjectCheckMenuItem(LanguageInfo.Command_Play, "")
					{
						TriggerObject = abstractNodeObject
					});
				}
				if (abstractNodeObject is PageViewObject)
				{
					this.InsertItem(this.menuItemAddPage);
				}
				else if (abstractNodeObject.GetType() == typeof(ListViewObject))
				{
					this.InsertItem(this.menuItemAddWidget);
				}
				else if (abstractNodeObject.GetType() == typeof(PanelObject) || abstractNodeObject.GetType() == typeof(ScrollViewObject) || abstractNodeObject.GetType() == typeof(FileNodeObject) || abstractNodeObject.GetType() == typeof(AbstractNodeObject))
				{
					this.InsertItem(this.menuItemAddObject);
				}
				else
				{
					this.AddOppositeMenu(this.menuManager.getWidgetMenu(abstractNodeObject.GetType()), abstractNodeObject);
				}
			}
		}

		private void AddOppositeMenu(NodeObjectMenu widgetmenu, AbstractNodeObject guiControl)
		{
			if (widgetmenu != null)
			{
				this.currentWidget = widgetmenu;
				this.currentWidget.TriggerButton = guiControl;
				foreach (MenuItem item in this.currentWidget.GetCustomMenu())
				{
					this.InsertItem(item);
				}
			}
		}

		protected override void UpdateOperationSensitive(bool hasSelected = true)
		{
			bool flag = false;
			if (base.SelectedObjectList != null && base.SelectedObjectList.Count >= 1)
			{
				flag = true;
			}
			base.UpdateOperationSensitive(hasSelected);
			this.menuItemRenameObject.Sensitive = (flag && hasSelected);
			this.menuItemNodeRanking.Sensitive = (base.SelectedObjectList.Count == 1);
			this.UpdateDeprecatedMenu();
		}

		private void UpdateDeprecatedMenu()
		{
			bool flag = true;
			Menu menu = this.menuItemAddObject.Submenu as Menu;
			if (flag)
			{
				if (!menu.Children.Contains(this.menuItemAddTextAtlasMenu))
				{
					menu.Insert(this.menuItemAddTextAtlasMenu, this.textAtlasPosition);
				}
			}
			else if (menu.Children.Contains(this.menuItemAddTextAtlasMenu))
			{
				menu.Remove(this.menuItemAddTextAtlasMenu);
			}
		}

		private void InsertAddPageItem()
		{
			MenuItem widget = new MenuItem(LanguageInfo.ContexMenu_addPage);
			base.Add(widget);
			this.menuItemAddPage = widget;
			this.menuItemAddPage.ButtonReleaseEvent += new ButtonReleaseEventHandler(this.menuItemAddPage_Click);
		}

		private void DeleteAddPageItem()
		{
			if (this.menuItemAddPage != null)
			{
				base.Remove(this.menuItemAddPage);
			}
		}

		private void DeleteAddSpriteItem()
		{
			if (this.menuItemAddSprite != null)
			{
				base.Remove(this.menuItemAddSprite);
			}
		}

		protected override void OnScreenChanged(Screen previous_screen)
		{
			base.OnScreenChanged(previous_screen);
			if (base.SelectedObjectList != null && base.SelectedObjectList.Count > 0)
			{
				if (base.SelectedObjectList.Count == 1)
				{
					this.SelectedOne();
				}
				else
				{
					this.SelectedMultipe();
				}
			}
		}

		private void SelectedOne()
		{
		}

		private void SelectedMultipe()
		{
		}

		private void RemoveCustomMenu()
		{
			List<MenuItem> list = new List<MenuItem>();
			list.Add(this.menuItemCopyComponent);
			list.Add(this.menuItemPasteComponent);
			list.Add(this.menuItemDeleteObject);
			list.Add(this.menuItemCutComponent);
			list.Add(this.menuItemRenameObject);
			list.Add(this.menuItemNodeRanking);
			foreach (object obj in this)
			{
				MenuItem item = (MenuItem)obj;
				if (!list.Contains(item))
				{
					this.DeleteItem(item);
				}
			}
			if (base.SelectedObjectList.Count > 1)
			{
				this.DeleteItem(this.menuItemRenameObject);
			}
			if (this.currentWidget != null)
			{
				this.currentWidget.TriggerButton = null;
			}
		}

		private void InsertItem(MenuItem item)
		{
			if (item != null)
			{
				base.Add(item);
			}
		}

		private void DeleteItem(MenuItem item)
		{
			if (item != null)
			{
				base.Remove(item);
			}
		}

		public override void CanShow(ContextMenuShowingArgs args)
		{
			this.contextMenuArgs = args;
			if (args != null && args.ClickPoint != null)
			{
				this.clickPoint.X = (int)args.ClickPoint.X;
				this.clickPoint.Y = (int)args.ClickPoint.Y;
				if (base.Children.Contains(this.menuItemRenameObject))
				{
					this.DeleteItem(this.menuItemRenameObject);
				}
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
			this.UpdateMenuItems();
			base.ShowAll();
			base.CanShow(args);
		}

		public override void Activated(CocosItem project)
		{
			this.DataModel.RegisterEvent();
			base.Activated(project);
		}

		public override void Deactivated()
		{
			this.DataModel.UnregisterEvent();
			base.Deactivated();
		}

		public override string Type
		{
			get
			{
				return NodeType.Scene.ToString();
			}
		}

		private MenuItem menuItemAddWidget;

		private MenuItem menuItemAddPage;

		private MenuItem menuItemAddObject;

		private MenuItem menuItemAddSprite;

		private MenuItem menuItemAlignComponent;

		private MenuItem menuItemAddTextAtlasMenu;

		private int textAtlasPosition;

		private MenuItem menuItemRenameObject;

		private MenuItem menuItemNodeRanking;

		private MenuItem moveUp;

		private MenuItem moveDown;

		private MenuItem moveTop;

		private MenuItem moveBottom;

		private IUndoManager taskService;

		private new IEventAggregator eventAggregator;

		private static Point zeroPoint = new Point(0, 0);

		private NodeObjectMenu currentWidget = null;

		private MenuManager menuManager = new MenuManager();

		private ContextMenuShowingArgs contextMenuArgs;

		private ObjectContextMenuModel dataModel;

		private Point clickPoint;
	}
}

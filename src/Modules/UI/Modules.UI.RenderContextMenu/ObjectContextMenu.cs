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
	// Token: 0x0200001C RID: 28
	[Extension(typeof(IObjectContextMenu))]
	public class ObjectContextMenu : BaseMenu
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004854 File Offset: 0x00002A54
		// (set) Token: 0x060000AB RID: 171 RVA: 0x0000488A File Offset: 0x00002A8A
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

		// Token: 0x060000AC RID: 172 RVA: 0x00004894 File Offset: 0x00002A94
		public ObjectContextMenu()
		{
			this.dataModel = new ObjectContextMenuModel(this);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000048C0 File Offset: 0x00002AC0
		private MenuItem CreatMenuItem(string header)
		{
			MenuItem menuItem = new MenuItem(header);
			base.Add(menuItem);
			return menuItem;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000048E4 File Offset: 0x00002AE4
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

		// Token: 0x060000AF RID: 175 RVA: 0x00004945 File Offset: 0x00002B45
		[CommandUpdateHandler(CmdEnum.MoveDownCmd)]
		private void MoveDownCmdCanExecute(CommandInfo info)
		{
			info.Enabled = this.dataModel.IsCanMoveIndex(MoveOrderType.Down);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000495B File Offset: 0x00002B5B
		[CommandUpdateHandler(CmdEnum.MoveUpCmd)]
		private void MoveUpCmdCanExecute(CommandInfo info)
		{
			info.Enabled = this.dataModel.IsCanMoveIndex(MoveOrderType.Up);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004971 File Offset: 0x00002B71
		[CommandUpdateHandler(CmdEnum.MoveToTopCmd)]
		private void MoveToTopCmdCanExecute(CommandInfo info)
		{
			info.Enabled = this.dataModel.IsCanMoveIndex(MoveOrderType.Top);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004987 File Offset: 0x00002B87
		[CommandUpdateHandler(CmdEnum.MoveToBottomCmd)]
		private void MoveToBottomCmdExecute(CommandInfo info)
		{
			info.Enabled = this.dataModel.IsCanMoveIndex(MoveOrderType.Bottom);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000499D File Offset: 0x00002B9D
		[CommandHandler(CmdEnum.MoveDownCmd)]
		private void MoveDownCmdExecute()
		{
			this.dataModel.MoveNodeOrder(MoveOrderType.Down);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000049AD File Offset: 0x00002BAD
		[CommandHandler(CmdEnum.MoveUpCmd)]
		private void MoveUpCmdExecute()
		{
			this.dataModel.MoveNodeOrder(MoveOrderType.Up);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000049BD File Offset: 0x00002BBD
		[CommandHandler(CmdEnum.MoveToTopCmd)]
		private void MoveToTopCmdExecute()
		{
			this.dataModel.MoveNodeOrder(MoveOrderType.Top);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000049CD File Offset: 0x00002BCD
		[CommandHandler(CmdEnum.MoveToBottomCmd)]
		private void MoveToBottomExecute()
		{
			this.dataModel.MoveNodeOrder(MoveOrderType.Bottom);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000049E0 File Offset: 0x00002BE0
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

		// Token: 0x060000B8 RID: 184 RVA: 0x00004AA0 File Offset: 0x00002CA0
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

		// Token: 0x060000B9 RID: 185 RVA: 0x00004B8C File Offset: 0x00002D8C
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

		// Token: 0x060000BA RID: 186 RVA: 0x00004CAC File Offset: 0x00002EAC
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

		// Token: 0x060000BB RID: 187 RVA: 0x00004E9C File Offset: 0x0000309C
		private void RenderEngineInitCompletedEventHandled(RenderEngineLoadedEventArgs args)
		{
			this.eventAggregator.GetEvent<RenderEngineLoadedEvent>().Unsubscribe(new Action<RenderEngineLoadedEventArgs>(this.RenderEngineInitCompletedEventHandled));
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004EBC File Offset: 0x000030BC
		private void menuItemAddPage_Click(object sender, EventArgs e)
		{
			this.AddSubObject(typeof(PanelObject));
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004ED0 File Offset: 0x000030D0
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

		// Token: 0x060000BE RID: 190 RVA: 0x00004F68 File Offset: 0x00003168
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

		// Token: 0x060000BF RID: 191 RVA: 0x0000501C File Offset: 0x0000321C
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

		// Token: 0x060000C0 RID: 192 RVA: 0x00005070 File Offset: 0x00003270
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

		// Token: 0x060000C1 RID: 193 RVA: 0x000050E8 File Offset: 0x000032E8
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

		// Token: 0x060000C2 RID: 194 RVA: 0x00005254 File Offset: 0x00003454
		private void UpateNoProjectObject()
		{
			this.DeleteAddPageItem();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000525E File Offset: 0x0000345E
		private void UpdateNullObject()
		{
			base.Add(this.menuItemAddObject);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005270 File Offset: 0x00003470
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

		// Token: 0x060000C5 RID: 197 RVA: 0x000052BD File Offset: 0x000034BD
		private void UpdateOneObject()
		{
			this.UpdateComMenu();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000052C8 File Offset: 0x000034C8
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

		// Token: 0x060000C7 RID: 199 RVA: 0x00005408 File Offset: 0x00003608
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

		// Token: 0x060000C8 RID: 200 RVA: 0x0000548C File Offset: 0x0000368C
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

		// Token: 0x060000C9 RID: 201 RVA: 0x000054F8 File Offset: 0x000036F8
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

		// Token: 0x060000CA RID: 202 RVA: 0x00005574 File Offset: 0x00003774
		private void InsertAddPageItem()
		{
			MenuItem widget = new MenuItem(LanguageInfo.ContexMenu_addPage);
			base.Add(widget);
			this.menuItemAddPage = widget;
			this.menuItemAddPage.ButtonReleaseEvent += new ButtonReleaseEventHandler(this.menuItemAddPage_Click);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000055B4 File Offset: 0x000037B4
		private void DeleteAddPageItem()
		{
			if (this.menuItemAddPage != null)
			{
				base.Remove(this.menuItemAddPage);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000055E0 File Offset: 0x000037E0
		private void DeleteAddSpriteItem()
		{
			if (this.menuItemAddSprite != null)
			{
				base.Remove(this.menuItemAddSprite);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000560C File Offset: 0x0000380C
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

		// Token: 0x060000CE RID: 206 RVA: 0x0000566A File Offset: 0x0000386A
		private void SelectedOne()
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000566D File Offset: 0x0000386D
		private void SelectedMultipe()
		{
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005670 File Offset: 0x00003870
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

		// Token: 0x060000D1 RID: 209 RVA: 0x0000577C File Offset: 0x0000397C
		private void InsertItem(MenuItem item)
		{
			if (item != null)
			{
				base.Add(item);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000579C File Offset: 0x0000399C
		private void DeleteItem(MenuItem item)
		{
			if (item != null)
			{
				base.Remove(item);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000057BC File Offset: 0x000039BC
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

		// Token: 0x060000D4 RID: 212 RVA: 0x00005894 File Offset: 0x00003A94
		public override void Activated(CocosItem project)
		{
			this.DataModel.RegisterEvent();
			base.Activated(project);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000058AB File Offset: 0x00003AAB
		public override void Deactivated()
		{
			this.DataModel.UnregisterEvent();
			base.Deactivated();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000058C4 File Offset: 0x00003AC4
		public override string Type
		{
			get
			{
				return NodeType.Scene.ToString();
			}
		}

		// Token: 0x0400004C RID: 76
		private MenuItem menuItemAddWidget;

		// Token: 0x0400004D RID: 77
		private MenuItem menuItemAddPage;

		// Token: 0x0400004E RID: 78
		private MenuItem menuItemAddObject;

		// Token: 0x0400004F RID: 79
		private MenuItem menuItemAddSprite;

		// Token: 0x04000050 RID: 80
		private MenuItem menuItemAlignComponent;

		// Token: 0x04000051 RID: 81
		private MenuItem menuItemAddTextAtlasMenu;

		// Token: 0x04000052 RID: 82
		private int textAtlasPosition;

		// Token: 0x04000053 RID: 83
		private MenuItem menuItemRenameObject;

		// Token: 0x04000054 RID: 84
		private MenuItem menuItemNodeRanking;

		// Token: 0x04000055 RID: 85
		private MenuItem moveUp;

		// Token: 0x04000056 RID: 86
		private MenuItem moveDown;

		// Token: 0x04000057 RID: 87
		private MenuItem moveTop;

		// Token: 0x04000058 RID: 88
		private MenuItem moveBottom;

		// Token: 0x04000059 RID: 89
		private IUndoManager taskService;

		// Token: 0x0400005A RID: 90
		private new IEventAggregator eventAggregator;

		// Token: 0x0400005B RID: 91
		private static Point zeroPoint = new Point(0, 0);

		// Token: 0x0400005C RID: 92
		private NodeObjectMenu currentWidget = null;

		// Token: 0x0400005D RID: 93
		private MenuManager menuManager = new MenuManager();

		// Token: 0x0400005E RID: 94
		private ContextMenuShowingArgs contextMenuArgs;

		// Token: 0x0400005F RID: 95
		private ObjectContextMenuModel dataModel;

		// Token: 0x04000060 RID: 96
		private Point clickPoint;
	}
}

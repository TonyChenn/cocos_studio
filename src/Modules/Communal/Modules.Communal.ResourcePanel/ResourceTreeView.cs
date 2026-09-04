using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Model.Event;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UserStatistics;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide.Tasks;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200002A RID: 42
	public class ResourceTreeView : ScrolledWindow
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000793F File Offset: 0x00005B3F
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00007947 File Offset: 0x00005B47
		public ExtendTreeView Tree
		{
			get
			{
				return this.tree;
			}
			set
			{
				this.tree = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007950 File Offset: 0x00005B50
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00007958 File Offset: 0x00005B58
		public TreeStore Store
		{
			get
			{
				return this.store;
			}
			set
			{
				this.store = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007961 File Offset: 0x00005B61
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00007969 File Offset: 0x00005B69
		public TreeViewColumn Completecolumn
		{
			get
			{
				return this.complete_column;
			}
			set
			{
				this.complete_column = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007972 File Offset: 0x00005B72
		// (set) Token: 0x06000167 RID: 359 RVA: 0x0000797A File Offset: 0x00005B7A
		public ResourceTreeBuilder Builder
		{
			get
			{
				return this.builder;
			}
			set
			{
				this.builder = value;
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007984 File Offset: 0x00005B84
		public ResourceTreeView()
		{
			this.tree = new ExtendTreeView();
			base.CanFocus = true;
			this.tree.CanFocus = true;
			base.Events = EventMask.AllEventsMask;
			this.Initialize();
			this.builder = new ResourceTreeBuilder(this);
			Gtk.Drag.DestSet(this, DestDefaults.All, ResourceTreeView.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
			Services.MainWindow.ButtonPressEvent += this.MainWindow_ButtonPressEvent;
			Services.ProjectOperations.CurrentSelectedSolutionClosed += this.ProjectOperations_CurrentSelectedSolutionClosed;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007A17 File Offset: 0x00005C17
		private void ProjectOperations_CurrentSelectedSolutionClosed(object sender, SolutionEventArgs e)
		{
			this.builder.Clear();
			this.ResourceWidget.Reset();
			this.text_render.NodeInfo = null;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007A3C File Offset: 0x00005C3C
		[ConnectBefore]
		private void MainWindow_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (this.IsRanameStatus)
			{
				double xroot = args.Event.XRoot;
				double yroot = args.Event.YRoot;
				int width = base.Allocation.Width;
				int height = base.Allocation.Height;
				int num;
				int num2;
				base.GdkWindow.GetOrigin(out num, out num2);
				if (xroot < (double)num || xroot > (double)(num + width) || yroot < (double)num2 || yroot > (double)(num2 + height))
				{
					base.HasFocus = true;
				}
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007AB4 File Offset: 0x00005CB4
		public ITreeBuild LoadTree(object nodeObject)
		{
			this.ResourceWidget.Reset();
			this.builder.Clear();
			this.builder.AddChild(nodeObject, true);
			this.builder.Expanded = true;
			this.InitialSelection();
			return this.builder;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007AF1 File Offset: 0x00005CF1
		public ITreeBuild AddChild(object nodeObject)
		{
			this.builder.AddChild(nodeObject, true);
			this.builder.Expanded = true;
			this.InitialSelection();
			return this.builder;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007B18 File Offset: 0x00005D18
		public void RemoveChild(object nodeObject)
		{
			if (this.builder.MoveToObject(nodeObject))
			{
				this.builder.Remove();
				this.InitialSelection();
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007B3C File Offset: 0x00005D3C
		private void InitialSelection()
		{
			TreeIter iter;
			if (this.tree.Selection.CountSelectedRows() == 0 && this.Tree.CurrentModel.GetIterFirst(out iter))
			{
				TreePath path = this.Tree.CurrentModel.GetPath(iter);
				this.tree.SetCursor(path, this.tree.Columns[0], false);
				this.tree.ExpandRow(path, false);
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007BAC File Offset: 0x00005DAC
		public object GetValueByTreePath(TreePath treePath)
		{
			TreeIter iter;
			this.Tree.CurrentModel.GetIter(out iter, treePath);
			NodeInfo nodeInfo = this.Tree.CurrentModel.GetValue(iter, 0) as NodeInfo;
			if (nodeInfo == null)
			{
				return null;
			}
			return nodeInfo.DataItem;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007BF0 File Offset: 0x00005DF0
		public void Initialize()
		{
			this.builderContext = new TreeBuilderContext(this);
			this.store = new TreeStore(new Type[]
			{
				typeof(NodeInfo)
			});
			this.SetBuilders(ResourcePanelManager.Instance.NodeBuildes);
			this.tree.Model = this.store;
			this.tree.Selection.Mode = SelectionMode.Multiple;
			this.store.DefaultSortFunc = new TreeIterCompareFunc(this.CompareNodes);
			this.store.SetSortColumnId(-1, SortType.Ascending);
			this.tree.HeadersVisible = false;
			this.complete_column = new TreeViewColumn();
			this.text_render = new CustomCellRendererText(this);
			this.text_render.Ypad = 0U;
			this.text_render.Zoom = 1.0;
			this.text_render.CustomFont = this.tree.Style.FontDescription;
			this.tree.ColumnsAutosize();
			this.text_render.EditingStarted += this.HandleEditingStarted;
			this.text_render.Edited += this.HandleOnEdit;
			this.text_render.EditingCanceled += this.HandleOnEditCancelled;
			this.tree.MotionNotifyEvent += this.HandleMotionNotifyEvent;
			this.tree.LeaveNotifyEvent += this.HandleLeaveNotifyEvent;
			this.complete_column.PackStart(this.text_render, true);
			this.complete_column.AddAttribute(this.text_render, "node-info", 0);
			base.Hadjustment.StepIncrement += 20.0;
			this.tree.AppendColumn(this.complete_column);
			this.tree.ModifyBase(StateType.Normal, new Color(0, 0, byte.MaxValue));
			base.Add(this.tree);
			base.ShowAll();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007DDC File Offset: 0x00005FDC
		[ConnectBefore]
		private void HandleMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			bool flag = false;
			TreePath path;
			TreeViewColumn treeViewColumn;
			int num;
			int num2;
			TreeIter treeIter;
			if (this.tree.GetPathAtPos((int)args.Event.X, (int)args.Event.Y, out path, out treeViewColumn, out num, out num2) && this.store.GetIter(out treeIter, path))
			{
				NodeInfo nodeInfo = (NodeInfo)this.store.GetValue(treeIter, 0);
				if (nodeInfo != null && !string.IsNullOrWhiteSpace(nodeInfo.StatusMessage) && nodeInfo.IconInfo.StatusIconInternal != null)
				{
					Rectangle cellArea = this.tree.GetCellArea(path, this.tree.Columns[0]);
					this.tree.QueueDrawArea(cellArea.X, cellArea.Y, cellArea.Width, cellArea.Height);
					Rectangle cellArea2 = this.tree.GetCellArea(path, this.tree.Columns[0]);
					int num3;
					int width;
					treeViewColumn.CellGetPosition(this.text_render, out num3, out width);
					cellArea2.X += num3;
					cellArea2.Width = width;
					Rectangle statusIconArea = this.text_render.GetStatusIconArea(this.tree, cellArea2);
					if (num >= statusIconArea.X && num <= statusIconArea.Right)
					{
						double value = base.Hadjustment.Value;
						statusIconArea.X -= (int)value;
						this.ShowStatusMessage(treeIter, statusIconArea, nodeInfo);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.HideStatusMessage();
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007F54 File Offset: 0x00006154
		[ConnectBefore]
		private void HandleLeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			this.HideStatusMessage();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007F5C File Offset: 0x0000615C
		private void ShowStatusMessage(TreeIter it, Rectangle rect, NodeInfo info)
		{
			if (this.statusMessageVisible && this.store.GetPath(it).Equals(this.store.GetPath(this.statusIconIter)))
			{
				return;
			}
			if (this.statusPopover != null)
			{
				this.statusPopover.Destroy();
			}
			this.statusMessageVisible = true;
			this.statusIconIter = it;
			this.statusPopover = new TooltipPopoverWindow
			{
				ShowArrow = true,
				Text = info.StatusMessage,
				Severity = new TaskSeverity?(TaskSeverity.Warning)
			};
			rect.Y += 2;
			this.statusPopover.ShowPopup(this.tree, rect, PopupPosition.Bottom);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008005 File Offset: 0x00006205
		private void HideStatusMessage()
		{
			if (this.statusMessageVisible)
			{
				this.statusMessageVisible = false;
				this.statusPopover.Destroy();
				this.statusPopover = null;
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008028 File Offset: 0x00006228
		public int CompareNodes(TreeModel model, TreeIter a, TreeIter b)
		{
			ResourceItem resourceItem = this.builder.GetDateItemByIter(a) as ResourceItem;
			ResourceItem resourceItem2 = this.builder.GetDateItemByIter(b) as ResourceItem;
			if ((resourceItem is ResourceFolder && resourceItem2 is ResourceFolder) || (resourceItem is ResourceFile && resourceItem2 is ResourceFile))
			{
				return string.Compare(resourceItem.Name, resourceItem2.Name);
			}
			if (resourceItem is ResourceFolder)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000080B4 File Offset: 0x000062B4
		internal NodeBuilder[] GetBuilderChain(Type type)
		{
			NodeBuilder[] array;
			this.builderChains.TryGetValue(type, out array);
			if (array == null)
			{
				IEnumerable<NodeBuilder> enumerable = from n in ResourcePanelManager.Instance.NodeBuildes
				where n.NodeDataType == type
				select n;
				if (enumerable != null && enumerable.Count<NodeBuilder>() > 0)
				{
					array = enumerable.ToArray<NodeBuilder>();
				}
				else
				{
					array = this.GetBuilderChain(type.BaseType);
				}
				this.builderChains[type] = array;
			}
			return array;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008160 File Offset: 0x00006360
		internal NodeBuilder GetBuilder(Type type)
		{
			if (!typeof(ResourceItem).IsAssignableFrom(type) && !typeof(Solution).IsAssignableFrom(type))
			{
				return null;
			}
			NodeBuilder[] builderChain = this.GetBuilderChain(type);
			NodeBuilder nodeBuilder = builderChain.FirstOrDefault((NodeBuilder n) => n.NodeDataType == type);
			if (nodeBuilder == null)
			{
				nodeBuilder = this.GetBuilder(type.BaseType);
			}
			return nodeBuilder;
		}

		internal Pixbuf GetResourceIcon(ResourceItem resourceItem)
		{
			TreeIter iter;
			if (resourceItem == null || !this.builder.GetFirstNode(resourceItem, out iter))
			{
				return null;
			}
			NodeInfo nodeInfo = this.store.GetValue(iter, 0) as NodeInfo;
			if (nodeInfo == null || nodeInfo.IconInfo == null || nodeInfo.IconInfo.ExpandIcon == null)
			{
				return null;
			}
			return nodeInfo.IconInfo.ExpandIcon.GetPixbuf();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000081E0 File Offset: 0x000063E0
		private void SetBuilders(IList<NodeBuilder> buildersArray)
		{
			foreach (NodeBuilder nodeBuilder in buildersArray)
			{
				nodeBuilder.SetContext(this.builderContext);
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008230 File Offset: 0x00006430
		private bool IsMutilSelecteState()
		{
			return this.tree.Selection.CountSelectedRows() > 1;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008248 File Offset: 0x00006448
		[CommandHandler(CmdEnum.RefreshCmd)]
		private void OnRefreshCmd()
		{
			this.builder.UpdateAll();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008255 File Offset: 0x00006455
		[CommandUpdateHandler(CmdEnum.RefreshCmd)]
		private void RefreshCmd_Update(CommandInfo info)
		{
			if (Services.ProjectOperations.CurrentSelectedSolution == null)
			{
				info.Enabled = false;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000826A File Offset: 0x0000646A
		[CommandHandler(CmdEnum.ResOpenDirCmd)]
		private void OnOpenInResourceManageCmd()
		{
			this.builder.OpenInResourceManageHanlder();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008278 File Offset: 0x00006478
		[CommandHandler(CmdEnum.CreateSerialFrameCmd)]
		private void OnCreateSerialFrame()
		{
			List<ResourceItem> currentSelectes = this.builder.GetCurrentSelectes();
			Services.EventsService.GetEvent<CreateSerialFrameEvent>().Publish(new CreateSerialFrameEventArgs(null, currentSelectes));
			Tracker.Add(ViewRegions.ResourcePanel, "RightMenuCreateSpriteSheetAnimation", "", "");
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000082BC File Offset: 0x000064BC
		[CommandUpdateHandler(CmdEnum.CreateSerialFrameCmd)]
		private void CreateSerialFrameCanExecuted(CommandInfo info)
		{
			List<ResourceItem> currentSelectes = this.builder.GetCurrentSelectes();
			if (currentSelectes == null || currentSelectes.Count < 2 || Services.ProjectOperations.CurrentSelectedProject == null || !(Services.ProjectOperations.CurrentSelectedProject.CocosFile is GameFile) || Services.ProjectOperations.CurrentSelectedProject.Is3DFile())
			{
				info.Enabled = false;
				return;
			}
			foreach (ResourceItem resourceItem in currentSelectes)
			{
				if ((!(resourceItem is ImageFile) && !(resourceItem is PlistImageFile)) || (resourceItem as ResourceFile).DataError != null)
				{
					info.Enabled = false;
					break;
				}
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000837C File Offset: 0x0000657C
		[CommandHandler(CmdEnum.NewFolderCmd)]
		private void NewFolderCmd()
		{
			this.builder.NewFolderHanlder();
			Tracker.Add(ViewRegions.ResourcePanel, "RightMenuNewFolder", "", "");
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000083A0 File Offset: 0x000065A0
		[CommandUpdateHandler(CmdEnum.ImportFileCmd)]
		[CommandUpdateHandler(CmdEnum.ImportDirCmd)]
		[CommandUpdateHandler(CmdEnum.NewFolderCmd)]
		[CommandUpdateHandler(CmdEnum.NewFileCmd)]
		private void NewFileCmd_Update(CommandInfo info)
		{
			if (Services.ProjectOperations.CurrentSelectedSolution == null || this.IsMutilSelecteState())
			{
				info.Enabled = false;
				return;
			}
			if (this.Tree.IsSearchState)
			{
				info.Enabled = false;
				return;
			}
			TreeIter treeIter;
			NodeInfo nodeInfo = this.builder.GetFristSelecteValue(0, out treeIter) as NodeInfo;
			if (nodeInfo != null)
			{
				info.Enabled = this.ExisteResource(nodeInfo.DataItem);
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008408 File Offset: 0x00006608
		[CommandUpdateHandler(CmdEnum.ResOpenDirCmd)]
		private void OpenCmd_Update(CommandInfo info)
		{
			if (Services.ProjectOperations.CurrentSelectedSolution == null || this.IsMutilSelecteState())
			{
				info.Enabled = false;
				return;
			}
			TreeIter treeIter;
			NodeInfo nodeInfo = this.builder.GetFristSelecteValue(0, out treeIter) as NodeInfo;
			if (nodeInfo != null)
			{
				info.Enabled = this.ExisteResource(nodeInfo.DataItem);
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000845C File Offset: 0x0000665C
		private bool ExisteResource(object ressourceData)
		{
			if (ressourceData is Solution)
			{
				if (!Directory.Exists(((Solution)ressourceData).BaseDirectory))
				{
					return false;
				}
			}
			else if (ressourceData is ResourceFile)
			{
				if (!File.Exists(((ResourceFile)ressourceData).FullPath))
				{
					return false;
				}
			}
			else if (ressourceData is ResourceFolder)
			{
				if (ressourceData is PlistImageFolder)
				{
					if (!Directory.Exists(((ResourceFolder)ressourceData).BaseDirectory))
					{
						return false;
					}
				}
				else if (!Directory.Exists(((ResourceFolder)ressourceData).FullPath))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000084E4 File Offset: 0x000066E4
		[CommandUpdateHandler(CmdEnum.DeleteCmd2)]
		[CommandUpdateHandler(CmdEnum.DeleteCmd)]
		private void DeleteCmdCanExecute(CommandInfo info)
		{
			if (this.ResourceWidget.IsGridViewActive && !this.ResourceWidget.GridHasSelection)
			{
				info.Enabled = false;
				info.Bypass = true;
				return;
			}
			if (this.IsRanameStatus || this.ResourceWidget.SearchBoxFocus)
			{
				info.Enabled = false;
				info.Bypass = true;
				return;
			}
			if (Services.ProjectOperations.CurrentSelectedSolution == null)
			{
				info.Enabled = false;
				info.Bypass = true;
				return;
			}
			NodeInfo nodeInfo = this.builder.GetFristSelecteValue(0) as NodeInfo;
			if (nodeInfo != null && nodeInfo.DataItem != null)
			{
				NodeBuilder nodeBuilder = this.GetBuilder(nodeInfo.DataItem.GetType());
				if (nodeBuilder != null)
				{
					info.Enabled = nodeBuilder.CanDelete();
					return;
				}
			}
			else
			{
				info.Enabled = false;
				info.Bypass = true;
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000857D File Offset: 0x0000677D
		[CommandHandler(CmdEnum.DeleteCmd2)]
		[CommandHandler(CmdEnum.DeleteCmd)]
		private void DeleteCmdExecute()
		{
			this.builder.DeleteResourceHandler();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000858A File Offset: 0x0000678A
		[CommandHandler(CmdEnum.RenameCmd)]
		public void RenameCmd()
		{
			if (this.ResourceWidget.IsGridViewActive)
			{
				this.ResourceWidget.StartGridLabelEdit();
				Tracker.Add(ViewRegions.ResourcePanel, "RightMenuRename", "", "");
				return;
			}
			this.IsRanameStatus = true;
			GLib.Timeout.Add(20U, new TimeoutHandler(this.wantFocus));
			Tracker.Add(ViewRegions.ResourcePanel, "RightMenuRename", "", "");
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000085BC File Offset: 0x000067BC
		[CommandUpdateHandler(CmdEnum.RenameCmd)]
		public void RenameCmd(CommandInfo info)
		{
			if (this.ResourceWidget.IsGridViewActive && !this.ResourceWidget.GridHasSelection)
			{
				info.Enabled = false;
				return;
			}
			if (Services.ProjectOperations.CurrentSelectedSolution == null || this.IsMutilSelecteState() || this.Tree.Selection.GetSelectedRows().Length == 0)
			{
				info.Enabled = false;
				return;
			}
			NodeInfo nodeInfo = this.builder.GetFristSelecteValue(0) as NodeInfo;
			if (nodeInfo != null && nodeInfo.DataItem != null)
			{
				NodeBuilder nodeBuilder = this.GetBuilder(nodeInfo.DataItem.GetType());
				if (nodeBuilder != null)
				{
					bool flag = nodeBuilder.CanRename();
					info.Enabled = (flag && this.ExisteResource(nodeInfo));
					if (!flag)
					{
						return;
					}
				}
			}
			else
			{
				info.Enabled = false;
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008650 File Offset: 0x00006850
		private void TimeLineCanMove(CommandInfo info)
		{
			if (this.IsRanameStatus)
			{
				info.Enabled = false;
				info.Bypass = true;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008668 File Offset: 0x00006868
		[CommandHandler(CmdEnum.DuplicateCmd)]
		public void DuplicateCmd()
		{
			List<ResourceItem> currentSelectes = this.builder.GetCurrentSelectes();
			this.copyItems = this.builder.FilterChildren(currentSelectes);
			if (this.copyItems != null)
			{
				List<ResourceItem> list = new List<ResourceItem>();
				foreach (ResourceItem resourceItem in this.copyItems)
				{
					TreeIter iter;
					this.builder.GetFirstNode(resourceItem.Parent, out iter);
					ResourceItem resourceItem2 = StaticVariable.CopyScene(resourceItem.Parent, resourceItem);
					if (resourceItem2 != null)
					{
						this.builder.AddChildToTreeIter(resourceItem2, iter, false);
						list.Add(resourceItem2);
					}
				}
				this.builder.SetSelecteResources(list);
			}
			Services.ProjectOperations.CurrentSelectedSolution.Save(Services.ProgressMonitors.Default);
			Tracker.Add(ViewRegions.ResourcePanel, "RightMenuDuplicateFile", "", "");
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008758 File Offset: 0x00006958
		[CommandUpdateHandler(CmdEnum.DuplicateCmd)]
		public void DuplicateCmd(CommandInfo info)
		{
			List<ResourceItem> currentSelectes = this.builder.GetCurrentSelectes();
			bool enabled = false;
			if (currentSelectes != null)
			{
				foreach (ResourceItem resourceItem in currentSelectes)
				{
					CocosItem cocosItem = resourceItem as CocosItem;
					if (cocosItem == null || !(cocosItem.ContentType != "Plist") || cocosItem.DataError != null)
					{
						enabled = false;
						break;
					}
					enabled = true;
				}
			}
			info.Enabled = enabled;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000087E4 File Offset: 0x000069E4
		private bool wantFocus()
		{
			this.tree.GrabFocus();
			this.StartLabelEditInternal();
			return false;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000088AC File Offset: 0x00006AAC
		public void StartLabelEditInternal()
		{
			TreeIter iter;
			NodeInfo nodeInfo = this.builder.GetFristSelecteValue(0, out iter) as NodeInfo;
			if (nodeInfo != null)
			{
				ResourceItem item = nodeInfo.DataItem as ResourceItem;
				Idle.Add(delegate
				{
					Entry entry = this.currentLabelEditable;
					if (item == null || entry == null || string.IsNullOrWhiteSpace(item.Name))
					{
						return false;
					}
					if (item is ResourceFile)
					{
						System.IO.Path.GetExtension(item.FullPath);
						int length = System.IO.Path.GetFileNameWithoutExtension(item.FullPath).Length;
						if (length > 0)
						{
							entry.SelectRegion(0, length);
						}
						entry.DeleteText(length, item.Name.Length);
					}
					else
					{
						entry.SelectRegion(0, item.Name.Length);
					}
					return false;
				});
				this.text_render.Editable = true;
				this.tree.SetCursor(this.Tree.CurrentModel.GetPath(iter), this.complete_column, true);
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000892F File Offset: 0x00006B2F
		[ConnectBefore]
		private void HandleEditingStarted(object o, EditingStartedArgs e)
		{
			this.currentLabelEditable = (e.Editable as Entry);
			this.currentLabelEditable.MaxLength = 50;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008950 File Offset: 0x00006B50
		[ConnectBefore]
		private void HandleOnEdit(object o, EditedArgs e)
		{
			this.text_render.Editable = false;
			this.currentLabelEditable = null;
			TreeIter iter;
			if (!this.Tree.CurrentModel.GetIterFromString(out iter, e.Path))
			{
				throw new Exception("Error calculating iter for path " + e.Path);
			}
			this.Raname(iter, e.NewText, true);
			this.IsRanameStatus = false;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000089B8 File Offset: 0x00006BB8
		private void Raname(TreeIter iter, string newName, bool isOnEdit = false)
		{
			try
			{
				if (RegexModel.IsSystemReserveName(newName))
				{
					MessageBox.Show(LanguageInfo.MessageBox215_WindowsNameLimit, MessageBoxImage.Other, null, null);
				}
				else
				{
					this.text_render.Editable = false;
					this.currentLabelEditable = null;
					string pattern = "^[A-Za-z0-9, ._@-]+$";
					Regex regex = new Regex(pattern);
					if (string.IsNullOrWhiteSpace(newName))
					{
						MessageBox.Show(LanguageInfo.MessageBox_Content67, MessageBoxImage.Other, null, null);
					}
					else if (!regex.IsMatch(newName))
					{
						MessageBox.Show(LanguageInfo.MessageBox201_CanOnlyUseEnNum, MessageBoxImage.Other, null, null);
					}
					else if (!FileService.IsValidFileName(newName))
					{
						MessageBox.Show(LanguageInfo.MessageBox201_CanOnlyUseEnNum, MessageBoxImage.Other, null, null);
					}
					else if (newName != null && newName.Length > 0)
					{
						ResourceItem resourceItem = null;
						if (isOnEdit)
						{
							NodeInfo nodeInfo = this.Tree.CurrentModel.GetValue(iter, 0) as NodeInfo;
							if (nodeInfo != null)
							{
								resourceItem = (nodeInfo.DataItem as ResourceItem);
							}
						}
						else
						{
							NodeInfo nodeInfo2 = this.store.GetValue(iter, 0) as NodeInfo;
							if (nodeInfo2 != null)
							{
								resourceItem = (nodeInfo2.DataItem as ResourceItem);
							}
						}
						if (resourceItem != null)
						{
							string directoryName = System.IO.Path.GetDirectoryName(resourceItem.FullPath);
							string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(resourceItem.FullPath);
							if (!(fileNameWithoutExtension == newName))
							{
								if (!string.Equals(fileNameWithoutExtension, newName, StringComparison.CurrentCultureIgnoreCase))
								{
									string extension = System.IO.Path.GetExtension(resourceItem.FullPath);
									string text = System.IO.Path.Combine(directoryName, newName + extension);
									if ((resourceItem is ResourceFile && File.Exists(text)) || (resourceItem is ResourceFolder && Directory.Exists(text)) || this.IsSubitem(resourceItem.Parent as ResourceFolder, text))
									{
										MessageBox.Show(LanguageInfo.MessageBox193_SameNameExist.Replace("<", "&lt;"), MessageBoxImage.Other, null, null);
										return;
									}
								}
								NodeBuilder nodeBuilder = this.GetBuilder(resourceItem.GetType());
								if (nodeBuilder != null && nodeBuilder.CanRename())
								{
									nodeBuilder.Rename(this.builder, resourceItem, newName);
									Services.ProjectsService.ResourceChangeService.NotifyResourceChanged();
								}
								this.builder.Update(resourceItem);
							}
						}
					}
				}
			}
			catch (IOException exception)
			{
				LogConfig.Output.Debug(LanguageInfo.MessageBox259_ProcessCannotAccess, exception);
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
			}
		}

		internal void RenameResource(ResourceItem resourceItem, string newName)
		{
			TreeIter iter;
			if (resourceItem != null && this.builder.GetFirstNode(resourceItem, out iter))
			{
				this.Raname(iter, newName, false);
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008C08 File Offset: 0x00006E08
		private bool IsSubitem(ResourceFolder parent, FilePath path)
		{
			if (parent.Items.Count > 0)
			{
				foreach (ResourceItem resourceItem in parent.Items)
				{
					if (path == resourceItem.FullPath)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008C78 File Offset: 0x00006E78
		[ConnectBefore]
		private void HandleOnEditCancelled(object s, EventArgs args)
		{
			string text = this.currentLabelEditable.Text;
			this.Raname(this.builder.CurrentIter, text, false);
			this.editingText = false;
			this.text_render.Editable = false;
			this.currentLabelEditable = null;
			this.IsRanameStatus = false;
		}

		// Token: 0x0400005D RID: 93
		public const int NodeInfoColumn = 0;

		// Token: 0x0400005E RID: 94
		private Dictionary<Type, NodeBuilder[]> builderChains = new Dictionary<Type, NodeBuilder[]>();

		// Token: 0x0400005F RID: 95
		private ExtendTreeView tree;

		// Token: 0x04000060 RID: 96
		private TreeStore store;

		// Token: 0x04000061 RID: 97
		private TreeViewColumn complete_column;

		// Token: 0x04000062 RID: 98
		private CustomCellRendererImage icon_render;

		// Token: 0x04000063 RID: 99
		private CustomCellRendererText text_render;

		// Token: 0x04000064 RID: 100
		private ResourceTreeBuilder builder;

		// Token: 0x04000065 RID: 101
		public ResourceWidget ResourceWidget;

		// Token: 0x04000066 RID: 102
		private static TargetEntry[] target_tableWindows = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		// Token: 0x04000067 RID: 103
		private bool statusMessageVisible;

		// Token: 0x04000068 RID: 104
		private TreeIter statusIconIter;

		// Token: 0x04000069 RID: 105
		private TooltipPopoverWindow statusPopover;

		// Token: 0x0400006A RID: 106
		private bool IsRanameStatus;

		// Token: 0x0400006B RID: 107
		private List<ResourceItem> copyItems;

		// Token: 0x0400006C RID: 108
		private Entry currentLabelEditable;

		// Token: 0x0400006D RID: 109
		private bool editingText;

		// Token: 0x0400006E RID: 110
		private TreeBuilderContext builderContext;
	}
}

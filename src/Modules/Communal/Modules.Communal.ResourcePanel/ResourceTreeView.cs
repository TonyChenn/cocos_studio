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
	public class ResourceTreeView : ScrolledWindow
	{
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

		private void ProjectOperations_CurrentSelectedSolutionClosed(object sender, SolutionEventArgs e)
		{
			this.builder.Clear();
			this.ResourceWidget.Reset();
			this.text_render.NodeInfo = null;
		}

		[ConnectBefore]
		private void MainWindow_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (this.IsRanameStatus)
			{
				double xroot = args.Event.XRoot;
				double yroot = args.Event.YRoot;
				int width = base.Allocation.Width;
				int height = base.Allocation.Height;
                base.GdkWindow.GetOrigin(out int num, out int num2);
                if (xroot < (double)num || xroot > (double)(num + width) || yroot < (double)num2 || yroot > (double)(num2 + height))
				{
					base.HasFocus = true;
				}
			}
		}

		public ITreeBuild LoadTree(object nodeObject)
		{
			this.ResourceWidget.Reset();
			this.builder.Clear();
			this.builder.AddChild(nodeObject, true);
			this.builder.Expanded = true;
			this.InitialSelection();
			return this.builder;
		}

		public ITreeBuild AddChild(object nodeObject)
		{
			this.builder.AddChild(nodeObject, true);
			this.builder.Expanded = true;
			this.InitialSelection();
			return this.builder;
		}

		public void RemoveChild(object nodeObject)
		{
			if (this.builder.MoveToObject(nodeObject))
			{
				this.builder.Remove();
				this.InitialSelection();
			}
		}

		private void InitialSelection()
		{
            if (this.tree.Selection.CountSelectedRows() == 0 && this.Tree.CurrentModel.GetIterFirst(out TreeIter iter))
            {
                TreePath path = this.Tree.CurrentModel.GetPath(iter);
                this.tree.SetCursor(path, this.tree.Columns[0], false);
                this.tree.ExpandRow(path, false);
            }
        }

		public object GetValueByTreePath(TreePath treePath)
		{
            this.Tree.CurrentModel.GetIter(out TreeIter iter, treePath);
            NodeInfo nodeInfo = this.Tree.CurrentModel.GetValue(iter, 0) as NodeInfo;
			if (nodeInfo == null)
			{
				return null;
			}
			return nodeInfo.DataItem;
		}

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

		[ConnectBefore]
		private void HandleMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			bool flag = false;
            if (this.tree.GetPathAtPos((int)args.Event.X, (int)args.Event.Y, out TreePath path, out TreeViewColumn treeViewColumn, out int num, out int num2) && this.store.GetIter(out TreeIter treeIter, path))
            {
                NodeInfo nodeInfo = (NodeInfo)this.store.GetValue(treeIter, 0);
                if (nodeInfo != null && !string.IsNullOrWhiteSpace(nodeInfo.StatusMessage) && nodeInfo.IconInfo.StatusIconInternal != null)
                {
                    Rectangle cellArea = this.tree.GetCellArea(path, this.tree.Columns[0]);
                    this.tree.QueueDrawArea(cellArea.X, cellArea.Y, cellArea.Width, cellArea.Height);
                    Rectangle cellArea2 = this.tree.GetCellArea(path, this.tree.Columns[0]);
                    treeViewColumn.CellGetPosition(this.text_render, out int num3, out int width);
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

		[ConnectBefore]
		private void HandleLeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			this.HideStatusMessage();
		}

		private void ShowStatusMessage(TreeIter it, Rectangle rect, NodeInfo info)
		{
			if (this.statusMessageVisible && this.store.GetPath(it).Equals(this.store.GetPath(this.statusIconIter)))
			{
				return;
			}
			this.statusPopover?.Destroy();
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

		private void HideStatusMessage()
		{
			if (this.statusMessageVisible)
			{
				this.statusMessageVisible = false;
				this.statusPopover.Destroy();
				this.statusPopover = null;
			}
		}

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

		internal NodeBuilder[] GetBuilderChain(Type type)
		{
            this.builderChains.TryGetValue(type, out NodeBuilder[] array);
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
            if (resourceItem == null || !this.builder.GetFirstNode(resourceItem, out TreeIter iter))
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

		private void SetBuilders(IList<NodeBuilder> buildersArray)
		{
			foreach (NodeBuilder nodeBuilder in buildersArray)
			{
				nodeBuilder.SetContext(this.builderContext);
			}
		}

		private bool IsMutilSelecteState()
		{
			return this.tree.Selection.CountSelectedRows() > 1;
		}

		[CommandHandler(CmdEnum.RefreshCmd)]
		private void OnRefreshCmd()
		{
			this.builder.UpdateAll();
		}

		[CommandUpdateHandler(CmdEnum.RefreshCmd)]
		private void RefreshCmd_Update(CommandInfo info)
		{
			if (Services.ProjectOperations.CurrentSelectedSolution == null)
			{
				info.Enabled = false;
			}
		}

		[CommandHandler(CmdEnum.ResOpenDirCmd)]
		private void OnOpenInResourceManageCmd()
		{
			this.builder.OpenInResourceManageHanlder();
		}

		[CommandHandler(CmdEnum.CreateSerialFrameCmd)]
		private void OnCreateSerialFrame()
		{
			List<ResourceItem> currentSelectes = this.builder.GetCurrentSelectes();
			Services.EventsService.GetEvent<CreateSerialFrameEvent>().Publish(new CreateSerialFrameEventArgs(null, currentSelectes));
			Tracker.Add(ViewRegions.ResourcePanel, "RightMenuCreateSpriteSheetAnimation", "", "");
		}

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

		[CommandHandler(CmdEnum.NewFolderCmd)]
		private void NewFolderCmd()
		{
			this.builder.NewFolderHanlder();
			Tracker.Add(ViewRegions.ResourcePanel, "RightMenuNewFolder", "", "");
		}

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
            NodeInfo nodeInfo = this.builder.GetFristSelecteValue(0, out TreeIter treeIter) as NodeInfo;
            if (nodeInfo != null)
			{
				info.Enabled = this.ExisteResource(nodeInfo.DataItem);
			}
		}

		[CommandUpdateHandler(CmdEnum.ResOpenDirCmd)]
		private void OpenCmd_Update(CommandInfo info)
		{
			if (Services.ProjectOperations.CurrentSelectedSolution == null || this.IsMutilSelecteState())
			{
				info.Enabled = false;
				return;
			}
            NodeInfo nodeInfo = this.builder.GetFristSelecteValue(0, out TreeIter treeIter) as NodeInfo;
            if (nodeInfo != null)
			{
				info.Enabled = this.ExisteResource(nodeInfo.DataItem);
			}
		}

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
            if (this.builder.GetFristSelecteValue(0) is NodeInfo nodeInfo && nodeInfo.DataItem != null)
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

		[CommandHandler(CmdEnum.DeleteCmd2)]
		[CommandHandler(CmdEnum.DeleteCmd)]
		private void DeleteCmdExecute()
		{
			this.builder.DeleteResourceHandler();
		}

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

		private void TimeLineCanMove(CommandInfo info)
		{
			if (this.IsRanameStatus)
			{
				info.Enabled = false;
				info.Bypass = true;
			}
		}

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
                    this.builder.GetFirstNode(resourceItem.Parent, out TreeIter iter);
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

		[CommandUpdateHandler(CmdEnum.DuplicateCmd)]
		public void DuplicateCmd(CommandInfo info)
		{
			List<ResourceItem> currentSelectes = this.builder.GetCurrentSelectes();
			bool enabled = false;
			if (currentSelectes != null)
			{
				foreach (ResourceItem resourceItem in currentSelectes)
				{
                    if (!(resourceItem is CocosItem cocosItem) || !(cocosItem.ContentType != "Plist") || cocosItem.DataError != null)
                    {
                        enabled = false;
                        break;
                    }
                    enabled = true;
				}
			}
			info.Enabled = enabled;
		}

		private bool wantFocus()
		{
			this.tree.GrabFocus();
			this.StartLabelEditInternal();
			return false;
		}

		public void StartLabelEditInternal()
		{
            if (this.builder.GetFristSelecteValue(0, out TreeIter iter) is NodeInfo nodeInfo)
            {
                Idle.Add(delegate
                {
                    Entry entry = this.currentLabelEditable;
                    if (!(nodeInfo.DataItem is ResourceItem item) || entry == null || string.IsNullOrWhiteSpace(item.Name))
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

		[ConnectBefore]
		private void HandleEditingStarted(object o, EditingStartedArgs e)
		{
			this.currentLabelEditable = (e.Editable as Entry);
			this.currentLabelEditable.MaxLength = 50;
		}

		[ConnectBefore]
		private void HandleOnEdit(object o, EditedArgs e)
		{
			this.text_render.Editable = false;
			this.currentLabelEditable = null;
            if (!this.Tree.CurrentModel.GetIterFromString(out TreeIter iter, e.Path))
            {
                throw new Exception("Error calculating iter for path " + e.Path);
            }
            this.Raname(iter, e.NewText, true);
			this.IsRanameStatus = false;
		}

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
                            if (this.Tree.CurrentModel.GetValue(iter, 0) is NodeInfo nodeInfo)
                            {
                                resourceItem = (nodeInfo.DataItem as ResourceItem);
                            }
                        }
						else
						{
                            if (this.store.GetValue(iter, 0) is NodeInfo nodeInfo2)
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
            if (resourceItem != null && this.builder.GetFirstNode(resourceItem, out TreeIter iter))
            {
                this.Raname(iter, newName, false);
            }
        }

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

		public const int NodeInfoColumn = 0;

		private Dictionary<Type, NodeBuilder[]> builderChains = new Dictionary<Type, NodeBuilder[]>();

		private ExtendTreeView tree;

		private TreeStore store;

		private TreeViewColumn complete_column;

		private CustomCellRendererImage icon_render;

		private CustomCellRendererText text_render;

		private ResourceTreeBuilder builder;

		public ResourceWidget ResourceWidget;

		private static TargetEntry[] target_tableWindows = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		private bool statusMessageVisible;

		private TreeIter statusIconIter;

		private TooltipPopoverWindow statusPopover;

		private bool IsRanameStatus;

		private List<ResourceItem> copyItems;

		private Entry currentLabelEditable;

		private bool editingText;

		private TreeBuilderContext builderContext;
	}
}

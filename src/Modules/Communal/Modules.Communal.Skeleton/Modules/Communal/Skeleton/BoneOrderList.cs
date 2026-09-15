using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components;

namespace Modules.Communal.Skeleton
{
	public class BoneOrderList : EventBox
	{
		public BoneOrderList()
		{
			this.boneListSw = new CompactScrolledWindow();
			this.treeTable = new Table(2U, 5U, false);
			this.boneList = new ContextMenuTreeView();
			this.boneListSw.HScrollbar.Visible = true;
			this.boneListSw.VScrollbar.Visible = true;
			this.boneListSw.Add(this.boneList);
			this.SetDrag();
			this.boneList.Selection.Mode = SelectionMode.Multiple;
			this.upButton = new IconButton(ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Up.png"));
			this.upButton.TooltipText = LanguageInfo.Animation_BoneTreetMenu_NodeMoveUp;
			this.downButton = new IconButton(ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Down.png"));
			this.downButton.TooltipText = LanguageInfo.Animation_BoneTreetMenu_NodeMoveDown;
			this.topButton = new IconButton(ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Top.png"));
			this.topButton.TooltipText = LanguageInfo.Animation_BoneTreetMenu_NodeMoveTop;
			this.bottomButton = new IconButton(ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Bottom.png"));
			this.bottomButton.TooltipText = LanguageInfo.Animation_BoneTreetMenu_NodeMoveBottom;
			this.treeTable.Attach(this.upButton, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.treeTable.Attach(this.downButton, 1U, 2U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.treeTable.Attach(this.topButton, 2U, 3U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.treeTable.Attach(this.bottomButton, 3U, 4U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.treeTable.Attach(new Label(), 4U, 5U, 0U, 1U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.treeTable.Attach(this.boneListSw, 0U, 5U, 1U, 2U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill, 0U, 0U);
			this.treeTable.Attach(new HSeparator
			{
				HeightRequest = 1
			}, 0U, 5U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.upButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.upButton_Clicked);
			this.downButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.downButton_Clicked);
			this.topButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.topButton_Clicked);
			this.bottomButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.bottomButton_Clicked);
			base.Add(this.treeTable);
			base.ShowAll();
			this.treeTable.ColumnSpacing = 2U;
			this.complete_column = new TreeViewColumn();
			this.treeStore = new ListStore(new Type[]
			{
				typeof(BoneObject)
			});
			this.boneList.Model = this.treeStore;
			this.boneList.HeadersVisible = false;
			this.cellItem = new BoneCellRenderer();
			this.cellItem.Editable = false;
			this.complete_column.PackStart(this.cellItem, true);
			this.complete_column.AddAttribute(this.cellItem, "bone", 0);
			this.boneList.AppendColumn(this.complete_column);
			this.treeStore.DefaultSortFunc = new TreeIterCompareFunc(this.TreeIterCompareFunc);
			this.treeStore.SetSortColumnId(-1, SortType.Ascending);
			this.RegesterEvent();
		}

		private int TreeIterCompareFunc(TreeModel model, TreeIter a, TreeIter b)
		{
			BoneObject bone = this.GetBone(a);
			BoneObject bone2 = this.GetBone(b);
			return bone.ZOrder.CompareTo(bone2.ZOrder);
		}

		private void SetDrag()
		{
			this.boneList.EnableModelDragDest(BoneOrderList.target_table, DragAction.Copy | DragAction.Move | DragAction.Link);
			Gtk.Drag.SourceSet(this.boneList, ModifierType.Button1Mask, BoneOrderList.target_table, DragAction.Copy | DragAction.Move | DragAction.Link);
		}

		public void InitModel()
		{
			this.ClearModel();
			if (Services.Workbench.ActiveDocument == null)
			{
				return;
			}
			CocosItem file = Services.Workbench.ActiveDocument.File;
			SkeletonObject skeletonObject = file.GetRootNode() as SkeletonObject;
			this._rootSkeleton = skeletonObject;
			if (skeletonObject == null)
			{
				return;
			}
			SkeletonObject rootSkeleton = this._rootSkeleton;
			rootSkeleton.SubBonesZOrderChangeEvent = (EventHandler<ZOrderChangeEventArgs>)Delegate.Combine(rootSkeleton.SubBonesZOrderChangeEvent, new EventHandler<ZOrderChangeEventArgs>(this.ZOrderChangedEvent));
			List<BoneObject> allSubBones = this._rootSkeleton.GetAllSubBones();
			List<BoneObject> list = new List<BoneObject>();
			foreach (BoneObject boneObject in allSubBones)
			{
				if (boneObject.IsSelected)
				{
					list.Add(boneObject);
				}
				TreeIter value = this.treeStore.AppendValues(new object[]
				{
					boneObject
				});
				this.nodeHash.Add(boneObject, value);
			}
			this.SetSelectedItems(list);
		}

		private void ClearModel()
		{
			if (this._rootSkeleton != null)
			{
				SkeletonObject rootSkeleton = this._rootSkeleton;
				rootSkeleton.SubBonesZOrderChangeEvent = (EventHandler<ZOrderChangeEventArgs>)Delegate.Remove(rootSkeleton.SubBonesZOrderChangeEvent, new EventHandler<ZOrderChangeEventArgs>(this.ZOrderChangedEvent));
			}
			if (this.nodeHash.Count != 0)
			{
				this.nodeHash.Clear();
				this.treeStore.Clear();
			}
		}

		protected override void OnDestroyed()
		{
			this.isSendSelectChangedEvent = false;
			this.ClearModel();
			base.OnDestroyed();
			this.isSendSelectChangedEvent = true;
		}

		private void MoveBoneNextTo(TreeIter boneIter, TreeIter preIter)
		{
			this.treeStore.DefaultSortFunc = ((TreeModel a, TreeIter b, TreeIter c) => 0);
			BoneObject bone = this.GetBone(boneIter);
			int zorder = bone.ZOrder;
			TreeIter iter = TreeIter.Zero;
			this.treeStore.GetIterFromString(out iter, "0");
			BoneObject bone2 = this.GetBone(iter);
			int num;
			if (!preIter.Equals(TreeIter.Zero))
			{
				BoneObject bone3 = this.GetBone(preIter);
				num = bone3.ZOrder;
				num++;
				iter = this.GetNextIter(preIter);
				bone2 = this.GetBone(iter);
			}
			else
			{
				num = bone2.ZOrder;
			}
			bone.ZOrder = num;
			while (!iter.Equals(TreeIter.Zero))
			{
				if (bone2 != bone)
				{
					int zorder2 = bone2.ZOrder;
					if (zorder2 > num)
					{
						break;
					}
					num++;
					bone2.ZOrder = num;
				}
				iter = this.GetNextIter(iter);
				bone2 = this.GetBone(iter);
			}
			this.treeStore.DefaultSortFunc = new TreeIterCompareFunc(this.TreeIterCompareFunc);
		}

		private void bottomButton_Clicked(object sender, EventArgs e)
		{
			int num = this.nodeHash.Count<KeyValuePair<object, TreeIter>>() - 1;
			bool flag = false;
			TreeIter treeIter;
			this.treeStore.GetIterFromString(out treeIter, num.ToString());
			List<TreeIter> currentSelectIter = this.GetCurrentSelectIter();
			int num2 = currentSelectIter.Count<TreeIter>() - 1;
			using (CompositeTask.Run("bonezorders", null))
			{
				for (int i = 0; i <= num2; i++)
				{
					if (!currentSelectIter[i].Equals(treeIter))
					{
						this.MoveBoneNextTo(currentSelectIter[i], treeIter);
						this.treeStore.GetIterFromString(out treeIter, num.ToString());
						flag = true;
					}
				}
				if (flag)
				{
					this.SetSelectedItems(currentSelectIter);
				}
			}
		}

		private void topButton_Clicked(object sender, EventArgs e)
		{
			this.nodeHash.Count<KeyValuePair<object, TreeIter>>();
			TreeIter zero = TreeIter.Zero;
			TreeIter zero2 = TreeIter.Zero;
			List<TreeIter> currentSelectIter = this.GetCurrentSelectIter();
			bool flag = false;
			int num = currentSelectIter.Count<TreeIter>() - 1;
			this.treeStore.GetIterFirst(out zero2);
			using (CompositeTask.Run("bonezorders", null))
			{
				for (int i = num; i >= 0; i--)
				{
					if (!currentSelectIter[i].Equals(zero) && !currentSelectIter.Contains(zero2))
					{
						this.MoveBoneNextTo(currentSelectIter[i], zero);
						this.treeStore.GetIterFromString(out zero, "0");
						flag = true;
					}
				}
				if (flag)
				{
					this.SetSelectedItems(currentSelectIter);
				}
			}
		}

		private void downButton_Clicked(object sender, EventArgs e)
		{
			TreeIter treeIter = TreeIter.Zero;
			TreeIter treeIter2 = TreeIter.Zero;
			List<TreeIter> currentSelectIter = this.GetCurrentSelectIter();
			bool flag = false;
			int num = currentSelectIter.Count<TreeIter>() - 1;
			using (CompositeTask.Run("bonezorders", null))
			{
				for (int i = num; i >= 0; i--)
				{
					treeIter = currentSelectIter[i];
					treeIter2 = this.GetNextIter(treeIter);
					if (!TreeIter.Zero.Equals(treeIter2) && !currentSelectIter.Contains(treeIter2))
					{
						this.MoveBoneNextTo(treeIter, treeIter2);
						flag = true;
					}
				}
				if (flag)
				{
					this.SetSelectedItems(currentSelectIter);
				}
			}
		}

		private void upButton_Clicked(object sender, EventArgs e)
		{
			TreeIter treeIter = TreeIter.Zero;
			TreeIter treeIter2 = TreeIter.Zero;
			List<TreeIter> currentSelectIter = this.GetCurrentSelectIter();
			bool flag = false;
			int num = currentSelectIter.Count<TreeIter>() - 1;
			using (CompositeTask.Run("bonezorders", null))
			{
				for (int i = 0; i <= num; i++)
				{
					treeIter = currentSelectIter[i];
					treeIter2 = this.GetPrevIter(treeIter);
					if (!TreeIter.Zero.Equals(treeIter2) && !currentSelectIter.Contains(treeIter2))
					{
						treeIter2 = this.GetPrevIter(treeIter2);
						this.MoveBoneNextTo(treeIter, treeIter2);
						flag = true;
					}
				}
				if (flag)
				{
					this.SetSelectedItems(currentSelectIter);
				}
			}
		}

		private TreeIter GetNextIter(TreeIter iter)
		{
			TreeIter zero = TreeIter.Zero;
			TreePath path = this.treeStore.GetPath(iter);
			path.Next();
			this.treeStore.GetIter(out zero, path);
			return zero;
		}

		private TreeIter GetPrevIter(TreeIter iter)
		{
			TreeIter zero = TreeIter.Zero;
			if (!iter.Equals(TreeIter.Zero))
			{
				TreePath path = this.treeStore.GetPath(iter);
				if (path.Prev())
				{
					this.treeStore.GetIter(out zero, path);
				}
			}
			return zero;
		}

		private BoneObject GetBone(TreeIter iter)
		{
			return this.treeStore.GetValue(iter, 0) as BoneObject;
		}

		private void RegesterEvent()
		{
			this.boneList.DragDrop += this.boneList_DragDrop;
			this.boneList.Selection.Changed += this.OnSelection_Changed;
			Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>().Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.SelectedChangeEventHandle));
		}

		private void OnSelection_Changed(object sender, EventArgs e)
		{
			this.currentSelectedBone = this.GetCurrentSelectBone();
			if (this.isSendSelectChangedEvent)
			{
				HashSet<AbstractNodeObject> selecteParent = this.GetSelecteParent(this.currentSelectedBone);
				SelectedVisualObjectsChangeEventArgs payload = new SelectedVisualObjectsChangeEventArgs(this.currentSelectedBone, selecteParent, false);
				Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>().Publish(payload);
			}
		}

		private HashSet<AbstractNodeObject> GetSelecteParent(IEnumerable<AbstractNodeObject> selectedItems)
		{
			HashSet<AbstractNodeObject> hashSet = new HashSet<AbstractNodeObject>();
			foreach (AbstractNodeObject abstractNodeObject in selectedItems)
			{
				if (!selectedItems.Contains(abstractNodeObject.Parent))
				{
					hashSet.Add(abstractNodeObject);
				}
			}
			return hashSet;
		}

		private void SelectedChangeEventHandle(SelectedVisualObjectsChangeEventArgs obj)
		{
			if (this.isSendSelectChangedEvent)
			{
				this.SetSelectedItems(obj.SelectedObject);
			}
		}

		private void SetSelectedItems(List<TreeIter> selectedITree)
		{
			this.isSendSelectChangedEvent = false;
			if (selectedITree != null && this.boneList.Selection != null)
			{
				List<TreePath> selectPath = new List<TreePath>();
				foreach (TreeIter iter in selectedITree)
				{
					TreePath path2 = this.treeStore.GetPath(iter);
					selectPath.Add(path2);
				}
				if (selectPath == null)
				{
					return;
				}
				this.boneList.Selection.UnselectAll();
				this.boneList.Selection.SelectFunction = ((TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected) => selectPath.Contains(path));
				this.boneList.ScrollToCell(selectPath.FirstOrDefault<TreePath>(), this.complete_column, false, 0.5f, 0.5f);
				this.boneList.Selection.SelectAll();
				this.boneList.Selection.SelectFunction = ((TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected) => true);
			}
			this.isSendSelectChangedEvent = true;
		}

		private void SetSelectedItems(IEnumerable<VisualObject> selectedItem)
		{
			this.isSendSelectChangedEvent = false;
			if (selectedItem != null && this.boneList.Selection != null)
			{
				List<TreePath> selectPath = new List<TreePath>();
				foreach (VisualObject key in selectedItem)
				{
					if (this.nodeHash.ContainsKey(key))
					{
						TreeIter iter = this.nodeHash[key];
						TreePath path3 = this.treeStore.GetPath(iter);
						selectPath.Add(path3);
					}
				}
				if (selectPath == null)
				{
					return;
				}
				this.boneList.Selection.UnselectAll();
				this.boneList.Selection.SelectFunction = ((TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected) => selectPath.Contains(path));
				this.boneList.Selection.SelectAll();
				this.boneList.Selection.SelectFunction = ((TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected) => true);
				TreePath path2 = selectPath.FirstOrDefault<TreePath>();
				this.boneList.ScrollToCell(path2, this.complete_column, true, 0.5f, 0.5f);
			}
			this.isSendSelectChangedEvent = true;
		}

		[ConnectBefore]
		private void boneList_DragDrop(object o, DragDropArgs args)
		{
			this.boneList.Selection.Changed -= this.OnSelection_Changed;
			TreePath treePath;
			TreeViewDropPosition pos;
			this.boneList.GetDestRowAtPos(args.X, args.Y, out treePath, out pos);
			if (treePath == null)
			{
				return;
			}
			TreeIter target;
			this.treeStore.GetIter(out target, treePath);
			target = this.GetTarget(target, pos);
			List<TreeIter> currentSelectIter = this.GetCurrentSelectIter();
			using (CompositeTask.Run("bonezorders", null))
			{
				foreach (TreeIter boneIter in currentSelectIter)
				{
					this.MoveBoneNextTo(boneIter, target);
				}
				this.SetSelectedItems(currentSelectIter);
			}
			this.boneList.Selection.Changed += this.OnSelection_Changed;
		}

		private TreeIter GetTarget(TreeIter iter, TreeViewDropPosition pos)
		{
			TreeIter zero = TreeIter.Zero;
			switch (pos)
			{
			case TreeViewDropPosition.Before:
				return this.GetPrevIter(iter);
			case TreeViewDropPosition.After:
			case TreeViewDropPosition.IntoOrBefore:
			case TreeViewDropPosition.IntoOrAfter:
				return iter;
			default:
				return zero;
			}
		}

		private List<TreeIter> GetCurrentSelectIter()
		{
			TreePath[] selectedRows = this.boneList.Selection.GetSelectedRows();
			List<TreeIter> list = new List<TreeIter>();
			if (selectedRows != null && selectedRows.Count<TreePath>() != 0)
			{
				foreach (TreePath path in selectedRows)
				{
					TreeIter zero = TreeIter.Zero;
					this.treeStore.GetIter(out zero, path);
					if (!object.Equals(zero, TreeIter.Zero))
					{
						list.Add(zero);
					}
				}
			}
			return list;
		}

		private List<BoneObject> GetCurrentSelectBone()
		{
			TreePath[] selectedRows = this.boneList.Selection.GetSelectedRows();
			List<BoneObject> list = new List<BoneObject>();
			if (selectedRows != null && selectedRows.Count<TreePath>() != 0)
			{
				foreach (TreePath path in selectedRows)
				{
					TreeIter zero = TreeIter.Zero;
					this.treeStore.GetIter(out zero, path);
					if (!object.Equals(zero, TreeIter.Zero))
					{
						BoneObject item = this.treeStore.GetValue(zero, 0) as BoneObject;
						list.Add(item);
					}
				}
			}
			return list;
		}

		internal void AddBoneItem(BoneObject bone)
		{
			this.boneList.Selection.Changed -= this.OnSelection_Changed;
			TreeIter value = this.treeStore.AppendValues(new object[]
			{
				bone
			});
			this.nodeHash.Add(bone, value);
			this.boneList.Selection.Changed += this.OnSelection_Changed;
		}

		internal void RemoveBoneItem(BoneObject bone)
		{
			this.boneList.Selection.Changed -= this.OnSelection_Changed;
			if (this.nodeHash.ContainsKey(bone))
			{
				TreeIter treeIter = this.nodeHash[bone];
				this.treeStore.Remove(ref treeIter);
				this.nodeHash.Remove(bone);
			}
			this.boneList.Selection.Changed += this.OnSelection_Changed;
		}

		private void ZOrderChangedEvent(object sender, ZOrderChangeEventArgs e)
		{
			this.isSendSelectChangedEvent = false;
			BoneObject boneObject = sender as BoneObject;
			if (boneObject == null || this.boneList.Selection == null)
			{
				return;
			}
			TreeIter iter = this.nodeHash[boneObject];
			this.treeStore.GetPath(iter);
			this.treeStore.SetValue(iter, 0, boneObject);
			this.boneList.Selection.UnselectAll();
			this.boneList.Selection.SelectIter(iter);
			this.isSendSelectChangedEvent = true;
		}

		private const int BoneColumn = 0;

		private ContextMenuTreeView boneList;

		private static TargetEntry[] target_table = new TargetEntry[]
		{
			DragTargetType.CocoStudioTarget
		};

		private Dictionary<object, TreeIter> nodeHash = new Dictionary<object, TreeIter>();

		private List<BoneObject> currentSelectedBone;

		private TreeViewColumn complete_column;

		private BoneCellRenderer cellItem;

		private ListStore treeStore;

		private Table treeTable;

		private IconButton upButton;

		private IconButton downButton;

		private IconButton topButton;

		private IconButton bottomButton;

		private bool isSendSelectChangedEvent = true;

		private CompactScrolledWindow boneListSw;

		private SkeletonObject _rootSkeleton;
	}
}

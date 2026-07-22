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
	// Token: 0x0200000F RID: 15
	public class BoneOrderList : EventBox
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00003490 File Offset: 0x00001690
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

		// Token: 0x06000056 RID: 86 RVA: 0x000037AC File Offset: 0x000019AC
		private int TreeIterCompareFunc(TreeModel model, TreeIter a, TreeIter b)
		{
			BoneObject bone = this.GetBone(a);
			BoneObject bone2 = this.GetBone(b);
			return bone.ZOrder.CompareTo(bone2.ZOrder);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000037DD File Offset: 0x000019DD
		private void SetDrag()
		{
			this.boneList.EnableModelDragDest(BoneOrderList.target_table, DragAction.Copy | DragAction.Move | DragAction.Link);
			Gtk.Drag.SourceSet(this.boneList, ModifierType.Button1Mask, BoneOrderList.target_table, DragAction.Copy | DragAction.Move | DragAction.Link);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003808 File Offset: 0x00001A08
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

		// Token: 0x06000059 RID: 89 RVA: 0x00003908 File Offset: 0x00001B08
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

		// Token: 0x0600005A RID: 90 RVA: 0x00003967 File Offset: 0x00001B67
		protected override void OnDestroyed()
		{
			this.isSendSelectChangedEvent = false;
			this.ClearModel();
			base.OnDestroyed();
			this.isSendSelectChangedEvent = true;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003988 File Offset: 0x00001B88
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

		// Token: 0x0600005C RID: 92 RVA: 0x00003A9C File Offset: 0x00001C9C
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

		// Token: 0x0600005D RID: 93 RVA: 0x00003B6C File Offset: 0x00001D6C
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

		// Token: 0x0600005E RID: 94 RVA: 0x00003C44 File Offset: 0x00001E44
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

		// Token: 0x0600005F RID: 95 RVA: 0x00003CF4 File Offset: 0x00001EF4
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

		// Token: 0x06000060 RID: 96 RVA: 0x00003DAC File Offset: 0x00001FAC
		private TreeIter GetNextIter(TreeIter iter)
		{
			TreeIter zero = TreeIter.Zero;
			TreePath path = this.treeStore.GetPath(iter);
			path.Next();
			this.treeStore.GetIter(out zero, path);
			return zero;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003DE4 File Offset: 0x00001FE4
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

		// Token: 0x06000062 RID: 98 RVA: 0x00003E35 File Offset: 0x00002035
		private BoneObject GetBone(TreeIter iter)
		{
			return this.treeStore.GetValue(iter, 0) as BoneObject;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003E4C File Offset: 0x0000204C
		private void RegesterEvent()
		{
			this.boneList.DragDrop += this.boneList_DragDrop;
			this.boneList.Selection.Changed += this.OnSelection_Changed;
			Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>().Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.SelectedChangeEventHandle));
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003EA8 File Offset: 0x000020A8
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

		// Token: 0x06000065 RID: 101 RVA: 0x00003EF4 File Offset: 0x000020F4
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

		// Token: 0x06000066 RID: 102 RVA: 0x00003F54 File Offset: 0x00002154
		private void SelectedChangeEventHandle(SelectedVisualObjectsChangeEventArgs obj)
		{
			if (this.isSendSelectChangedEvent)
			{
				this.SetSelectedItems(obj.SelectedObject);
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003F88 File Offset: 0x00002188
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

		// Token: 0x06000068 RID: 104 RVA: 0x000040D8 File Offset: 0x000022D8
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

		// Token: 0x06000069 RID: 105 RVA: 0x00004228 File Offset: 0x00002428
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

		// Token: 0x0600006A RID: 106 RVA: 0x00004320 File Offset: 0x00002520
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

		// Token: 0x0600006B RID: 107 RVA: 0x00004358 File Offset: 0x00002558
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

		// Token: 0x0600006C RID: 108 RVA: 0x000043D8 File Offset: 0x000025D8
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

		// Token: 0x0600006D RID: 109 RVA: 0x0000446C File Offset: 0x0000266C
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

		// Token: 0x0600006E RID: 110 RVA: 0x000044D8 File Offset: 0x000026D8
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

		// Token: 0x0600006F RID: 111 RVA: 0x00004554 File Offset: 0x00002754
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

		// Token: 0x04000010 RID: 16
		private const int BoneColumn = 0;

		// Token: 0x04000011 RID: 17
		private ContextMenuTreeView boneList;

		// Token: 0x04000012 RID: 18
		private static TargetEntry[] target_table = new TargetEntry[]
		{
			DragTargetType.CocoStudioTarget
		};

		// Token: 0x04000013 RID: 19
		private Dictionary<object, TreeIter> nodeHash = new Dictionary<object, TreeIter>();

		// Token: 0x04000014 RID: 20
		private List<BoneObject> currentSelectedBone;

		// Token: 0x04000015 RID: 21
		private TreeViewColumn complete_column;

		// Token: 0x04000016 RID: 22
		private BoneCellRenderer cellItem;

		// Token: 0x04000017 RID: 23
		private ListStore treeStore;

		// Token: 0x04000018 RID: 24
		private Table treeTable;

		// Token: 0x04000019 RID: 25
		private IconButton upButton;

		// Token: 0x0400001A RID: 26
		private IconButton downButton;

		// Token: 0x0400001B RID: 27
		private IconButton topButton;

		// Token: 0x0400001C RID: 28
		private IconButton bottomButton;

		// Token: 0x0400001D RID: 29
		private bool isSendSelectChangedEvent = true;

		// Token: 0x0400001E RID: 30
		private CompactScrolledWindow boneListSw;

		// Token: 0x0400001F RID: 31
		private SkeletonObject _rootSkeleton;
	}
}

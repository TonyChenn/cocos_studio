using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.ControlLib;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components.Commands;
using Stetic;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000032 RID: 50
	[ToolboxItem(true)]
	public class ResourceWidget : EventBox, ICommandRouter
	{
		// Token: 0x060001DC RID: 476 RVA: 0x0000A299 File Offset: 0x00008499
		public ResourceWidget()
		{
			this.Build();
			this.InitiTreeView();
			this.AddButton();
			this.InitiStyle();
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000A2BC File Offset: 0x000084BC
		private void AddButton()
		{
			string resourceID = StaticVariable.GetResourceID("import.png");
			this.btn_exprot = new IconButton(ImageIcon.GetIcon(resourceID));
			this.btn_exprot.SetSizeRequest(20, 20);
			this.btn_exprot.TooltipText = LanguageInfo.Dialog_ButtonNew + "...";
			this.btn_exprot.CanFocus = true;
			this.hb_flot.Add(this.btn_exprot);
			Box.BoxChild boxChild = (Box.BoxChild)this.hb_flot[this.btn_exprot];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment2.RightPadding = 10U;
			string resourceID2 = StaticVariable.GetResourceID("refresh.png");
			this.btn_refresh = new IconButton(ImageIcon.GetIcon(resourceID2));
			this.btn_refresh.TooltipText = LanguageInfo.Command_Refresh;
			this.btn_refresh.SetSizeRequest(20, 20);
			this.btn_refresh.CanFocus = true;
			this.hb_flot.Add(this.btn_refresh);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hb_flot[this.btn_refresh];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.btn_exprot.Clicked += this.btn_exprot_Clicked;
			this.btn_refresh.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.btn_refresh_Clicked);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000A458 File Offset: 0x00008658
		private void btn_exprot_Clicked(object sender, ButtonReleaseEventArgs e)
		{
			if (ResourceMenu.AdditionMenu != null)
			{
				int menuHeight = 58;
				if (ResourceMenu.AdditionMenu.Allocation.Height != 1)
				{
					menuHeight = ResourceMenu.AdditionMenu.Allocation.Height;
				}
				this.treeview.HasFocus = true;
				MenuPositionFunc func = delegate(Menu m, out int x, out int y, out bool pushIn)
				{
					int num;
					int num2;
					this.btn_exprot.GdkWindow.GetOrigin(out num, out num2);
					x = num;
					y = num2 - menuHeight;
					pushIn = false;
				};
				ResourceMenu.AdditionMenu.ShowAll();
				ResourceMenu.AdditionMenu.Popup(null, null, func, 0U, 0U);
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000A4DC File Offset: 0x000086DC
		private void btn_refresh_Clicked(object sender, EventArgs e)
		{
			this.treeview.Builder.UpdateAll();
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000A4EE File Offset: 0x000086EE
		public bool SearchBoxFocus
		{
			get
			{
				return this.searchBox.HasFocus;
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000A4FC File Offset: 0x000086FC
		public void InitiTreeView()
		{
			this.treeview = new ResourceTreeView();
			this.treeview.ResourceWidget = this;
			this.evnt_TreeView.Add(this.treeview);
			ImageIcon.GetIcon("Modules.Communal.ResourcePanel.Images.Close.png");
			this.searchBox = new SearchEntry();
			this.searchBox.HeightRequest = 20;
			this.searchBox.Ready = true;
			this.searchBox.HasFrame = true;
			this.searchBox.WidthRequest = 150;
			this.searchBox.Changed += this.filterTextChanged;
			this.searchBox.Show();
			this.footEvent.BorderWidth = 0U;
			this.footEvent.Add(this.searchBox);
			this.filter = new TreeModelFilter(this.treeview.Store, null);
			this.filter.VisibleFunc = new TreeModelFilterVisibleFunc(this.FilterTree);
			this.treeview.Tree.Model = this.filter;
			this.treeview.Tree.Filter = this.filter;
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			if (currentSelectedSolution != null)
			{
				this.treeview.LoadTree(currentSelectedSolution);
			}
			base.ShowAll();
			this.treeview.Tree.MotionNotifyEvent += this.TreeView_MotionNotifyEvent;
			this.treeview.Tree.ScrollEvent += this.TreeView_ScrollEvent;
			this.treeview.Tree.LeaveNotifyEvent += this.TreeView_LeaveNotifyEvent;
			this.treeview.Tree.OnMouseLeave += this.TreeView_OnMouseLeave;
			this.treeview.Tree.DragMotion += this.TreeView_DragMotion;
			base.FocusOutEvent += this.ResourceWidget_FocusOutEvent;
			base.ButtonReleaseEvent += this.ResourceWidget_ButtonReleaseEvent;
			this.previewcontrol = new PreviewControl(base.GdkWindow);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000A6F8 File Offset: 0x000088F8
		private void filterTextChanged(object sender, EventArgs e)
		{
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			if (currentSelectedSolution == null)
			{
				return;
			}
			ResourceFolder rootFolder = currentSelectedSolution.GetRootFolder();
			this.filterText = this.searchBox.Entry.Text;
			if (!string.IsNullOrWhiteSpace(this.filterText))
			{
				this.treeview.Tree.IsSearchState = true;
				ResourceWidget.filterHash.Clear();
				ResourceWidget.Filter(rootFolder, this.filterText);
				this.filter.Refilter();
			}
			this.filter.Refilter();
			if (!string.IsNullOrWhiteSpace(this.filterText))
			{
				this.treeview.Tree.ExpandAll();
				return;
			}
			TreeIter iter;
			this.treeview.Tree.CurrentModel.GetIterFirst(out iter);
			TreePath path = this.treeview.Tree.CurrentModel.GetPath(iter);
			this.treeview.Tree.ExpandRow(path, false);
			this.treeview.Tree.IsSearchState = false;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000A7F0 File Offset: 0x000089F0
		private bool FilterTree(TreeModel model, TreeIter iter)
		{
			if (string.IsNullOrWhiteSpace(this.filterText))
			{
				return true;
			}
			NodeInfo nodeInfo = this.treeview.Store.GetValue(iter, 0) as NodeInfo;
			object dataItem = nodeInfo.DataItem;
			if (dataItem is Solution)
			{
				return true;
			}
			if (dataItem is ResourceItem)
			{
				bool result;
				ResourceWidget.filterHash.TryGetValue((ResourceItem)dataItem, out result);
				return result;
			}
			return false;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000A853 File Offset: 0x00008A53
		private void InitiStyle()
		{
			this.treeview.Tree.ModifyBg(StateType.Normal, new Color(byte.MaxValue, 0, 0));
			this.vb_root.ModifyBg(StateType.Normal, new Color(byte.MaxValue, 0, 0));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000A88A File Offset: 0x00008A8A
		public object GetNextCommandTarget()
		{
			return this.treeview;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000A892 File Offset: 0x00008A92
		private void TreeView_OnMouseLeave(object sender, EventArgs e)
		{
			this.previewcontrol.ShowImagePath = null;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000A8A0 File Offset: 0x00008AA0
		[ConnectBefore]
		private void TreeView_DragMotion(object o, DragMotionArgs args)
		{
			this.previewcontrol.ShowImagePath = null;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000A8AE File Offset: 0x00008AAE
		[ConnectBefore]
		private void TreeView_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			this.SetPreviewControlData((int)args.Event.X, (int)args.Event.Y);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		[ConnectBefore]
		private void TreeView_ScrollEvent(object o, ScrollEventArgs args)
		{
			EventScroll @event = args.Event;
			if (@event != null)
			{
				this.SetPreviewControlData((int)@event.X, (int)@event.Y);
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A8FB File Offset: 0x00008AFB
		[ConnectBefore]
		private void TreeView_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			this.previewcontrol.ShowImagePath = null;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000A90C File Offset: 0x00008B0C
		private void SetPreviewControlData(int x, int y)
		{
			TreePath treePath;
			TreeViewDropPosition treeViewDropPosition;
			if (!this.treeview.Tree.GetDestRowAtPos(x, y, out treePath, out treeViewDropPosition))
			{
				this.previewcontrol.ShowImagePath = null;
				return;
			}
			ResourceItem resourceItem = this.treeview.GetValueByTreePath(treePath) as ResourceItem;
			if (resourceItem != null)
			{
				this.previewcontrol.ShowImagePath = resourceItem.PreviewImageInfo;
				this.SetPreviewControlLocation(x, y);
				return;
			}
			this.previewcontrol.ShowImagePath = null;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A97C File Offset: 0x00008B7C
		private void SetPreviewControlLocation(int mx, int my)
		{
			if (this.previewcontrol.IsShown)
			{
				int num;
				int num2;
				this.treeview.GdkWindow.GetOrigin(out num, out num2);
				int num3 = num - this.previewcontrol.Width - 5;
				int num4 = num2;
				int num5 = num + this.treeview.Allocation.Width + 1;
				int num6 = num4 + this.treeview.Allocation.Height - this.previewcontrol.Height;
				int num7 = this.previewcontrol.Height / 2;
				int x = (base.Screen.Width - num - this.treeview.Allocation.Width - this.previewcontrol.ShowMaxValue - this.previewcontrol.BorderWidth * 2 < 0) ? num3 : num5;
				if (num - this.previewcontrol.ShowMaxValue - this.previewcontrol.BorderWidth * 2 < 0 && num5 + this.previewcontrol.ShowMaxValue + this.previewcontrol.BorderWidth * 2 > base.Screen.Width)
				{
					x = num + this.treeview.Allocation.Width - this.previewcontrol.Width - 22;
				}
				int num8 = num2 + my - num7;
				num8 = ((num8 > num6) ? num6 : num8);
				num8 = ((num8 < num4) ? num4 : num8);
				this.previewcontrol.Move(x, num8);
				this.previewcontrol.ShowAll();
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000AAED File Offset: 0x00008CED
		[ConnectBefore]
		private void ResourceWidget_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 3U)
			{
				this.previewcontrol.ShowImagePath = null;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000AB09 File Offset: 0x00008D09
		[ConnectBefore]
		private void ResourceWidget_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.previewcontrol.ShowImagePath = null;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000AB18 File Offset: 0x00008D18
		public static bool Filter(ResourceItem root, string filterText)
		{
			bool flag = false;
			ResourceWidget.filterHash.Add(root, flag);
			if (root.Name.ToLower().Contains(filterText.ToLower()))
			{
				ResourceWidget.filterHash[root] = true;
				flag = true;
			}
			ResourceFolder resourceFolder = root as ResourceFolder;
			if (resourceFolder != null && resourceFolder.Items != null && resourceFolder.Items.Count > 0)
			{
				foreach (ResourceItem root2 in resourceFolder.Items)
				{
					bool flag2 = ResourceWidget.Filter(root2, filterText);
					if (flag2)
					{
						flag = flag2;
					}
				}
			}
			ResourceWidget.filterHash[root] = flag;
			return flag;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000ABD4 File Offset: 0x00008DD4
		public void Reset()
		{
			this.searchBox.Entry.Text = string.Empty;
			this.filterText = string.Empty;
			this.treeview.Tree.IsSearchState = false;
			ResourceWidget.filterHash.Clear();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000AC14 File Offset: 0x00008E14
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.ResourcePanel.ResourceWidget";
			this.entbx_root = new EventBox();
			this.entbx_root.Name = "entbx_root";
			this.alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.alignment1.TopPadding = 12U;
			this.vb_root = new VBox();
			this.vb_root.Name = "vb_root";
			this.vb_root.Spacing = 6;
			this.evnt_TreeView = new EventBox();
			this.evnt_TreeView.Name = "evnt_TreeView";
			this.vb_root.Add(this.evnt_TreeView);
			Box.BoxChild boxChild = (Box.BoxChild)this.vb_root[this.evnt_TreeView];
			boxChild.Position = 0;
			this.alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment2.HeightRequest = 20;
			this.alignment2.Name = "alignment2";
			this.hb_flot = new HBox();
			this.hb_flot.HeightRequest = 20;
			this.hb_flot.Name = "hb_flot";
			this.hb_flot.Spacing = 2;
			this.footEvent = new EventBox();
			this.footEvent.Name = "footEvent";
			this.hb_flot.Add(this.footEvent);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hb_flot[this.footEvent];
			boxChild2.Position = 1;
			this.alignment2.Add(this.hb_flot);
			this.vb_root.Add(this.alignment2);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vb_root[this.alignment2];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.alignment1.Add(this.vb_root);
			this.entbx_root.Add(this.alignment1);
			base.Add(this.entbx_root);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x0400009F RID: 159
		private IconButton btn_exprot;

		// Token: 0x040000A0 RID: 160
		private IconButton btn_refresh;

		// Token: 0x040000A1 RID: 161
		public bool IsSearchState;

		// Token: 0x040000A2 RID: 162
		private ResourceTreeView treeview;

		// Token: 0x040000A3 RID: 163
		private SearchEntry searchBox;

		// Token: 0x040000A4 RID: 164
		private TreeModelFilter filter;

		// Token: 0x040000A5 RID: 165
		private string filterText;

		// Token: 0x040000A6 RID: 166
		private PreviewControl previewcontrol;

		// Token: 0x040000A7 RID: 167
		public static Dictionary<ResourceItem, bool> filterHash = new Dictionary<ResourceItem, bool>();

		// Token: 0x040000A8 RID: 168
		private EventBox entbx_root;

		// Token: 0x040000A9 RID: 169
		private Alignment alignment1;

		// Token: 0x040000AA RID: 170
		private VBox vb_root;

		// Token: 0x040000AB RID: 171
		private EventBox evnt_TreeView;

		// Token: 0x040000AC RID: 172
		private Alignment alignment2;

		// Token: 0x040000AD RID: 173
		private HBox hb_flot;

		// Token: 0x040000AE RID: 174
		private EventBox footEvent;
	}
}

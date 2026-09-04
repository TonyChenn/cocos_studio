using System;
using CocoStudio.Core;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.Skeleton
{
	// Token: 0x0200002A RID: 42
	public class SkeletonGraphDialog : Gtk.Window
	{
		// Token: 0x060001DB RID: 475 RVA: 0x000098C4 File Offset: 0x00007AC4
		public SkeletonGraphDialog() : base(Gtk.WindowType.Toplevel)
		{
			this.hpaned = new HPaned();
			this.graphicalTable = new Table(2U, 3U, false);
			this.hbox = new HBox();
			this.animationFixed = new FixedEx();
			this.sw = new ScrolledWindow();
			this.button_Normal = new IconRadioButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Arrow.png"));
			this.hbox.PackStart(this.button_Normal, false, false, 0U);
			this.button_BindBone = new IconRadioButton(ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Link.png"));
			this.hbox.PackStart(this.button_BindBone, false, false, 0U);
			this.button_Normal.IsChecked = true;
			this.button_UnBindBone = new IconButton(ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.UnLink.png"));
			this.hbox.PackStart(this.button_UnBindBone, false, false, 0U);
			this.button_Reference = new IconButton(ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Reference.png"));
			this.hbox.PackStart(this.button_Reference, false, false, 0U);
			this.sw.HScrollbar.Visible = true;
			this.sw.VScrollbar.Visible = true;
			this.sw.WidthRequest = 900;
			this.sw.HeightRequest = 600;
			this.hbox.ShowAll();
			this.bottomTable = new Table(2U, 4U, false);
			this.combox = new ComboBox();
			ListStore listStore = new ListStore(new Type[]
			{
				typeof(string)
			});
			listStore.AppendValues(new object[]
			{
				"50%"
			});
			listStore.AppendValues(new object[]
			{
				"100%"
			});
			listStore.AppendValues(new object[]
			{
				"150%"
			});
			this.combox.Model = listStore;
			CellRendererText cell = new CellRendererText();
			this.combox.PackStart(cell, true);
			this.combox.AddAttribute(cell, "text", 0);
			this.combox.Active = 1;
			this.boneLabel = new Label();
			this.skinLabel = new Label();
			this.boneLabel.WidthRequest = 130;
			this.skinLabel.WidthRequest = 130;
			this.bottomTable.Attach(new HSeparator
			{
				WidthRequest = 1
			}, 0U, 4U, 0U, 1U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.bottomTable.Attach(this.combox, 3U, 4U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.bottomTable.Attach(new Label(), 0U, 1U, 1U, 2U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.bottomTable.Attach(this.boneLabel, 1U, 2U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.bottomTable.Attach(this.skinLabel, 2U, 3U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.bottomTable.ColumnSpacing = 10U;
			this.treeOrder = new BoneOrderList();
			this.treeOrder.WidthRequest = 180;
			this.sw.AddWithViewport(this.animationFixed);
			this.graphicalTable.Attach(new VSeparator
			{
				WidthRequest = 1
			}, 0U, 1U, 0U, 3U, AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill, 0U, 0U);
			this.graphicalTable.Attach(this.hbox, 1U, 2U, 0U, 1U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.graphicalTable.Attach(this.sw, 1U, 2U, 1U, 2U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill, 0U, 0U);
			this.hpaned.Add(this.treeOrder);
			Paned.PanedChild panedChild = (Paned.PanedChild)this.hpaned[this.treeOrder];
			panedChild.Shrink = false;
			this.hpaned.Add(this.graphicalTable);
			Paned.PanedChild panedChild2 = (Paned.PanedChild)this.hpaned[this.graphicalTable];
			this.mainTable = new Table(2U, 3U, false);
			this.mainTable.Attach(new Label
			{
				WidthRequest = 5
			}, 0U, 1U, 0U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.mainTable.Attach(this.hpaned, 1U, 2U, 0U, 1U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill, 0U, 0U);
			this.mainTable.Attach(this.bottomTable, 1U, 2U, 1U, 2U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.mainTable.Attach(new Label
			{
				WidthRequest = 5
			}, 2U, 3U, 0U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			base.Add(this.mainTable);
			base.ShowAll();
			this.InitTreeMap();
			this.InitBoneOrderList();
			this.animationFixed.SetOrderListView(this.treeOrder);
			this.InitToolTip();
			Services.Workbench.ActiveDocumentChanged += this.Workbench_ActiveDocumentChanged;
			this.combox.Changed += this.combox_Changed;
			base.SizeAllocated += this.AnimationRelation_SizeAllocated;
			this.button_Normal.CheckChanged += this.button_Normal_CheckChanged;
			this.button_BindBone.CheckChanged += this.button_BindBone_CheckChanged;
			this.button_UnBindBone.Clicked += this.button_UnBindBone_Clicked;
			this.button_Reference.Clicked += this.button_Reference_Clicked;
			this.CenterToParentWindow(ApplicationCurrent.MainWindow);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009DD4 File Offset: 0x00007FD4
		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if ((Platform.IsMac && KeyboardExtend.IsModifyKeyPressed(ModifierType.Mod2Mask)) || (!Platform.IsMac && KeyboardExtend.IsModifyKeyPressed(ModifierType.ControlMask)))
			{
				IUndoManager instance = TaskServiceSingleton.Instance;
				DocumentExtend activeDocument = Services.Workbench.ActiveDocument;
				if ((evnt.Key == Gdk.Key.z | evnt.Key == Gdk.Key.Z) && instance.CanUndo(activeDocument))
				{
					instance.Undo(activeDocument);
				}
				else if ((evnt.Key == Gdk.Key.y | evnt.Key == Gdk.Key.Y) && instance.CanRedo(activeDocument))
				{
					instance.Redo(activeDocument);
				}
			}
			if (evnt.Key == Gdk.Key.Escape)
			{
				this.Destroy();
			}
			return base.OnKeyReleaseEvent(evnt);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009E7D File Offset: 0x0000807D
		private void Workbench_ActiveDocumentChanged(object sender, EventArgs e)
		{
			if (this.animationFixed != null)
			{
				this.InitTreeMap();
				this.InitBoneOrderList();
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009E93 File Offset: 0x00008093
		protected override void OnDestroyed()
		{
			Services.Workbench.ActiveDocumentChanged -= this.Workbench_ActiveDocumentChanged;
			base.OnDestroyed();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009EB1 File Offset: 0x000080B1
		private void combox_Changed(object sender, EventArgs e)
		{
			this.animationFixed.CurrentScale = (double)(this.combox.Active + 1) / 2.0;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009ED8 File Offset: 0x000080D8
		private void InitToolTip()
		{
			this.button_Normal.TooltipText = LanguageInfo.Group_Routine;
			this.button_BindBone.TooltipText = LanguageInfo.Skeleton_Binding;
			this.button_UnBindBone.TooltipText = LanguageInfo.Skeleton_Unbinding;
			this.button_Reference.TooltipText = LanguageInfo.Property_Reset;
			base.Title = LanguageInfo.Skeleton_SkeletonView;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009F30 File Offset: 0x00008130
		private void button_Reference_Clicked(object sender, ButtonReleaseEventArgs e)
		{
			this.InitTreeMap();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009F38 File Offset: 0x00008138
		private void button_UnBindBone_Clicked(object sender, ButtonReleaseEventArgs e)
		{
			this.animationFixed.Unbinding();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009F45 File Offset: 0x00008145
		private void button_BindBone_CheckChanged(object sender, EventArgs e)
		{
			if (this.button_BindBone.IsChecked)
			{
				this.animationFixed.CurrentType = ChoiceType.Binding;
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009F60 File Offset: 0x00008160
		private void button_Normal_CheckChanged(object sender, EventArgs e)
		{
			if (this.button_Normal.IsChecked)
			{
				this.animationFixed.CurrentType = ChoiceType.Normal;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009F7B File Offset: 0x0000817B
		private void AnimationRelation_SizeAllocated(object o, SizeAllocatedArgs args)
		{
			this.animationFixed.DrawLine();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009F88 File Offset: 0x00008188
		private void InitTreeMap()
		{
			this.animationFixed.SkeletonDialog = this;
			this.animationFixed.InitData();
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009FA1 File Offset: 0x000081A1
		public void BoneSkinSum(int bonecount, int skinSum)
		{
			this.boneLabel.Text = string.Format(LanguageInfo.Skeleton_BoneNum, bonecount);
			this.skinLabel.Text = string.Format(LanguageInfo.Skeleton_SkinNum, skinSum);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009FD9 File Offset: 0x000081D9
		private void InitBoneOrderList()
		{
			this.treeOrder.InitModel();
		}

		// Token: 0x04000088 RID: 136
		private FixedEx animationFixed;

		// Token: 0x04000089 RID: 137
		private ScrolledWindow sw;

		// Token: 0x0400008A RID: 138
		private HBox hbox;

		// Token: 0x0400008B RID: 139
		private IconRadioButton button_Normal;

		// Token: 0x0400008C RID: 140
		private IconRadioButton button_BindBone;

		// Token: 0x0400008D RID: 141
		private IconButton button_UnBindBone;

		// Token: 0x0400008E RID: 142
		private IconButton button_Reference;

		// Token: 0x0400008F RID: 143
		private Table graphicalTable;

		// Token: 0x04000090 RID: 144
		private ComboBox combox;

		// Token: 0x04000091 RID: 145
		private Table bottomTable;

		// Token: 0x04000092 RID: 146
		private Table mainTable;

		// Token: 0x04000093 RID: 147
		private HPaned hpaned;

		// Token: 0x04000094 RID: 148
		private BoneOrderList treeOrder;

		// Token: 0x04000095 RID: 149
		private Label boneLabel;

		// Token: 0x04000096 RID: 150
		private Label skinLabel;
	}
}

using System;
using CocoStudio.Core;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.Skeleton
{
	public class SkeletonGraphDialog : Gtk.Window
	{
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

		private void Workbench_ActiveDocumentChanged(object sender, EventArgs e)
		{
			if (this.animationFixed != null)
			{
				this.InitTreeMap();
				this.InitBoneOrderList();
			}
		}

		protected override void OnDestroyed()
		{
			Services.Workbench.ActiveDocumentChanged -= this.Workbench_ActiveDocumentChanged;
			base.OnDestroyed();
		}

		private void combox_Changed(object sender, EventArgs e)
		{
			this.animationFixed.CurrentScale = (double)(this.combox.Active + 1) / 2.0;
		}

		private void InitToolTip()
		{
			this.button_Normal.TooltipText = LanguageInfo.Group_Routine;
			this.button_BindBone.TooltipText = LanguageInfo.Skeleton_Binding;
			this.button_UnBindBone.TooltipText = LanguageInfo.Skeleton_Unbinding;
			this.button_Reference.TooltipText = LanguageInfo.Property_Reset;
			base.Title = LanguageInfo.Skeleton_SkeletonView;
		}

		private void button_Reference_Clicked(object sender, ButtonReleaseEventArgs e)
		{
			this.InitTreeMap();
		}

		private void button_UnBindBone_Clicked(object sender, ButtonReleaseEventArgs e)
		{
			this.animationFixed.Unbinding();
		}

		private void button_BindBone_CheckChanged(object sender, EventArgs e)
		{
			if (this.button_BindBone.IsChecked)
			{
				this.animationFixed.CurrentType = ChoiceType.Binding;
			}
		}

		private void button_Normal_CheckChanged(object sender, EventArgs e)
		{
			if (this.button_Normal.IsChecked)
			{
				this.animationFixed.CurrentType = ChoiceType.Normal;
			}
		}

		private void AnimationRelation_SizeAllocated(object o, SizeAllocatedArgs args)
		{
			this.animationFixed.DrawLine();
		}

		private void InitTreeMap()
		{
			this.animationFixed.SkeletonDialog = this;
			this.animationFixed.InitData();
		}

		public void BoneSkinSum(int bonecount, int skinSum)
		{
			this.boneLabel.Text = string.Format(LanguageInfo.Skeleton_BoneNum, bonecount);
			this.skinLabel.Text = string.Format(LanguageInfo.Skeleton_SkinNum, skinSum);
		}

		private void InitBoneOrderList()
		{
			this.treeOrder.InitModel();
		}

		private FixedEx animationFixed;

		private ScrolledWindow sw;

		private HBox hbox;

		private IconRadioButton button_Normal;

		private IconRadioButton button_BindBone;

		private IconButton button_UnBindBone;

		private IconButton button_Reference;

		private Table graphicalTable;

		private ComboBox combox;

		private Table bottomTable;

		private Table mainTable;

		private HPaned hpaned;

		private BoneOrderList treeOrder;

		private Label boneLabel;

		private Label skinLabel;
	}
}

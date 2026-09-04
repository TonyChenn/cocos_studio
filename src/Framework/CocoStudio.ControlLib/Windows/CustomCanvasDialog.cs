using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Stetic;

namespace CocoStudio.ControlLib.Windows
{
	// Token: 0x0200000C RID: 12
	public class CustomCanvasDialog : Dialog
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004634 File Offset: 0x00002834
		// (set) Token: 0x06000054 RID: 84 RVA: 0x0000464B File Offset: 0x0000284B
		public int Order { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00004654 File Offset: 0x00002854
		// (set) Token: 0x06000056 RID: 86 RVA: 0x0000466B File Offset: 0x0000286B
		public bool IsOK { get; set; }

		// Token: 0x06000057 RID: 87 RVA: 0x00004674 File Offset: 0x00002874
		public CustomCanvasDialog(int defaultWidth, int defaultHeight, string name = "", int order = 0)
		{
			this.parentWnd = MessageService.GetDefaultModalParent();
			this.PreSetBeforeShow(300, 180, this.parentWnd);
			this.isParentModal = this.parentWnd.Modal;
			this.parentWnd.Modal = false;
			this.Build();
			this.ChangeBtnPosion();
			this.buttonOk.Name = "MainButton";
			this.readLanuageConfigFile();
			this.Init();
			base.Name = name;
			this.spinbuttonName.Text = name;
			this.spinbuttonName.MaxLength = 30;
			this.spinbutton_Width.Value = (float)defaultWidth;
			this.spinbuttonHeight.Value = (float)defaultHeight;
			this.buttonOk.Clicked += this.buttonOk_Clicked;
			this.buttonCancel.Clicked += this.buttonCancel_Clicked;
			base.DeleteEvent += this.CustomCanvasWindow_DeleteEvent;
			base.ShowAll();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000480C File Offset: 0x00002A0C
		private void ChangeBtnPosion()
		{
			if (!Platform.IsMac)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				buttonBoxChild.Position = 0;
				buttonBoxChild2.Position = 1;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004860 File Offset: 0x00002A60
		private void Init()
		{
			this.SetToDialogStyle(null, true, true, true);
			this.buttonOk.GrabDefault();
			this.alignment_nameEntry.Add(this.spinbuttonName);
			this.alignment_widthEntry.Add(this.spinbutton_Width);
			this.alignment_heightEntry.Add(this.spinbuttonHeight);
			this.table_main.ShowAll();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000048C7 File Offset: 0x00002AC7
		private void CustomCanvasWindow_DeleteEvent(object o, DeleteEventArgs args)
		{
			this.IsOK = false;
			this.Destroy();
			this.parentWnd.Modal = this.isParentModal;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000048EB File Offset: 0x00002AEB
		private void buttonCancel_Clicked(object sender, EventArgs e)
		{
			this.IsOK = false;
			this.Destroy();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004900 File Offset: 0x00002B00
		private void buttonOk_Clicked(object sender, EventArgs e)
		{
			this.IsOK = true;
			this.canvasSize = new Size((int)this.spinbutton_Width.Value, (int)this.spinbuttonHeight.Value);
			base.Name = this.spinbuttonName.Text;
			this.Destroy();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004954 File Offset: 0x00002B54
		public void readLanuageConfigFile()
		{
			base.Title = LanguageInfo.ScreeSize_Dialog_WindowTitle;
			this.labelName.Text = LanguageInfo.Display_Name;
			this.labelWidth.Text = LanguageInfo.NewFile_Width;
			this.labelHeight.Text = LanguageInfo.NewFile_Height;
			this.labelPx1.Text = (this.labelPx2.Text = "px");
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000049E4 File Offset: 0x00002BE4
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 300;
			base.HeightRequest = 180;
			base.Name = "CocoStudio.ControlLib.Windows.CustomCanvasDialog";
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.TopPadding = 15U;
			this.alignment_main.BottomPadding = 8U;
			this.alignment_main.BorderWidth = 8U;
			this.table_main = new Table(3U, 2U, false);
			this.table_main.Name = "table_main";
			this.table_main.RowSpacing = 15U;
			this.table_main.ColumnSpacing = 6U;
			this.alignment_nameEntry = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_nameEntry.Name = "alignment_nameEntry";
			this.table_main.Add(this.alignment_nameEntry);
			Table.TableChild tableChild = (Table.TableChild)this.table_main[this.alignment_nameEntry];
			tableChild.LeftAttach = 1U;
			tableChild.RightAttach = 2U;
			tableChild.YOptions = AttachOptions.Fill;
			this.hbox_height = new HBox();
			this.hbox_height.Name = "hbox_height";
			this.hbox_height.Spacing = 6;
			this.alignment_heightEntry = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_heightEntry.Name = "alignment_heightEntry";
			this.hbox_height.Add(this.alignment_heightEntry);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_height[this.alignment_heightEntry];
			boxChild.Position = 0;
			this.labelPx2 = new Label();
			this.labelPx2.Name = "labelPx2";
			this.labelPx2.LabelProp = Catalog.GetString("px");
			this.hbox_height.Add(this.labelPx2);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_height[this.labelPx2];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.table_main.Add(this.hbox_height);
			Table.TableChild tableChild2 = (Table.TableChild)this.table_main[this.hbox_height];
			tableChild2.TopAttach = 2U;
			tableChild2.BottomAttach = 3U;
			tableChild2.LeftAttach = 1U;
			tableChild2.RightAttach = 2U;
			tableChild2.YOptions = AttachOptions.Fill;
			this.hbox_width = new HBox();
			this.hbox_width.Name = "hbox_width";
			this.hbox_width.Spacing = 6;
			this.alignment_widthEntry = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_widthEntry.Name = "alignment_widthEntry";
			this.hbox_width.Add(this.alignment_widthEntry);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_width[this.alignment_widthEntry];
			boxChild3.Position = 0;
			this.labelPx1 = new Label();
			this.labelPx1.Name = "labelPx1";
			this.labelPx1.LabelProp = Catalog.GetString("px");
			this.hbox_width.Add(this.labelPx1);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_width[this.labelPx1];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.table_main.Add(this.hbox_width);
			Table.TableChild tableChild3 = (Table.TableChild)this.table_main[this.hbox_width];
			tableChild3.TopAttach = 1U;
			tableChild3.BottomAttach = 2U;
			tableChild3.LeftAttach = 1U;
			tableChild3.RightAttach = 2U;
			tableChild3.YOptions = AttachOptions.Fill;
			this.labelHeight = new Label();
			this.labelHeight.Name = "labelHeight";
			this.labelHeight.Xalign = 1f;
			this.labelHeight.LabelProp = Catalog.GetString("高");
			this.table_main.Add(this.labelHeight);
			Table.TableChild tableChild4 = (Table.TableChild)this.table_main[this.labelHeight];
			tableChild4.TopAttach = 2U;
			tableChild4.BottomAttach = 3U;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			this.labelName = new Label();
			this.labelName.Name = "labelName";
			this.labelName.Xalign = 1f;
			this.labelName.LabelProp = Catalog.GetString("名称");
			this.table_main.Add(this.labelName);
			Table.TableChild tableChild5 = (Table.TableChild)this.table_main[this.labelName];
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.labelWidth = new Label();
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Xalign = 1f;
			this.labelWidth.LabelProp = Catalog.GetString("宽");
			this.table_main.Add(this.labelWidth);
			Table.TableChild tableChild6 = (Table.TableChild)this.table_main[this.labelWidth];
			tableChild6.TopAttach = 1U;
			tableChild6.BottomAttach = 2U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			this.alignment_main.Add(this.table_main);
			vbox.Add(this.alignment_main);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox[this.alignment_main];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.buttonCancel = new Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			base.AddActionWidget(this.buttonCancel, -6);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			this.buttonOk = new Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			base.AddActionWidget(this.buttonOk, -5);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 300;
			base.DefaultHeight = 180;
			base.Show();
		}

		// Token: 0x0400003D RID: 61
		public Size canvasSize = new Size(1, 1);

		// Token: 0x0400003E RID: 62
		private EntryEx spinbuttonName = new EntryEx();

		// Token: 0x0400003F RID: 63
		private EntryIntEx spinbutton_Width = new EntryIntEx
		{
			MinValue = 0,
			MaxValue = 100000,
			WidthRequest = 70,
			CanFocus = true,
			IsInteger = true
		};

		// Token: 0x04000040 RID: 64
		private EntryIntEx spinbuttonHeight = new EntryIntEx
		{
			MinValue = 0,
			MaxValue = 100000,
			WidthRequest = 70,
			CanFocus = true,
			IsInteger = true
		};

		// Token: 0x04000041 RID: 65
		private bool isParentModal;

		// Token: 0x04000042 RID: 66
		private Gtk.Window parentWnd;

		// Token: 0x04000043 RID: 67
		private Alignment alignment_main;

		// Token: 0x04000044 RID: 68
		private Table table_main;

		// Token: 0x04000045 RID: 69
		private Alignment alignment_nameEntry;

		// Token: 0x04000046 RID: 70
		private HBox hbox_height;

		// Token: 0x04000047 RID: 71
		private Alignment alignment_heightEntry;

		// Token: 0x04000048 RID: 72
		private Label labelPx2;

		// Token: 0x04000049 RID: 73
		private HBox hbox_width;

		// Token: 0x0400004A RID: 74
		private Alignment alignment_widthEntry;

		// Token: 0x0400004B RID: 75
		private Label labelPx1;

		// Token: 0x0400004C RID: 76
		private Label labelHeight;

		// Token: 0x0400004D RID: 77
		private Label labelName;

		// Token: 0x0400004E RID: 78
		private Label labelWidth;

		// Token: 0x0400004F RID: 79
		private Button buttonCancel;

		// Token: 0x04000050 RID: 80
		private Button buttonOk;
	}
}

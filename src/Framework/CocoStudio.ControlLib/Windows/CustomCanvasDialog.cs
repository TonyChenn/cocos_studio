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
	public class CustomCanvasDialog : Dialog
	{
		public int Order { get; set; }

		public bool IsOK { get; set; }

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

		private void Init()
		{
			this.SetToDialogStyle(null, true, true, true);
			this.buttonOk.GrabDefault();
			this.alignment_nameEntry.Add(this.spinbuttonName);
			this.alignment_widthEntry.Add(this.spinbutton_Width);
			this.alignment_heightEntry.Add(this.spinbuttonHeight);
			this.table_main.ShowAll();
		}

		private void CustomCanvasWindow_DeleteEvent(object o, DeleteEventArgs args)
		{
			this.IsOK = false;
			this.Destroy();
			this.parentWnd.Modal = this.isParentModal;
		}

		private void buttonCancel_Clicked(object sender, EventArgs e)
		{
			this.IsOK = false;
			this.Destroy();
		}

		private void buttonOk_Clicked(object sender, EventArgs e)
		{
			this.IsOK = true;
			this.canvasSize = new Size((int)this.spinbutton_Width.Value, (int)this.spinbuttonHeight.Value);
			base.Name = this.spinbuttonName.Text;
			this.Destroy();
		}

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

		public Size canvasSize = new Size(1, 1);

		private EntryEx spinbuttonName = new EntryEx();

		private EntryIntEx spinbutton_Width = new EntryIntEx
		{
			MinValue = 0,
			MaxValue = 100000,
			WidthRequest = 70,
			CanFocus = true,
			IsInteger = true
		};

		private EntryIntEx spinbuttonHeight = new EntryIntEx
		{
			MinValue = 0,
			MaxValue = 100000,
			WidthRequest = 70,
			CanFocus = true,
			IsInteger = true
		};

		private bool isParentModal;

		private Gtk.Window parentWnd;

		private Alignment alignment_main;

		private Table table_main;

		private Alignment alignment_nameEntry;

		private HBox hbox_height;

		private Alignment alignment_heightEntry;

		private Label labelPx2;

		private HBox hbox_width;

		private Alignment alignment_widthEntry;

		private Label labelPx1;

		private Label labelHeight;

		private Label labelName;

		private Label labelWidth;

		private Button buttonCancel;

		private Button buttonOk;
	}
}

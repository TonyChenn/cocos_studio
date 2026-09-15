using System;
using CocoStudio.Model.ViewModel;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.Render
{
	public class NewGuidesDialog : Dialog
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.Render.NewGuidesDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 20;
			this.vbox2.BorderWidth = 20U;
			this.frame1 = new Gtk.Frame();
			this.frame1.Name = "frame1";
			this.GtkAlignment2 = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = 12U;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.vbox3 = new VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 8;
			this.vbox3.BorderWidth = 8U;
			this.radiobutton_horizontal = new RadioButton(Catalog.GetString("radiobutton1"));
			this.radiobutton_horizontal.CanFocus = true;
			this.radiobutton_horizontal.Name = "radiobutton_horizontal";
			this.radiobutton_horizontal.DrawIndicator = true;
			this.radiobutton_horizontal.UseUnderline = true;
			this.radiobutton_horizontal.Group = new SList(IntPtr.Zero);
			this.vbox3.Add(this.radiobutton_horizontal);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox3[this.radiobutton_horizontal];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.radiobutton_vertical = new RadioButton(Catalog.GetString("radiobutton2"));
			this.radiobutton_vertical.CanFocus = true;
			this.radiobutton_vertical.Name = "radiobutton_vertical";
			this.radiobutton_vertical.DrawIndicator = true;
			this.radiobutton_vertical.UseUnderline = true;
			this.radiobutton_vertical.Group = this.radiobutton_horizontal.Group;
			this.vbox3.Add(this.radiobutton_vertical);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox3[this.radiobutton_vertical];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.hbox1.Add(this.vbox3);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox1[this.vbox3];
			boxChild3.Position = 0;
			this.GtkAlignment2.Add(this.hbox1);
			this.frame1.Add(this.GtkAlignment2);
			this.GtkLabel2 = new Label();
			this.GtkLabel2.Name = "GtkLabel2";
			this.GtkLabel2.LabelProp = Catalog.GetString("<b>GtkFrame</b>");
			this.GtkLabel2.UseMarkup = true;
			this.frame1.LabelWidget = this.GtkLabel2;
			this.vbox2.Add(this.frame1);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox2[this.frame1];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.hbox2 = new HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.label_position = new Label();
			this.label_position.Name = "label_position";
			this.label_position.Xalign = 1f;
			this.label_position.LabelProp = Catalog.GetString("label2");
			this.hbox2.Add(this.label_position);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox2[this.label_position];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.alignment_pixel = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_pixel.Name = "alignment_pixel";
			this.hbox2.Add(this.alignment_pixel);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox2[this.alignment_pixel];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.label_pixel = new Label();
			this.label_pixel.Name = "label_pixel";
			this.label_pixel.LabelProp = Catalog.GetString("label3");
			this.hbox2.Add(this.label_pixel);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox2[this.label_pixel];
			boxChild7.Position = 2;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.vbox2.Add(this.hbox2);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox2[this.hbox2];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			vbox.Add(this.vbox2);
			Box.BoxChild boxChild9 = (Box.BoxChild)vbox[this.vbox2];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
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
			base.DefaultWidth = 378;
			base.DefaultHeight = 211;
			base.Hide();
		}

		public NewGuidesDialog()
		{
			this.Build();
			this.Initialize();
			base.ShowAll();
		}

		private void Initialize()
		{
			base.SetSizeRequest(300, 210);
			base.AllowGrow = false;
			this.SetToDialogStyle(null, true, true, true);
			this.CenterToParentWindow(ApplicationCurrent.MainWindow);
			this.InitializeView();
			this.InitializeLanuage();
			this.InitializeEvent();
			this.buttonOk.GrabDefault();
			this.entry_pixel.ActivatesDefault = true;
		}

		private void InitializeView()
		{
			this.radiobutton_vertical.Active = true;
			this.buttonOk.Name = "MainButton";
			this.entry_pixel.Value = 0f;
			this.alignment_pixel.Add(this.entry_pixel);
			if (Platform.IsWindows)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				buttonBoxChild2.Position = 0;
				buttonBoxChild.Position = 1;
			}
		}

		private void InitializeLanuage()
		{
			base.Title = LanguageInfo.NewGuidesTitle;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.frame1.Label = LanguageInfo.Group_Direction;
			this.label_position.Text = LanguageInfo.TextBlock_Position;
			this.label_pixel.Text = LanguageInfo.NewFile_Pixel;
			this.radiobutton_horizontal.Label = LanguageInfo.Radio_HorizontalGuides;
			this.radiobutton_vertical.Label = LanguageInfo.Radio_VerticalGuides;
		}

		private void InitializeEvent()
		{
			this.buttonOk.Clicked += this.buttonOk_Clicked;
			this.buttonCancel.Clicked += this.buttonCancel_Clicked;
			base.DeleteEvent += new DeleteEventHandler(this.DestoryDialog);
		}

		private void AddGuides()
		{
			Orientation direction;
			if (this.radiobutton_vertical.Active)
			{
				direction = Orientation.Vertical;
			}
			else
			{
				direction = Orientation.Horizontal;
			}
			GuidesObject guidesObject = new GuidesObject(direction);
			guidesObject.Position = this.entry_pixel.Value;
			GuidesService.Instance.Add(guidesObject);
			GuidesService.Instance.Sort();
		}

		private void buttonOk_Clicked(object sender, EventArgs e)
		{
			this.buttonOk.GrabFocus();
			this.AddGuides();
			this.Destroy();
		}

		private void buttonCancel_Clicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		[ConnectBefore]
		private void DestoryDialog(object sender, EventArgs e)
		{
			this.Destroy();
		}

		private VBox vbox2;

		private Gtk.Frame frame1;

		private Alignment GtkAlignment2;

		private HBox hbox1;

		private VBox vbox3;

		private RadioButton radiobutton_horizontal;

		private RadioButton radiobutton_vertical;

		private Label GtkLabel2;

		private HBox hbox2;

		private Label label_position;

		private Alignment alignment_pixel;

		private Label label_pixel;

		private Button buttonCancel;

		private Button buttonOk;

		private EntryIntEx entry_pixel = new EntryIntEx
		{
			MinValue = -10000,
			MaxValue = 10000,
			WidthRequest = 80,
			CanFocus = true,
			IsInteger = false
		};
	}
}

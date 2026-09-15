using System;
using Gdk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using Stetic;

namespace Gtk
{
	public class ColorPickerDialog : Dialog
	{
		public ColorSelection ColorPicker { get; private set; }

		public ColorPickerDialog(Color initColor)
		{
			base.Modal = true;
			this.InitColorSelection(initColor);
			this.InitWidgetReference();
			this.Build();
			this.eventbox_bg.Add(this.ColorPicker);
			this.InitButton();
			this.InitMultiLanguage();
			base.Destroyed += this.DestroyedHandler;
			this.SetToDialogStyle(null, true, true, true);
		}

		private void InitColorSelection(Color initColor)
		{
			this.ColorPicker = new ColorSelection();
			this.ColorPicker.PreviousColor = initColor;
			this.ColorPicker.CurrentColor = initColor;
			this.ColorPicker.HasOpacityControl = false;
			HBox hbox = this.ColorPicker.Children[0] as HBox;
			VBox vbox = hbox.Children[1] as VBox;
			this.palette = new PaletteWidget(this.ColorPicker);
			vbox.PackEnd(this.palette, false, false, 0U);
			this.palette.Show();
			this.ColorPicker.ShowAll();
		}

		private void InitWidgetReference()
		{
			HBox hbox = this.ColorPicker.Children[0] as HBox;
			VBox vbox = hbox.Children[0] as VBox;
			this.wColorWheel = vbox.Children[0];
			HBox hbox2 = vbox.Children[1] as HBox;
			if (hbox2.Children.Length > 1)
			{
				this.wDropper = hbox2.Children[1];
			}
			Frame frame = hbox2.Children[0] as Frame;
			HBox hbox3 = frame.Children[0] as HBox;
			this.wPreColor = hbox3.Children[0];
			this.wCurColor = hbox3.Children[1];
			VBox vbox2 = hbox.Children[1] as VBox;
			Table table = vbox2.Children[0] as Table;
			this.entryColorName = (table.Children[0] as Entry);
			this.labelColorName = (table.Children[1] as Label);
			this.spinBlue = (table.Children[6] as SpinButton);
			this.labelBlue = (table.Children[7] as Label);
			this.spinGreen = (table.Children[8] as SpinButton);
			this.labelGreen = (table.Children[9] as Label);
			this.spinRed = (table.Children[10] as SpinButton);
			this.labelRed = (table.Children[11] as Label);
			this.spinValue = (table.Children[12] as SpinButton);
			this.labelValue = (table.Children[13] as Label);
			this.spinSaturation = (table.Children[14] as SpinButton);
			this.labelSaturation = (table.Children[15] as Label);
			this.spinHue = (table.Children[16] as SpinButton);
			this.labelHue = (table.Children[17] as Label);
			if (Platform.IsMac && this.wDropper != null)
			{
				hbox2.Remove(this.wDropper);
			}
		}

		private void InitButton()
		{
			this.buttonOk.Name = "MainButton";
			this.buttonOk.HasFocus = true;
			if (Platform.IsWindows)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				buttonBoxChild.Position = 0;
				buttonBoxChild2.Position = 1;
			}
		}

		private void InitMultiLanguage()
		{
			base.Title = LanguageInfo.Property_ColorPicker;
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.labelHue.LabelProp = LanguageInfo.ColorPicker_Hue;
			this.labelSaturation.LabelProp = LanguageInfo.ColorPicker_Saturation;
			this.labelValue.LabelProp = LanguageInfo.ColorPicker_Value;
			this.labelRed.LabelProp = LanguageInfo.ColorPicker_Red;
			this.labelGreen.LabelProp = LanguageInfo.ColorPicker_Green;
			this.labelBlue.LabelProp = LanguageInfo.ColorPicker_Blue;
			this.labelColorName.LabelProp = LanguageInfo.ColorPicker_ColorName;
			this.spinHue.TooltipText = LanguageInfo.ColorPicker_ColorWheel;
			this.spinSaturation.TooltipText = LanguageInfo.ColorPicker_Deepness;
			this.spinValue.TooltipText = LanguageInfo.ColorPicker_Brightness;
			this.spinRed.TooltipText = LanguageInfo.ColorPicker_RedAmount;
			this.spinGreen.TooltipText = LanguageInfo.ColorPicker_GreenAmount;
			this.spinBlue.TooltipText = LanguageInfo.ColorPicker_BlueAmount;
			this.entryColorName.TooltipText = LanguageInfo.ColorPicker_EnterInfo;
			this.wPreColor.TooltipText = LanguageInfo.ColorPicker_PreColor;
			this.wCurColor.TooltipText = LanguageInfo.ColorPicker_CurColor;
			this.wColorWheel.TooltipText = LanguageInfo.ColorPicker_SelectInfo;
			if (Platform.IsWindows)
			{
				this.wDropper.TooltipText = LanguageInfo.ColorPicker_Dropper;
			}
		}

		private void DestroyedHandler(object sender, EventArgs e)
		{
			this.palette.SaveColors();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Gtk.ColorPickerDialog";
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.eventbox_bg = new EventBox();
			this.eventbox_bg.Name = "eventbox_bg";
			this.eventbox_bg.BorderWidth = 12U;
			vbox.Add(this.eventbox_bg);
			Box.BoxChild boxChild = (Box.BoxChild)vbox[this.eventbox_bg];
			boxChild.Position = 0;
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
			base.DefaultWidth = 535;
			base.DefaultHeight = 294;
			base.Hide();
		}

		private Label labelHue;

		private Label labelSaturation;

		private Label labelValue;

		private Label labelRed;

		private Label labelGreen;

		private Label labelBlue;

		private Label labelColorName;

		private Widget wColorWheel;

		private Widget wPreColor;

		private Widget wCurColor;

		private Widget wDropper;

		private SpinButton spinHue;

		private SpinButton spinSaturation;

		private SpinButton spinValue;

		private SpinButton spinRed;

		private SpinButton spinGreen;

		private SpinButton spinBlue;

		private Entry entryColorName;

		private PaletteWidget palette;

		private EventBox eventbox_bg;

		private Button buttonCancel;

		private Button buttonOk;
	}
}

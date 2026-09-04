using System;
using Gdk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using Stetic;

namespace Gtk
{
	// Token: 0x02000096 RID: 150
	public class ColorPickerDialog : Dialog
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000CE6C File Offset: 0x0000B06C
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0000CE83 File Offset: 0x0000B083
		public ColorSelection ColorPicker { get; private set; }

		// Token: 0x06000328 RID: 808 RVA: 0x0000CE8C File Offset: 0x0000B08C
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

		// Token: 0x06000329 RID: 809 RVA: 0x0000CF00 File Offset: 0x0000B100
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

		// Token: 0x0600032A RID: 810 RVA: 0x0000CF9C File Offset: 0x0000B19C
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

		// Token: 0x0600032B RID: 811 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
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

		// Token: 0x0600032C RID: 812 RVA: 0x0000D228 File Offset: 0x0000B428
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

		// Token: 0x0600032D RID: 813 RVA: 0x0000D3A2 File Offset: 0x0000B5A2
		private void DestroyedHandler(object sender, EventArgs e)
		{
			this.palette.SaveColors();
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000D3B4 File Offset: 0x0000B5B4
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

		// Token: 0x040003AC RID: 940
		private Label labelHue;

		// Token: 0x040003AD RID: 941
		private Label labelSaturation;

		// Token: 0x040003AE RID: 942
		private Label labelValue;

		// Token: 0x040003AF RID: 943
		private Label labelRed;

		// Token: 0x040003B0 RID: 944
		private Label labelGreen;

		// Token: 0x040003B1 RID: 945
		private Label labelBlue;

		// Token: 0x040003B2 RID: 946
		private Label labelColorName;

		// Token: 0x040003B3 RID: 947
		private Widget wColorWheel;

		// Token: 0x040003B4 RID: 948
		private Widget wPreColor;

		// Token: 0x040003B5 RID: 949
		private Widget wCurColor;

		// Token: 0x040003B6 RID: 950
		private Widget wDropper;

		// Token: 0x040003B7 RID: 951
		private SpinButton spinHue;

		// Token: 0x040003B8 RID: 952
		private SpinButton spinSaturation;

		// Token: 0x040003B9 RID: 953
		private SpinButton spinValue;

		// Token: 0x040003BA RID: 954
		private SpinButton spinRed;

		// Token: 0x040003BB RID: 955
		private SpinButton spinGreen;

		// Token: 0x040003BC RID: 956
		private SpinButton spinBlue;

		// Token: 0x040003BD RID: 957
		private Entry entryColorName;

		// Token: 0x040003BE RID: 958
		private PaletteWidget palette;

		// Token: 0x040003BF RID: 959
		private EventBox eventbox_bg;

		// Token: 0x040003C0 RID: 960
		private Button buttonCancel;

		// Token: 0x040003C1 RID: 961
		private Button buttonOk;
	}
}

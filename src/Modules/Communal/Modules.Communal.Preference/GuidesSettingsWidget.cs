using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Basic;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Preference.Model;
using Modules.Communal.Render.Model;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.Preference
{
	// Token: 0x02000016 RID: 22
	[ToolboxItem(true)]
	public class GuidesSettingsWidget : Bin, IPreferenceWidget
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000A244 File Offset: 0x00008444
		public EnumPreferenceSetting SettingID
		{
			get
			{
				return EnumPreferenceSetting.GuidesSettings;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000A247 File Offset: 0x00008447
		public string DisplayName
		{
			get
			{
				return LanguageInfo.Menu_View_Guides;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000A24E File Offset: 0x0000844E
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x0000A258 File Offset: 0x00008458
		public System.Drawing.Color SelectedColor
		{
			get
			{
				return this.selectedColor;
			}
			set
			{
				this.selectedColor = value;
				Gdk.Color color = Colors.DrawingToGdkColor(value);
				this.eventbox1.ModifyBg(StateType.Normal, color);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000A280 File Offset: 0x00008480
		public GuidesSettingsWidget()
		{
			this.Build();
			this.Initialize();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000A2AB File Offset: 0x000084AB
		private void Initialize()
		{
			this.InitView();
			this.InitDefaultValue();
			this.InitEvent();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000A2C0 File Offset: 0x000084C0
		private void InitView()
		{
			this.combobox_color.Clear();
			this.liststore = new ListStore(new Type[]
			{
				typeof(string),
				typeof(GuidesColorInfo)
			});
			this.combobox_color.Model = this.liststore;
			CellGuidesColorInfo cell = new CellGuidesColorInfo();
			this.combobox_color.PackStart(cell, false);
			this.combobox_color.AddAttribute(cell, "text", 0);
			this.combobox_color.AddAttribute(cell, "Cell_Guides_Data", 1);
			ListStore listStore = this.liststore;
			object[] array = new object[2];
			array[0] = "  " + LanguageInfo.Dialog_Publish_Custom;
			listStore.AppendValues(array);
			foreach (GuidesColorInfo guidesColorInfo in GuidesColorData.Instance.GuidesColorList)
			{
				this.liststore.AppendValues(new object[]
				{
					"     " + guidesColorInfo.Name,
					guidesColorInfo
				});
			}
			this.GtkLabel_download.Text = string.Format(" {0} ", LanguageInfo.Menu_View_Guides);
			this.label1.Text = LanguageInfo.MainTool_Color;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000A410 File Offset: 0x00008610
		private void InitEvent()
		{
			this.combobox_color.Changed += this.combobox_color_Changed;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000A444 File Offset: 0x00008644
		private void InitDefaultValue()
		{
			if (!string.IsNullOrEmpty(Option.UserConfig.GuidesColor))
			{
				int argb;
				bool flag = int.TryParse(Option.UserConfig.GuidesColor, out argb);
				if (flag)
				{
					System.Drawing.Color color = System.Drawing.Color.FromArgb(argb);
					int num = GuidesColorData.Instance.GuidesColorList.FindIndex((GuidesColorInfo g) => g.RenderColor == color);
					if (num >= 0)
					{
						this.combobox_color.Active = num + 1;
					}
					else
					{
						this.combobox_color.Active = 0;
					}
					this.SelectedColor = color;
					return;
				}
			}
			this.combobox_color.Active = 1;
			this.SelectedColor = this.defaultColor;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000A4EC File Offset: 0x000086EC
		private void combobox_color_Changed(object sender, EventArgs e)
		{
			TreeIter zero = TreeIter.Zero;
			this.combobox_color.GetActiveIter(out zero);
			GuidesColorInfo guidesColorInfo = this.combobox_color.Model.GetValue(zero, 1) as GuidesColorInfo;
			if (guidesColorInfo == null)
			{
				Gdk.Color initColor = Colors.DrawingToGdkColor(this.SelectedColor);
				ColorPickerDialog colorPickerDialog = new ColorPickerDialog(initColor);
				if (colorPickerDialog.Run() == -5)
				{
					this.SelectedColor = Colors.GdkToDrawingColor(colorPickerDialog.ColorPicker.CurrentColor);
				}
				colorPickerDialog.Destroy();
				return;
			}
			this.SelectedColor = guidesColorInfo.RenderColor;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000A570 File Offset: 0x00008770
		public void ApplySetting()
		{
			Option.UserConfig.GuidesColor = this.SelectedColor.ToArgb().ToString();
			GuidesService.Instance.LineColor = this.SelectedColor;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000A5AD File Offset: 0x000087AD
		public bool CanApply(out string output)
		{
			output = "";
			return true;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000A5B7 File Offset: 0x000087B7
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000A5BC File Offset: 0x000087BC
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.Preference.GuidesSettingsWidget";
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.frame_download = new Frame();
			this.frame_download.Name = "frame_download";
			this.GtkAlignment_download = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_download.Name = "GtkAlignment_download";
			this.GtkAlignment_download.LeftPadding = 20U;
			this.GtkAlignment_download.TopPadding = 15U;
			this.GtkAlignment_download.RightPadding = 10U;
			this.GtkAlignment_download.BottomPadding = 15U;
			this.vbox_content = new VBox();
			this.vbox_content.Name = "vbox_content";
			this.vbox_content.Spacing = 6;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.label1 = new Label();
			this.label1.Name = "label1";
			this.label1.Yalign = 0.1f;
			this.label1.LabelProp = Catalog.GetString("颜色");
			this.hbox1.Add(this.label1);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox1[this.label1];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment_combox = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_combox.Name = "alignment_combox";
			this.alignment_combox.BottomPadding = 16U;
			this.combobox_color = ComboBox.NewText();
			this.combobox_color.WidthRequest = 100;
			this.combobox_color.Name = "combobox_color";
			this.alignment_combox.Add(this.combobox_color);
			this.hbox1.Add(this.alignment_combox);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox1[this.alignment_combox];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment2.Name = "alignment2";
			this.hbox1.Add(this.alignment2);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox1[this.alignment2];
			boxChild3.Position = 2;
			this.alignment3 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment3.Name = "alignment3";
			this.alignment3.BottomPadding = 3U;
			this.eventbox1 = new EventBox();
			this.eventbox1.WidthRequest = 40;
			this.eventbox1.Name = "eventbox1";
			this.alignment3.Add(this.eventbox1);
			this.hbox1.Add(this.alignment3);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox1[this.alignment3];
			boxChild4.Position = 3;
			boxChild4.Expand = false;
			this.vbox_content.Add(this.hbox1);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_content[this.hbox1];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.GtkAlignment_download.Add(this.vbox_content);
			this.frame_download.Add(this.GtkAlignment_download);
			this.GtkLabel_download = new Label();
			this.GtkLabel_download.Name = "GtkLabel_download";
			this.GtkLabel_download.LabelProp = Catalog.GetString("参考线");
			this.GtkLabel_download.UseMarkup = true;
			this.frame_download.LabelWidget = this.GtkLabel_download;
			this.vbox2.Add(this.frame_download);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox2[this.frame_download];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.vbox2.Add(this.alignment1);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox2[this.alignment1];
			boxChild7.Position = 1;
			base.Add(this.vbox2);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x040000DD RID: 221
		private ListStore liststore;

		// Token: 0x040000DE RID: 222
		private System.Drawing.Color selectedColor;

		// Token: 0x040000DF RID: 223
		private System.Drawing.Color defaultColor = System.Drawing.Color.FromArgb(74, 255, 255);

		// Token: 0x040000E0 RID: 224
		private VBox vbox2;

		// Token: 0x040000E1 RID: 225
		private Frame frame_download;

		// Token: 0x040000E2 RID: 226
		private Alignment GtkAlignment_download;

		// Token: 0x040000E3 RID: 227
		private VBox vbox_content;

		// Token: 0x040000E4 RID: 228
		private HBox hbox1;

		// Token: 0x040000E5 RID: 229
		private Label label1;

		// Token: 0x040000E6 RID: 230
		private Alignment alignment_combox;

		// Token: 0x040000E7 RID: 231
		private ComboBox combobox_color;

		// Token: 0x040000E8 RID: 232
		private Alignment alignment2;

		// Token: 0x040000E9 RID: 233
		private Alignment alignment3;

		// Token: 0x040000EA RID: 234
		private EventBox eventbox1;

		// Token: 0x040000EB RID: 235
		private Label GtkLabel_download;

		// Token: 0x040000EC RID: 236
		private Alignment alignment1;
	}
}

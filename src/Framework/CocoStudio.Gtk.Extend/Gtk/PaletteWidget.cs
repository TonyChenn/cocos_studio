using System;
using System.ComponentModel;
using CocoStudio.Basic;
using Gdk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Gtk
{
	[ToolboxItem(true)]
	public class PaletteWidget : Bin
	{
		public PaletteWidget(ColorSelection selection)
		{
			this.Build();
			this.InitColorTable(selection);
			this.label_title.LabelProp = LanguageInfo.ColorPicker_Palette;
		}

		private void InitColorTable(ColorSelection selection)
		{
			this.colorList = new Color[20];
			this.colorList[0] = new Color(0, 0, 0);
			this.colorList[1] = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.colorList[2] = new Color(127, 127, 127);
			this.colorList[3] = new Color(byte.MaxValue, 0, 0);
			this.colorList[4] = new Color(128, 0, 128);
			this.colorList[5] = new Color(0, 0, byte.MaxValue);
			this.colorList[6] = new Color(173, 216, 230);
			this.colorList[7] = new Color(0, 128, 0);
			this.colorList[8] = new Color(byte.MaxValue, byte.MaxValue, 0);
			this.colorList[9] = new Color(byte.MaxValue, 165, 0);
			this.colorList[10] = new Color(230, 230, 250);
			this.colorList[11] = new Color(165, 42, 42);
			this.colorList[12] = new Color(139, 105, 20);
			this.colorList[13] = new Color(30, 144, byte.MaxValue);
			this.colorList[14] = new Color(byte.MaxValue, 192, 203);
			this.colorList[15] = new Color(144, 238, 144);
			this.colorList[16] = new Color(26, 26, 26);
			this.colorList[17] = new Color(77, 77, 77);
			this.colorList[18] = new Color(191, 191, 191);
			this.colorList[19] = new Color(229, 229, 229);
			this.colorList = this.LoadColors(this.colorList);
			for (int i = 1; i >= 0; i--)
			{
				for (int j = 9; j >= 0; j--)
				{
					PaletteBlockWidget paletteBlockWidget = new PaletteBlockWidget(this.colorList[j + i * 10], selection);
					this.table_palette.Attach(paletteBlockWidget, (uint)j, (uint)(j + 1), (uint)i, (uint)(i + 1));
					paletteBlockWidget.Show();
				}
			}
			this.table_palette.TooltipText = LanguageInfo.ColorPicker_PaletteInfo;
		}

		private Color[] LoadColors(Color[] colors)
		{
			string paletteColors = Option.UserConfig.PaletteColors;
			Color[] result;
			if (string.IsNullOrEmpty(paletteColors))
			{
				result = colors;
			}
			else
			{
				try
				{
					Color[] array = ColorSelection.PaletteFromString(paletteColors);
					for (int i = 0; i < array.Length; i++)
					{
						if (i >= colors.Length)
						{
							break;
						}
						colors[i] = array[i];
					}
					result = colors;
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("从配置文件获取历史颜色记录时出错", exception);
					result = colors;
				}
			}
			return result;
		}

		public void SaveColors()
		{
			Color[] array = new Color[20];
			int num = 0;
			foreach (object obj in this.table_palette)
			{
				PaletteBlockWidget paletteBlockWidget = (PaletteBlockWidget)obj;
				array[num] = paletteBlockWidget.Color;
				num++;
			}
			string paletteColors = ColorSelection.PaletteToString(array);
			Option.UserConfig.PaletteColors = paletteColors;
			Option.UserConfig.Save();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Gtk.PaletteWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.label_title = new Label();
			this.label_title.Name = "label_title";
			this.label_title.Xalign = 0f;
			this.label_title.LabelProp = Catalog.GetString("调色板(_P)：");
			this.label_title.UseUnderline = true;
			this.vbox_main.Add(this.label_title);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.label_title];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.table_palette = new Table(2U, 10U, false);
			this.table_palette.Name = "table_palette";
			this.table_palette.RowSpacing = 1U;
			this.table_palette.ColumnSpacing = 1U;
			this.vbox_main.Add(this.table_palette);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_main[this.table_palette];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private Color[] colorList;

		private VBox vbox_main;

		private Label label_title;

		private Table table_palette;
	}
}

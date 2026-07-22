using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.ProjectSetting
{
	// Token: 0x02000011 RID: 17
	[ToolboxItem(true)]
	public class HTML5Widget : Bin, IProjectSettingWidget
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00009D40 File Offset: 0x00007F40
		public HTML5Widget()
		{
			this.Build();
			this.InitWidget();
			this.InitStyle();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00009D5C File Offset: 0x00007F5C
		private void InitWidget()
		{
			PackageParams packageParams = PackageServices.Instance.PackageParams;
			this.checkbutton_sourceMap.Active = packageParams.EnableSourceMap;
			this.checkbutton_advanced.Active = packageParams.EnableHTML5Advanced;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00009D98 File Offset: 0x00007F98
		private void InitStyle()
		{
			this.GtkLabel_package.Text = " " + LanguageInfo.Package_PackageSetting + " ";
			this.checkbutton_sourceMap.Label = LanguageInfo.Package_EnableSourceMap;
			this.checkbutton_advanced.Label = LanguageInfo.Package_EnableAdvanced;
			this.checkbutton_advanced.TooltipText = LanguageInfo.Package_AdvancedDesc;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00009DF4 File Offset: 0x00007FF4
		public EnumProjectSetting SettingID
		{
			get
			{
				return EnumProjectSetting.HTML5;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00009DF7 File Offset: 0x00007FF7
		public string DisplayName
		{
			get
			{
				return "HTML5";
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00009E00 File Offset: 0x00008000
		public void ApplySetting()
		{
			PackageParams packageParams = PackageServices.Instance.PackageParams;
			packageParams.EnableSourceMap = this.checkbutton_sourceMap.Active;
			packageParams.EnableHTML5Advanced = this.checkbutton_advanced.Active;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00009E3A File Offset: 0x0000803A
		public bool CanApply(out string output)
		{
			output = "";
			return true;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00009E44 File Offset: 0x00008044
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00009E47 File Offset: 0x00008047
		public List<IProjectSettingWidget> SubWidgets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00009E4C File Offset: 0x0000804C
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.ProjectSetting.HTML5Widget";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 10;
			this.frame_package = new Frame();
			this.frame_package.Name = "frame_package";
			this.GtkAlignment_package = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_package.Name = "GtkAlignment_package";
			this.GtkAlignment_package.LeftPadding = 15U;
			this.GtkAlignment_package.TopPadding = 10U;
			this.GtkAlignment_package.RightPadding = 10U;
			this.GtkAlignment_package.BottomPadding = 10U;
			this.GtkAlignment_package.BorderWidth = 5U;
			this.vbox_package = new VBox();
			this.vbox_package.Name = "vbox_package";
			this.vbox_package.Spacing = 12;
			this.hbox_sourceMap = new HBox();
			this.hbox_sourceMap.Name = "hbox_sourceMap";
			this.hbox_sourceMap.Spacing = 6;
			this.checkbutton_sourceMap = new CheckButton();
			this.checkbutton_sourceMap.CanFocus = true;
			this.checkbutton_sourceMap.Name = "checkbutton_sourceMap";
			this.checkbutton_sourceMap.Label = Catalog.GetString("启用 Source Map");
			this.checkbutton_sourceMap.DrawIndicator = true;
			this.checkbutton_sourceMap.UseUnderline = true;
			this.hbox_sourceMap.Add(this.checkbutton_sourceMap);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_sourceMap[this.checkbutton_sourceMap];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.vbox_package.Add(this.hbox_sourceMap);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_package[this.hbox_sourceMap];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.hbox_advanced = new HBox();
			this.hbox_advanced.Name = "hbox_advanced";
			this.hbox_advanced.Spacing = 6;
			this.checkbutton_advanced = new CheckButton();
			this.checkbutton_advanced.CanFocus = true;
			this.checkbutton_advanced.Name = "checkbutton_advanced";
			this.checkbutton_advanced.Label = Catalog.GetString("启用高级编译");
			this.checkbutton_advanced.DrawIndicator = true;
			this.checkbutton_advanced.UseUnderline = true;
			this.hbox_advanced.Add(this.checkbutton_advanced);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_advanced[this.checkbutton_advanced];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			this.vbox_package.Add(this.hbox_advanced);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_package[this.hbox_advanced];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.GtkAlignment_package.Add(this.vbox_package);
			this.frame_package.Add(this.GtkAlignment_package);
			this.GtkLabel_package = new Label();
			this.GtkLabel_package.Name = "GtkLabel_package";
			this.GtkLabel_package.LabelProp = Catalog.GetString(" 打包设置 ");
			this.GtkLabel_package.UseMarkup = true;
			this.frame_package.LabelWidget = this.GtkLabel_package;
			this.vbox_main.Add(this.frame_package);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_main[this.frame_package];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.alignment_main.Add(this.vbox_main);
			base.Add(this.alignment_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x040000CC RID: 204
		private Alignment alignment_main;

		// Token: 0x040000CD RID: 205
		private VBox vbox_main;

		// Token: 0x040000CE RID: 206
		private Frame frame_package;

		// Token: 0x040000CF RID: 207
		private Alignment GtkAlignment_package;

		// Token: 0x040000D0 RID: 208
		private VBox vbox_package;

		// Token: 0x040000D1 RID: 209
		private HBox hbox_sourceMap;

		// Token: 0x040000D2 RID: 210
		private CheckButton checkbutton_sourceMap;

		// Token: 0x040000D3 RID: 211
		private HBox hbox_advanced;

		// Token: 0x040000D4 RID: 212
		private CheckButton checkbutton_advanced;

		// Token: 0x040000D5 RID: 213
		private Label GtkLabel_package;
	}
}

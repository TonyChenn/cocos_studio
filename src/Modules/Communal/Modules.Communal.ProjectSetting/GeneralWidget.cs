using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.ProjectSetting
{
	// Token: 0x02000010 RID: 16
	[ToolboxItem(true)]
	public class GeneralWidget : Bin, IProjectSettingWidget
	{
		// Token: 0x0600007B RID: 123 RVA: 0x000098AA File Offset: 0x00007AAA
		public GeneralWidget()
		{
			this.Build();
			this.Init();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000098C0 File Offset: 0x00007AC0
		private void Init()
		{
			this.isOldNameStandardized = Services.ProjectsService.CurrentSolution.Config.IsNameStandardized;
			this.checkbutton_nameFormat.Active = this.isOldNameStandardized;
			this.alignment_warring.Remove(this.label_warring);
			this.checkbutton_nameFormat.Toggled += this.CheckButtonToggledHandler;
			this.GtkLabel_name.Text = " " + LanguageInfo.ProjSetting_ControlName + " ";
			this.checkbutton_nameFormat.Label = LanguageInfo.ProjSetting_EnableUniformName;
			this.label_warring.Text = LanguageInfo.ProjSetting_NameingDesc;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000995F File Offset: 0x00007B5F
		public EnumProjectSetting SettingID
		{
			get
			{
				return EnumProjectSetting.General;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00009962 File Offset: 0x00007B62
		public string DisplayName
		{
			get
			{
				return LanguageInfo.Group_Routine;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00009969 File Offset: 0x00007B69
		public void ApplySetting()
		{
			Services.ProjectsService.CurrentSolution.Config.IsNameStandardized = this.checkbutton_nameFormat.Active;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000998A File Offset: 0x00007B8A
		public bool CanApply(out string output)
		{
			output = "";
			return true;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00009994 File Offset: 0x00007B94
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00009997 File Offset: 0x00007B97
		public List<IProjectSettingWidget> SubWidgets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000999C File Offset: 0x00007B9C
		private void CheckButtonToggledHandler(object sender, EventArgs e)
		{
			if (!this.isOldNameStandardized && this.checkbutton_nameFormat.Active)
			{
				this.alignment_warring.Add(this.label_warring);
				return;
			}
			if (this.alignment_warring.Child != null)
			{
				this.alignment_warring.Remove(this.alignment_warring.Child);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000099F3 File Offset: 0x00007BF3
		protected void VboxSizeAllocatedHandler(object o, SizeAllocatedArgs args)
		{
			this.label_warring.WidthRequest = args.Allocation.Width;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00009A0C File Offset: 0x00007C0C
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.ProjectSetting.GeneralWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 10;
			this.frame_name = new Frame();
			this.frame_name.Name = "frame_name";
			this.GtkAlignment_name = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_name.Name = "GtkAlignment_name";
			this.GtkAlignment_name.LeftPadding = 20U;
			this.GtkAlignment_name.TopPadding = 10U;
			this.GtkAlignment_name.RightPadding = 10U;
			this.GtkAlignment_name.BottomPadding = 10U;
			this.vbox_name = new VBox();
			this.vbox_name.Name = "vbox_name";
			this.vbox_name.Spacing = 6;
			this.checkbutton_nameFormat = new CheckButton();
			this.checkbutton_nameFormat.CanFocus = true;
			this.checkbutton_nameFormat.Name = "checkbutton_nameFormat";
			this.checkbutton_nameFormat.Label = Catalog.GetString("启用规范化命名");
			this.checkbutton_nameFormat.DrawIndicator = true;
			this.checkbutton_nameFormat.UseUnderline = true;
			this.vbox_name.Add(this.checkbutton_nameFormat);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_name[this.checkbutton_nameFormat];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment_warring = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_warring.Name = "alignment_warring";
			this.label_warring = new Label();
			this.label_warring.Name = "label_warring";
			this.label_warring.Xalign = 0f;
			this.label_warring.LabelProp = Catalog.GetString("规范化命名仅对之后的命名修改有效，并不会修改之前的控件命名");
			this.label_warring.Wrap = true;
			this.alignment_warring.Add(this.label_warring);
			this.vbox_name.Add(this.alignment_warring);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_name[this.alignment_warring];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.GtkAlignment_name.Add(this.vbox_name);
			this.frame_name.Add(this.GtkAlignment_name);
			this.GtkLabel_name = new Label();
			this.GtkLabel_name.Name = "GtkLabel_name";
			this.GtkLabel_name.LabelProp = Catalog.GetString(" 控件命名 ");
			this.GtkLabel_name.UseMarkup = true;
			this.frame_name.LabelWidget = this.GtkLabel_name;
			this.vbox_main.Add(this.frame_name);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_main[this.frame_name];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.vbox_name.SizeAllocated += this.VboxSizeAllocatedHandler;
		}

		// Token: 0x040000C3 RID: 195
		private bool isOldNameStandardized;

		// Token: 0x040000C4 RID: 196
		private VBox vbox_main;

		// Token: 0x040000C5 RID: 197
		private Frame frame_name;

		// Token: 0x040000C6 RID: 198
		private Alignment GtkAlignment_name;

		// Token: 0x040000C7 RID: 199
		private VBox vbox_name;

		// Token: 0x040000C8 RID: 200
		private CheckButton checkbutton_nameFormat;

		// Token: 0x040000C9 RID: 201
		private Alignment alignment_warring;

		// Token: 0x040000CA RID: 202
		private Label label_warring;

		// Token: 0x040000CB RID: 203
		private Label GtkLabel_name;
	}
}

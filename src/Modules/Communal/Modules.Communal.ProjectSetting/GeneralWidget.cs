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
	[ToolboxItem(true)]
	public class GeneralWidget : Bin, IProjectSettingWidget
	{
		public GeneralWidget()
		{
			this.Build();
			this.Init();
		}

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

		public EnumProjectSetting SettingID
		{
			get
			{
				return EnumProjectSetting.General;
			}
		}

		public string DisplayName
		{
			get
			{
				return LanguageInfo.Group_Routine;
			}
		}

		public void ApplySetting()
		{
			Services.ProjectsService.CurrentSolution.Config.IsNameStandardized = this.checkbutton_nameFormat.Active;
		}

		public bool CanApply(out string output)
		{
			output = "";
			return true;
		}

		public Widget GetWidget()
		{
			return this;
		}

		public List<IProjectSettingWidget> SubWidgets
		{
			get
			{
				return null;
			}
		}

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

		protected void VboxSizeAllocatedHandler(object o, SizeAllocatedArgs args)
		{
			this.label_warring.WidthRequest = args.Allocation.Width;
		}

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

		private bool isOldNameStandardized;

		private VBox vbox_main;

		private Frame frame_name;

		private Alignment GtkAlignment_name;

		private VBox vbox_name;

		private CheckButton checkbutton_nameFormat;

		private Alignment alignment_warring;

		private Label label_warring;

		private Label GtkLabel_name;
	}
}

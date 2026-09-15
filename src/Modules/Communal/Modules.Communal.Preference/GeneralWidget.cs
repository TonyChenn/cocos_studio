using System;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Basic;
using Gtk;
using Modules.Communal.CocosCodeIDE;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.Preference
{
	[ToolboxItem(true)]
	public class GeneralWidget : Bin, IPreferenceWidget
	{
		public EnumPreferenceSetting SettingID
		{
			get
			{
				return EnumPreferenceSetting.General;
			}
		}

		public string DisplayName
		{
			get
			{
				return LanguageInfo.Display_Component_Group_General;
			}
		}

		public GeneralWidget()
		{
			this.Build();
			this.checkbutton_mouse.Active = !Option.UserConfig.IsUseMouseWheel;
			this.checkbutton_simplifyRes.Active = Option.UserConfig.IsSimplifyDefaultRes;
			if (Platform.IsWindows)
			{
				this.codeIDEWidget = new SelectPathWidget(true);
				this.vbox_codeIDE.PackStart(this.codeIDEWidget, false, false, 0U);
				this.codeIDEWidget.Show();
			}
			else
			{
				this.vbox_main.Remove(this.frame_codeIDE);
			}
			this.InitMultiSampleComboBox();
			this.SetMultiLanugage();
		}

		private void InitMultiSampleComboBox()
		{
			string b = ((EnumMultiply)Option.UserConfig.MultiplySample).ToString();
			string[] names = Enum.GetNames(typeof(EnumMultiply));
			for (int i = 0; i < names.Count<string>(); i++)
			{
				this.combobox_multiSample.AppendText(names[i]);
				if (names[i] == b)
				{
					this.combobox_multiSample.Active = i;
				}
			}
			this.combobox_multiSample.Changed += this.MultiSampleComboBoxChangedHandler;
		}

		private void SetMultiLanugage()
		{
			this.GtkLabel_ctrls.Text = " " + LanguageInfo.ComToolPad + " ";
			this.checkbutton_simplifyRes.Label = LanguageInfo.Preference_SimplifyResource;
			this.GtkLabel_mouse.Text = " " + LanguageInfo.Preference_Mouse + " ";
			this.checkbutton_mouse.Label = LanguageInfo.Preference_DisableWheel;
			this.GtkLabel_display.Text = " " + LanguageInfo.Preference_Display + " ";
			this.label_multiSample.Text = LanguageInfo.Preference_MultiSample;
		}

		private void MultiSampleComboBoxChangedHandler(object sender, EventArgs e)
		{
			if (this.sampleIcon == null)
			{
				this.sampleIcon = new TooltipIcon();
				this.sampleIcon.Text = LanguageInfo.Preference_RestartCocosTooltip;
				this.hbox_multiSample.PackStart(this.sampleIcon, false, false, 0U);
				this.sampleIcon.Show();
			}
		}

		public void ApplySetting()
		{
			Option.UserConfig.IsUseMouseWheel = !this.checkbutton_mouse.Active;
			Option.UserConfig.IsSimplifyDefaultRes = this.checkbutton_simplifyRes.Active;
			Array values = Enum.GetValues(typeof(EnumMultiply));
			Option.UserConfig.MultiplySample = (int)values.GetValue(this.combobox_multiSample.Active);
			if (this.codeIDEWidget != null)
			{
				this.codeIDEWidget.ApplySetting();
			}
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

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.Preference.GeneralWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 15;
			this.frame_ctrls = new Frame();
			this.frame_ctrls.Name = "frame_ctrls";
			this.GtkAlignment_ctrls = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_ctrls.Name = "GtkAlignment_ctrls";
			this.GtkAlignment_ctrls.LeftPadding = 20U;
			this.GtkAlignment_ctrls.TopPadding = 15U;
			this.GtkAlignment_ctrls.RightPadding = 10U;
			this.GtkAlignment_ctrls.BottomPadding = 15U;
			this.checkbutton_simplifyRes = new CheckButton();
			this.checkbutton_simplifyRes.CanFocus = true;
			this.checkbutton_simplifyRes.Name = "checkbutton_simplifyRes";
			this.checkbutton_simplifyRes.Label = Catalog.GetString("简化控件初始资源");
			this.checkbutton_simplifyRes.Active = true;
			this.checkbutton_simplifyRes.DrawIndicator = true;
			this.checkbutton_simplifyRes.UseUnderline = true;
			this.GtkAlignment_ctrls.Add(this.checkbutton_simplifyRes);
			this.frame_ctrls.Add(this.GtkAlignment_ctrls);
			this.GtkLabel_ctrls = new Label();
			this.GtkLabel_ctrls.Name = "GtkLabel_ctrls";
			this.GtkLabel_ctrls.LabelProp = Catalog.GetString(" 控件 ");
			this.GtkLabel_ctrls.UseMarkup = true;
			this.frame_ctrls.LabelWidget = this.GtkLabel_ctrls;
			this.vbox_main.Add(this.frame_ctrls);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.frame_ctrls];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.frame_mouse = new Frame();
			this.frame_mouse.Name = "frame_mouse";
			this.GtkAlignment_mouse = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_mouse.Name = "GtkAlignment_mouse";
			this.GtkAlignment_mouse.LeftPadding = 20U;
			this.GtkAlignment_mouse.TopPadding = 15U;
			this.GtkAlignment_mouse.RightPadding = 10U;
			this.GtkAlignment_mouse.BottomPadding = 15U;
			this.checkbutton_mouse = new CheckButton();
			this.checkbutton_mouse.CanFocus = true;
			this.checkbutton_mouse.Name = "checkbutton_mouse";
			this.checkbutton_mouse.Label = Catalog.GetString("禁用鼠标滚轮缩放");
			this.checkbutton_mouse.DrawIndicator = true;
			this.checkbutton_mouse.UseUnderline = true;
			this.GtkAlignment_mouse.Add(this.checkbutton_mouse);
			this.frame_mouse.Add(this.GtkAlignment_mouse);
			this.GtkLabel_mouse = new Label();
			this.GtkLabel_mouse.Name = "GtkLabel_mouse";
			this.GtkLabel_mouse.LabelProp = Catalog.GetString(" 鼠标 ");
			this.GtkLabel_mouse.UseMarkup = true;
			this.frame_mouse.LabelWidget = this.GtkLabel_mouse;
			this.vbox_main.Add(this.frame_mouse);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_main[this.frame_mouse];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.frame_codeIDE = new Frame();
			this.frame_codeIDE.Name = "frame_codeIDE";
			this.GtkAlignment_codeIDE = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_codeIDE.Name = "GtkAlignment_codeIDE";
			this.GtkAlignment_codeIDE.LeftPadding = 20U;
			this.GtkAlignment_codeIDE.TopPadding = 15U;
			this.GtkAlignment_codeIDE.RightPadding = 10U;
			this.GtkAlignment_codeIDE.BottomPadding = 15U;
			this.vbox_codeIDE = new VBox();
			this.vbox_codeIDE.Name = "vbox_codeIDE";
			this.vbox_codeIDE.Spacing = 6;
			this.GtkAlignment_codeIDE.Add(this.vbox_codeIDE);
			this.frame_codeIDE.Add(this.GtkAlignment_codeIDE);
			this.GtkLabel_codeIDE = new Label();
			this.GtkLabel_codeIDE.Name = "GtkLabel_codeIDE";
			this.GtkLabel_codeIDE.LabelProp = Catalog.GetString(" CodeIDE ");
			this.GtkLabel_codeIDE.UseMarkup = true;
			this.frame_codeIDE.LabelWidget = this.GtkLabel_codeIDE;
			this.vbox_main.Add(this.frame_codeIDE);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_main[this.frame_codeIDE];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			this.frame_display = new Frame();
			this.frame_display.Name = "frame_display";
			this.GtkAlignment_display = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_display.Name = "GtkAlignment_display";
			this.GtkAlignment_display.LeftPadding = 20U;
			this.GtkAlignment_display.TopPadding = 15U;
			this.GtkAlignment_display.RightPadding = 10U;
			this.GtkAlignment_display.BottomPadding = 15U;
			this.hbox_multiSample = new HBox();
			this.hbox_multiSample.Name = "hbox_multiSample";
			this.hbox_multiSample.Spacing = 10;
			this.label_multiSample = new Label();
			this.label_multiSample.Name = "label_multiSample";
			this.label_multiSample.LabelProp = Catalog.GetString("多重采样");
			this.hbox_multiSample.Add(this.label_multiSample);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_multiSample[this.label_multiSample];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.combobox_multiSample = ComboBox.NewText();
			this.combobox_multiSample.WidthRequest = 100;
			this.combobox_multiSample.Name = "combobox_multiSample";
			this.hbox_multiSample.Add(this.combobox_multiSample);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_multiSample[this.combobox_multiSample];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.GtkAlignment_display.Add(this.hbox_multiSample);
			this.frame_display.Add(this.GtkAlignment_display);
			this.GtkLabel_display = new Label();
			this.GtkLabel_display.Name = "GtkLabel_display";
			this.GtkLabel_display.LabelProp = Catalog.GetString(" 显示 ");
			this.GtkLabel_display.UseMarkup = true;
			this.frame_display.LabelWidget = this.GtkLabel_display;
			this.vbox_main.Add(this.frame_display);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_main[this.frame_display];
			boxChild6.Position = 3;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private SelectPathWidget codeIDEWidget;

		private TooltipIcon sampleIcon;

		private VBox vbox_main;

		private Frame frame_ctrls;

		private Alignment GtkAlignment_ctrls;

		private CheckButton checkbutton_simplifyRes;

		private Label GtkLabel_ctrls;

		private Frame frame_mouse;

		private Alignment GtkAlignment_mouse;

		private CheckButton checkbutton_mouse;

		private Label GtkLabel_mouse;

		private Frame frame_codeIDE;

		private Alignment GtkAlignment_codeIDE;

		private VBox vbox_codeIDE;

		private Label GtkLabel_codeIDE;

		private Frame frame_display;

		private Alignment GtkAlignment_display;

		private HBox hbox_multiSample;

		private Label label_multiSample;

		private ComboBox combobox_multiSample;

		private Label GtkLabel_display;
	}
}

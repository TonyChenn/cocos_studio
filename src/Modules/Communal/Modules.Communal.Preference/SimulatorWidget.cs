using System;
using System.ComponentModel;
using CocoStudio.Basic;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.Preference
{
	[ToolboxItem(true)]
	public class SimulatorWidget : Bin, IPreferenceWidget
	{
		public SimulatorWidget()
		{
			this.Build();
			this.GtkLabel_simulatorOption.Text = " " + LanguageInfo.Preference_OutputInfo + " ";
			this.radiobutton_outputPad.Label = LanguageInfo.Preference_ShowInOutputPad;
			this.radiobutton_cmdWnd.Label = LanguageInfo.Preference_ShowInCmd;
			if (Option.UserConfig.IsShowSimulatorCmd)
			{
				this.radiobutton_cmdWnd.Active = true;
			}
			else
			{
				this.radiobutton_outputPad.Active = true;
			}
			this.vbox_main.Remove(this.frame_simulatorOption);
		}

		public EnumPreferenceSetting SettingID
		{
			get
			{
				return EnumPreferenceSetting.Simulator;
			}
		}

		public string DisplayName
		{
			get
			{
				return LanguageInfo.ProjSetting_Simulator;
			}
		}

		public void ApplySetting()
		{
			Option.UserConfig.IsShowSimulatorCmd = this.radiobutton_cmdWnd.Active;
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
			base.Name = "Modules.Communal.Preference.SimulatorWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.frame_simulatorOption = new Frame();
			this.frame_simulatorOption.Name = "frame_simulatorOption";
			this.GtkAlignment_simulatorOption = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_simulatorOption.Name = "GtkAlignment_simulatorOption";
			this.GtkAlignment_simulatorOption.LeftPadding = 20U;
			this.GtkAlignment_simulatorOption.TopPadding = 15U;
			this.GtkAlignment_simulatorOption.RightPadding = 10U;
			this.GtkAlignment_simulatorOption.BottomPadding = 15U;
			this.hbox_simulatorOption = new HBox();
			this.hbox_simulatorOption.Name = "hbox_simulatorOption";
			this.hbox_simulatorOption.Spacing = 6;
			this.vbox_simulatorOption = new VBox();
			this.vbox_simulatorOption.Name = "vbox_simulatorOption";
			this.vbox_simulatorOption.Spacing = 6;
			this.radiobutton_outputPad = new RadioButton(Catalog.GetString("在输出面板显示"));
			this.radiobutton_outputPad.CanFocus = true;
			this.radiobutton_outputPad.Name = "radiobutton_outputPad";
			this.radiobutton_outputPad.Active = true;
			this.radiobutton_outputPad.DrawIndicator = true;
			this.radiobutton_outputPad.UseUnderline = true;
			this.radiobutton_outputPad.Group = new SList(IntPtr.Zero);
			this.vbox_simulatorOption.Add(this.radiobutton_outputPad);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_simulatorOption[this.radiobutton_outputPad];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.radiobutton_cmdWnd = new RadioButton(Catalog.GetString("在命令行窗口显示"));
			this.radiobutton_cmdWnd.CanFocus = true;
			this.radiobutton_cmdWnd.Name = "radiobutton_cmdWnd";
			this.radiobutton_cmdWnd.DrawIndicator = true;
			this.radiobutton_cmdWnd.UseUnderline = true;
			this.radiobutton_cmdWnd.Group = this.radiobutton_outputPad.Group;
			this.vbox_simulatorOption.Add(this.radiobutton_cmdWnd);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_simulatorOption[this.radiobutton_cmdWnd];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.hbox_simulatorOption.Add(this.vbox_simulatorOption);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_simulatorOption[this.vbox_simulatorOption];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			this.GtkAlignment_simulatorOption.Add(this.hbox_simulatorOption);
			this.frame_simulatorOption.Add(this.GtkAlignment_simulatorOption);
			this.GtkLabel_simulatorOption = new Label();
			this.GtkLabel_simulatorOption.Name = "GtkLabel_simulatorOption";
			this.GtkLabel_simulatorOption.LabelProp = Catalog.GetString(" 输出信息 ");
			this.GtkLabel_simulatorOption.UseMarkup = true;
			this.frame_simulatorOption.LabelWidget = this.GtkLabel_simulatorOption;
			this.vbox_main.Add(this.frame_simulatorOption);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.frame_simulatorOption];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private VBox vbox_main;

		private Frame frame_simulatorOption;

		private Alignment GtkAlignment_simulatorOption;

		private HBox hbox_simulatorOption;

		private VBox vbox_simulatorOption;

		private RadioButton radiobutton_outputPad;

		private RadioButton radiobutton_cmdWnd;

		private Label GtkLabel_simulatorOption;
	}
}

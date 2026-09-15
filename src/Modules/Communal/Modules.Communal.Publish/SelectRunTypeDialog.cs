using System;
using System.Collections.Generic;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.CocosAdapter.Platform;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.Publish
{
	public class SelectRunTypeDialog : Dialog
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.Publish.SelectRunTypeDialog";
			base.Title = Catalog.GetString("运行项目");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			vbox.BorderWidth = 2U;
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.BorderWidth = 12U;
			this.frame_type = new Frame();
			this.frame_type.WidthRequest = 300;
			this.frame_type.Name = "frame_type";
			this.GtkAlignment_type = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_type.Name = "GtkAlignment_type";
			this.GtkAlignment_type.LeftPadding = 12U;
			this.vbox_type = new VBox();
			this.vbox_type.Name = "vbox_type";
			this.vbox_type.Spacing = 6;
			this.vbox_type.BorderWidth = 12U;
			this.GtkAlignment_type.Add(this.vbox_type);
			this.frame_type.Add(this.GtkAlignment_type);
			this.GtkLabel_selectType = new Label();
			this.GtkLabel_selectType.Name = "GtkLabel_selectType";
			this.GtkLabel_selectType.LabelProp = Catalog.GetString("选择运行平台");
			this.GtkLabel_selectType.UseMarkup = true;
			this.frame_type.LabelWidget = this.GtkLabel_selectType;
			this.alignment_main.Add(this.frame_type);
			vbox.Add(this.alignment_main);
			Box.BoxChild boxChild = (Box.BoxChild)vbox[this.alignment_main];
			boxChild.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 8U;
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
			base.DefaultWidth = 328;
			base.DefaultHeight = 223;
			base.Hide();
		}

		public EnumPlatform RunType { get; private set; }

		public SelectRunTypeDialog()
		{
			this.Build();
			this.InitWidget();
			this.InitStyle();
			this.SetToDialogStyle(null, true, true, true);
		}

		private void InitWidget()
		{
			this.radioBtnRunTypeDictionary = new Dictionary<RadioButton, EnumPlatform>();
			EnumPlatform lastRunType = CocosRecentServices.Instance.LastRunType;
			bool flag = false;
			RadioButton radioButton = null;
			foreach (IPlatform platform in Cocos2dxServices.PlatformServices.PlatformList)
			{
				if (platform.CanShow(EnumOperationType.Run))
				{
					RadioButton radioButton2 = new RadioButton(platform.GetDisplayName(EnumOperationType.Run));
					if (radioButton == null)
					{
						radioButton2.Group = new SList(IntPtr.Zero);
						radioButton = radioButton2;
					}
					else
					{
						radioButton2.Group = radioButton.Group;
					}
					this.vbox_type.PackStart(radioButton2, false, false, 0U);
					radioButton2.Show();
					radioButton2.Toggled += this.RadioButtonToggledHandler;
					this.radioBtnRunTypeDictionary[radioButton2] = platform.PlatformType;
					if (platform.PlatformType == lastRunType)
					{
						radioButton2.Active = true;
						flag = true;
						this.RunType = platform.PlatformType;
					}
				}
			}
			if (!flag)
			{
				RadioButton radioButton3 = this.vbox_type.Children[0] as RadioButton;
				radioButton3.Active = true;
				this.RunType = this.radioBtnRunTypeDictionary[radioButton3];
			}
		}

		private void InitStyle()
		{
			this.buttonOk.Name = "MainButton";
			base.Title = LanguageInfo.Menu_Project_RunProject;
			this.GtkLabel_selectType.Text = " " + LanguageInfo.Run_SelectPlatform + " ";
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			if (Platform.IsWindows)
			{
				HButtonBox actionArea = base.ActionArea;
				ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.buttonCancel];
				ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[this.buttonOk];
				buttonBoxChild2.Position = 0;
				buttonBoxChild.Position = 1;
			}
		}

		private void RadioButtonToggledHandler(object sender, EventArgs e)
		{
			RadioButton radioButton = sender as RadioButton;
			if (radioButton != null && radioButton.Active)
			{
				this.RunType = this.radioBtnRunTypeDictionary[radioButton];
			}
		}

		private Alignment alignment_main;

		private Frame frame_type;

		private Alignment GtkAlignment_type;

		private VBox vbox_type;

		private Label GtkLabel_selectType;

		private Button buttonCancel;

		private Button buttonOk;

		private Dictionary<RadioButton, EnumPlatform> radioBtnRunTypeDictionary;
	}
}

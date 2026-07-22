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
	// Token: 0x02000004 RID: 4
	public class SelectRunTypeDialog : Dialog
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002080 File Offset: 0x00000280
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

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000023D3 File Offset: 0x000005D3
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000023DB File Offset: 0x000005DB
		public EnumPlatform RunType { get; private set; }

		// Token: 0x06000009 RID: 9 RVA: 0x000023E4 File Offset: 0x000005E4
		public SelectRunTypeDialog()
		{
			this.Build();
			this.InitWidget();
			this.InitStyle();
			this.SetToDialogStyle(null, true, true, true);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002408 File Offset: 0x00000608
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

		// Token: 0x0600000B RID: 11 RVA: 0x00002548 File Offset: 0x00000748
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

		// Token: 0x0600000C RID: 12 RVA: 0x000025F0 File Offset: 0x000007F0
		private void RadioButtonToggledHandler(object sender, EventArgs e)
		{
			RadioButton radioButton = sender as RadioButton;
			if (radioButton != null && radioButton.Active)
			{
				this.RunType = this.radioBtnRunTypeDictionary[radioButton];
			}
		}

		// Token: 0x04000002 RID: 2
		private Alignment alignment_main;

		// Token: 0x04000003 RID: 3
		private Frame frame_type;

		// Token: 0x04000004 RID: 4
		private Alignment GtkAlignment_type;

		// Token: 0x04000005 RID: 5
		private VBox vbox_type;

		// Token: 0x04000006 RID: 6
		private Label GtkLabel_selectType;

		// Token: 0x04000007 RID: 7
		private Button buttonCancel;

		// Token: 0x04000008 RID: 8
		private Button buttonOk;

		// Token: 0x04000009 RID: 9
		private Dictionary<RadioButton, EnumPlatform> radioBtnRunTypeDictionary;
	}
}

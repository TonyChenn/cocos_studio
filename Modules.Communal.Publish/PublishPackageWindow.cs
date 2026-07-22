using System;
using System.Collections.Generic;
using CocoStudio.Core.Commands;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.CocosAdapter.Platform;
using Modules.Communal.MultiLanguage;
using Modules.Communal.ProjectSetting;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.Publish
{
	// Token: 0x0200000B RID: 11
	public class PublishPackageWindow : Gtk.Window
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00003D01 File Offset: 0x00001F01
		public PublishPackageWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.InitWidget();
			this.InitEvent();
			this.InitStyle();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003D24 File Offset: 0x00001F24
		private void InitWidget()
		{
			if (CocosRecentServices.Instance.IsLastPublish)
			{
				this.radiobutton_publish.Active = true;
				this.frame_publish.Sensitive = true;
				this.frame_package.Sensitive = false;
			}
			else
			{
				this.radiobutton_package.Active = true;
				this.frame_publish.Sensitive = false;
				this.frame_package.Sensitive = true;
			}
			this.InitPublishWidget();
			this.InitPackageWidget();
			if (Platform.IsWindows)
			{
				Box.BoxChild boxChild = (Box.BoxChild)this.hbox_bottomBtn[this.buttonOK];
				Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_bottomBtn[this.buttonCancel];
				boxChild.Position = 1;
				boxChild2.Position = 0;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003DD8 File Offset: 0x00001FD8
		private void InitPublishWidget()
		{
			switch (CocosRecentServices.Instance.LastPublishType)
			{
			case EnumPublishType.Resource:
				this.radiobutton_publishRes.Active = true;
				break;
			case EnumPublishType.CodeIDE:
				this.radiobutton_publishIDE.Active = true;
				break;
			case EnumPublishType.VS:
				this.radiobutton_publishVS.Active = true;
				break;
			case EnumPublishType.XCode:
				this.radiobutton_publsihXcode.Active = true;
				break;
			}
			if (Platform.IsWindows)
			{
				this.vbox_publishMode.Remove(this.radiobutton_publsihXcode);
			}
			else if (Platform.IsMac)
			{
				this.vbox_publishMode.Remove(this.radiobutton_publishVS);
			}
			this.radiobutton_publishIDE.Sensitive = PublishHelper.CanPublishToCocosCodeIDE();
			this.radiobutton_publishVS.Sensitive = PublishHelper.CanPublishToVS();
			this.radiobutton_publsihXcode.Sensitive = PublishHelper.CanPublishToXcode();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003EA0 File Offset: 0x000020A0
		private void InitPackageWidget()
		{
			PackageParams packageParams = Cocos2dxServices.PackageServices.PackageParams;
			this.checkBtnPlatformDictionary = new Dictionary<CheckButton, EnumPlatform>();
			foreach (IPlatform platform in Cocos2dxServices.PlatformServices.PlatformList)
			{
				if (platform.CanShow(EnumOperationType.Package))
				{
					CheckButton checkButton = new CheckButton(platform.GetDisplayName(EnumOperationType.Package));
					checkButton.Active = ((packageParams.Platform & platform.PlatformType) != EnumPlatform.None);
					this.checkBtnPlatformDictionary[checkButton] = platform.PlatformType;
					this.vbox_packageMode.PackStart(checkButton);
					checkButton.Show();
				}
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003F54 File Offset: 0x00002154
		private void InitEvent()
		{
			base.KeyPressEvent += this.HandleKeyPressed;
			this.radiobutton_publish.Toggled += this.HandleRadioButtonToggled;
			this.radiobutton_package.Toggled += this.HandleRadioButtonToggled;
			this.buttonOK.Clicked += this.HandleButtonOKClicked;
			this.buttonCancel.Clicked += this.HandleButtonCancelClicked;
			this.button_publishSetting.Clicked += this.HandlePublishSettingClicked;
			this.button_packageSetting.Clicked += this.HandlePackageSettingClicked;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004000 File Offset: 0x00002200
		private void InitStyle()
		{
			base.Title = LanguageInfo.Menu_Project_PublishPackage;
			this.GtkLabel_publishMode1.Text = string.Format(" {0} ", LanguageInfo.Dialog_PublishType);
			this.GtkLabel_packageMode.Text = string.Format(" {0} ", LanguageInfo.Dialog_PackageType);
			this.radiobutton_publish.Label = LanguageInfo.Dialog_Publish_Title;
			this.radiobutton_package.Label = LanguageInfo.Package;
			this.radiobutton_publishRes.Label = LanguageInfo.Menu_File_PublishRes;
			this.radiobutton_publishIDE.Label = LanguageInfo.Dialog_Publish_CodeIDE;
			this.radiobutton_publishVS.Label = LanguageInfo.Dialog_Publish_VS;
			this.radiobutton_publsihXcode.Label = LanguageInfo.Dialog_Publish_Xcode;
			this.button_packageSetting.Label = LanguageInfo.Dialog_Publish_Setting;
			this.button_publishSetting.Label = LanguageInfo.Dialog_Publish_Setting;
			this.buttonOK.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.buttonOK.Name = "MainButton";
			this.SetToDialogStyle(null, true, true, true);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004108 File Offset: 0x00002308
		private bool CheckValidity()
		{
			if (this.radiobutton_package.Active)
			{
				bool flag = false;
				foreach (object obj in this.vbox_packageMode)
				{
					CheckButton checkButton = (CheckButton)obj;
					if (checkButton.Active)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					MessageBox.Show(LanguageInfo.MessageBox252_atLeastOnePlatform, MessageBoxImage.Other, null, null);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004188 File Offset: 0x00002388
		private void Run()
		{
			if (!this.CheckValidity())
			{
				return;
			}
			this.SaveUserData();
			this.Close();
			GlobalCommand.PublishPackageLastCmd.RaiseExecute(null);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000041AC File Offset: 0x000023AC
		private void SaveUserData()
		{
			if (this.radiobutton_publish.Active)
			{
				EnumPublishType lastPublishType;
				if (this.radiobutton_publishRes.Active)
				{
					lastPublishType = EnumPublishType.Resource;
				}
				else if (this.radiobutton_publishIDE.Active)
				{
					lastPublishType = EnumPublishType.CodeIDE;
				}
				else if (this.radiobutton_publishVS.Active)
				{
					lastPublishType = EnumPublishType.VS;
				}
				else
				{
					lastPublishType = EnumPublishType.XCode;
				}
				CocosRecentServices.Instance.LastPublishType = lastPublishType;
				CocosRecentServices.Instance.IsLastPublish = true;
				return;
			}
			int num = 0;
			foreach (object obj in this.vbox_packageMode)
			{
				CheckButton checkButton = (CheckButton)obj;
				if (checkButton.Active)
				{
					EnumPlatform enumPlatform = this.checkBtnPlatformDictionary[checkButton];
					num |= (int)enumPlatform;
				}
			}
			PackageServices.Instance.PackageParams.Platform = (EnumPlatform)num;
			CocosRecentServices.Instance.IsLastPublish = false;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004294 File Offset: 0x00002494
		private void Close()
		{
			this.Destroy();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000429C File Offset: 0x0000249C
		private void HandleRadioButtonToggled(object sender, EventArgs e)
		{
			this.frame_publish.Sensitive = this.radiobutton_publish.Active;
			this.frame_package.Sensitive = this.radiobutton_package.Active;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000042CC File Offset: 0x000024CC
		[ConnectBefore]
		private void HandleKeyPressed(object o, KeyPressEventArgs args)
		{
			Gdk.Key key = args.Event.Key;
			if (key == Gdk.Key.Escape)
			{
				this.Close();
				return;
			}
			if (KeyboardExtend.IsEnterKey(key))
			{
				this.Run();
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004302 File Offset: 0x00002502
		private void HandlePackageSettingClicked(object sender, EventArgs e)
		{
			GlobalCommand.ProjectSettingCmd.RaiseExecute(EnumProjectSetting.Package);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004314 File Offset: 0x00002514
		private void HandlePublishSettingClicked(object sender, EventArgs e)
		{
			GlobalCommand.ProjectSettingCmd.RaiseExecute(EnumProjectSetting.Publish);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004326 File Offset: 0x00002526
		private void HandleButtonCancelClicked(object sender, EventArgs e)
		{
			this.Close();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000432E File Offset: 0x0000252E
		private void HandleButtonOKClicked(object sender, EventArgs e)
		{
			this.Run();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004338 File Offset: 0x00002538
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 400;
			base.Name = "Modules.Communal.Publish.PublishPackageWindow";
			base.Title = Catalog.GetString("项目发布与打包");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.LeftPadding = 6U;
			this.alignment_main.RightPadding = 6U;
			this.alignment_main.BorderWidth = 12U;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.radiobutton_publish = new RadioButton(Catalog.GetString("发布"));
			this.radiobutton_publish.CanFocus = true;
			this.radiobutton_publish.Name = "radiobutton_publish";
			this.radiobutton_publish.DrawIndicator = true;
			this.radiobutton_publish.UseUnderline = true;
			this.radiobutton_publish.Group = new SList(IntPtr.Zero);
			this.vbox_main.Add(this.radiobutton_publish);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.radiobutton_publish];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.alignment_publish = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_publish.Name = "alignment_publish";
			this.alignment_publish.LeftPadding = 30U;
			this.frame_publish = new Frame();
			this.frame_publish.Name = "frame_publish";
			this.GtkAlignment = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment.Name = "GtkAlignment";
			this.GtkAlignment.LeftPadding = 12U;
			this.hbox_publish = new HBox();
			this.hbox_publish.Name = "hbox_publish";
			this.hbox_publish.Spacing = 6;
			this.hbox_publish.BorderWidth = 12U;
			this.vbox_publishMode = new VBox();
			this.vbox_publishMode.Name = "vbox_publishMode";
			this.vbox_publishMode.Spacing = 6;
			this.radiobutton_publishRes = new RadioButton(Catalog.GetString("发布资源"));
			this.radiobutton_publishRes.CanFocus = true;
			this.radiobutton_publishRes.Name = "radiobutton_publishRes";
			this.radiobutton_publishRes.DrawIndicator = true;
			this.radiobutton_publishRes.UseUnderline = true;
			this.radiobutton_publishRes.Group = new SList(IntPtr.Zero);
			this.vbox_publishMode.Add(this.radiobutton_publishRes);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_publishMode[this.radiobutton_publishRes];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.radiobutton_publishIDE = new RadioButton(Catalog.GetString("发布为Code IDE工程"));
			this.radiobutton_publishIDE.CanFocus = true;
			this.radiobutton_publishIDE.Name = "radiobutton_publishIDE";
			this.radiobutton_publishIDE.DrawIndicator = true;
			this.radiobutton_publishIDE.UseUnderline = true;
			this.radiobutton_publishIDE.Group = this.radiobutton_publishRes.Group;
			this.vbox_publishMode.Add(this.radiobutton_publishIDE);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_publishMode[this.radiobutton_publishIDE];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.radiobutton_publishVS = new RadioButton(Catalog.GetString("发布为Visual Studio工程"));
			this.radiobutton_publishVS.CanFocus = true;
			this.radiobutton_publishVS.Name = "radiobutton_publishVS";
			this.radiobutton_publishVS.DrawIndicator = true;
			this.radiobutton_publishVS.UseUnderline = true;
			this.radiobutton_publishVS.Group = this.radiobutton_publishRes.Group;
			this.vbox_publishMode.Add(this.radiobutton_publishVS);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_publishMode[this.radiobutton_publishVS];
			boxChild4.Position = 2;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.radiobutton_publsihXcode = new RadioButton(Catalog.GetString("发布为Xcode工程"));
			this.radiobutton_publsihXcode.CanFocus = true;
			this.radiobutton_publsihXcode.Name = "radiobutton_publsihXcode";
			this.radiobutton_publsihXcode.DrawIndicator = true;
			this.radiobutton_publsihXcode.UseUnderline = true;
			this.radiobutton_publsihXcode.Group = this.radiobutton_publishRes.Group;
			this.vbox_publishMode.Add(this.radiobutton_publsihXcode);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_publishMode[this.radiobutton_publsihXcode];
			boxChild5.Position = 3;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.hbox_publish.Add(this.vbox_publishMode);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_publish[this.vbox_publishMode];
			boxChild6.Position = 0;
			this.vbox_publishSetting = new VBox();
			this.vbox_publishSetting.Name = "vbox_publishSetting";
			this.vbox_publishSetting.Spacing = 6;
			this.button_publishSetting = new Button();
			this.button_publishSetting.WidthRequest = 75;
			this.button_publishSetting.HeightRequest = 24;
			this.button_publishSetting.CanFocus = true;
			this.button_publishSetting.Name = "button_publishSetting";
			this.button_publishSetting.UseUnderline = true;
			this.button_publishSetting.Label = Catalog.GetString("设置");
			this.vbox_publishSetting.Add(this.button_publishSetting);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_publishSetting[this.button_publishSetting];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 2;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.hbox_publish.Add(this.vbox_publishSetting);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox_publish[this.vbox_publishSetting];
			boxChild8.PackType = PackType.End;
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			this.GtkAlignment.Add(this.hbox_publish);
			this.frame_publish.Add(this.GtkAlignment);
			this.GtkLabel_publishMode1 = new Label();
			this.GtkLabel_publishMode1.Name = "GtkLabel_publishMode1";
			this.GtkLabel_publishMode1.LabelProp = Catalog.GetString(" 选择发布类型 ");
			this.GtkLabel_publishMode1.UseMarkup = true;
			this.frame_publish.LabelWidget = this.GtkLabel_publishMode1;
			this.alignment_publish.Add(this.frame_publish);
			this.vbox_main.Add(this.alignment_publish);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_main[this.alignment_publish];
			boxChild9.Position = 1;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.radiobutton_package = new RadioButton(Catalog.GetString("打包"));
			this.radiobutton_package.CanFocus = true;
			this.radiobutton_package.Name = "radiobutton_package";
			this.radiobutton_package.DrawIndicator = true;
			this.radiobutton_package.UseUnderline = true;
			this.radiobutton_package.Group = this.radiobutton_publish.Group;
			this.vbox_main.Add(this.radiobutton_package);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_main[this.radiobutton_package];
			boxChild10.Position = 2;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.alignment_package = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_package.Name = "alignment_package";
			this.alignment_package.LeftPadding = 30U;
			this.alignment_package.BottomPadding = 15U;
			this.frame_package = new Frame();
			this.frame_package.Name = "frame_package";
			this.GtkAlignment1 = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment1.Name = "GtkAlignment1";
			this.GtkAlignment1.LeftPadding = 12U;
			this.hbox_package = new HBox();
			this.hbox_package.Name = "hbox_package";
			this.hbox_package.Spacing = 6;
			this.hbox_package.BorderWidth = 12U;
			this.vbox_packageMode = new VBox();
			this.vbox_packageMode.Name = "vbox_packageMode";
			this.vbox_packageMode.Spacing = 6;
			this.hbox_package.Add(this.vbox_packageMode);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.hbox_package[this.vbox_packageMode];
			boxChild11.Position = 0;
			this.vbox_packageSetting = new VBox();
			this.vbox_packageSetting.Name = "vbox_packageSetting";
			this.vbox_packageSetting.Spacing = 6;
			this.button_packageSetting = new Button();
			this.button_packageSetting.WidthRequest = 75;
			this.button_packageSetting.HeightRequest = 24;
			this.button_packageSetting.CanFocus = true;
			this.button_packageSetting.Name = "button_packageSetting";
			this.button_packageSetting.UseUnderline = true;
			this.button_packageSetting.Label = Catalog.GetString("设置");
			this.vbox_packageSetting.Add(this.button_packageSetting);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_packageSetting[this.button_packageSetting];
			boxChild12.PackType = PackType.End;
			boxChild12.Position = 2;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.hbox_package.Add(this.vbox_packageSetting);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.hbox_package[this.vbox_packageSetting];
			boxChild13.PackType = PackType.End;
			boxChild13.Position = 1;
			boxChild13.Expand = false;
			this.GtkAlignment1.Add(this.hbox_package);
			this.frame_package.Add(this.GtkAlignment1);
			this.GtkLabel_packageMode = new Label();
			this.GtkLabel_packageMode.Name = "GtkLabel_packageMode";
			this.GtkLabel_packageMode.LabelProp = Catalog.GetString(" 选择发布类型 ");
			this.GtkLabel_packageMode.UseMarkup = true;
			this.frame_package.LabelWidget = this.GtkLabel_packageMode;
			this.alignment_package.Add(this.frame_package);
			this.vbox_main.Add(this.alignment_package);
			Box.BoxChild boxChild14 = (Box.BoxChild)this.vbox_main[this.alignment_package];
			boxChild14.Position = 3;
			this.hbox_bottomBtn = new HBox();
			this.hbox_bottomBtn.Name = "hbox_bottomBtn";
			this.hbox_bottomBtn.Spacing = 6;
			this.buttonOK = new Button();
			this.buttonOK.WidthRequest = 80;
			this.buttonOK.HeightRequest = 24;
			this.buttonOK.CanFocus = true;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseUnderline = true;
			this.buttonOK.Label = Catalog.GetString("确定");
			this.hbox_bottomBtn.Add(this.buttonOK);
			Box.BoxChild boxChild15 = (Box.BoxChild)this.hbox_bottomBtn[this.buttonOK];
			boxChild15.PackType = PackType.End;
			boxChild15.Position = 1;
			boxChild15.Expand = false;
			boxChild15.Fill = false;
			this.buttonCancel = new Button();
			this.buttonCancel.WidthRequest = 80;
			this.buttonCancel.HeightRequest = 24;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = Catalog.GetString("取消");
			this.hbox_bottomBtn.Add(this.buttonCancel);
			Box.BoxChild boxChild16 = (Box.BoxChild)this.hbox_bottomBtn[this.buttonCancel];
			boxChild16.PackType = PackType.End;
			boxChild16.Position = 2;
			boxChild16.Expand = false;
			boxChild16.Fill = false;
			this.vbox_main.Add(this.hbox_bottomBtn);
			Box.BoxChild boxChild17 = (Box.BoxChild)this.vbox_main[this.hbox_bottomBtn];
			boxChild17.Position = 4;
			boxChild17.Expand = false;
			boxChild17.Fill = false;
			this.alignment_main.Add(this.vbox_main);
			base.Add(this.alignment_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 400;
			base.DefaultHeight = 359;
			base.Hide();
		}

		// Token: 0x04000023 RID: 35
		private Dictionary<CheckButton, EnumPlatform> checkBtnPlatformDictionary;

		// Token: 0x04000024 RID: 36
		private Alignment alignment_main;

		// Token: 0x04000025 RID: 37
		private VBox vbox_main;

		// Token: 0x04000026 RID: 38
		private RadioButton radiobutton_publish;

		// Token: 0x04000027 RID: 39
		private Alignment alignment_publish;

		// Token: 0x04000028 RID: 40
		private Frame frame_publish;

		// Token: 0x04000029 RID: 41
		private Alignment GtkAlignment;

		// Token: 0x0400002A RID: 42
		private HBox hbox_publish;

		// Token: 0x0400002B RID: 43
		private VBox vbox_publishMode;

		// Token: 0x0400002C RID: 44
		private RadioButton radiobutton_publishRes;

		// Token: 0x0400002D RID: 45
		private RadioButton radiobutton_publishIDE;

		// Token: 0x0400002E RID: 46
		private RadioButton radiobutton_publishVS;

		// Token: 0x0400002F RID: 47
		private RadioButton radiobutton_publsihXcode;

		// Token: 0x04000030 RID: 48
		private VBox vbox_publishSetting;

		// Token: 0x04000031 RID: 49
		private Button button_publishSetting;

		// Token: 0x04000032 RID: 50
		private Label GtkLabel_publishMode1;

		// Token: 0x04000033 RID: 51
		private RadioButton radiobutton_package;

		// Token: 0x04000034 RID: 52
		private Alignment alignment_package;

		// Token: 0x04000035 RID: 53
		private Frame frame_package;

		// Token: 0x04000036 RID: 54
		private Alignment GtkAlignment1;

		// Token: 0x04000037 RID: 55
		private HBox hbox_package;

		// Token: 0x04000038 RID: 56
		private VBox vbox_packageMode;

		// Token: 0x04000039 RID: 57
		private VBox vbox_packageSetting;

		// Token: 0x0400003A RID: 58
		private Button button_packageSetting;

		// Token: 0x0400003B RID: 59
		private Label GtkLabel_packageMode;

		// Token: 0x0400003C RID: 60
		private HBox hbox_bottomBtn;

		// Token: 0x0400003D RID: 61
		private Button buttonOK;

		// Token: 0x0400003E RID: 62
		private Button buttonCancel;
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.Preference
{
	// Token: 0x02000010 RID: 16
	[ToolboxItem(true)]
	public class PlatformWidget : Bin, IPreferenceWidget
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003FFE File Offset: 0x000021FE
		public EnumPreferenceSetting SettingID
		{
			get
			{
				return EnumPreferenceSetting.Platform;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00004001 File Offset: 0x00002201
		public string DisplayName
		{
			get
			{
				return LanguageInfo.Preference_Platform;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004008 File Offset: 0x00002208
		public PlatformWidget()
		{
			this.Build();
			this.InitWidgets();
			this.InitView();
			this.InitEvent();
			this.SetMultiLanugage();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004030 File Offset: 0x00002230
		private void InitWidgets()
		{
			if (string.IsNullOrEmpty(Option.UserConfig.ANTPath))
			{
				Option.UserConfig.ANTPath = PackageServices.GetANTPath();
			}
			if (string.IsNullOrEmpty(Option.UserConfig.JDKPath))
			{
				Option.UserConfig.JDKPath = PackageServices.GetJDKPath();
			}
			this.entry_SDK.Text = Option.UserConfig.SDKPath;
			this.entry_NDK.Text = Option.UserConfig.NDKPath;
			this.entry_ANT.Text = Option.UserConfig.ANTPath;
			this.entry_JDK.Text = Option.UserConfig.JDKPath;
			CellRendererText cell = new CellRendererText();
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			treeViewColumn.PackStart(cell, false);
			treeViewColumn.AddAttribute(cell, "text", 0);
			treeViewColumn.Alignment = -40f;
			this.treeview_framework.AppendColumn(treeViewColumn);
			this.treeview_framework.HasTooltip = false;
			this.treeview_framework.HeadersVisible = false;
			if (Services.NetworkService.IsOK)
			{
				this.alignment_netWarning.RemoveChild();
				return;
			}
			this.button_config.Sensitive = false;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004148 File Offset: 0x00002348
		private void InitView()
		{
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.WarningIcon.Studio.png");
			this.imagebin_sdk.SetImageView(icon);
			this.imagebin_ndk.SetImageView(icon);
			this.imagebin_jdk.SetImageView(icon);
			this.imagebin_ant.SetImageView(icon);
			this.imagebin_framework.SetImageView(icon);
			this.treeview_framework.Name = "DarkTreeView";
			this.eventbox_framework.ModifyBg(StateType.Normal, new Gdk.Color(44, 44, 46));
			this.button_config.Name = "MainButton";
			if (LanguageOption.CurrentLanguage == LanguageType.English)
			{
				this.button_config.WidthRequest = 140;
			}
			else
			{
				this.button_config.WidthRequest = 100;
			}
			this.RefreshUI();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004200 File Offset: 0x00002400
		private void InitEvent()
		{
			this.button_browseANT.Clicked += this.ButtonAntClickedHandler;
			this.button_browseJDK.Clicked += this.ButtonJdkClickedHandler;
			this.button_browseNDK.Clicked += this.ButtonNdkClickedHandler;
			this.button_browseSDK.Clicked += this.ButtonSdkClickedHandler;
			this.entry_ANT.Changed += this.PathEntryChangedHandler;
			this.entry_JDK.Changed += this.PathEntryChangedHandler;
			this.entry_NDK.Changed += this.PathEntryChangedHandler;
			this.entry_SDK.Changed += this.PathEntryChangedHandler;
			this.button_config.Clicked += this.ButtonConfigClickedHandler;
			Cocos2dxServices.InstallerServices.InstallFinished += this.ConfigFinishedHandler;
			base.SizeAllocated += this.SizeAllocatedHandler;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004304 File Offset: 0x00002504
		private void SetMultiLanugage()
		{
			this.GtkLabel_framework.LabelProp = string.Format(" {0} ", LanguageInfo.Installer_Framework);
			this.GtkLabel_android.LabelProp = string.Format(" {0} ", LanguageInfo.Preference_AndroidPath);
			this.label_SDK.Text = "SDK";
			this.label_NDK.Text = "NDK";
			this.label_ANT.Text = "ANT";
			this.label_JDK.Text = "JDK";
			this.button_browseANT.Label = LanguageInfo.Dialog_ButtonBrowse + "...";
			this.button_browseJDK.Label = LanguageInfo.Dialog_ButtonBrowse + "...";
			this.button_browseNDK.Label = LanguageInfo.Dialog_ButtonBrowse + "...";
			this.button_browseSDK.Label = LanguageInfo.Dialog_ButtonBrowse + "...";
			this.label_prompt.Text = LanguageInfo.Preference_Prompt;
			this.tooltipicon_net.Text = LanguageInfo.Tooltip_NoNetwork;
			this.button_config.Label = LanguageInfo.Preference_Configure;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004420 File Offset: 0x00002620
		public void ApplySetting()
		{
			if (!string.IsNullOrEmpty(this.entry_SDK.Text))
			{
				Option.UserConfig.SDKPath = this.entry_SDK.Text;
			}
			if (!string.IsNullOrEmpty(this.entry_NDK.Text))
			{
				Option.UserConfig.NDKPath = this.entry_NDK.Text;
			}
			if (!string.IsNullOrEmpty(this.entry_ANT.Text))
			{
				Option.UserConfig.ANTPath = this.entry_ANT.Text;
			}
			if (!string.IsNullOrEmpty(this.entry_JDK.Text))
			{
				Option.UserConfig.JDKPath = this.entry_JDK.Text;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000044C9 File Offset: 0x000026C9
		public bool CanApply(out string output)
		{
			output = "";
			return true;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000044D3 File Offset: 0x000026D3
		Widget IPreferenceWidget.GetWidget()
		{
			return this;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000044D8 File Offset: 0x000026D8
		private void RefreshUI()
		{
			if (string.IsNullOrEmpty(this.entry_ANT.Text))
			{
				this.imagebin_ant.Show();
			}
			else
			{
				this.imagebin_ant.Hide();
			}
			if (string.IsNullOrEmpty(this.entry_JDK.Text))
			{
				this.imagebin_jdk.Show();
			}
			else
			{
				this.imagebin_jdk.Hide();
			}
			if (string.IsNullOrEmpty(this.entry_NDK.Text))
			{
				this.imagebin_ndk.Show();
			}
			else
			{
				this.imagebin_ndk.Hide();
			}
			if (string.IsNullOrEmpty(this.entry_SDK.Text))
			{
				this.imagebin_sdk.Show();
			}
			else
			{
				this.imagebin_sdk.Hide();
			}
			IReadOnlyList<string> enabledVersions = FrameworkHelper.EnabledVersions;
			if (enabledVersions.Count == 0)
			{
				this.imagebin_framework.Show();
				this.eventbox_framework.Hide();
				this.label_frameworkInfo.Text = LanguageInfo.Preference_FwNotInstalled;
				return;
			}
			this.imagebin_framework.Hide();
			this.label_frameworkInfo.Text = LanguageInfo.Preference_FrameworkList;
			TreeStore treeStore = new TreeStore(new Type[]
			{
				typeof(string)
			});
			foreach (string text in enabledVersions)
			{
				treeStore.AppendValues(new object[]
				{
					text
				});
			}
			this.treeview_framework.Model = treeStore;
			this.eventbox_framework.Show();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004660 File Offset: 0x00002860
		private void SizeAllocatedHandler(object o, SizeAllocatedArgs args)
		{
			this.label_prompt.WidthRequest = this.vbox_storePrompt.Allocation.Width;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000467D File Offset: 0x0000287D
		private void ButtonConfigClickedHandler(object sender, EventArgs e)
		{
			Cocos2dxServices.InstallerServices.StartInstaller(true, true);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000468C File Offset: 0x0000288C
		private void ConfigFinishedHandler(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Option.UserConfig.NDKPath))
			{
				this.entry_NDK.Text = Option.UserConfig.NDKPath;
			}
			if (!string.IsNullOrEmpty(Option.UserConfig.SDKPath))
			{
				this.entry_SDK.Text = Option.UserConfig.SDKPath;
			}
			if (!string.IsNullOrEmpty(Option.UserConfig.JDKPath))
			{
				this.entry_JDK.Text = Option.UserConfig.JDKPath;
			}
			if (!string.IsNullOrEmpty(Option.UserConfig.ANTPath))
			{
				this.entry_ANT.Text = Option.UserConfig.ANTPath;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004731 File Offset: 0x00002931
		private void PathEntryChangedHandler(object sender, EventArgs e)
		{
			this.RefreshUI();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000473C File Offset: 0x0000293C
		private void ButtonSdkClickedHandler(object sender, EventArgs e)
		{
			string selectFolderPath = this.GetSelectFolderPath("SDK", this.entry_SDK.Text);
			if (!string.IsNullOrEmpty(selectFolderPath))
			{
				this.entry_SDK.Text = selectFolderPath;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004774 File Offset: 0x00002974
		private void ButtonNdkClickedHandler(object sender, EventArgs e)
		{
			string selectFolderPath = this.GetSelectFolderPath("NDK", this.entry_NDK.Text);
			if (!string.IsNullOrEmpty(selectFolderPath))
			{
				this.entry_NDK.Text = selectFolderPath;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000047AC File Offset: 0x000029AC
		private void ButtonJdkClickedHandler(object sender, EventArgs e)
		{
			string selectFolderPath = this.GetSelectFolderPath("JDK", this.entry_JDK.Text);
			if (!string.IsNullOrEmpty(selectFolderPath))
			{
				this.entry_JDK.Text = selectFolderPath;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000047E4 File Offset: 0x000029E4
		private void ButtonAntClickedHandler(object sender, EventArgs e)
		{
			string selectFolderPath = this.GetSelectFolderPath("ANT", this.entry_ANT.Text);
			if (!string.IsNullOrEmpty(selectFolderPath))
			{
				this.entry_ANT.Text = selectFolderPath;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000481C File Offset: 0x00002A1C
		private string GetSelectFolderPath(string tilteName, string initDir)
		{
			string result = string.Empty;
			SelectFolderDialog selectFolderDialog = new SelectFolderDialog();
			selectFolderDialog.TransientFor = MessageService.GetDefaultModalParent();
			selectFolderDialog.Title = string.Format(LanguageInfo.Preference_SelectPath, tilteName);
			selectFolderDialog.Action = FileChooserAction.SelectFolder;
			selectFolderDialog.SelectMultiple = false;
			selectFolderDialog.CurrentFolder = initDir;
			if (selectFolderDialog.Run() && !string.IsNullOrEmpty(selectFolderDialog.SelectedFile))
			{
				result = selectFolderDialog.SelectedFile;
			}
			return result;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004894 File Offset: 0x00002A94
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.Preference.PlatformWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.frame_framework = new Frame();
			this.frame_framework.Name = "frame_framework";
			this.GtkAlignment_framework = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_framework.Name = "GtkAlignment_framework";
			this.GtkAlignment_framework.LeftPadding = 5U;
			this.GtkAlignment_framework.TopPadding = 10U;
			this.GtkAlignment_framework.RightPadding = 12U;
			this.GtkAlignment_framework.BottomPadding = 8U;
			this.vbox_framework = new VBox();
			this.vbox_framework.Name = "vbox_framework";
			this.vbox_framework.Spacing = 6;
			this.hbox_frameworkInfo = new HBox();
			this.hbox_frameworkInfo.Name = "hbox_frameworkInfo";
			this.hbox_frameworkInfo.Spacing = 6;
			this.vbox1 = new VBox();
			this.vbox1.Name = "vbox1";
			this.alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.vbox1.Add(this.alignment1);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox1[this.alignment1];
			boxChild.Position = 0;
			this.imagebin_framework = new ImageBin();
			this.imagebin_framework.Events = EventMask.ButtonPressMask;
			this.imagebin_framework.Name = "imagebin_framework";
			this.vbox1.Add(this.imagebin_framework);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox1[this.imagebin_framework];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment2.Name = "alignment2";
			this.vbox1.Add(this.alignment2);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox1[this.alignment2];
			boxChild3.Position = 2;
			this.hbox_frameworkInfo.Add(this.vbox1);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_frameworkInfo[this.vbox1];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.label_frameworkInfo = new Label();
			this.label_frameworkInfo.Name = "label_frameworkInfo";
			this.label_frameworkInfo.LabelProp = Catalog.GetString("未下载安装Cocos Framework，该框架与运行、发布和打包相关");
			this.hbox_frameworkInfo.Add(this.label_frameworkInfo);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_frameworkInfo[this.label_frameworkInfo];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.vbox_framework.Add(this.hbox_frameworkInfo);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_framework[this.hbox_frameworkInfo];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.eventbox_framework = new EventBox();
			this.eventbox_framework.HeightRequest = 100;
			this.eventbox_framework.Name = "eventbox_framework";
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.GtkScrolledWindow.BorderWidth = 1U;
			this.treeview_framework = new TreeView();
			this.treeview_framework.CanFocus = true;
			this.treeview_framework.Name = "treeview_framework";
			this.GtkScrolledWindow.Add(this.treeview_framework);
			this.eventbox_framework.Add(this.GtkScrolledWindow);
			this.vbox_framework.Add(this.eventbox_framework);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_framework[this.eventbox_framework];
			boxChild7.Position = 1;
			this.GtkAlignment_framework.Add(this.vbox_framework);
			this.frame_framework.Add(this.GtkAlignment_framework);
			this.GtkLabel_framework = new Label();
			this.GtkLabel_framework.Name = "GtkLabel_framework";
			this.GtkLabel_framework.LabelProp = Catalog.GetString(" 框架 ");
			this.GtkLabel_framework.UseMarkup = true;
			this.frame_framework.LabelWidget = this.GtkLabel_framework;
			this.vbox_main.Add(this.frame_framework);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.frame_framework];
			boxChild8.Position = 0;
			boxChild8.Expand = false;
			this.frame_androod = new Frame();
			this.frame_androod.Name = "frame_androod";
			this.GtkAlignment = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment.Name = "GtkAlignment";
			this.GtkAlignment.LeftPadding = 5U;
			this.GtkAlignment.TopPadding = 8U;
			this.GtkAlignment.RightPadding = 12U;
			this.GtkAlignment.BottomPadding = 8U;
			this.vbox_android = new VBox();
			this.vbox_android.Name = "vbox_android";
			this.vbox_android.Spacing = 6;
			this.table_android = new Table(4U, 3U, false);
			this.table_android.Name = "table_android";
			this.table_android.RowSpacing = 6U;
			this.table_android.ColumnSpacing = 3U;
			this.hbox_ant = new HBox();
			this.hbox_ant.Name = "hbox_ant";
			this.hbox_ant.Spacing = 6;
			this.entry_ANT = new Entry();
			this.entry_ANT.CanFocus = true;
			this.entry_ANT.Name = "entry_ANT";
			this.entry_ANT.IsEditable = false;
			this.entry_ANT.InvisibleChar = '●';
			this.hbox_ant.Add(this.entry_ANT);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_ant[this.entry_ANT];
			boxChild9.Position = 0;
			this.button_browseANT = new Button();
			this.button_browseANT.WidthRequest = 70;
			this.button_browseANT.CanFocus = true;
			this.button_browseANT.Name = "button_browseANT";
			this.button_browseANT.UseUnderline = true;
			this.button_browseANT.Label = Catalog.GetString("浏览");
			this.hbox_ant.Add(this.button_browseANT);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_ant[this.button_browseANT];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.table_android.Add(this.hbox_ant);
			Table.TableChild tableChild = (Table.TableChild)this.table_android[this.hbox_ant];
			tableChild.TopAttach = 2U;
			tableChild.BottomAttach = 3U;
			tableChild.LeftAttach = 2U;
			tableChild.RightAttach = 3U;
			tableChild.YOptions = AttachOptions.Fill;
			this.hbox_jdk = new HBox();
			this.hbox_jdk.Name = "hbox_jdk";
			this.hbox_jdk.Spacing = 6;
			this.entry_JDK = new Entry();
			this.entry_JDK.CanFocus = true;
			this.entry_JDK.Name = "entry_JDK";
			this.entry_JDK.IsEditable = false;
			this.entry_JDK.InvisibleChar = '●';
			this.hbox_jdk.Add(this.entry_JDK);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.hbox_jdk[this.entry_JDK];
			boxChild11.Position = 0;
			this.button_browseJDK = new Button();
			this.button_browseJDK.WidthRequest = 70;
			this.button_browseJDK.CanFocus = true;
			this.button_browseJDK.Name = "button_browseJDK";
			this.button_browseJDK.UseUnderline = true;
			this.button_browseJDK.Label = Catalog.GetString("浏览");
			this.hbox_jdk.Add(this.button_browseJDK);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.hbox_jdk[this.button_browseJDK];
			boxChild12.Position = 1;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.table_android.Add(this.hbox_jdk);
			Table.TableChild tableChild2 = (Table.TableChild)this.table_android[this.hbox_jdk];
			tableChild2.TopAttach = 3U;
			tableChild2.BottomAttach = 4U;
			tableChild2.LeftAttach = 2U;
			tableChild2.RightAttach = 3U;
			tableChild2.YOptions = AttachOptions.Fill;
			this.hbox_ndk = new HBox();
			this.hbox_ndk.Name = "hbox_ndk";
			this.hbox_ndk.Spacing = 6;
			this.entry_NDK = new Entry();
			this.entry_NDK.CanFocus = true;
			this.entry_NDK.Name = "entry_NDK";
			this.entry_NDK.IsEditable = false;
			this.entry_NDK.InvisibleChar = '●';
			this.hbox_ndk.Add(this.entry_NDK);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.hbox_ndk[this.entry_NDK];
			boxChild13.Position = 0;
			this.button_browseNDK = new Button();
			this.button_browseNDK.WidthRequest = 70;
			this.button_browseNDK.CanFocus = true;
			this.button_browseNDK.Name = "button_browseNDK";
			this.button_browseNDK.UseUnderline = true;
			this.button_browseNDK.Label = Catalog.GetString("浏览");
			this.hbox_ndk.Add(this.button_browseNDK);
			Box.BoxChild boxChild14 = (Box.BoxChild)this.hbox_ndk[this.button_browseNDK];
			boxChild14.Position = 1;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			this.table_android.Add(this.hbox_ndk);
			Table.TableChild tableChild3 = (Table.TableChild)this.table_android[this.hbox_ndk];
			tableChild3.TopAttach = 1U;
			tableChild3.BottomAttach = 2U;
			tableChild3.LeftAttach = 2U;
			tableChild3.RightAttach = 3U;
			tableChild3.YOptions = AttachOptions.Fill;
			this.hbox_sdk = new HBox();
			this.hbox_sdk.Name = "hbox_sdk";
			this.hbox_sdk.Spacing = 6;
			this.entry_SDK = new Entry();
			this.entry_SDK.CanFocus = true;
			this.entry_SDK.Name = "entry_SDK";
			this.entry_SDK.IsEditable = false;
			this.entry_SDK.InvisibleChar = '●';
			this.hbox_sdk.Add(this.entry_SDK);
			Box.BoxChild boxChild15 = (Box.BoxChild)this.hbox_sdk[this.entry_SDK];
			boxChild15.Position = 0;
			this.button_browseSDK = new Button();
			this.button_browseSDK.WidthRequest = 70;
			this.button_browseSDK.CanFocus = true;
			this.button_browseSDK.Name = "button_browseSDK";
			this.button_browseSDK.UseUnderline = true;
			this.button_browseSDK.Label = Catalog.GetString("浏览");
			this.hbox_sdk.Add(this.button_browseSDK);
			Box.BoxChild boxChild16 = (Box.BoxChild)this.hbox_sdk[this.button_browseSDK];
			boxChild16.Position = 1;
			boxChild16.Expand = false;
			boxChild16.Fill = false;
			this.table_android.Add(this.hbox_sdk);
			Table.TableChild tableChild4 = (Table.TableChild)this.table_android[this.hbox_sdk];
			tableChild4.LeftAttach = 2U;
			tableChild4.RightAttach = 3U;
			tableChild4.YOptions = AttachOptions.Fill;
			this.label_ANT = new Label();
			this.label_ANT.WidthRequest = 32;
			this.label_ANT.Name = "label_ANT";
			this.label_ANT.Xalign = 0f;
			this.label_ANT.LabelProp = Catalog.GetString("ANT");
			this.table_android.Add(this.label_ANT);
			Table.TableChild tableChild5 = (Table.TableChild)this.table_android[this.label_ANT];
			tableChild5.TopAttach = 2U;
			tableChild5.BottomAttach = 3U;
			tableChild5.LeftAttach = 1U;
			tableChild5.RightAttach = 2U;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			this.label_JDK = new Label();
			this.label_JDK.WidthRequest = 32;
			this.label_JDK.Name = "label_JDK";
			this.label_JDK.Xalign = 0f;
			this.label_JDK.LabelProp = Catalog.GetString("JDK");
			this.table_android.Add(this.label_JDK);
			Table.TableChild tableChild6 = (Table.TableChild)this.table_android[this.label_JDK];
			tableChild6.TopAttach = 3U;
			tableChild6.BottomAttach = 4U;
			tableChild6.LeftAttach = 1U;
			tableChild6.RightAttach = 2U;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			this.label_NDK = new Label();
			this.label_NDK.WidthRequest = 32;
			this.label_NDK.Name = "label_NDK";
			this.label_NDK.Xalign = 0f;
			this.label_NDK.LabelProp = Catalog.GetString("NDK");
			this.table_android.Add(this.label_NDK);
			Table.TableChild tableChild7 = (Table.TableChild)this.table_android[this.label_NDK];
			tableChild7.TopAttach = 1U;
			tableChild7.BottomAttach = 2U;
			tableChild7.LeftAttach = 1U;
			tableChild7.RightAttach = 2U;
			tableChild7.XOptions = AttachOptions.Fill;
			tableChild7.YOptions = AttachOptions.Fill;
			this.label_SDK = new Label();
			this.label_SDK.WidthRequest = 32;
			this.label_SDK.Name = "label_SDK";
			this.label_SDK.Xalign = 0f;
			this.label_SDK.LabelProp = Catalog.GetString("SDK");
			this.table_android.Add(this.label_SDK);
			Table.TableChild tableChild8 = (Table.TableChild)this.table_android[this.label_SDK];
			tableChild8.LeftAttach = 1U;
			tableChild8.RightAttach = 2U;
			tableChild8.XOptions = AttachOptions.Fill;
			tableChild8.YOptions = AttachOptions.Fill;
			this.vbox2 = new VBox();
			this.vbox2.WidthRequest = 16;
			this.vbox2.Name = "vbox2";
			this.alignment3 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment3.Name = "alignment3";
			this.vbox2.Add(this.alignment3);
			Box.BoxChild boxChild17 = (Box.BoxChild)this.vbox2[this.alignment3];
			boxChild17.Position = 0;
			this.imagebin_sdk = new ImageBin();
			this.imagebin_sdk.Events = EventMask.ButtonPressMask;
			this.imagebin_sdk.Name = "imagebin_sdk";
			this.vbox2.Add(this.imagebin_sdk);
			Box.BoxChild boxChild18 = (Box.BoxChild)this.vbox2[this.imagebin_sdk];
			boxChild18.Position = 1;
			boxChild18.Expand = false;
			boxChild18.Fill = false;
			this.alignment4 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment4.Name = "alignment4";
			this.vbox2.Add(this.alignment4);
			Box.BoxChild boxChild19 = (Box.BoxChild)this.vbox2[this.alignment4];
			boxChild19.Position = 2;
			this.table_android.Add(this.vbox2);
			Table.TableChild tableChild9 = (Table.TableChild)this.table_android[this.vbox2];
			tableChild9.XOptions = AttachOptions.Fill;
			tableChild9.YOptions = AttachOptions.Fill;
			this.vbox3 = new VBox();
			this.vbox3.Name = "vbox3";
			this.alignment5 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment5.Name = "alignment5";
			this.vbox3.Add(this.alignment5);
			Box.BoxChild boxChild20 = (Box.BoxChild)this.vbox3[this.alignment5];
			boxChild20.Position = 0;
			this.imagebin_ndk = new ImageBin();
			this.imagebin_ndk.WidthRequest = 0;
			this.imagebin_ndk.Events = EventMask.ButtonPressMask;
			this.imagebin_ndk.Name = "imagebin_ndk";
			this.vbox3.Add(this.imagebin_ndk);
			Box.BoxChild boxChild21 = (Box.BoxChild)this.vbox3[this.imagebin_ndk];
			boxChild21.Position = 1;
			boxChild21.Expand = false;
			boxChild21.Fill = false;
			this.alignment6 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment6.Name = "alignment6";
			this.vbox3.Add(this.alignment6);
			Box.BoxChild boxChild22 = (Box.BoxChild)this.vbox3[this.alignment6];
			boxChild22.Position = 2;
			this.table_android.Add(this.vbox3);
			Table.TableChild tableChild10 = (Table.TableChild)this.table_android[this.vbox3];
			tableChild10.TopAttach = 1U;
			tableChild10.BottomAttach = 2U;
			tableChild10.XOptions = AttachOptions.Fill;
			tableChild10.YOptions = AttachOptions.Fill;
			this.vbox4 = new VBox();
			this.vbox4.Name = "vbox4";
			this.alignment7 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment7.Name = "alignment7";
			this.vbox4.Add(this.alignment7);
			Box.BoxChild boxChild23 = (Box.BoxChild)this.vbox4[this.alignment7];
			boxChild23.Position = 0;
			this.imagebin_ant = new ImageBin();
			this.imagebin_ant.Events = EventMask.ButtonPressMask;
			this.imagebin_ant.Name = "imagebin_ant";
			this.vbox4.Add(this.imagebin_ant);
			Box.BoxChild boxChild24 = (Box.BoxChild)this.vbox4[this.imagebin_ant];
			boxChild24.Position = 1;
			boxChild24.Expand = false;
			boxChild24.Fill = false;
			this.alignment8 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment8.Name = "alignment8";
			this.vbox4.Add(this.alignment8);
			Box.BoxChild boxChild25 = (Box.BoxChild)this.vbox4[this.alignment8];
			boxChild25.Position = 2;
			this.table_android.Add(this.vbox4);
			Table.TableChild tableChild11 = (Table.TableChild)this.table_android[this.vbox4];
			tableChild11.TopAttach = 2U;
			tableChild11.BottomAttach = 3U;
			tableChild11.XOptions = AttachOptions.Fill;
			tableChild11.YOptions = AttachOptions.Fill;
			this.vbox5 = new VBox();
			this.vbox5.Name = "vbox5";
			this.alignment9 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment9.Name = "alignment9";
			this.vbox5.Add(this.alignment9);
			Box.BoxChild boxChild26 = (Box.BoxChild)this.vbox5[this.alignment9];
			boxChild26.Position = 0;
			this.imagebin_jdk = new ImageBin();
			this.imagebin_jdk.Events = EventMask.ButtonPressMask;
			this.imagebin_jdk.Name = "imagebin_jdk";
			this.vbox5.Add(this.imagebin_jdk);
			Box.BoxChild boxChild27 = (Box.BoxChild)this.vbox5[this.imagebin_jdk];
			boxChild27.Position = 1;
			boxChild27.Expand = false;
			boxChild27.Fill = false;
			this.alignment10 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment10.Name = "alignment10";
			this.vbox5.Add(this.alignment10);
			Box.BoxChild boxChild28 = (Box.BoxChild)this.vbox5[this.alignment10];
			boxChild28.Position = 2;
			this.table_android.Add(this.vbox5);
			Table.TableChild tableChild12 = (Table.TableChild)this.table_android[this.vbox5];
			tableChild12.TopAttach = 3U;
			tableChild12.BottomAttach = 4U;
			tableChild12.XOptions = AttachOptions.Fill;
			tableChild12.YOptions = AttachOptions.Fill;
			this.vbox_android.Add(this.table_android);
			Box.BoxChild boxChild29 = (Box.BoxChild)this.vbox_android[this.table_android];
			boxChild29.Position = 0;
			boxChild29.Expand = false;
			boxChild29.Fill = false;
			this.GtkAlignment.Add(this.vbox_android);
			this.frame_androod.Add(this.GtkAlignment);
			this.GtkLabel_android = new Label();
			this.GtkLabel_android.Name = "GtkLabel_android";
			this.GtkLabel_android.LabelProp = Catalog.GetString(" Android相关路径 ");
			this.GtkLabel_android.UseMarkup = true;
			this.frame_androod.LabelWidget = this.GtkLabel_android;
			this.vbox_main.Add(this.frame_androod);
			Box.BoxChild boxChild30 = (Box.BoxChild)this.vbox_main[this.frame_androod];
			boxChild30.Position = 1;
			boxChild30.Expand = false;
			boxChild30.Fill = false;
			this.hbox_config = new HBox();
			this.hbox_config.Name = "hbox_config";
			this.hbox_config.Spacing = 6;
			this.alignment_storePropmt = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_storePropmt.Name = "alignment_storePropmt";
			this.vbox_storePrompt = new VBox();
			this.vbox_storePrompt.Name = "vbox_storePrompt";
			this.alignment_storePromptTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_storePromptTop.Name = "alignment_storePromptTop";
			this.vbox_storePrompt.Add(this.alignment_storePromptTop);
			Box.BoxChild boxChild31 = (Box.BoxChild)this.vbox_storePrompt[this.alignment_storePromptTop];
			boxChild31.Position = 0;
			this.label_prompt = new Label();
			this.label_prompt.Name = "label_prompt";
			this.label_prompt.Xalign = 0f;
			this.label_prompt.Yalign = 0f;
			this.label_prompt.LabelProp = Catalog.GetString("注：SDK, NDK, ANT和JDK可到Cocos商店获取");
			this.label_prompt.Wrap = true;
			this.vbox_storePrompt.Add(this.label_prompt);
			Box.BoxChild boxChild32 = (Box.BoxChild)this.vbox_storePrompt[this.label_prompt];
			boxChild32.Position = 1;
			boxChild32.Expand = false;
			boxChild32.Fill = false;
			this.alignment_storePromptBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_storePromptBottom.Name = "alignment_storePromptBottom";
			this.vbox_storePrompt.Add(this.alignment_storePromptBottom);
			Box.BoxChild boxChild33 = (Box.BoxChild)this.vbox_storePrompt[this.alignment_storePromptBottom];
			boxChild33.Position = 2;
			this.alignment_storePropmt.Add(this.vbox_storePrompt);
			this.hbox_config.Add(this.alignment_storePropmt);
			Box.BoxChild boxChild34 = (Box.BoxChild)this.hbox_config[this.alignment_storePropmt];
			boxChild34.Position = 0;
			this.alignment_configBtn = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_configBtn.Name = "alignment_configBtn";
			this.button_config = new Button();
			this.button_config.WidthRequest = 85;
			this.button_config.HeightRequest = 30;
			this.button_config.CanFocus = true;
			this.button_config.Name = "button_config";
			this.button_config.UseUnderline = true;
			this.button_config.Label = Catalog.GetString("一键配置");
			this.alignment_configBtn.Add(this.button_config);
			this.hbox_config.Add(this.alignment_configBtn);
			Box.BoxChild boxChild35 = (Box.BoxChild)this.hbox_config[this.alignment_configBtn];
			boxChild35.PackType = PackType.End;
			boxChild35.Position = 1;
			boxChild35.Expand = false;
			boxChild35.Fill = false;
			this.vbox_netWarning = new VBox();
			this.vbox_netWarning.Name = "vbox_netWarning";
			this.alignment_netTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_netTop.Name = "alignment_netTop";
			this.vbox_netWarning.Add(this.alignment_netTop);
			Box.BoxChild boxChild36 = (Box.BoxChild)this.vbox_netWarning[this.alignment_netTop];
			boxChild36.Position = 0;
			this.alignment_netWarning = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_netWarning.Name = "alignment_netWarning";
			this.tooltipicon_net = new TooltipIcon();
			this.tooltipicon_net.Events = EventMask.ButtonPressMask;
			this.tooltipicon_net.Name = "tooltipicon_net";
			this.alignment_netWarning.Add(this.tooltipicon_net);
			this.vbox_netWarning.Add(this.alignment_netWarning);
			Box.BoxChild boxChild37 = (Box.BoxChild)this.vbox_netWarning[this.alignment_netWarning];
			boxChild37.Position = 1;
			boxChild37.Expand = false;
			boxChild37.Fill = false;
			this.alignment_netBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_netBottom.Name = "alignment_netBottom";
			this.vbox_netWarning.Add(this.alignment_netBottom);
			Box.BoxChild boxChild38 = (Box.BoxChild)this.vbox_netWarning[this.alignment_netBottom];
			boxChild38.Position = 2;
			this.hbox_config.Add(this.vbox_netWarning);
			Box.BoxChild boxChild39 = (Box.BoxChild)this.hbox_config[this.vbox_netWarning];
			boxChild39.PackType = PackType.End;
			boxChild39.Position = 2;
			boxChild39.Expand = false;
			this.vbox_main.Add(this.hbox_config);
			Box.BoxChild boxChild40 = (Box.BoxChild)this.vbox_main[this.hbox_config];
			boxChild40.Position = 2;
			boxChild40.Expand = false;
			boxChild40.Fill = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x04000043 RID: 67
		private VBox vbox_main;

		// Token: 0x04000044 RID: 68
		private Frame frame_framework;

		// Token: 0x04000045 RID: 69
		private Alignment GtkAlignment_framework;

		// Token: 0x04000046 RID: 70
		private VBox vbox_framework;

		// Token: 0x04000047 RID: 71
		private HBox hbox_frameworkInfo;

		// Token: 0x04000048 RID: 72
		private VBox vbox1;

		// Token: 0x04000049 RID: 73
		private Alignment alignment1;

		// Token: 0x0400004A RID: 74
		private ImageBin imagebin_framework;

		// Token: 0x0400004B RID: 75
		private Alignment alignment2;

		// Token: 0x0400004C RID: 76
		private Label label_frameworkInfo;

		// Token: 0x0400004D RID: 77
		private EventBox eventbox_framework;

		// Token: 0x0400004E RID: 78
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x0400004F RID: 79
		private TreeView treeview_framework;

		// Token: 0x04000050 RID: 80
		private Label GtkLabel_framework;

		// Token: 0x04000051 RID: 81
		private Frame frame_androod;

		// Token: 0x04000052 RID: 82
		private Alignment GtkAlignment;

		// Token: 0x04000053 RID: 83
		private VBox vbox_android;

		// Token: 0x04000054 RID: 84
		private Table table_android;

		// Token: 0x04000055 RID: 85
		private HBox hbox_ant;

		// Token: 0x04000056 RID: 86
		private Entry entry_ANT;

		// Token: 0x04000057 RID: 87
		private Button button_browseANT;

		// Token: 0x04000058 RID: 88
		private HBox hbox_jdk;

		// Token: 0x04000059 RID: 89
		private Entry entry_JDK;

		// Token: 0x0400005A RID: 90
		private Button button_browseJDK;

		// Token: 0x0400005B RID: 91
		private HBox hbox_ndk;

		// Token: 0x0400005C RID: 92
		private Entry entry_NDK;

		// Token: 0x0400005D RID: 93
		private Button button_browseNDK;

		// Token: 0x0400005E RID: 94
		private HBox hbox_sdk;

		// Token: 0x0400005F RID: 95
		private Entry entry_SDK;

		// Token: 0x04000060 RID: 96
		private Button button_browseSDK;

		// Token: 0x04000061 RID: 97
		private Label label_ANT;

		// Token: 0x04000062 RID: 98
		private Label label_JDK;

		// Token: 0x04000063 RID: 99
		private Label label_NDK;

		// Token: 0x04000064 RID: 100
		private Label label_SDK;

		// Token: 0x04000065 RID: 101
		private VBox vbox2;

		// Token: 0x04000066 RID: 102
		private Alignment alignment3;

		// Token: 0x04000067 RID: 103
		private ImageBin imagebin_sdk;

		// Token: 0x04000068 RID: 104
		private Alignment alignment4;

		// Token: 0x04000069 RID: 105
		private VBox vbox3;

		// Token: 0x0400006A RID: 106
		private Alignment alignment5;

		// Token: 0x0400006B RID: 107
		private ImageBin imagebin_ndk;

		// Token: 0x0400006C RID: 108
		private Alignment alignment6;

		// Token: 0x0400006D RID: 109
		private VBox vbox4;

		// Token: 0x0400006E RID: 110
		private Alignment alignment7;

		// Token: 0x0400006F RID: 111
		private ImageBin imagebin_ant;

		// Token: 0x04000070 RID: 112
		private Alignment alignment8;

		// Token: 0x04000071 RID: 113
		private VBox vbox5;

		// Token: 0x04000072 RID: 114
		private Alignment alignment9;

		// Token: 0x04000073 RID: 115
		private ImageBin imagebin_jdk;

		// Token: 0x04000074 RID: 116
		private Alignment alignment10;

		// Token: 0x04000075 RID: 117
		private Label GtkLabel_android;

		// Token: 0x04000076 RID: 118
		private HBox hbox_config;

		// Token: 0x04000077 RID: 119
		private Alignment alignment_storePropmt;

		// Token: 0x04000078 RID: 120
		private VBox vbox_storePrompt;

		// Token: 0x04000079 RID: 121
		private Alignment alignment_storePromptTop;

		// Token: 0x0400007A RID: 122
		private Label label_prompt;

		// Token: 0x0400007B RID: 123
		private Alignment alignment_storePromptBottom;

		// Token: 0x0400007C RID: 124
		private Alignment alignment_configBtn;

		// Token: 0x0400007D RID: 125
		private Button button_config;

		// Token: 0x0400007E RID: 126
		private VBox vbox_netWarning;

		// Token: 0x0400007F RID: 127
		private Alignment alignment_netTop;

		// Token: 0x04000080 RID: 128
		private Alignment alignment_netWarning;

		// Token: 0x04000081 RID: 129
		private TooltipIcon tooltipicon_net;

		// Token: 0x04000082 RID: 130
		private Alignment alignment_netBottom;
	}
}

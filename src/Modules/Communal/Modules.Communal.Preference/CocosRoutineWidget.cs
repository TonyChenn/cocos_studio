using System;
using System.ComponentModel;
using System.IO;
using Cocos.Launcher.Control;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Stetic;

namespace Modules.Communal.Preference
{
	// Token: 0x02000015 RID: 21
	[ToolboxItem(true)]
	public class CocosRoutineWidget : Bin, IPreferenceWidget
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x000096FC File Offset: 0x000078FC
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.Preference.CocosRoutineWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 12;
			this.frame_download = new Frame();
			this.frame_download.Name = "frame_download";
			this.GtkAlignment_download = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_download.Name = "GtkAlignment_download";
			this.GtkAlignment_download.LeftPadding = 20U;
			this.GtkAlignment_download.TopPadding = 15U;
			this.GtkAlignment_download.RightPadding = 10U;
			this.GtkAlignment_download.BottomPadding = 15U;
			this.hbox_path = new HBox();
			this.hbox_path.Name = "hbox_path";
			this.hbox_path.Spacing = 6;
			this.entry_downloadPath = new Entry();
			this.entry_downloadPath.CanFocus = true;
			this.entry_downloadPath.Name = "entry_downloadPath";
			this.entry_downloadPath.IsEditable = false;
			this.entry_downloadPath.InvisibleChar = '●';
			this.hbox_path.Add(this.entry_downloadPath);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_path[this.entry_downloadPath];
			boxChild.Position = 0;
			this.vbox_browse = new VBox();
			this.vbox_browse.Name = "vbox_browse";
			this.alignment_browseTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_browseTop.Name = "alignment_browseTop";
			this.vbox_browse.Add(this.alignment_browseTop);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_browse[this.alignment_browseTop];
			boxChild2.Position = 0;
			this.alignment_browse = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_browse.WidthRequest = 80;
			this.alignment_browse.HeightRequest = 26;
			this.alignment_browse.Name = "alignment_browse";
			this.vbox_browse.Add(this.alignment_browse);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_browse[this.alignment_browse];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			this.alignment_browseBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_browseBottom.Name = "alignment_browseBottom";
			this.vbox_browse.Add(this.alignment_browseBottom);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_browse[this.alignment_browseBottom];
			boxChild4.Position = 2;
			this.hbox_path.Add(this.vbox_browse);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_path[this.vbox_browse];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			this.vbox_openFolder = new VBox();
			this.vbox_openFolder.Name = "vbox_openFolder";
			this.alignment_openTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_openTop.Name = "alignment_openTop";
			this.vbox_openFolder.Add(this.alignment_openTop);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_openFolder[this.alignment_openTop];
			boxChild6.Position = 0;
			this.alignment_open = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_open.WidthRequest = 80;
			this.alignment_open.HeightRequest = 26;
			this.alignment_open.Name = "alignment_open";
			this.vbox_openFolder.Add(this.alignment_open);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_openFolder[this.alignment_open];
			boxChild7.Position = 1;
			this.alignment_openBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_openBottom.Name = "alignment_openBottom";
			this.vbox_openFolder.Add(this.alignment_openBottom);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_openFolder[this.alignment_openBottom];
			boxChild8.Position = 2;
			this.hbox_path.Add(this.vbox_openFolder);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_path[this.vbox_openFolder];
			boxChild9.Position = 2;
			boxChild9.Expand = false;
			this.GtkAlignment_download.Add(this.hbox_path);
			this.frame_download.Add(this.GtkAlignment_download);
			this.GtkLabel_download = new Label();
			this.GtkLabel_download.Name = "GtkLabel_download";
			this.GtkLabel_download.LabelProp = Catalog.GetString(" 下载路径 ");
			this.GtkLabel_download.UseMarkup = true;
			this.frame_download.LabelWidget = this.GtkLabel_download;
			this.vbox_main.Add(this.frame_download);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_main[this.frame_download];
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.frame_procedure = new Frame();
			this.frame_procedure.Name = "frame_procedure";
			this.GtkAlignment_procedure = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_procedure.Name = "GtkAlignment_procedure";
			this.GtkAlignment_procedure.LeftPadding = 20U;
			this.GtkAlignment_procedure.TopPadding = 15U;
			this.GtkAlignment_procedure.RightPadding = 10U;
			this.GtkAlignment_procedure.BottomPadding = 15U;
			this.frame_procedure.Add(this.GtkAlignment_procedure);
			this.GtkLabel_procedure = new Label();
			this.GtkLabel_procedure.Name = "GtkLabel_procedure";
			this.GtkLabel_procedure.LabelProp = Catalog.GetString(" 程序 ");
			this.GtkLabel_procedure.UseMarkup = true;
			this.frame_procedure.LabelWidget = this.GtkLabel_procedure;
			this.vbox_main.Add(this.frame_procedure);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_main[this.frame_procedure];
			boxChild11.Position = 1;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00009D6F File Offset: 0x00007F6F
		public EnumPreferenceSetting SettingID
		{
			get
			{
				return EnumPreferenceSetting.CocosGeneral;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00009D72 File Offset: 0x00007F72
		public string DisplayName
		{
			get
			{
				return LanguageInfo.Group_Routine;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00009D7C File Offset: 0x00007F7C
		private string startupCocosLnkPath
		{
			get
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
				return System.IO.Path.Combine(folderPath, "Cocos.lnk");
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00009D9D File Offset: 0x00007F9D
		public CocosRoutineWidget()
		{
			this.Build();
			this.InitDownload();
			this.InitProcedure();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00009DB8 File Offset: 0x00007FB8
		private void InitProcedure()
		{
			if (Platform.IsMac)
			{
				this.vbox_main.Remove(this.frame_procedure);
				return;
			}
			this.GtkLabel_procedure.LabelProp = string.Format(" {0} ", LanguageInfo.Preference_Procedure);
			this.checkboxView = new CheckboxView();
			this.checkboxView.Label = LanguageInfo.Preference_SinceStartup;
			this.checkboxView.Active = File.Exists(this.startupCocosLnkPath);
			this.GtkAlignment_procedure.Add(this.checkboxView);
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.GtkLabel_procedure.SetFontSize(13.0);
				this.checkboxView.SetFontSize(13.0);
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00009E6C File Offset: 0x0000806C
		private void InitDownload()
		{
			this.GtkLabel_download.Text = " " + LanguageInfo.Preference_SaveDir + " ";
			this.entry_downloadPath.Text = Option.UserConfig.CocosStorePath;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				GeneralLauncherButton generalLauncherButton = new GeneralLauncherButton();
				generalLauncherButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.HandleButtonBrowseClicked);
				generalLauncherButton.Text = LanguageInfo.Dialog_ButtonBrowse + "...";
				generalLauncherButton.SetFontSize(13.0);
				this.alignment_browse.Add(generalLauncherButton);
				generalLauncherButton.Show();
				GeneralLauncherButton generalLauncherButton2 = new GeneralLauncherButton();
				generalLauncherButton2.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.ButtonOpenFolderClickedHandler);
				generalLauncherButton2.Text = LanguageInfo.Launcher_Open;
				generalLauncherButton2.SetFontSize(13.0);
				this.alignment_open.Add(generalLauncherButton2);
				generalLauncherButton2.Show();
				this.GtkLabel_download.SetFontSize(13.0);
				this.entry_downloadPath.SetFontSize(13.0);
				return;
			}
			Button button = new Button(LanguageInfo.Dialog_ButtonBrowse + "...");
			button.Clicked += this.HandleButtonBrowseClicked;
			this.alignment_browse.Add(button);
			button.Show();
			Button button2 = new Button(LanguageInfo.Launcher_Open);
			button2.Clicked += this.ButtonOpenFolderClickedHandler;
			this.alignment_open.Add(button2);
			button2.Show();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00009FDC File Offset: 0x000081DC
		public void ApplySetting()
		{
			Option.UserConfig.CocosStorePath = this.entry_downloadPath.Text;
			if (Platform.IsWindows)
			{
				try
				{
					if (this.checkboxView.Active && !File.Exists(this.startupCocosLnkPath))
					{
						string text = System.IO.Path.Combine(Option.CocosInstallDir, "Cocos Studio", "Cocos.lnk");
						if (File.Exists(text))
						{
							FileService.CopyFile(text, this.startupCocosLnkPath);
						}
					}
					else if (!this.checkboxView.Active && File.Exists(this.startupCocosLnkPath))
					{
						File.Delete(this.startupCocosLnkPath);
					}
				}
				catch (Exception arg)
				{
					LogConfig.Logger.Error("Cocos常规设置失败：" + arg);
				}
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000A09C File Offset: 0x0000829C
		public bool CanApply(out string output)
		{
			string text = this.entry_downloadPath.Text;
			if (!this.CheckDownloadDirValidity(text, out output))
			{
				return false;
			}
			output = "";
			return true;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000A0CC File Offset: 0x000082CC
		private bool CheckDownloadDirValidity(string dir, out string output)
		{
			if (string.IsNullOrEmpty(dir))
			{
				output = LanguageInfo.Preference_SaveDirIllegal;
				this.entry_downloadPath.HasFocus = true;
				return false;
			}
			if (!System.IO.Path.IsPathRooted(dir))
			{
				output = LanguageInfo.Preference_SaveDirIllegal;
				this.entry_downloadPath.HasFocus = true;
				this.entry_downloadPath.SelectAll();
				return false;
			}
			if (!Option.CheckIsWritableDir(dir))
			{
				output = LanguageInfo.Preference_SaveDirIllegal;
				this.entry_downloadPath.HasFocus = true;
				this.entry_downloadPath.SelectAll();
				return false;
			}
			output = "";
			return true;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000A14E File Offset: 0x0000834E
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000A154 File Offset: 0x00008354
		private void HandleButtonBrowseClicked(object sender, EventArgs e)
		{
			SelectFolderDialog selectFolderDialog = new SelectFolderDialog();
			selectFolderDialog.TransientFor = MessageService.GetDefaultModalParent();
			selectFolderDialog.Title = LanguageInfo.Preference_SelectSaveDir;
			selectFolderDialog.Action = FileChooserAction.SelectFolder;
			selectFolderDialog.SelectMultiple = false;
			selectFolderDialog.CurrentFolder = this.entry_downloadPath.Text;
			if (selectFolderDialog.Run() && !string.IsNullOrEmpty(selectFolderDialog.SelectedFile))
			{
				this.entry_downloadPath.Text = selectFolderDialog.SelectedFile;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000A1D4 File Offset: 0x000083D4
		private void ButtonOpenFolderClickedHandler(object sender, EventArgs args)
		{
			try
			{
				string text = this.entry_downloadPath.Text;
				string info;
				if (!this.CheckDownloadDirValidity(text, out info))
				{
					MessageBox.Show(info, MessageBoxImage.Other, null, null);
				}
				else
				{
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					PlatformAdapter.PlatformService.OpenFile(text);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("打开下载保存目录时出错", exception);
			}
		}

		// Token: 0x040000CB RID: 203
		private VBox vbox_main;

		// Token: 0x040000CC RID: 204
		private Frame frame_download;

		// Token: 0x040000CD RID: 205
		private Alignment GtkAlignment_download;

		// Token: 0x040000CE RID: 206
		private HBox hbox_path;

		// Token: 0x040000CF RID: 207
		private Entry entry_downloadPath;

		// Token: 0x040000D0 RID: 208
		private VBox vbox_browse;

		// Token: 0x040000D1 RID: 209
		private Alignment alignment_browseTop;

		// Token: 0x040000D2 RID: 210
		private Alignment alignment_browse;

		// Token: 0x040000D3 RID: 211
		private Alignment alignment_browseBottom;

		// Token: 0x040000D4 RID: 212
		private VBox vbox_openFolder;

		// Token: 0x040000D5 RID: 213
		private Alignment alignment_openTop;

		// Token: 0x040000D6 RID: 214
		private Alignment alignment_open;

		// Token: 0x040000D7 RID: 215
		private Alignment alignment_openBottom;

		// Token: 0x040000D8 RID: 216
		private Label GtkLabel_download;

		// Token: 0x040000D9 RID: 217
		private Frame frame_procedure;

		// Token: 0x040000DA RID: 218
		private Alignment GtkAlignment_procedure;

		// Token: 0x040000DB RID: 219
		private Label GtkLabel_procedure;

		// Token: 0x040000DC RID: 220
		private CheckboxView checkboxView;
	}
}

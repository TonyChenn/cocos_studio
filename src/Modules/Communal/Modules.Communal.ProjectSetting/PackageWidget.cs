using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.ProjectSetting
{
	[ToolboxItem(true)]
	public class PackageWidget : Bin, IProjectSettingWidget
	{
		public PackageWidget()
		{
			this.Build();
			this.InitDisplayText();
			this.Init();
		}

		private void Init()
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			this.entry_generatePath.Text = currentSolution.Config.PackageDirectory;
			int active = -1;
			int num = 0;
			IReadOnlyList<string> enabledVersions = FrameworkHelper.EnabledVersions;
			foreach (string text in enabledVersions)
			{
				this.combobox_framework.AppendText(text);
				if (PackageServices.Instance.PackageParams.FrameworkVersion.Equals(text))
				{
					active = num;
				}
				num++;
			}
			this.combobox_framework.Active = active;
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("Modules.Communal.ProjectSetting.Image.StudioMessageWarring.png");
			this.imagebin_warring.SetImageView(icon);
			this.alignment_warring.Remove(this.hbox_warring);
			HelpButton helpButton = new HelpButton(null);
			this.alignment_help.Add(helpButton);
			helpButton.Show();
			helpButton.URL = LanguageAdapter.GetLocalizedUrl(HelpLinkUrl.FrameworkVersionUpgrade);
		}

		private void InitDisplayText()
		{
			this.GtkLabel_generatePath.Text = " " + LanguageInfo.Package_GeneratePath + " ";
			this.button_browse.Label = LanguageInfo.Dialog_ButtonBrowse + "...";
			this.button_openFolder.Label = LanguageInfo.Launcher_Open + "...";
			this.button_openFolder.TooltipText = LanguageInfo.Command_OpenDirectory;
			this.GtkLabel_framework.Text = " " + LanguageInfo.Dialog_New_EngineVersion + " ";
			this.label_warning.Text = LanguageInfo.ProjSetting_ChangeVersionWarring;
		}

		private void DeleteDir(string directory)
		{
			if (Directory.Exists(directory))
			{
				try
				{
					Directory.Delete(directory, true);
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error(string.Format("删除路径{0}失败", directory), exception);
				}
			}
		}

		public EnumProjectSetting SettingID
		{
			get
			{
				return EnumProjectSetting.Package;
			}
		}

		public string DisplayName
		{
			get
			{
				return LanguageInfo.Package;
			}
		}

		public void ApplySetting()
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			currentSolution.Config.PackageDirectory = this.entry_generatePath.Text;
			currentSolution.SetPackageDirectory();
			if (this.combobox_framework.Active != -1 && !string.IsNullOrWhiteSpace(this.combobox_framework.ActiveText))
			{
				string frameworkVersion = PackageServices.Instance.PackageParams.FrameworkVersion;
				string activeText = this.combobox_framework.ActiveText;
				if (!activeText.Equals(frameworkVersion))
				{
					string directory = System.IO.Path.Combine(new string[]
					{
						currentSolution.BaseDirectory,
						"frameworks",
						"runtime-src",
						"proj.android",
						"obj"
					});
					string directory2 = System.IO.Path.Combine(currentSolution.BaseDirectory, "proj.android", "obj");
					this.DeleteDir(directory);
					this.DeleteDir(directory2);
				}
				PackageServices.Instance.PackageParams.FrameworkVersion = activeText;
			}
		}

		public bool CanApply(out string output)
		{
			if (!ProjectSettingHelper.CheckPathValidity(this.entry_generatePath.Text))
			{
				this.entry_generatePath.SelectAll();
				this.entry_generatePath.HasFocus = true;
				output = LanguageInfo.MessageBox257_illegalPkgPath;
				return false;
			}
			if (FrameworkHelper.EnabledVersions.Count != 0 && this.combobox_framework.Active == -1)
			{
				output = LanguageInfo.MessageBox258_pleaseSelectFw;
				return false;
			}
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
				if (this.subWidgets == null)
				{
					this.subWidgets = new List<IProjectSettingWidget>();
					if (Platform.IsMac)
					{
						this.subWidgets.Add(new IOSWidget());
					}
					this.subWidgets.Add(new AndroidWidget());
					if (Cocos2dxServices.CocosProperties.ProgramLanguage == EnumProgramLanguage.js)
					{
						this.subWidgets.Add(new HTML5Widget());
					}
				}
				return this.subWidgets;
			}
		}

		protected void HandleButtonBrowseClicked(object sender, EventArgs e)
		{
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			string text = this.entry_generatePath.Text;
			if (!System.IO.Path.IsPathRooted(text))
			{
				text = System.IO.Path.Combine(currentSelectedSolution.BaseDirectory, text);
				text = System.IO.Path.GetFullPath(text);
			}
			if (!Directory.Exists(text))
			{
				text = System.IO.Path.GetFullPath(currentSelectedSolution.BaseDirectory);
			}
			string text2 = "";
			if (Option.IsXP)
			{
				text2 = FileChooserDialogModel.GetBrowseDialogPath(LanguageInfo.Dialog_ProjectPath, false, text, false).Folder;
			}
			else
			{
				SelectFolderDialog selectFolderDialog = new SelectFolderDialog();
				selectFolderDialog.TransientFor = MessageService.GetDefaultModalParent();
				selectFolderDialog.Title = LanguageInfo.ProjSetting_PackageDir;
				selectFolderDialog.Action = FileChooserAction.SelectFolder;
				selectFolderDialog.SelectMultiple = false;
				selectFolderDialog.CurrentFolder = text;
				if (selectFolderDialog.Run())
				{
					text2 = selectFolderDialog.SelectedFile;
				}
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				this.entry_generatePath.Text = ProjectSettingHelper.GetUnifiedPath(text2, currentSelectedSolution.BaseDirectory);
			}
		}

		protected void HandleComboboxChanged(object sender, EventArgs e)
		{
			if (this.combobox_framework.Active != -1)
			{
				string frameworkVersion = PackageServices.Instance.PackageParams.FrameworkVersion;
				if (frameworkVersion.Equals(this.combobox_framework.ActiveText))
				{
					if (this.alignment_warring.Child != null)
					{
						this.alignment_warring.Remove(this.hbox_warring);
						return;
					}
				}
				else if (this.alignment_warring.Child == null)
				{
					this.alignment_warring.Add(this.hbox_warring);
					this.hbox_warring.ShowAll();
				}
			}
		}

		protected void ButtonOpenFolderClickedHandler(object sender, EventArgs e)
		{
			try
			{
				if (ProjectSettingHelper.CheckPathValidity(this.entry_generatePath.Text))
				{
					string text = ProjectSettingHelper.ConvertToAbsolutePath(this.entry_generatePath.Text);
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					PlatformAdapter.PlatformService.OpenFile(text);
				}
				else
				{
					MessageBox.Show(LanguageInfo.MessageBox257_illegalPkgPath, MessageBoxImage.Error, null, null);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("打开打包目录时出错", exception);
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.ProjectSetting.PackageWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.frame_generatePath = new Frame();
			this.frame_generatePath.Name = "frame_generatePath";
			this.GtkAlignment_generatePath = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_generatePath.Name = "GtkAlignment_generatePath";
			this.GtkAlignment_generatePath.LeftPadding = 20U;
			this.GtkAlignment_generatePath.TopPadding = 10U;
			this.GtkAlignment_generatePath.RightPadding = 10U;
			this.GtkAlignment_generatePath.BottomPadding = 10U;
			this.hbox_generatePath = new HBox();
			this.hbox_generatePath.Name = "hbox_generatePath";
			this.hbox_generatePath.Spacing = 6;
			this.entry_generatePath = new Entry();
			this.entry_generatePath.CanFocus = true;
			this.entry_generatePath.Name = "entry_generatePath";
			this.entry_generatePath.IsEditable = true;
			this.entry_generatePath.InvisibleChar = '●';
			this.hbox_generatePath.Add(this.entry_generatePath);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_generatePath[this.entry_generatePath];
			boxChild.Position = 0;
			this.button_browse = new Button();
			this.button_browse.WidthRequest = 65;
			this.button_browse.CanFocus = true;
			this.button_browse.Name = "button_browse";
			this.button_browse.Label = Catalog.GetString("浏览");
			this.hbox_generatePath.Add(this.button_browse);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_generatePath[this.button_browse];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.button_openFolder = new Button();
			this.button_openFolder.WidthRequest = 65;
			this.button_openFolder.CanFocus = true;
			this.button_openFolder.Name = "button_openFolder";
			this.button_openFolder.UseUnderline = true;
			this.button_openFolder.Label = Catalog.GetString("打开");
			this.hbox_generatePath.Add(this.button_openFolder);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_generatePath[this.button_openFolder];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.GtkAlignment_generatePath.Add(this.hbox_generatePath);
			this.frame_generatePath.Add(this.GtkAlignment_generatePath);
			this.GtkLabel_generatePath = new Label();
			this.GtkLabel_generatePath.Name = "GtkLabel_generatePath";
			this.GtkLabel_generatePath.LabelProp = Catalog.GetString(" 生成路径 ");
			this.GtkLabel_generatePath.UseMarkup = true;
			this.frame_generatePath.LabelWidget = this.GtkLabel_generatePath;
			this.vbox_main.Add(this.frame_generatePath);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.frame_generatePath];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.frame_framework = new Frame();
			this.frame_framework.Name = "frame_framework";
			this.GtkAlignment_framework = new Alignment(0f, 0f, 1f, 1f);
			this.GtkAlignment_framework.Name = "GtkAlignment_framework";
			this.GtkAlignment_framework.LeftPadding = 20U;
			this.GtkAlignment_framework.TopPadding = 10U;
			this.GtkAlignment_framework.RightPadding = 10U;
			this.GtkAlignment_framework.BottomPadding = 10U;
			this.vbox_framework = new VBox();
			this.vbox_framework.Name = "vbox_framework";
			this.hbox_version = new HBox();
			this.hbox_version.Name = "hbox_version";
			this.hbox_version.Spacing = 6;
			this.label_cocosFramework = new Label();
			this.label_cocosFramework.Name = "label_cocosFramework";
			this.label_cocosFramework.LabelProp = Catalog.GetString("Cocos Framework");
			this.hbox_version.Add(this.label_cocosFramework);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_version[this.label_cocosFramework];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.combobox_framework = ComboBox.NewText();
			this.combobox_framework.WidthRequest = 200;
			this.combobox_framework.Name = "combobox_framework";
			this.hbox_version.Add(this.combobox_framework);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_version[this.combobox_framework];
			boxChild6.Position = 1;
			this.vbox_framework.Add(this.hbox_version);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_framework[this.hbox_version];
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.GtkAlignment_framework.Add(this.vbox_framework);
			this.frame_framework.Add(this.GtkAlignment_framework);
			this.GtkLabel_framework = new Label();
			this.GtkLabel_framework.Name = "GtkLabel_framework";
			this.GtkLabel_framework.LabelProp = Catalog.GetString(" 引擎版本 ");
			this.GtkLabel_framework.UseMarkup = true;
			this.frame_framework.LabelWidget = this.GtkLabel_framework;
			this.vbox_main.Add(this.frame_framework);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.frame_framework];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.alignment_warring = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_warring.Name = "alignment_warring";
			this.alignment_warring.LeftPadding = 21U;
			this.hbox_warring = new HBox();
			this.hbox_warring.Name = "hbox_warring";
			this.hbox_warring.Spacing = 6;
			this.imagebin_warring = new ImageBin();
			this.imagebin_warring.Events = EventMask.ButtonPressMask;
			this.imagebin_warring.Name = "imagebin_warring";
			this.hbox_warring.Add(this.imagebin_warring);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_warring[this.imagebin_warring];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.label_warning = new Label();
			this.label_warning.Name = "label_warning";
			this.label_warning.LabelProp = Catalog.GetString("更换Framework版本将可能会引起API不适配的问题");
			this.hbox_warring.Add(this.label_warning);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_warring[this.label_warning];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.vbox_help = new VBox();
			this.vbox_help.Name = "vbox_help";
			this.alignment_helpTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_helpTop.Name = "alignment_helpTop";
			this.vbox_help.Add(this.alignment_helpTop);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_help[this.alignment_helpTop];
			boxChild11.Position = 0;
			this.alignment_help = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_help.Name = "alignment_help";
			this.alignment_help.LeftPadding = 20U;
			this.vbox_help.Add(this.alignment_help);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_help[this.alignment_help];
			boxChild12.Position = 1;
			this.alignment_helpBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_helpBottom.Name = "alignment_helpBottom";
			this.vbox_help.Add(this.alignment_helpBottom);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.vbox_help[this.alignment_helpBottom];
			boxChild13.Position = 2;
			this.hbox_warring.Add(this.vbox_help);
			Box.BoxChild boxChild14 = (Box.BoxChild)this.hbox_warring[this.vbox_help];
			boxChild14.Position = 2;
			boxChild14.Expand = false;
			this.alignment_warring.Add(this.hbox_warring);
			this.vbox_main.Add(this.alignment_warring);
			Box.BoxChild boxChild15 = (Box.BoxChild)this.vbox_main[this.alignment_warring];
			boxChild15.Position = 2;
			boxChild15.Expand = false;
			boxChild15.Fill = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.button_browse.Clicked += this.HandleButtonBrowseClicked;
			this.button_openFolder.Clicked += this.ButtonOpenFolderClickedHandler;
			this.combobox_framework.Changed += this.HandleComboboxChanged;
		}

		private List<IProjectSettingWidget> subWidgets;

		private VBox vbox_main;

		private Frame frame_generatePath;

		private Alignment GtkAlignment_generatePath;

		private HBox hbox_generatePath;

		private Entry entry_generatePath;

		private Button button_browse;

		private Button button_openFolder;

		private Label GtkLabel_generatePath;

		private Frame frame_framework;

		private Alignment GtkAlignment_framework;

		private VBox vbox_framework;

		private HBox hbox_version;

		private Label label_cocosFramework;

		private ComboBox combobox_framework;

		private Label GtkLabel_framework;

		private Alignment alignment_warring;

		private HBox hbox_warring;

		private ImageBin imagebin_warring;

		private Label label_warning;

		private VBox vbox_help;

		private Alignment alignment_helpTop;

		private Alignment alignment_help;

		private Alignment alignment_helpBottom;
	}
}

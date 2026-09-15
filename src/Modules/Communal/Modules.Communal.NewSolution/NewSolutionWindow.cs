using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using CocoStudio.UserStatistics;
using Gdk;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	public class NewSolutionWindow : Gtk.Window
	{
		public event EventHandler<SolutionCreatedArgs> SolutionCreated;

		public NewSolutionWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Init();
		}

		private void Init()
		{
			this.InitStyles();
			this.InitButtons();
			this.InitWidget();
			this.InitWindow();
		}

		private void InitStyles()
		{
			base.Title = LanguageInfo.Menu_File_NewProject;
			this.label_title.SetFontSize(14.0);
			this.button_next.Name = "MainButton";
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.pageTag_1.png");
			Xwt.Drawing.Image icon2 = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.pageTag_2.png");
			this.pageTagImage1 = new ImageView(icon);
			this.pageTagImage2 = new ImageView(icon2);
		}

		private void InitButtons()
		{
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.launcherNextBtn = new GeneralLauncherButton();
				this.launcherNextBtn.SetButtonStyle(true);
				this.launcherNextBtn.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.OnNextBtnClicked);
				this.alignment_next.Remove(this.button_next);
				this.alignment_next.Add(this.launcherNextBtn);
				this.launcherNextBtn.Text = LanguageInfo.NewSolution_Next;
				this.launcherNextBtn.Show();
				this.launcherPreviousBtn = new GeneralLauncherButton();
				this.launcherPreviousBtn.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.OnPreviousBtnClicked);
				this.alignment_previous.Remove(this.button_previous);
				this.alignment_previous.Add(this.launcherPreviousBtn);
				this.launcherPreviousBtn.Text = LanguageInfo.NewSolution_Previous;
				this.launcherPreviousBtn.Show();
				return;
			}
			this.button_next.Label = LanguageInfo.NewSolution_Next;
			this.button_previous.Label = LanguageInfo.NewSolution_Previous;
		}

		private void InitWidget()
		{
			this.selectCardWidget = new SelectTemplateWidget();
			this.selectCardWidget.TemplateSelected += this.HandleProjectTypeSelected;
			this.GotoSelectTemplatePage();
			HelpButton helpButton = new HelpButton(null);
			this.alignment_help.Add(helpButton);
			helpButton.Show();
			helpButton.URL = LanguageAdapter.GetLocalizedUrl(HelpLinkUrl.NewProject);
		}

		private void InitWindow()
		{
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.CenterToParentWindow(ApplicationCurrent.MainWindow);
				base.TransientFor = ApplicationCurrent.MainWindow;
				ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.title_newProject.png");
				this.customTitleBar.SetParentWindow(this);
				this.customTitleBar.Title = LanguageInfo.Menu_File_NewProject;
				this.customTitleBar.CloseClicked += this.OnTitleCloseButtonClicked;
				this.evtbx_bg.ModifyBg(StateType.Normal, NewSolutionStyles.Launcher_BgGray);
				this.evtbx_contentBorder.ModifyBg(StateType.Normal, NewSolutionStyles.Launcher_BorderGray);
				this.evtbx_contentBg.ModifyBg(StateType.Normal, NewSolutionStyles.White);
				this.RemoveWindowBorder();
				if (Platform.IsWindows)
				{
					this.evtbx_windowBorder.VisibleWindow = true;
					this.evtbx_windowBorder.ModifyBg(StateType.Normal, NewSolutionStyles.Launcher_WindowBorder);
					this.vbox_window.BorderWidth = 1U;
					return;
				}
			}
			else
			{
				int heightRequest = this.vbox_window.HeightRequest;
				int heightRequest2 = this.customTitleBar.HeightRequest;
				this.vbox_main.Remove(this.customTitleBar);
				this.customTitleBar.Destroy();
				this.vbox_window.HeightRequest = heightRequest - heightRequest2;
				this.SetToDialogStyle(null, true, true, true);
				this.evtbx_bg.ModifyBg(StateType.Normal, NewSolutionStyles.Studio_BgGray);
				this.evtbx_contentBorder.ModifyBg(StateType.Normal, NewSolutionStyles.Studio_HoverDarkGray);
				this.evtbx_contentBg.ModifyBg(StateType.Normal, NewSolutionStyles.Studio_BgGray);
			}
		}

		private void CreateCocosItem(CreateParams prms)
		{
			if (Services.ProjectOperations.CloseSolution())
			{
				Services.RecentFileService.LastCreatePrjDirectory = prms.Directory;
				CocosMonitor cocosMonitor = new CocosMonitor(false);
				string text;
				this.selectCardWidget.CurrentTemplate.CreateNewSolution(prms, cocosMonitor, out text);
				bool flag = cocosMonitor.IsSuccessed;
				string outputDetail = string.Empty;
				if (flag)
				{
					if (Option.CurrentApp == EnumApp.Launcher)
					{
						if (this.SolutionCreated != null)
						{
							SolutionCreatedArgs e = new SolutionCreatedArgs(text);
							this.SolutionCreated(this, e);
						}
					}
					else
					{
						try
						{
							string filePath = System.IO.Path.Combine(prms.Directory, prms.ProjName, prms.ProjName + ".ccs");
							IProgressMonitor defaultMonitor = Services.ProjectsService.DefaultMonitor;
							Solution solution = Services.ProjectOperations.OpenSolution(defaultMonitor, filePath);
							if (solution != null)
							{
								ResourceGroup resourceGroup = solution.RootFolder.Items[0] as ResourceGroup;
								CocosItem cocosItem = resourceGroup.FindResourceItem(text) as CocosItem;
								if (cocosItem != null)
								{
									Services.Workbench.OpenDocument(cocosItem);
								}
							}
						}
						catch (Exception ex)
						{
							LogConfig.Logger.Error("项目创建后在打开过程中出错", ex);
							flag = false;
							outputDetail = ex.ToString();
						}
					}
					if (prms.EngineInfo != null)
					{
						string arg = (prms.EngineInfo.VersionText == "IDE cocos") ? "none" : prms.EngineInfo.VersionText;
						string arg2 = prms.UseX86 ? "SupportX86" : "NotSupportX86";
						string methodName = string.Format("{0} & {1} & {2}", prms.Language.ToString(), arg, arg2);
						Tracker.Add(ViewRegions.None, "NewProject", methodName, "");
					}
				}
				else
				{
					outputDetail = cocosMonitor.FullOutputInfo;
				}
				if (!flag)
				{
					this.ShowCreateFailedDlg(outputDetail);
				}
			}
			this.CloseWindow();
		}

		private void ShowCreateFailedDlg(string outputDetail)
		{
			LogConfig.OutputWithoutTip.Info(LanguageInfo.Dialog_New_CreateFailed, true);
			base.Modal = false;
			CustomTitleDialog customTitleDialog = new CustomTitleDialog();
			OutputViewWidget widget = new OutputViewWidget(customTitleDialog, outputDetail);
			customTitleDialog.InitView(LanguageInfo.MessageBox_Error, widget, this);
			customTitleDialog.Run();
			customTitleDialog.Destroy();
			base.Modal = true;
		}

		private void GotoSelectTemplatePage()
		{
			this.currentPage = 0;
			this.ReleaseSetWidget();
			if (this.alignment_previous.Child != null)
			{
				this.alignment_previous.Remove(this.alignment_previous.Child);
			}
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.launcherNextBtn.Text = LanguageInfo.NewSolution_Next;
			}
			else
			{
				this.button_next.Label = LanguageInfo.NewSolution_Next;
			}
			this.label_title.Text = LanguageInfo.NewSolution_StepOne;
			if (this.alignment_titleIcon.Child != null)
			{
				this.alignment_titleIcon.Remove(this.alignment_titleIcon.Child);
			}
			this.alignment_titleIcon.Add(this.pageTagImage1);
			this.pageTagImage1.Show();
			if (this.alignment_content.Child != null)
			{
				this.alignment_content.Remove(this.alignment_content.Child);
			}
			this.alignment_content.Add(this.selectCardWidget);
			this.selectCardWidget.Show();
			this.RefreshUI();
		}

		private void GotoSetPropertyPage(ISolutionTemplate slnTemplate)
		{
			if (slnTemplate.Info.NeedFramework && Cocos2dxInfo.GetSimplifiedConsole() == null && FrameworkHelper.EnabledVersions.Count == 0)
			{
				MessageBox.Show(LanguageInfo.MessageBox228_FrameworkNotFound, MessageBoxImage.Other, null, null);
				return;
			}
			this.currentPage = 1;
			if (this.alignment_previous.Child == null)
			{
				if (Option.CurrentApp == EnumApp.Launcher)
				{
					this.alignment_previous.Add(this.launcherPreviousBtn);
					this.launcherNextBtn.Text = LanguageInfo.guide_LastOne;
				}
				else
				{
					this.alignment_previous.Add(this.button_previous);
					this.button_next.Label = LanguageInfo.guide_LastOne;
				}
			}
			if (this.alignment_titleIcon.Child != null)
			{
				this.alignment_titleIcon.Remove(this.alignment_titleIcon.Child);
			}
			this.alignment_titleIcon.Add(this.pageTagImage2);
			this.pageTagImage2.Show();
			this.label_title.Text = LanguageInfo.NewSolution_StepTwo;
			this.ReleaseSetWidget();
			this.curSlnSetWidget = new PropertiesWidget(slnTemplate);
			this.curSlnSetWidget.EnableChanged += this.OnButtonEnableChanged;
			this.curSlnSetWidget.CreateParamsSet += this.OnCreateParamsSet;
			if (this.alignment_content.Child != null)
			{
				this.alignment_content.Remove(this.alignment_content.Child);
			}
			this.alignment_content.Add(this.curSlnSetWidget);
			this.curSlnSetWidget.CanDefault = true;
			this.curSlnSetWidget.GrabDefault();
			this.curSlnSetWidget.Show();
			this.RefreshUI();
		}

		private void RefreshUI()
		{
			if (this.currentPage == 0)
			{
				this.button_next.Sensitive = true;
				this.button_next.HasFocus = true;
				return;
			}
			if (this.currentPage == 1)
			{
				this.button_next.Sensitive = this.curSlnSetWidget.IsEnable;
			}
		}

		private void ReleaseSetWidget()
		{
			if (this.curSlnSetWidget != null)
			{
				this.curSlnSetWidget.EnableChanged -= this.OnButtonEnableChanged;
				this.curSlnSetWidget.CreateParamsSet -= this.OnCreateParamsSet;
				this.curSlnSetWidget.Destroy();
				this.curSlnSetWidget = null;
			}
		}

		private void CloseWindow()
		{
			this.ReleaseSetWidget();
			this.selectCardWidget.TemplateSelected -= this.HandleProjectTypeSelected;
			this.Destroy();
		}

		protected void OnPreviousBtnClicked(object sender, EventArgs e)
		{
			this.GotoSelectTemplatePage();
		}

		protected void OnNextBtnClicked(object sender, EventArgs e)
		{
			if (this.currentPage == 1)
			{
				string info;
				CreateParams createParams = this.curSlnSetWidget.GetCreateParams(out info);
				if (createParams == null)
				{
					MessageBox.Show(info, MessageBoxImage.Warning, null, null);
					return;
				}
				this.CreateCocosItem(createParams);
				return;
			}
			else
			{
				if (this.currentPage == 0)
				{
					this.GotoSetPropertyPage(this.selectCardWidget.CurrentTemplate);
					return;
				}
				throw new Exception("页码索引错误");
			}
		}

		private void HandleProjectTypeSelected(object sender, TemplateSelectedArgs e)
		{
			this.GotoSetPropertyPage(e.SolutionTemplate);
		}

		protected void OnKeyPressed(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Escape)
			{
				this.CloseWindow();
			}
		}

		private void OnButtonEnableChanged(object o, EnableChangedArgs args)
		{
			this.button_next.Sensitive = args.IsEnable;
		}

		private void OnCreateParamsSet(object o, CreateParamsSetArgs args)
		{
			this.CreateCocosItem(args.Params);
		}

		private void OnTitleCloseButtonClicked(object sender, EventArgs args)
		{
			this.CloseWindow();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.HeightRequest = 0;
			base.Name = "Modules.Communal.NewSolution.NewSolutionWindow";
			base.Title = Catalog.GetString("新建项目");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			this.evtbx_windowBorder = new EventBox();
			this.evtbx_windowBorder.Name = "evtbx_windowBorder";
			this.evtbx_windowBorder.VisibleWindow = false;
			this.vbox_window = new VBox();
			this.vbox_window.WidthRequest = 636;
			this.vbox_window.HeightRequest = 500;
			this.vbox_window.Name = "vbox_window";
			this.customTitleBar = new CustomTitleBar();
			this.customTitleBar.HeightRequest = 26;
			this.customTitleBar.Events = EventMask.ButtonPressMask;
			this.customTitleBar.Name = "customTitleBar";
			this.vbox_window.Add(this.customTitleBar);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_window[this.customTitleBar];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.alignment_base = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_base.Name = "alignment_base";
			this.alignment_base.LeftPadding = 24U;
			this.alignment_base.RightPadding = 24U;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.alignment_title = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_title.Name = "alignment_title";
			this.hbox_innerTitle = new HBox();
			this.hbox_innerTitle.Name = "hbox_innerTitle";
			this.alignment_titleIcon = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_titleIcon.Name = "alignment_titleIcon";
			this.alignment_titleIcon.TopPadding = 15U;
			this.alignment_titleIcon.RightPadding = 10U;
			this.alignment_titleIcon.BottomPadding = 15U;
			this.image2 = new Gtk.Image();
			this.image2.Name = "image2";
			this.image2.Pixbuf = IconLoader.LoadIcon(this, "gtk-dialog-info", IconSize.Menu);
			this.alignment_titleIcon.Add(this.image2);
			this.hbox_innerTitle.Add(this.alignment_titleIcon);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_innerTitle[this.alignment_titleIcon];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.label_title = new Label();
			this.label_title.Name = "label_title";
			this.label_title.LabelProp = Catalog.GetString("设置项目信息");
			this.hbox_innerTitle.Add(this.label_title);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_innerTitle[this.label_title];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.vbox_help = new VBox();
			this.vbox_help.Name = "vbox_help";
			this.alignment_helpTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_helpTop.Name = "alignment_helpTop";
			this.vbox_help.Add(this.alignment_helpTop);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_help[this.alignment_helpTop];
			boxChild4.Position = 0;
			this.alignment_help = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_help.Name = "alignment_help";
			this.vbox_help.Add(this.alignment_help);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_help[this.alignment_help];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			this.alignment_helpBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_helpBottom.Name = "alignment_helpBottom";
			this.vbox_help.Add(this.alignment_helpBottom);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_help[this.alignment_helpBottom];
			boxChild6.Position = 2;
			this.hbox_innerTitle.Add(this.vbox_help);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_innerTitle[this.vbox_help];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 3;
			boxChild7.Expand = false;
			this.alignment_title.Add(this.hbox_innerTitle);
			this.vbox_main.Add(this.alignment_title);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.alignment_title];
			boxChild8.Position = 0;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.evtbx_contentBorder = new EventBox();
			this.evtbx_contentBorder.Name = "evtbx_contentBorder";
			this.alignment_contentBorder = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_contentBorder.Name = "alignment_contentBorder";
			this.alignment_contentBorder.BorderWidth = 1U;
			this.evtbx_contentBg = new EventBox();
			this.evtbx_contentBg.Name = "evtbx_contentBg";
			this.alignment_content = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_content.Name = "alignment_content";
			this.evtbx_contentBg.Add(this.alignment_content);
			this.alignment_contentBorder.Add(this.evtbx_contentBg);
			this.evtbx_contentBorder.Add(this.alignment_contentBorder);
			this.vbox_main.Add(this.evtbx_contentBorder);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_main[this.evtbx_contentBorder];
			boxChild9.Position = 1;
			this.alignment_bottomBtn = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bottomBtn.Name = "alignment_bottomBtn";
			this.alignment_bottomBtn.TopPadding = 10U;
			this.alignment_bottomBtn.BottomPadding = 10U;
			this.hbox_bottomBtn = new HBox();
			this.hbox_bottomBtn.Name = "hbox_bottomBtn";
			this.hbox_bottomBtn.Spacing = 10;
			this.alignment_next = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_next.Name = "alignment_next";
			this.button_next = new Button();
			this.button_next.WidthRequest = 70;
			this.button_next.HeightRequest = 26;
			this.button_next.CanFocus = true;
			this.button_next.Name = "button_next";
			this.button_next.UseUnderline = true;
			this.button_next.Label = Catalog.GetString("下一步");
			this.alignment_next.Add(this.button_next);
			this.hbox_bottomBtn.Add(this.alignment_next);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_bottomBtn[this.alignment_next];
			boxChild10.PackType = PackType.End;
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.alignment_previous = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_previous.Name = "alignment_previous";
			this.button_previous = new Button();
			this.button_previous.WidthRequest = 70;
			this.button_previous.HeightRequest = 26;
			this.button_previous.CanFocus = true;
			this.button_previous.Name = "button_previous";
			this.button_previous.UseUnderline = true;
			this.button_previous.Label = Catalog.GetString("上一步");
			this.alignment_previous.Add(this.button_previous);
			this.hbox_bottomBtn.Add(this.alignment_previous);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.hbox_bottomBtn[this.alignment_previous];
			boxChild11.PackType = PackType.End;
			boxChild11.Position = 1;
			boxChild11.Expand = false;
			this.alignment_bottomBtn.Add(this.hbox_bottomBtn);
			this.vbox_main.Add(this.alignment_bottomBtn);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_main[this.alignment_bottomBtn];
			boxChild12.PackType = PackType.End;
			boxChild12.Position = 2;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.alignment_base.Add(this.vbox_main);
			this.evtbx_bg.Add(this.alignment_base);
			this.vbox_window.Add(this.evtbx_bg);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.vbox_window[this.evtbx_bg];
			boxChild13.Position = 1;
			this.evtbx_windowBorder.Add(this.vbox_window);
			base.Add(this.evtbx_windowBorder);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 636;
			base.DefaultHeight = 500;
			base.Hide();
			base.KeyPressEvent += this.OnKeyPressed;
			this.button_previous.Clicked += this.OnPreviousBtnClicked;
			this.button_next.Clicked += this.OnNextBtnClicked;
		}

		private ImageView pageTagImage1;

		private ImageView pageTagImage2;

		private SelectTemplateWidget selectCardWidget;

		private PropertiesWidget curSlnSetWidget;

		private int currentPage;

		private GeneralLauncherButton launcherNextBtn;

		private GeneralLauncherButton launcherPreviousBtn;

		private EventBox evtbx_windowBorder;

		private VBox vbox_window;

		private CustomTitleBar customTitleBar;

		private EventBox evtbx_bg;

		private Alignment alignment_base;

		private VBox vbox_main;

		private Alignment alignment_title;

		private HBox hbox_innerTitle;

		private Alignment alignment_titleIcon;

		private Gtk.Image image2;

		private Label label_title;

		private VBox vbox_help;

		private Alignment alignment_helpTop;

		private Alignment alignment_help;

		private Alignment alignment_helpBottom;

		private EventBox evtbx_contentBorder;

		private Alignment alignment_contentBorder;

		private EventBox evtbx_contentBg;

		private Alignment alignment_content;

		private Alignment alignment_bottomBtn;

		private HBox hbox_bottomBtn;

		private Alignment alignment_next;

		private Button button_next;

		private Alignment alignment_previous;

		private Button button_previous;
	}
}

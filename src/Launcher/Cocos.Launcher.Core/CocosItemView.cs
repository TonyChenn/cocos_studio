using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Cocos.Launcher.Control;
using CocoStudio.Core;
using CocoStudio.UserStatistics;
using Gtk;
using Modules.Communal.CocosCodeIDE;
using Modules.Communal.MultiLanguage;
using Modules.Communal.NewSolution;
using Mono.Unix;
using MonoDevelop.Components;
using Stetic;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class CocosItemView : Bin
	{
		public CocosItemView()
		{
			this.Build();
			this.RecentCocosItem = new ObservableCollection<CocosItemModel>(Services.RecentFileService.CocosItemRecordList);
			this.Initialize();
			base.ShowAll();
		}

		private void Initialize()
		{
			this.InitDefault();
			this.Initlanguage();
			this.CreateRecentCocosItemLabels();
			this.InitEvent();
		}

		private void InitDefault()
		{
			this.eventbox1.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLineColor);
			this.button_newCocosItem = new ButtonView();
			this.button_newCocosItem.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.button_normal.png");
			this.button_newCocosItem.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.button_move.png");
			this.button_newCocosItem.SetPressBack("Cocos.Launcher.Resource.LauncherResource.button_press.png");
			this.button_newCocosItem.SetLableNormalColor(ConstantConfig.Colors.MainContentColor);
			this.button_newCocosItem.SetLableFontSize(14.0);
			this.button_newCocosItem.SetSize(100, 26);
			this.button_openCocosItem = new ButtonView();
			this.button_openCocosItem.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.button_normal.png");
			this.button_openCocosItem.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.button_move.png");
			this.button_openCocosItem.SetPressBack("Cocos.Launcher.Resource.LauncherResource.button_press.png");
			this.button_openCocosItem.SetLableNormalColor(ConstantConfig.Colors.MainContentColor);
			this.button_openCocosItem.SetLableFontSize(14.0);
			this.button_openCocosItem.SetSize(100, 26);
			this.hbox_project.PackStart(this.button_newCocosItem, false, false, 0U);
			this.hbox_project.PackStart(this.button_openCocosItem, false, false, 0U);
			this.label_recentlyProject.SetFontSize(16.0);
			this.label_recentlyProject.ModifyFg(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
			this.image_recentlyProject.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.dir.png"));
		}

		private void InitEvent()
		{
			Services.RecentFileService.RecentDocumentChanged += this.Instance_RecentProjectChanged;
			this.button_newCocosItem.ButtonReleaseEvent += this.button_newProject_ButtonReleaseEvent;
			this.button_openCocosItem.ButtonReleaseEvent += this.button_openProject_ButtonReleaseEvent;
			Services.MainWindow.ButtonReleaseEvent += this.MainWindow_ButtonReleaseEvent;
		}

		private void CreateRecentCocosItemLabels()
		{
			int num = (this.RecentCocosItem.Count > 7) ? 7 : this.RecentCocosItem.Count;
			for (int i = 0; i < num; i++)
			{
				RecentlyCocosItemView recentlyCocosItemView = new RecentlyCocosItemView(this.RecentCocosItem[i]);
				recentlyCocosItemView.OpenClicked += this.prjLabel_OpenClicked;
				recentlyCocosItemView.ButtonPressEvent += this.prjLabel_ButtonPressEvent;
				recentlyCocosItemView.TooltipText = this.RecentCocosItem[i].LocalPath;
				this.vbox_projectList.PackStart(recentlyCocosItemView, false, false, 0U);
			}
		}

		private void Initlanguage()
		{
			this.button_openCocosItem.SetLabelText(LanguageInfo.Menu_File_OpenProject);
			this.button_newCocosItem.SetLabelText(LanguageInfo.Dialog_NewProject);
			this.label_recentlyProject.Text = LanguageInfo.Launcher_OpenRecent;
		}

		private void StartProcedure(string cocosItemPath)
		{
			string cocosStudioExePath = ConstantConfig.Paths.CocosStudioExePath;
			if (File.Exists(cocosStudioExePath))
			{
				Process.Start(new ProcessStartInfo(cocosStudioExePath)
				{
					WorkingDirectory = System.IO.Path.GetDirectoryName(cocosStudioExePath),
					Arguments = this.GetArguments(cocosItemPath)
				});
			}
		}

		private string GetArguments(string cocosItemPath)
		{
			return string.Format("\"{0}\"", cocosItemPath);
		}

		private void Instance_RecentProjectChanged(object sender, RecentDocumentChangeEventArgs args)
		{
			if (args.ChangeType == EnumRecentPrjChangeType.New || args.ChangeType == EnumRecentPrjChangeType.Reorder)
			{
				this.CreateRecentCocosItemLabels();
				return;
			}
			if (args.ChangeType == EnumRecentPrjChangeType.Remove)
			{
				CocosItemModel cocosItemModel = args.CocosItemModel;
				foreach (RecentlyCocosItemView recentlyCocosItemView in this.vbox_projectList.Children)
				{
					if (recentlyCocosItemView.Tag == cocosItemModel)
					{
						this.vbox_projectList.Remove(recentlyCocosItemView);
						recentlyCocosItemView.OpenClicked -= this.prjLabel_OpenClicked;
						recentlyCocosItemView.ButtonPressEvent -= this.prjLabel_ButtonPressEvent;
						recentlyCocosItemView.Dispose();
						return;
					}
				}
			}
		}

		private void button_openProject_ButtonReleaseEvent(object sender, ButtonReleaseEventArgs args)
		{
			string lastBrowserLocation = Services.RecentFileService.LastBrowserLocation;
			SelectFileDialog selectFileDialog = new SelectFileDialog();
			selectFileDialog.TransientFor = Services.MainWindow;
			selectFileDialog.Title = LanguageInfo.Menu_File_OpenProject;
			selectFileDialog.Action = FileChooserAction.Open;
			selectFileDialog.SelectMultiple = false;
			selectFileDialog.CurrentFolder = lastBrowserLocation;
			selectFileDialog.AddFilter("Solution Files", new string[]
			{
				"*.ccs"
			});
			if (selectFileDialog.Run())
			{
				string text = selectFileDialog.SelectedFile;
				string featureName = "OpenProject";
				if (RegexModel.HasChinese(text))
				{
					MessageBox.Show(LanguageInfo.Launcher_ContainChs, MessageBoxImage.Info, Services.MainWindow, null);
					featureName = "OpenProjectFailed";
				}
				else
				{
					this.StartProcedure(text);
				}
				Services.RecentFileService.LastBrowserLocation = System.IO.Path.GetDirectoryName(text);
				Tracker.Add(ViewRegions.None, featureName, "", "");
			}
		}

		private void button_newProject_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			NewSolutionWindow newSolutionWindow = new NewSolutionWindow();
			newSolutionWindow.SolutionCreated += this.OnNewSolutionCreated;
			newSolutionWindow.Show();
		}

		private void OnNewSolutionCreated(object sender, SolutionCreatedArgs args)
		{
			if (File.Exists(args.DefaultScenePath))
			{
				this.StartProcedure(args.DefaultScenePath);
			}
		}

		private void prjLabel_OpenClicked(object sender, OpenClickedEventArgs e)
		{
			CocosItemModel cocosItemModel = e.Tag as CocosItemModel;
			if (e.StartItem == StartEnum.CocosStudio)
			{
				if (CocosItemLoadHelp.Instance.CheckCocosItem(cocosItemModel))
				{
					this.StartProcedure(cocosItemModel.LocalPath);
					Tracker.Add(ViewRegions.None, "OpenProject", "", "");
					return;
				}
			}
			else
			{
				string directoryName = System.IO.Path.GetDirectoryName(cocosItemModel.LocalPath);
				string text = CocosCodeIDEService.Instance.OpenCocosItemWithCocosCodeIDE(directoryName, false);
				if (!string.IsNullOrWhiteSpace(text))
				{
					MessageBox.Show(text, MessageBoxImage.Info, Services.MainWindow, null);
				}
			}
		}

		private void prjLabel_ButtonPressEvent(object sender, ButtonPressEventArgs args)
		{
			if (this.oldSelectedCocosItemView != null)
			{
				this.oldSelectedCocosItemView.IsSelected = false;
			}
			RecentlyCocosItemView recentlyCocosItemView = sender as RecentlyCocosItemView;
			recentlyCocosItemView.IsSelected = true;
			this.oldSelectedCocosItemView = recentlyCocosItemView;
		}

		private void MainWindow_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (this.oldSelectedCocosItemView != null)
			{
				this.oldSelectedCocosItemView.IsSelected = false;
				this.oldSelectedCocosItemView = null;
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Cocos.Launcher.Core.ProjectView";
			this.alignment_view = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_view.Name = "alignment_view";
			this.alignment_view.BorderWidth = 40U;
			this.vbox_project = new VBox();
			this.vbox_project.WidthRequest = 257;
			this.vbox_project.HeightRequest = 390;
			this.vbox_project.Name = "vbox_project";
			this.vbox_project.Spacing = 14;
			this.hbox3 = new HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 10;
			this.image_recentlyProject = new ImageBin();
			this.image_recentlyProject.Name = "image_recentlyProject";
			this.hbox3.Add(this.image_recentlyProject);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox3[this.image_recentlyProject];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.label_recentlyProject = new Label();
			this.label_recentlyProject.Name = "label_recentlyProject";
			this.label_recentlyProject.Xalign = 0f;
			this.label_recentlyProject.LabelProp = Catalog.GetString("最近打开的项目");
			this.hbox3.Add(this.label_recentlyProject);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox3[this.label_recentlyProject];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.hbox_project = new HBox();
			this.hbox_project.Name = "hbox_project";
			this.hbox_project.Spacing = 12;
			this.hbox3.Add(this.hbox_project);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox3[this.hbox_project];
			boxChild3.PackType = PackType.End;
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment2.Name = "alignment2";
			this.hbox3.Add(this.alignment2);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox3[this.alignment2];
			boxChild4.PackType = PackType.End;
			boxChild4.Position = 3;
			this.vbox_project.Add(this.hbox3);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_project[this.hbox3];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.alignment_projectList = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_projectList.Name = "alignment_projectList";
			this.alignment_projectList.LeftPadding = 26U;
			this.vbox_4 = new VBox();
			this.vbox_4.Name = "vbox_4";
			this.eventbox1 = new EventBox();
			this.eventbox1.HeightRequest = 1;
			this.eventbox1.Name = "eventbox1";
			this.vbox_4.Add(this.eventbox1);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_4[this.eventbox1];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.vbox_projectList = new VBox();
			this.vbox_projectList.Name = "vbox_projectList";
			this.vbox_4.Add(this.vbox_projectList);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_4[this.vbox_projectList];
			boxChild7.Position = 1;
			this.alignment_projectList.Add(this.vbox_4);
			this.vbox_project.Add(this.alignment_projectList);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_project[this.alignment_projectList];
			boxChild8.Position = 1;
			this.alignment_view.Add(this.vbox_project);
			base.Add(this.alignment_view);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private ObservableCollection<CocosItemModel> RecentCocosItem;

		private ButtonView button_openCocosItem;

		private ButtonView button_newCocosItem;

		private RecentlyCocosItemView oldSelectedCocosItemView;

		private Alignment alignment_view;

		private VBox vbox_project;

		private HBox hbox3;

		private ImageBin image_recentlyProject;

		private Label label_recentlyProject;

		private HBox hbox_project;

		private Alignment alignment2;

		private Alignment alignment_projectList;

		private VBox vbox_4;

		private EventBox eventbox1;

		private VBox vbox_projectList;
	}
}

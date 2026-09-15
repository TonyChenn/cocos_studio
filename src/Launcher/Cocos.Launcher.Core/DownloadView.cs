using System;
using System.ComponentModel;
using Cocos.Launcher.Control;
using Gtk;
using Modules.Communal.MultiLanguage;
using Stetic;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class DownloadView : EventBox, IDownloadView
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.WidthRequest = 960;
			base.HeightRequest = 620;
			base.Name = "Cocos.Launcher.Core.DownloadView";
			this.eventbox1 = new EventBox();
			this.eventbox1.Name = "eventbox1";
			this.vbox_all = new VBox();
			this.vbox_all.Name = "vbox_all";
			this.vbox_all.Spacing = 6;
			this.alignment_button = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_button.Name = "alignment_button";
			this.alignment_button.LeftPadding = 30U;
			this.alignment_button.TopPadding = 10U;
			this.alignment_button.RightPadding = 30U;
			this.alignment_button.BottomPadding = 10U;
			this.hbox_button = new HBox();
			this.hbox_button.Name = "hbox_button";
			this.hbox_button.Spacing = 6;
			this.alignment_button.Add(this.hbox_button);
			this.vbox_all.Add(this.alignment_button);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_all[this.alignment_button];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.fixed3 = new Fixed();
			this.fixed3.HeightRequest = 2;
			this.fixed3.Name = "fixed3";
			this.fixed3.HasWindow = false;
			this.eventbox_underline = new EventBox();
			this.eventbox_underline.WidthRequest = 900;
			this.eventbox_underline.HeightRequest = 1;
			this.eventbox_underline.Name = "eventbox_underline";
			this.fixed3.Add(this.eventbox_underline);
			Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.fixed3[this.eventbox_underline];
			fixedChild.X = 30;
			fixedChild.Y = 1;
			this.eventbox_line1 = new EventBox();
			this.eventbox_line1.WidthRequest = 75;
			this.eventbox_line1.HeightRequest = 2;
			this.eventbox_line1.Name = "eventbox_line1";
			this.fixed3.Add(this.eventbox_line1);
			Fixed.FixedChild fixedChild2 = (Fixed.FixedChild)this.fixed3[this.eventbox_line1];
			fixedChild2.X = 30;
			this.vbox_all.Add(this.fixed3);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_all[this.fixed3];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.alignment_notebook = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_notebook.Name = "alignment_notebook";
			this.alignment_notebook.LeftPadding = 30U;
			this.alignment_notebook.TopPadding = 30U;
			this.alignment_notebook.RightPadding = 2U;
			this.alignment_notebook.BottomPadding = 30U;
			this.vbox_all.Add(this.alignment_notebook);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_all[this.alignment_notebook];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.eventbox1.Add(this.vbox_all);
			base.Add(this.eventbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		public DownloadView()
		{
			this.Build();
			this.InitWidget();
			this.InitEvent();
			base.ShowAll();
		}

		private void InitWidget()
		{
			this.eventbox_underline.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLineColor);
			this.eventbox_line1.ModifyBg(StateType.Normal, ConstantConfig.Colors.TabFontPressColor);
			this.eventbox_line1.WidthRequest = 450;
			this.link_pageOne = new LinkView();
			this.link_pageOne.SetLableText(LanguageInfo.Launcher_Downloading + "(0)");
			this.link_pageOne.SetLabelAlign(0.5f, 1f);
			this.link_pageOne.SetFontSize(16.0);
			this.link_pageOne.SetForeGroundColor(ConstantConfig.Colors.ContentLabelColor1);
			this.hbox_button.Add(this.link_pageOne);
			this.link_pageTwo = new LinkView();
			this.link_pageTwo.SetLableText(LanguageInfo.Launcher_Downloaded + "(0)");
			this.link_pageTwo.SetLabelAlign(0.5f, 1f);
			this.link_pageTwo.SetFontSize(16.0);
			this.link_pageTwo.SetForeGroundColor(ConstantConfig.Colors.ContentLabelColor1);
			this.hbox_button.Add(this.link_pageTwo);
			this.DoingScrollWindow = DownloadService.Instance.AssetManager.DoingWidget;
			this.FinishScrollWindow = DownloadService.Instance.AssetManager.FinishWidget;
			this.DoingScrollWindow.SetTab(this.link_pageOne, null);
			this.FinishScrollWindow.SetTab(this.link_pageTwo);
			this.alignment_notebook.Add(this.DoingScrollWindow);
			this.alignment_notebook.TopPadding = 30U;
			this.vbox_all.Spacing = 0;
			this.isPageOne = true;
			this.MoveLineToLoading(this.isPageOne);
		}

		private void InitEvent()
		{
			this.link_pageOne.LinkClicked += this.Link_LinkClicked;
			this.link_pageTwo.LinkClicked += this.Link_LinkClicked;
			Services.MainWindow.Closing += this.MainWindow_Closing;
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			if (!DownloadService.Instance.AssetManager.HaveLoading())
			{
				DownloadService.Instance.AssetManager.SavePluginListInfo();
				return;
			}
			string dialog_ButtonOK = LanguageInfo.Dialog_ButtonOK;
			string dialog_ButtonCancel = LanguageInfo.Dialog_ButtonCancel;
			ButtonText btnText = new ButtonText(dialog_ButtonOK, dialog_ButtonCancel, false, false);
			MessageBoxResult messageBoxResult = MessageBox.Show(LanguageInfo.Launcher_ConfirmClose, btnText, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				DownloadService.Instance.AssetManager.SavePluginListInfo();
				return;
			}
			e.Cancel = true;
		}

		public void SwitchPageOne(bool pageOne)
		{
			if (this.isPageOne == pageOne)
			{
				return;
			}
			if (pageOne)
			{
				this.alignment_notebook.Remove(this.FinishScrollWindow);
				this.alignment_notebook.Add(this.DoingScrollWindow);
			}
			else
			{
				this.alignment_notebook.Remove(this.DoingScrollWindow);
				this.alignment_notebook.Add(this.FinishScrollWindow);
			}
			this.isPageOne = pageOne;
			this.MoveLineToLoading(pageOne);
			base.ShowAll();
		}

		private void MoveLineToLoading(bool first)
		{
			Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.fixed3[this.eventbox_line1];
			if (first)
			{
				fixedChild.X = 30;
				return;
			}
			fixedChild.X = 480;
		}

		public void Refresh()
		{
			Widget child = this.alignment_notebook.Child;
			this.alignment_notebook.Remove(child);
			this.alignment_notebook.Add(child);
			base.ShowAll();
		}

		private void Link_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			bool pageOne = sender as LinkView == this.link_pageOne;
			this.SwitchPageOne(pageOne);
		}

		private EventBox eventbox1;

		private VBox vbox_all;

		private Alignment alignment_button;

		private HBox hbox_button;

		private Fixed fixed3;

		private EventBox eventbox_underline;

		private EventBox eventbox_line1;

		private Alignment alignment_notebook;

		private DoingDownloadScrollWindow DoingScrollWindow;

		private FinishDownloadScrollWindow FinishScrollWindow;

		private LinkView link_pageOne;

		private LinkView link_pageTwo;

		private bool isPageOne = true;
	}
}

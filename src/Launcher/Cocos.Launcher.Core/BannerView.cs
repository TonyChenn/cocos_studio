using System;
using System.ComponentModel;
using System.Linq;
using Cocos.Launcher.Control;
using Cocos.Launcher.Core.View;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class BannerView : EventBox
	{
		public BannerView()
		{
			this.Initialize();
			base.ShowAll();
		}

		internal void InitDefault()
		{
			Services.TabGroupService.SelectedTabChanged += this.TabGroupService_SelectedTabChanged;
		}

		private void Initialize()
		{
			HBox hbox = new HBox();
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment.WidthRequest = 230;
			alignment.LeftPadding = 20U;
			HBox hbox2 = new HBox();
			Alignment widget = new Alignment(0.5f, 0.5f, 1f, 1f);
			ImageBin imageBin = new ImageBin();
			imageBin.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.logo.png"));
			imageBin.SetSizeRequest(134, 54);
			hbox2.PackStart(imageBin, false, false, 0U);
			hbox2.Add(widget);
			alignment.Add(hbox2);
			hbox.PackStart(alignment, false, false, 0U);
			AdvertView child = new AdvertView();
			hbox.PackStart(child, true, true, 0U);
			VBox vbox = new VBox();
			HBox hbox3 = new HBox();
			hbox3.WidthRequest = 230;
			VBox vbox2 = new VBox();
			vbox2.Spacing = 10;
			HBox hbox4 = new HBox();
			hbox4.Add(new Alignment(0.5f, 0.5f, 1f, 1f));
			hbox4.PackStart(new LoginView(), false, false, 0U);
			this.hBox_search = new HBox();
			this.hBox_search.Add(new Alignment(0.5f, 0.5f, 1f, 1f));
			this.searchEntry = new SearchEntry();
			vbox2.PackStart(hbox4, false, false, 0U);
			vbox2.PackStart(this.hBox_search, false, false, 0U);
			Alignment alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment2.WidthRequest = 20;
			hbox3.Add(vbox2);
			hbox3.PackStart(alignment2, false, false, 0U);
			vbox.PackStart(hbox3, true, false, 0U);
			hbox.PackStart(vbox, false, false, 0U);
			base.Add(hbox);
			base.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainTitleColor);
		}

		private void TabGroupService_SelectedTabChanged(object sender, EventArgs e)
		{
			switch (Services.TabGroupService.LastSelectedTabPage.Order)
			{
			case 1:
				if (!this.hBox_search.Children.Contains(this.searchEntry) && LanguageOption.CurrentLanguage == LanguageType.Chinese)
				{
					this.AddSearchEntry();
					return;
				}
				if (this.hBox_search.Children.Contains(this.searchEntry) && LanguageOption.CurrentLanguage != LanguageType.Chinese)
				{
					this.RemoveSearchEntry();
					return;
				}
				break;
			case 2:
				if (!this.hBox_search.Children.Contains(this.searchEntry))
				{
					this.AddSearchEntry();
					return;
				}
				break;
			default:
				if (this.hBox_search.Children.Contains(this.searchEntry))
				{
					this.RemoveSearchEntry();
				}
				break;
			}
		}

		private void RemoveSearchEntry()
		{
			this.hBox_search.Remove(this.searchEntry);
			this.hBox_search.ShowAll();
		}

		private void AddSearchEntry()
		{
			this.hBox_search.PackStart(this.searchEntry, false, false, 0U);
			this.hBox_search.ShowAll();
			this.searchEntry.CanDefault = true;
			this.searchEntry.GrabDefault();
		}

		private SearchEntry searchEntry;

		private HBox hBox_search;
	}
}

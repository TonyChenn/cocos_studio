using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AppKit;
using Cocos.Launcher.Control;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.CocoaChina;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class SearchEntry : HBox
	{
		public SearchEntry()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			this.InitView();
			this.InitEvent();
		}

		private void InitView()
		{
			base.WidthRequest = 190;
			ImageBin imageBin = new ImageBin();
			imageBin.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.border.png"));
			base.PackStart(imageBin, false, false, 0U);
			this.searchEntry = new Entry();
			this.searchEntry.CanFocus = true;
			this.searchEntry.Name = "GrayEntry";
			this.searchEntry.HeightRequest = 26;
			this.searchEntry.Xalign = 0.02f;
			this.searchEntry.ActivatesDefault = true;
			base.Add(this.searchEntry);
			this.closeBox = new EventBox();
			this.closeBox.ModifyBg(StateType.Normal, ConstantConfig.Colors.SearchBgColor);
			this.closeBox.WidthRequest = 20;
			this.closeButton = new ImageButtonView();
			this.closeButton.SetSizeRequest(12, 12);
			this.closeButton.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.close.png");
			this.closeButton.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.close_hover.png");
			this.closeBox.Add(this.closeButton);
			base.PackStart(this.closeBox, false, false, 0U);
			this.searchButton = new ImageButtonView();
			this.searchButton.SetSizeRequest(30, 26);
			this.searchButton.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.search.png");
			this.searchButton.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.search_hover.png");
			base.PackStart(this.searchButton, false, false, 0U);
			this.searchHotWords = new SearchHotWords();
			this.searchKeywords = new SearchKeywords();
			this.ChangeCloseButtonVisible();
		}

		private void InitEvent()
		{
			this.searchEntry.KeyPressEvent += this.searchEntry_KeyPressEvent;
			this.searchEntry.Changed += this.searchEntry_Changed;
			this.searchEntry.ButtonPressEvent += this.searchEntry_ButtonPressEvent;
			this.searchEntry.FocusInEvent += this.searchEntry_FocusInEvent;
			this.searchEntry.FocusOutEvent += this.searchEntry_FocusOutEvent;
			this.searchKeywords.Selection.Changed += this.Selection_Changed;
			this.searchHotWords.SelectedChanged += this.searchHotWords_SelectedChanged;
			this.searchButton.ButtonReleaseEvent += this.searchButton_ButtonReleaseEvent;
			this.closeButton.ButtonReleaseEvent += this.closeButton_ButtonReleaseEvent;
			Services.MainWindow.FocusOutEvent += this.MainWindow_FocusOutEvent;
		}

		private void HiddenSearchPopover()
		{
			this.closeBox.RemoveAll();
			this.closeBox.ShowAll();
			this.searchKeywords.Restore();
			if (this.searchPopover != null)
			{
				this.searchPopover.Visible = false;
			}
		}

		private void SetSearchEntryValue(string value)
		{
			this.isInputs = false;
			this.searchEntry.Text = value;
			this.searchEntry.Position = this.searchEntry.Text.Length;
			this.isInputs = true;
		}

		private void ChangeCloseButtonVisible()
		{
			if (string.IsNullOrEmpty(this.searchEntry.Text) && this.closeBox.Children.Contains(this.closeButton))
			{
				this.closeBox.Remove(this.closeButton);
			}
			else if (!string.IsNullOrEmpty(this.searchEntry.Text) && !this.closeBox.Children.Contains(this.closeButton))
			{
				this.closeBox.Add(this.closeButton);
			}
			this.closeBox.ShowAll();
		}

		private void SetTreeModel()
		{
			this.searchPopover.SetContent(this.searchKeywords);
			this.SetAssociatedKeywords(this.searchEntry.Text);
		}

		private void SetKeywordsModel(List<string> list)
		{
			if (list.Count < 1 && this.searchPopover.Visible)
			{
				this.searchPopover.Visible = false;
			}
			else if (list.Count > 0 && !this.searchPopover.Visible)
			{
				this.searchPopover.ShowPopover();
			}
			this.searchKeywords.Models = list;
		}

		private void StartSearch()
		{
			if (string.IsNullOrEmpty(this.searchEntry.Text) || !CocoStudio.Core.Services.NetworkService.IsOK)
			{
				return;
			}
			string filteredSearchValue = this.GetFilteredSearchValue();
			string arg = ((DataType)Services.TabGroupService.LastSelectedTabPage.Order).ToString().ToLower();
			string text = string.Format("{0}&goal={1}&text={2}", ConstantConfig.Constant.SearchUrl, arg, filteredSearchValue);
			Services.TabGroupService.LastSelectedTabPage.TabContent.Search(text);
		}

		protected override void OnActivate()
		{
			if (this.searchEntry.HasFocus)
			{
				this.StartSearch();
			}
		}

		private void SetAssociatedKeywords(string entryText)
		{
			HttpSync httpSync = new HttpSync();
			httpSync.OnResived += this.KeywordsResived;
			string data = "word=" + entryText;
			httpSync.GetSyncResponseOfString(ConstantConfig.Constant.SearchKeywordsUrl, "post", data, null);
		}

		private bool IsCheck(string text)
		{
			string pattern = "^[\\w\\s-+.]+$";
			return Regex.IsMatch(text, pattern);
		}

		private string GetFilteredSearchValue()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char value in this.searchEntry.Text)
			{
				if (this.IsCheck(value.ToString()))
				{
					stringBuilder.Append(value);
				}
				if (stringBuilder.Length >= 100)
				{
					break;
				}
			}
			return stringBuilder.ToString().GetCocoaUrlEncode();
		}

		private void KeywordsResived(object sender, HttpSync.HttpSyncArgs e)
		{
			try
			{
				HttpSync httpSync = sender as HttpSync;
				if (httpSync != null)
				{
					httpSync.OnResived -= this.KeywordsResived;
				}
				string message = e.Message;
				List<string> list = new List<string>();
				if (!string.IsNullOrWhiteSpace(message) && message.Contains("res"))
				{
					bool flag = bool.Parse(Login.GetJsonAnalysis(message, "res"));
					if (flag)
					{
						list = Login.GetJsonAnalysis(message, "data", "word");
					}
					else
					{
						LogConfig.Logger.Error("关键词列表获取失败!" + message);
					}
				}
				GLib.Timeout.Add(0U, delegate
				{
					this.SetKeywordsModel(list);
					return false;
				});
			}
			catch (Exception arg)
			{
				LogConfig.Logger.Error("关键词列表获取失败：" + arg);
			}
		}

		private void searchEntry_Changed(object sender, EventArgs e)
		{
			this.ChangeCloseButtonVisible();
			if (!this.isInputs || !CocoStudio.Core.Services.NetworkService.IsOK)
			{
				return;
			}
			string text = this.searchEntry.Text;
			if (string.IsNullOrEmpty(text))
			{
				if (!this.searchPopover.Visible)
				{
					this.searchPopover.ShowPopover();
				}
				this.searchHotWords.Restore();
				this.searchPopover.SetContent(this.searchHotWords);
				this.searchHotWords.ShowAll();
				return;
			}
			this.SetTreeModel();
		}

		private void searchEntry_FocusInEvent(object o, FocusInEventArgs args)
		{
			this.ChangeCloseButtonVisible();
			if (!CocoStudio.Core.Services.NetworkService.IsOK)
			{
				return;
			}
			if (this.searchPopover == null)
			{
				this.searchPopover = new SearchPopover(this);
				this.searchPopover.SetContent(this.searchHotWords);
				this.searchHotWords.ShowAll();
			}
			if (!string.IsNullOrEmpty(this.searchEntry.Text))
			{
				this.SetTreeModel();
				return;
			}
			this.searchPopover.ShowPopover();
		}

		private void searchEntry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.HiddenSearchPopover();
		}

		[ConnectBefore]
		private void searchEntry_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			try
			{
				if (!this.searchEntry.HasFocus)
				{
					if (Platform.IsMac)
					{
						IntPtr nsviewHandle = NativeGdkMac.GetNSViewHandle(Services.MainWindow.GdkWindow.Handle);
						NSView nsview = new NSView();
						nsview.Handle = nsviewHandle;
						if (nsview != nsview.Window.FirstResponder)
						{
							nsview.Window.MakeFirstResponder(nsview);
						}
						nsview.Handle = IntPtr.Zero;
					}
					Services.MainWindow.GrabFocus();
					this.searchEntry.GrabFocus();
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("搜索框夺取焦点失败:", exception);
			}
		}

		private void searchEntry_KeyPressEvent(object o, KeyPressEventArgs args)
		{
			args.RetVal = true;
			if (this.searchPopover == null || !this.searchPopover.Visible || string.IsNullOrEmpty(this.searchEntry.Text))
			{
				return;
			}
			switch (args.Event.Key)
			{
			case Gdk.Key.Up:
				this.isKeyDown = true;
				this.searchKeywords.GoBackItem();
				break;
			case Gdk.Key.Down:
				this.isKeyDown = true;
				this.searchKeywords.GoNextItem();
				break;
			}
			this.isKeyDown = false;
		}

		private void Selection_Changed(object sender, EventArgs e)
		{
			object selectedItem = this.searchKeywords.GetSelectedItem();
			if (selectedItem != null)
			{
				this.SetSearchEntryValue(selectedItem.ToString());
				if (!this.isKeyDown)
				{
					this.StartSearch();
				}
			}
		}

		private void searchHotWords_SelectedChanged(object sender, EventArgs e)
		{
			if (this.searchHotWords.ActivatedItem != null)
			{
				this.SetSearchEntryValue(this.searchHotWords.ActivatedItem.Text);
				this.StartSearch();
			}
		}

		private void searchButton_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.HiddenSearchPopover();
			this.StartSearch();
		}

		private void MainWindow_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.HiddenSearchPopover();
		}

		private void closeButton_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.searchEntry.Text = string.Empty;
		}

		private EventBox closeBox;

		private Entry searchEntry;

		private ImageButtonView searchButton;

		private ImageButtonView closeButton;

		private SearchPopover searchPopover;

		private SearchHotWords searchHotWords;

		private SearchKeywords searchKeywords;

		private bool isInputs = true;

		private bool isKeyDown;
	}
}

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
	// Token: 0x0200005A RID: 90
	[ToolboxItem(true)]
	public class SearchEntry : HBox
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0000C229 File Offset: 0x0000A429
		public SearchEntry()
		{
			this.Initialize();
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000C23E File Offset: 0x0000A43E
		private void Initialize()
		{
			this.InitView();
			this.InitEvent();
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000C24C File Offset: 0x0000A44C
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

		// Token: 0x0600030F RID: 783 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
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

		// Token: 0x06000310 RID: 784 RVA: 0x0000C4C7 File Offset: 0x0000A6C7
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

		// Token: 0x06000311 RID: 785 RVA: 0x0000C4FE File Offset: 0x0000A6FE
		private void SetSearchEntryValue(string value)
		{
			this.isInputs = false;
			this.searchEntry.Text = value;
			this.searchEntry.Position = this.searchEntry.Text.Length;
			this.isInputs = true;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000C538 File Offset: 0x0000A738
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

		// Token: 0x06000313 RID: 787 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		private void SetTreeModel()
		{
			this.searchPopover.SetContent(this.searchKeywords);
			this.SetAssociatedKeywords(this.searchEntry.Text);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000C5EC File Offset: 0x0000A7EC
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

		// Token: 0x06000315 RID: 789 RVA: 0x0000C64C File Offset: 0x0000A84C
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

		// Token: 0x06000316 RID: 790 RVA: 0x0000C6CA File Offset: 0x0000A8CA
		protected override void OnActivate()
		{
			if (this.searchEntry.HasFocus)
			{
				this.StartSearch();
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000C6E0 File Offset: 0x0000A8E0
		private void SetAssociatedKeywords(string entryText)
		{
			HttpSync httpSync = new HttpSync();
			httpSync.OnResived += this.KeywordsResived;
			string data = "word=" + entryText;
			httpSync.GetSyncResponseOfString(ConstantConfig.Constant.SearchKeywordsUrl, "post", data, null);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000C730 File Offset: 0x0000A930
		private bool IsCheck(string text)
		{
			string pattern = "^[\\w\\s-+.]+$";
			return Regex.IsMatch(text, pattern);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000C74C File Offset: 0x0000A94C
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

		// Token: 0x0600031A RID: 794 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
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

		// Token: 0x0600031B RID: 795 RVA: 0x0000C8B4 File Offset: 0x0000AAB4
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

		// Token: 0x0600031C RID: 796 RVA: 0x0000C938 File Offset: 0x0000AB38
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

		// Token: 0x0600031D RID: 797 RVA: 0x0000C9AC File Offset: 0x0000ABAC
		private void searchEntry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.HiddenSearchPopover();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000C9B4 File Offset: 0x0000ABB4
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

		// Token: 0x0600031F RID: 799 RVA: 0x0000CA58 File Offset: 0x0000AC58
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

		// Token: 0x06000320 RID: 800 RVA: 0x0000CAEC File Offset: 0x0000ACEC
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

		// Token: 0x06000321 RID: 801 RVA: 0x0000CB22 File Offset: 0x0000AD22
		private void searchHotWords_SelectedChanged(object sender, EventArgs e)
		{
			if (this.searchHotWords.ActivatedItem != null)
			{
				this.SetSearchEntryValue(this.searchHotWords.ActivatedItem.Text);
				this.StartSearch();
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000CB4D File Offset: 0x0000AD4D
		private void searchButton_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.HiddenSearchPopover();
			this.StartSearch();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000CB5B File Offset: 0x0000AD5B
		private void MainWindow_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.HiddenSearchPopover();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000CB63 File Offset: 0x0000AD63
		private void closeButton_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.searchEntry.Text = string.Empty;
		}

		// Token: 0x0400011C RID: 284
		private EventBox closeBox;

		// Token: 0x0400011D RID: 285
		private Entry searchEntry;

		// Token: 0x0400011E RID: 286
		private ImageButtonView searchButton;

		// Token: 0x0400011F RID: 287
		private ImageButtonView closeButton;

		// Token: 0x04000120 RID: 288
		private SearchPopover searchPopover;

		// Token: 0x04000121 RID: 289
		private SearchHotWords searchHotWords;

		// Token: 0x04000122 RID: 290
		private SearchKeywords searchKeywords;

		// Token: 0x04000123 RID: 291
		private bool isInputs = true;

		// Token: 0x04000124 RID: 292
		private bool isKeyDown;
	}
}

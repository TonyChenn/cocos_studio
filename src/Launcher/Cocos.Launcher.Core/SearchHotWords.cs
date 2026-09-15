using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cocos.Launcher.Control;
using CocoStudio.Basic;
using CocoStudio.Core;
using GLib;
using Gtk;
using Modules.Communal.CocoaChina;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class SearchHotWords : VBox
	{
		public event EventHandler<EventArgs> SelectedChanged;

		public HotWordsBox ActivatedItem { get; set; }

		public SearchHotWords()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.GetHotWords();
				return;
			}
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.GetHotWords();
				CocoStudio.Core.Services.NetworkService.NetworkChanged -= this.NetworkService_NetworkChanged;
			}
		}

		private void GetHotWords()
		{
			HttpSync httpSync = new HttpSync();
			httpSync.OnResived += this.HotWordsInfoResived;
			httpSync.GetSyncResponseOfString(ConstantConfig.Constant.SearchHotWordsUrl, "get", "", null);
		}

		private void HotWordsInfoResived(object sender, HttpSync.HttpSyncArgs e)
		{
			try
			{
				HttpSync httpSync = sender as HttpSync;
				if (httpSync != null)
				{
					httpSync.OnResived -= this.HotWordsInfoResived;
				}
				string message = e.Message;
				if (!string.IsNullOrWhiteSpace(message) && message.Contains("res"))
				{
					bool flag = bool.Parse(Login.GetJsonAnalysis(message, "res"));
					if (flag)
					{
						List<string> list = Login.GetJsonAnalysis(message, "data", "word");
						GLib.Timeout.Add(0U, delegate
						{
							this.SetContent(list);
							return false;
						});
					}
					else
					{
						LogConfig.Logger.Error("热词获取失败!" + message);
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("热词获取失败：", exception);
			}
		}

		private void SetContent(List<string> list)
		{
			if (list == null || list.Count <= 0)
			{
				return;
			}
			base.Spacing = 5;
			base.BorderWidth = 6U;
			Label label = new Label();
			label.Xalign = 0f;
			label.Text = LanguageInfo.Launcher_HotSearch;
			label.ModifyFg(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
			base.Add(label);
			for (int i = 0; i < list.Count; i++)
			{
				HBox hbox = new HBox();
				hbox.Spacing = 5;
				base.Add(hbox);
				HotWordsBox hotWordsBox = new HotWordsBox(list[i]);
				hotWordsBox.ButtonReleaseEvent += this.hotWordsBox_ButtonReleaseEvent;
				hotWordsBox.EnterNotifyEvent += this.hotWordsBox_EnterNotifyEvent;
				if (i + 1 < list.Count)
				{
					int length = this.GetLength(list[i]);
					int length2 = this.GetLength(list[i + 1]);
					if (length + length2 > 10)
					{
						hbox.Add(hotWordsBox);
					}
					else
					{
						HotWordsBox hotWordsBox2 = new HotWordsBox(list[i + 1]);
						hotWordsBox2.ButtonReleaseEvent += this.hotWordsBox_ButtonReleaseEvent;
						hotWordsBox2.EnterNotifyEvent += this.hotWordsBox_EnterNotifyEvent;
						if (length > length2)
						{
							hbox.Add(hotWordsBox);
							hbox.PackStart(hotWordsBox2, false, false, 0U);
						}
						else if (length == length2)
						{
							hbox.Add(hotWordsBox);
							hbox.Add(hotWordsBox2);
						}
						else
						{
							hbox.PackStart(hotWordsBox, false, false, 0U);
							hbox.Add(hotWordsBox2);
						}
						i++;
					}
				}
				else
				{
					hbox.Add(hotWordsBox);
				}
			}
			base.ShowAll();
		}

		private int GetLength(string text)
		{
			int result = text.Length;
			if (!RegexModel.HasChinese(text))
			{
				result = text.Length / 2 + text.Length % 2;
			}
			return result;
		}

		internal void Restore()
		{
			if (this.ActivatedItem != null)
			{
				this.ActivatedItem.SetNormalStyle();
			}
		}

		private void hotWordsBox_EnterNotifyEvent(object sender, EnterNotifyEventArgs args)
		{
			this.ActivatedItem = (sender as HotWordsBox);
		}

		private void hotWordsBox_ButtonReleaseEvent(object sender, ButtonReleaseEventArgs args)
		{
			if (this.SelectedChanged != null)
			{
				this.SelectedChanged(sender, args);
			}
		}
	}
}

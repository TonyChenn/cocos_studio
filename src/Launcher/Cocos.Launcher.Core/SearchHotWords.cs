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
	// Token: 0x02000058 RID: 88
	[ToolboxItem(true)]
	public class SearchHotWords : VBox
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060002F9 RID: 761 RVA: 0x0000BD38 File Offset: 0x00009F38
		// (remove) Token: 0x060002FA RID: 762 RVA: 0x0000BD70 File Offset: 0x00009F70
		public event EventHandler<EventArgs> SelectedChanged;

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000BDA5 File Offset: 0x00009FA5
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0000BDAD File Offset: 0x00009FAD
		public HotWordsBox ActivatedItem { get; set; }

		// Token: 0x060002FD RID: 765 RVA: 0x0000BDB6 File Offset: 0x00009FB6
		public SearchHotWords()
		{
			this.Initialize();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000BDC4 File Offset: 0x00009FC4
		private void Initialize()
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.GetHotWords();
				return;
			}
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000BDEF File Offset: 0x00009FEF
		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.GetHotWords();
				CocoStudio.Core.Services.NetworkService.NetworkChanged -= this.NetworkService_NetworkChanged;
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000BE1C File Offset: 0x0000A01C
		private void GetHotWords()
		{
			HttpSync httpSync = new HttpSync();
			httpSync.OnResived += this.HotWordsInfoResived;
			httpSync.GetSyncResponseOfString(ConstantConfig.Constant.SearchHotWordsUrl, "get", "", null);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000BE80 File Offset: 0x0000A080
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

		// Token: 0x06000302 RID: 770 RVA: 0x0000BF50 File Offset: 0x0000A150
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

		// Token: 0x06000303 RID: 771 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		private int GetLength(string text)
		{
			int result = text.Length;
			if (!RegexModel.HasChinese(text))
			{
				result = text.Length / 2 + text.Length % 2;
			}
			return result;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000C107 File Offset: 0x0000A307
		internal void Restore()
		{
			if (this.ActivatedItem != null)
			{
				this.ActivatedItem.SetNormalStyle();
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000C11C File Offset: 0x0000A31C
		private void hotWordsBox_EnterNotifyEvent(object sender, EnterNotifyEventArgs args)
		{
			this.ActivatedItem = (sender as HotWordsBox);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000C12A File Offset: 0x0000A32A
		private void hotWordsBox_ButtonReleaseEvent(object sender, ButtonReleaseEventArgs args)
		{
			if (this.SelectedChanged != null)
			{
				this.SelectedChanged(sender, args);
			}
		}
	}
}

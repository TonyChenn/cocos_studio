using System;
using System.Collections.Generic;
using System.Linq;
using Cocos.Launcher.Control;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200002D RID: 45
	public class FinishDownloadScrollWindow : ScrolledWindow
	{
		// Token: 0x06000188 RID: 392 RVA: 0x000084E8 File Offset: 0x000066E8
		public FinishDownloadScrollWindow(List<Widget> list)
		{
			this.InitWidget();
			this.InitValue(list);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008504 File Offset: 0x00006704
		private void InitValue(List<Widget> list)
		{
			this.finishList = list;
			this.finishList.Reverse();
			foreach (Widget widget in this.finishList)
			{
				widget.Destroyed += this.item_Destroyed;
			}
			this.UpdateAll();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000857C File Offset: 0x0000677C
		private void InitWidget()
		{
			base.Name = "GtkScrolledWindow";
			base.HeightRequest = 500;
			base.ShadowType = ShadowType.None;
			Viewport viewport = new Viewport();
			viewport.ShadowType = ShadowType.None;
			this.table = new Table(1U, this.columnNum, false);
			this.table.RowSpacing = 30U;
			this.table.ColumnSpacing = 75U;
			this.table.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainContentColor);
			viewport.Add(this.table);
			base.Add(viewport);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000860C File Offset: 0x0000680C
		public void AddAllToTable(List<Widget> list)
		{
			if (list == null || list.Count == 0)
			{
				return;
			}
			int count = list.Count;
			this.table.NColumns = this.columnNum;
			this.table.NRows = (uint)((count + 1) / (int)this.columnNum);
			if (this.columnNum == 1U)
			{
				this.table.NRows -= 1U;
			}
			uint num = 0U;
			uint num2 = 0U;
			foreach (Widget widget in list)
			{
				if (widget != null)
				{
					this.AddToTable(widget, num, num2);
					num += 1U;
					if (num > this.columnNum - 1U)
					{
						num = 0U;
						num2 += 1U;
					}
				}
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000086D0 File Offset: 0x000068D0
		private void AddToTable(Widget item, uint x, uint y)
		{
			this.table.Add(item);
			Table.TableChild tableChild = (Table.TableChild)this.table[item];
			tableChild.TopAttach = y;
			tableChild.BottomAttach = y + 1U;
			tableChild.LeftAttach = x;
			tableChild.RightAttach = x + 1U;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000872C File Offset: 0x0000692C
		private void item_Destroyed(object sender, EventArgs e)
		{
			Widget widget = sender as Widget;
			if (widget == null)
			{
				return;
			}
			this.finishList.Remove(widget);
			widget.Destroyed -= this.item_Destroyed;
			this.UpdateAll();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000876C File Offset: 0x0000696C
		private void UpdateAll()
		{
			if (this.table == null)
			{
				return;
			}
			while (this.table.Children.Length != 0)
			{
				this.table.Remove(this.table.Children.Last<Widget>());
			}
			this.AddAllToTable(this.finishList);
			this.table.ShowAll();
			this.SetPluginNumber(this.finishList.Count);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000087D4 File Offset: 0x000069D4
		public void SetTab(LinkView tab)
		{
			this.tabLink = tab;
			this.SetPluginNumber(this.finishList.Count);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000087EE File Offset: 0x000069EE
		public void AddItem(Widget item)
		{
			this.finishList.Insert(0, item);
			item.Destroyed += this.item_Destroyed;
			this.UpdateAll();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008815 File Offset: 0x00006A15
		private void SetPluginNumber(int num)
		{
			if (this.tabLink != null)
			{
				this.tabLink.SetLableText(LanguageInfo.Launcher_Downloaded + "(" + num.ToString() + ")");
			}
		}

		// Token: 0x0400007B RID: 123
		private LinkView tabLink;

		// Token: 0x0400007C RID: 124
		private uint columnNum = 3U;

		// Token: 0x0400007D RID: 125
		private Table table;

		// Token: 0x0400007E RID: 126
		private List<Widget> finishList;
	}
}

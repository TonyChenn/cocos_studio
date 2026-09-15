using System;
using System.Collections.Generic;
using System.Linq;
using Cocos.Launcher.Control;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	public class FinishDownloadScrollWindow : ScrolledWindow
	{
		public FinishDownloadScrollWindow(List<Widget> list)
		{
			this.InitWidget();
			this.InitValue(list);
		}

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

		public void SetTab(LinkView tab)
		{
			this.tabLink = tab;
			this.SetPluginNumber(this.finishList.Count);
		}

		public void AddItem(Widget item)
		{
			this.finishList.Insert(0, item);
			item.Destroyed += this.item_Destroyed;
			this.UpdateAll();
		}

		private void SetPluginNumber(int num)
		{
			if (this.tabLink != null)
			{
				this.tabLink.SetLableText(LanguageInfo.Launcher_Downloaded + "(" + num.ToString() + ")");
			}
		}

		private LinkView tabLink;

		private uint columnNum = 3U;

		private Table table;

		private List<Widget> finishList;
	}
}

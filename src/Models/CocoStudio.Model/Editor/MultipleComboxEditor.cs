using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Model.ViewModel;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000062 RID: 98
	internal class MultipleComboxEditor : BaseEditor
	{
		// Token: 0x06000348 RID: 840 RVA: 0x0000DDCC File Offset: 0x0000BFCC
		private void Init()
		{
			this.list.Clear();
			object value = PropertyItem.FirstObject.GetType().GetProperty("MultipleComboxType").GetValue(PropertyItem.FirstObject, null);
			Type type = value.GetType();
			this.source = Enum.GetNames(type);
			this.val = (int[])Enum.GetValues(type);
			int currentData = (int)base.PropertyItem.Values[0];
			this.checkItem(currentData);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000DE4C File Offset: 0x0000C04C
		private void checkItem(int currentData)
		{
			this.list.Clear();
			this.list.Add("All", false);
			this.list.Add("None", false);
			bool flag = false;
			bool flag2 = true;
			if (this.source != null && this.source.Count<string>() > 0)
			{
				for (int i = 0; i < this.source.Count<string>(); i++)
				{
					string valueBykey = LanguageOption.GetValueBykey(this.source[i]);
					bool flag3 = (this.val[i] & currentData) != 0;
					if (i == 0)
					{
						flag = flag3;
					}
					else if (flag2)
					{
						flag2 = (flag == flag3);
					}
					this.list.Add(valueBykey, flag3);
				}
			}
			this.list["All"] = (flag && flag2);
			this.list["None"] = (!flag && flag2);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000DF5C File Offset: 0x0000C15C
		protected override Widget OnCreateWidget()
		{
			this.entry = new Entry();
			this.entry.WidthRequest = 160;
			this.entry.IsEditable = false;
			TriangleComboButton triangleComboButton = new TriangleComboButton();
			triangleComboButton.Clicked += this.MenuButtonClickedHandler;
			FullEntryShell fullEntryShell = EntryShellBuilder.CreateShell(this.entry, triangleComboButton);
			this.widget.Add(fullEntryShell);
			this.Init();
			base.SetControl();
			this.widget.ShowAll();
			return this.widget;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		private void MenuButtonClickedHandler(object sender, EventArgs e)
		{
			this.isSelect = false;
			Menu menu = new Menu();
			menu.WidthRequest = 160;
			foreach (KeyValuePair<string, bool> keyValuePair in this.list)
			{
				CheckMenuItem checkMenuItem = new CheckMenuItem(keyValuePair.Key);
				checkMenuItem.Active = keyValuePair.Value;
				checkMenuItem.Name = keyValuePair.Key;
				checkMenuItem.ButtonReleaseEvent += this.item1_ButtonReleaseEvent;
				menu.Add(checkMenuItem);
			}
			MenuPositionFunc func = delegate(Menu menu1, out int x, out int y, out bool pushIn)
			{
				int num;
				int num2;
				this.widget.GdkWindow.GetOrigin(out num, out num2);
				x = num;
				y = num2 + 25;
				pushIn = false;
			};
			menu.ShowAll();
			menu.SelectionDone += this.menu_SelectionDone;
			menu.KeyPressEvent += this.menu_KeyPressEvent;
			menu.Popup(null, null, func, 0U, 0U);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000E0E8 File Offset: 0x0000C2E8
		[ConnectBefore]
		private void menu_KeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return)
			{
				this.isSelect = true;
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000E116 File Offset: 0x0000C316
		private void item1_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.isSelect = true;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000E120 File Offset: 0x0000C320
		private void menu_SelectionDone(object o, EventArgs args)
		{
			if (this.isSelect)
			{
				this.SetState((o as Menu).Active.Name);
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000E154 File Offset: 0x0000C354
		private void SetState(string name)
		{
			AbstractNodeObject abstractNodeObject = PropertyItem.FirstObject as AbstractNodeObject;
			bool flag = abstractNodeObject.GetVisualChildren().Count<VisualObject>() > 0;
			MessageBoxResult messageBoxResult = MessageBoxResult.None;
			if (flag)
			{
				messageBoxResult = MessageBox.Show(LanguageInfo.MessageBox244_ApplyChildren, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
			}
			if (messageBoxResult != MessageBoxResult.Cancel || !flag)
			{
				bool flag2 = this.list[name];
				this.list[name] = !this.list[name];
				List<string> test = new List<string>(this.list.Keys);
				KeyValuePair<string, bool> keyValuePair = this.list.FirstOrDefault((KeyValuePair<string, bool> w) => w.Key == test[2]);
				int num = this.val[this.RetStr(keyValuePair.Key)];
				string text = "";
				if (name == "All")
				{
					for (int i = 0; i < this.list.Count; i++)
					{
						this.list[test[i]] = true;
					}
					this.list["None"] = false;
					for (int i = 3; i < this.list.Count; i++)
					{
						this.list[test[i]] = true;
						if (this.list[test[i]])
						{
							num |= this.val[this.RetStr(test[i])];
							text += string.Format("|{0}", test[i]);
						}
					}
					this.entry.Text = "All";
					this.entry.TooltipText = "All";
					if (flag)
					{
						if (messageBoxResult == MessageBoxResult.Yes)
						{
							num |= 128;
						}
					}
					base.UpdatePropertyValue(num, null);
				}
				else if (name == "None")
				{
					for (int i = 0; i < this.list.Count; i++)
					{
						this.list[test[i]] = false;
					}
					this.list["None"] = true;
					this.entry.Text = "None";
					this.entry.TooltipText = "None";
					int num2 = 0;
					if (flag)
					{
						if (messageBoxResult == MessageBoxResult.Yes)
						{
							num2 |= 128;
						}
					}
					base.UpdatePropertyValue(num2, null);
				}
				else
				{
					num = 0;
					for (int i = 2; i < this.list.Count; i++)
					{
						if (this.list[test[i]])
						{
							if (num == 0)
							{
								text += string.Format("{0}", test[i]);
							}
							else
							{
								text += string.Format("|{0}", test[i]);
							}
							num |= this.val[this.RetStr(test[i])];
						}
					}
					if (text == "")
					{
						text = "None";
					}
					bool flag3 = true;
					for (int i = 0; i < this.list.Count; i++)
					{
						if (test[i] != "None" && test[i] != "All")
						{
							flag3 &= this.list[test[i]];
						}
					}
					if (flag3)
					{
						text = "All";
					}
					this.entry.Text = text;
					this.entry.TooltipText = text;
					this.checkItem(num);
					if (flag)
					{
						if (messageBoxResult == MessageBoxResult.Yes)
						{
							num |= 128;
						}
					}
					base.UpdatePropertyValue(num, null);
				}
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000E634 File Offset: 0x0000C834
		private int RetStr(string str)
		{
			for (int i = 0; i < this.source.Length; i++)
			{
				if (this.source[i] == str)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000E67C File Offset: 0x0000C87C
		protected override void OnSetControl()
		{
			if (PropertyItem.Objects.Count > 1)
			{
				int num = (int)base.PropertyItem.Values[0];
				for (int i = 1; i < PropertyItem.Objects.Count; i++)
				{
					int num2 = (int)base.PropertyItem.Values[i];
					if (num2 != num)
					{
						this.entry.Text = "---";
						this.entry.TooltipText = "---";
						break;
					}
				}
			}
			else
			{
				this.Init();
				KeyValuePair<string, bool> keyValuePair = this.list.FirstOrDefault((KeyValuePair<string, bool> w) => w.Value);
				string text = keyValuePair.Key;
				if (text != "All")
				{
					foreach (KeyValuePair<string, bool> keyValuePair2 in this.list)
					{
						if (!(keyValuePair2.Key == keyValuePair.Key))
						{
							if (keyValuePair2.Value)
							{
								text += string.Format("|{0}", keyValuePair2.Key);
							}
						}
					}
				}
				this.entry.Text = text;
				this.entry.TooltipText = text;
			}
		}

		// Token: 0x04000195 RID: 405
		private EventBox widget = new EventBox();

		// Token: 0x04000196 RID: 406
		private Entry entry;

		// Token: 0x04000197 RID: 407
		private string[] source;

		// Token: 0x04000198 RID: 408
		private int[] val;

		// Token: 0x04000199 RID: 409
		private bool isSelect = false;

		// Token: 0x0400019A RID: 410
		private Dictionary<string, bool> list = new Dictionary<string, bool>();
	}
}

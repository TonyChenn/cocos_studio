using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000012 RID: 18
	public class EnumEditor : BaseEditor
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003150 File Offset: 0x00001350
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003164 File Offset: 0x00001364
		protected override Widget OnCreateWidget()
		{
			this.comboBox = new EnumEditorComboBox();
			this.comboBox.Sensitive = true;
			ListStore model = new ListStore(new Type[]
			{
				typeof(string)
			});
			object firstValue = base.PropertyItem.FirstValue;
			this.enumType = firstValue.GetType();
			int active = -1;
			model = this.InitiData(firstValue.ToString(), out active);
			this.comboBox.Model = model;
			CellRendererText cell = new CellRendererText();
			this.comboBox.PackStart(cell, true);
			this.comboBox.AddAttribute(cell, "text", 0);
			this.comboBox.Active = active;
			this.comboBox.Changed += this.ComboBoxChangedHandler;
			this.comboBox.WidthRequest = 20;
			this.comboBox.Show();
			base.SetControl();
			BorderEventBox borderEventBox = new BorderEventBox();
			borderEventBox.Add(this.comboBox);
			borderEventBox.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
			borderEventBox.ModifyBg(StateType.Insensitive, WindowStyle.LineDarkColor);
			borderEventBox.ShowAll();
			if (Platform.IsMac)
			{
				borderEventBox.HeightRequest = 24;
			}
			return borderEventBox;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000032A4 File Offset: 0x000014A4
		private ListStore InitiData(string value, out int active)
		{
			ListStore listStore = new ListStore(new Type[]
			{
				typeof(string)
			});
			string[] names = Enum.GetNames(this.enumType);
			if (names != null && names.Count<string>() > 0)
			{
				int num = -1;
				for (int i = 0; i < names.Count<string>(); i++)
				{
					if (names[i] == value)
					{
						num = i;
					}
					string valueBykey = LanguageOption.GetValueBykey(names[i]);
					listStore.AppendValues(new object[]
					{
						valueBykey
					});
					this.listStr.Add(Tuple.Create<string, string>(names[i], valueBykey));
				}
				active = num;
			}
			else
			{
				active = -1;
			}
			return listStore;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003370 File Offset: 0x00001570
		private int GetActive(string value)
		{
			string[] names = Enum.GetNames(this.enumType);
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000033B8 File Offset: 0x000015B8
		protected override void OnSetControl()
		{
			object firstValue = base.PropertyItem.FirstValue;
			for (int i = 1; i < PropertyItem.Objects.Count; i++)
			{
				object obj = base.PropertyItem.Values[i];
				if (this.GetActive(obj.ToString()) != this.GetActive(firstValue.ToString()))
				{
					this.InternalSetComboBox(-1);
					return;
				}
			}
			int active = this.GetActive(firstValue.ToString());
			this.InternalSetComboBox(active);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003441 File Offset: 0x00001641
		private void InternalSetComboBox(int index)
		{
			this.isInternalChange = true;
			this.comboBox.Active = index;
			this.isInternalChange = false;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003460 File Offset: 0x00001660
		private void ComboBoxChangedHandler(object sender, EventArgs e)
		{
			if (!this.isInternalChange)
			{
				this.OnComboBoxChanged();
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000034B0 File Offset: 0x000016B0
		protected virtual void OnComboBoxChanged()
		{
			string selectedValue = this.comboBox.ActiveText;
			string item = this.listStr.FirstOrDefault((Tuple<string, string> w) => w.Item2 == selectedValue).Item1;
			object value = Enum.Parse(this.enumType, item);
			base.UpdatePropertyValue(value, null);
		}

		// Token: 0x04000019 RID: 25
		private EnumEditorComboBox comboBox;

		// Token: 0x0400001A RID: 26
		private Type enumType;

		// Token: 0x0400001B RID: 27
		private List<Tuple<string, string>> listStr = new List<Tuple<string, string>>();

		// Token: 0x0400001C RID: 28
		private bool isInternalChange = false;
	}
}

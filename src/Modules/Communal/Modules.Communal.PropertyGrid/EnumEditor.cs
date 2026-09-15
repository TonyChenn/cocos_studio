using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.PropertyGrid
{
	public class EnumEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

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

		private void InternalSetComboBox(int index)
		{
			this.isInternalChange = true;
			this.comboBox.Active = index;
			this.isInternalChange = false;
		}

		private void ComboBoxChangedHandler(object sender, EventArgs e)
		{
			if (!this.isInternalChange)
			{
				this.OnComboBoxChanged();
			}
		}

		protected virtual void OnComboBoxChanged()
		{
			string selectedValue = this.comboBox.ActiveText;
			string item = this.listStr.FirstOrDefault((Tuple<string, string> w) => w.Item2 == selectedValue).Item1;
			object value = Enum.Parse(this.enumType, item);
			base.UpdatePropertyValue(value, null);
		}

		private EnumEditorComboBox comboBox;

		private Type enumType;

		private List<Tuple<string, string>> listStr = new List<Tuple<string, string>>();

		private bool isInternalChange = false;
	}
}

using System;
using System.ComponentModel;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	internal class SingleEditor : BaseEditor
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
			double num = 1.0;
			DefaultValueAttribute defaultValueAttribute = base.PropertyItem.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
			if (defaultValueAttribute != null)
			{
				double.TryParse(defaultValueAttribute.Value.ToString(), out num);
			}
			this.entry = new NoUndoNumEntry();
			this.entry.DecimalPlaces = 2;
			ValueRangeAttribute valueRangeAttribute = base.PropertyItem.Attributes[typeof(ValueRangeAttribute)] as ValueRangeAttribute;
			if (valueRangeAttribute != null)
			{
				this.entry.MaxValue = valueRangeAttribute.MaxValue;
				this.entry.MinValue = valueRangeAttribute.MinValue;
				this.entry.ScrollNum = valueRangeAttribute.Step;
			}
			base.SetControl();
			this.entry.EntryValueChanged += new EventHandler<EntryIntEventArgs>(this.IntEditor_ValueChanged);
			EntryShell entryShell = new EntryShell(this.entry);
			entryShell.ShowAll();
			return entryShell;
		}

		protected override void OnSetControl()
		{
			if (!base.CheckIsSameValue())
			{
				this.entry.SetToSubState();
			}
			else
			{
				object firstValue = base.PropertyItem.FirstValue;
				if (firstValue != null)
				{
					this.entry.Value = Convert.ToSingle(firstValue);
				}
			}
		}

		private void IntEditor_ValueChanged(object sender, EventArgs e)
		{
			base.UpdatePropertyValue(Convert.ToSingle(this.entry.Value), null);
		}

		private NoUndoNumEntry entry;
	}
}

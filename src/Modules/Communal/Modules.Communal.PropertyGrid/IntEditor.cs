using System;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	internal class IntEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return base.PropertyItem.Name != "Tag";
			}
		}

		protected override Widget OnCreateWidget()
		{
			this.entry = new NoUndoNumEntry();
			this.entry.IsInteger = true;
			this.entry.DecimalPlaces = 0;
			ValueRangeAttribute valueRangeAttribute = base.PropertyItem.Attributes[typeof(ValueRangeAttribute)] as ValueRangeAttribute;
			if (valueRangeAttribute != null)
			{
				this.entry.MaxValue = valueRangeAttribute.MaxValue;
				this.entry.MinValue = valueRangeAttribute.MinValue;
				this.entry.ScrollNum = valueRangeAttribute.Step;
			}
			base.SetControl();
			this.entry.EntryValueChanged += new EventHandler<EntryIntEventArgs>(this.IntEditor_Changed);
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
					this.entry.Value = (float)Convert.ToInt32(firstValue);
				}
			}
		}

		private void IntEditor_Changed(object sender, EventArgs e)
		{
			base.UpdatePropertyValue(Convert.ToInt32(this.entry.Value), null);
		}

		private NoUndoNumEntry entry;
	}
}

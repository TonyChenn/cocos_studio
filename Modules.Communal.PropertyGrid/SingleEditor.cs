using System;
using System.ComponentModel;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200000B RID: 11
	internal class SingleEditor : BaseEditor
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002AEC File Offset: 0x00000CEC
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002B00 File Offset: 0x00000D00
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

		// Token: 0x06000052 RID: 82 RVA: 0x00002C08 File Offset: 0x00000E08
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

		// Token: 0x06000053 RID: 83 RVA: 0x00002C55 File Offset: 0x00000E55
		private void IntEditor_ValueChanged(object sender, EventArgs e)
		{
			base.UpdatePropertyValue(Convert.ToSingle(this.entry.Value), null);
		}

		// Token: 0x04000013 RID: 19
		private NoUndoNumEntry entry;
	}
}

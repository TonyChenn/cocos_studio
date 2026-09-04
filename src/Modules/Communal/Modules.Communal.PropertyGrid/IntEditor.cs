using System;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000014 RID: 20
	internal class IntEditor : BaseEditor
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000035E0 File Offset: 0x000017E0
		public override bool SupportMultiSelect
		{
			get
			{
				return base.PropertyItem.Name != "Tag";
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003608 File Offset: 0x00001808
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

		// Token: 0x06000087 RID: 135 RVA: 0x000036D4 File Offset: 0x000018D4
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

		// Token: 0x06000088 RID: 136 RVA: 0x00003722 File Offset: 0x00001922
		private void IntEditor_Changed(object sender, EventArgs e)
		{
			base.UpdatePropertyValue(Convert.ToInt32(this.entry.Value), null);
		}

		// Token: 0x0400001E RID: 30
		private NoUndoNumEntry entry;
	}
}

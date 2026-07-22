using System;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200000E RID: 14
	internal class OneNumberEditor : BaseEditor
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002F96 File Offset: 0x00001196
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002F99 File Offset: 0x00001199
		public OneNumberEditor(string label)
		{
			this.headText = label;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002FA8 File Offset: 0x000011A8
		protected override Widget OnCreateWidget()
		{
			this.innerEntry = new NoUndoNumEntry();
			this.innerEntry.DecimalPlaces = 2;
			base.SetControl();
			this.innerEntry.EntryValueChanged += this.EntryValueChangedHandler;
			FullEntryShell fullEntryShell = EntryShellBuilder.CreateShell(this.headText, this.innerEntry);
			fullEntryShell.ShowAll();
			return fullEntryShell;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003004 File Offset: 0x00001204
		protected override void OnSetControl()
		{
			float value = (float)base.PropertyItem.Values[0];
			this.innerEntry.Value = value;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003034 File Offset: 0x00001234
		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			base.UpdatePropertyValue(e.Value, null);
		}

		// Token: 0x0400003E RID: 62
		protected NoUndoNumEntry innerEntry;

		// Token: 0x0400003F RID: 63
		private string headText;
	}
}

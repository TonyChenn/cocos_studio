using System;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Editor
{
	// Token: 0x02000003 RID: 3
	internal class PaddingEditor : BaseEditor
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
		protected override Widget OnCreateWidget()
		{
			this.innerEntry = new NoUndoNumEntry();
			this.innerEntry.DecimalPlaces = 2;
			this.innerEntry.MaxValue = 200;
			this.innerEntry.MinValue = 0;
			EntryShell entryShell = EntryShellBuilder.CreateShell(this.innerEntry, LanguageInfo.NewFile_Pixel);
			base.SetControl();
			this.innerEntry.EntryValueChanged += this.EntryValueChangedHandler;
			entryShell.ShowAll();
			return entryShell;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002140 File Offset: 0x00000340
		protected override void OnSetControl()
		{
			object value = base.PropertyItem.Values[0];
			this.innerEntry.Value = Convert.ToSingle(value);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002170 File Offset: 0x00000370
		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			base.UpdatePropertyValue((int)e.Value, null);
		}

		// Token: 0x04000001 RID: 1
		private NoUndoNumEntry innerEntry;
	}
}

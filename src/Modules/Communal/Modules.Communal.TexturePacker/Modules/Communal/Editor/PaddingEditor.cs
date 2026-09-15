using System;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Editor
{
	internal class PaddingEditor : BaseEditor
	{
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

		protected override void OnSetControl()
		{
			object value = base.PropertyItem.Values[0];
			this.innerEntry.Value = Convert.ToSingle(value);
		}

		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			base.UpdatePropertyValue((int)e.Value, null);
		}

		private NoUndoNumEntry innerEntry;
	}
}

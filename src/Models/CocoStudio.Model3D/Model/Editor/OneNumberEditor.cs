using System;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class OneNumberEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		public OneNumberEditor(string label)
		{
			this.headText = label;
		}

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

		protected override void OnSetControl()
		{
			float value = (float)base.PropertyItem.Values[0];
			this.innerEntry.Value = value;
		}

		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			base.UpdatePropertyValue(e.Value, null);
		}

		protected NoUndoNumEntry innerEntry;

		private string headText;
	}
}

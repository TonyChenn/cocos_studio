using System;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class CallBackPropertyRootEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			this.entry = new EntryCallBackEx();
			base.SetControl();
			this.entry.KeyReleaseEvent += this.entry_KeyReleaseEvent;
			this.entry.FocusOutEvent += this.entry_FocusOutEvent;
			this.entryShell = new EntryShell(this.entry);
			this.entryShell.ShowAll();
			return this.entryShell;
		}

		private void entry_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && this.entry.IsFocus)
			{
				this.entry.Value = this.entry.Text.Trim(new char[]
				{
					' '
				});
				base.UpdatePropertyValue(this.entry.Text, null);
				base.ReportUserData("CustomClass");
			}
		}

		private void entry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.entry.Value = this.entry.Text.Trim(new char[]
			{
				' '
			});
			base.UpdatePropertyValue(this.entry.Text, null);
			base.ReportUserData("CustomClass");
		}

		protected override void OnSetControl()
		{
			string value = (string)base.PropertyItem.Values[0];
			this.entry.Value = value;
		}

		private EntryCallBackEx entry;

		private EntryShell entryShell;
	}
}

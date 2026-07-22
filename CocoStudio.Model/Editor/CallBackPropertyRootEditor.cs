using System;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200004F RID: 79
	internal class CallBackPropertyRootEditor : BaseEditor
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x00008EB8 File Offset: 0x000070B8
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

		// Token: 0x060002A8 RID: 680 RVA: 0x00008F30 File Offset: 0x00007130
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

		// Token: 0x060002A9 RID: 681 RVA: 0x00008FB0 File Offset: 0x000071B0
		private void entry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.entry.Value = this.entry.Text.Trim(new char[]
			{
				' '
			});
			base.UpdatePropertyValue(this.entry.Text, null);
			base.ReportUserData("CustomClass");
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00009008 File Offset: 0x00007208
		protected override void OnSetControl()
		{
			string value = (string)base.PropertyItem.Values[0];
			this.entry.Value = value;
		}

		// Token: 0x04000123 RID: 291
		private EntryCallBackEx entry;

		// Token: 0x04000124 RID: 292
		private EntryShell entryShell;
	}
}

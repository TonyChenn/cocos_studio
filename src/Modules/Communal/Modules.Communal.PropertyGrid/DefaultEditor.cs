using System;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	internal class DefaultEditor : BaseEditor
	{
		public TextEntry Entry { get; private set; }

		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		protected override Widget OnCreateWidget()
		{
			this.Entry = new TextEntry();
			this.Entry.ValueSet += this.EntryValueSetHandler;
			base.SetControl();
			EntryShell entryShell = new EntryShell(this.Entry);
			entryShell.ShowAll();
			return entryShell;
		}

		protected override void OnSetControl()
		{
			if (!base.CheckIsSameValue())
			{
				this.Entry.SetToSubState();
			}
			else
			{
				object firstValue = base.PropertyItem.FirstValue;
				if (firstValue != null)
				{
					this.Entry.Text = firstValue.ToString();
				}
			}
		}

		private void EntryValueSetHandler(object sender, EventArgs e)
		{
			base.UpdatePropertyValue(this.Entry.Text, null);
			if (base.PropertyItem.Name == "UserData" || base.PropertyItem.Name == "FrameEvent")
			{
				base.ReportUserData(base.PropertyItem.Name);
			}
		}
	}
}

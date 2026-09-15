using System;
using System.Text.RegularExpressions;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class NumberEntryEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			this.widget = new TextEntry();
			base.SetControl();
			this.widget.KeyReleaseEvent += this.widget_KeyReleaseEvent;
			this.widget.FocusOutEvent += this.widget_FocusOutEvent;
			this.widget.Changed += this.widget_Changed;
			EntryShell entryShell = EntryShellBuilder.CreateShell(this.widget);
			entryShell.ShowAll();
			return entryShell;
		}

		protected override void OnSetControl()
		{
			if (base.PropertyItem != null)
			{
				this.widget.KeyReleaseEvent -= this.widget_KeyReleaseEvent;
				this.widget.FocusOutEvent -= this.widget_FocusOutEvent;
				this.widget.Changed -= this.widget_Changed;
				object obj = base.PropertyItem.Values[0];
				if (obj != null)
				{
					this.widget.Text = (this.oldEntryValue = obj.ToString());
				}
				this.widget.KeyReleaseEvent += this.widget_KeyReleaseEvent;
				this.widget.FocusOutEvent += this.widget_FocusOutEvent;
				this.widget.Changed += this.widget_Changed;
			}
		}

		public static bool IsUnsign(string value)
		{
			return Regex.IsMatch(value, "^[0-9]*$");
		}

		private void widget_Changed(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.widget.Text))
			{
				string value = this.widget.Text.Replace(".", "").Replace("/", "");
				if (!NumberEntryEditor.IsUnsign(value))
				{
					this.widget.Text = this.oldEntryValue;
				}
				else
				{
					this.oldEntryValue = this.widget.Text;
				}
			}
			else
			{
				this.oldEntryValue = this.widget.Text;
			}
		}

		private void widget_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (!this.isKeyPress)
			{
				base.UpdatePropertyValue(this.widget.Text, null);
			}
		}

		private void widget_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key) && this.widget.IsFocus)
			{
				this.isKeyPress = true;
				base.UpdatePropertyValue(this.widget.Text, null);
				this.isKeyPress = false;
			}
		}

		private TextEntry widget;

		private string oldEntryValue = string.Empty;

		private bool isKeyPress = false;
	}
}

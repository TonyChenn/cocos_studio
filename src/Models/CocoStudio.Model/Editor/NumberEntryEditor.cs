using System;
using System.Text.RegularExpressions;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000064 RID: 100
	internal class NumberEntryEditor : BaseEditor
	{
		// Token: 0x06000357 RID: 855 RVA: 0x0000E8A0 File Offset: 0x0000CAA0
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

		// Token: 0x06000358 RID: 856 RVA: 0x0000E920 File Offset: 0x0000CB20
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

		// Token: 0x06000359 RID: 857 RVA: 0x0000EA08 File Offset: 0x0000CC08
		public static bool IsUnsign(string value)
		{
			return Regex.IsMatch(value, "^[0-9]*$");
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000EA28 File Offset: 0x0000CC28
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

		// Token: 0x0600035B RID: 859 RVA: 0x0000EAB8 File Offset: 0x0000CCB8
		private void widget_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (!this.isKeyPress)
			{
				base.UpdatePropertyValue(this.widget.Text, null);
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000EAE8 File Offset: 0x0000CCE8
		private void widget_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key) && this.widget.IsFocus)
			{
				this.isKeyPress = true;
				base.UpdatePropertyValue(this.widget.Text, null);
				this.isKeyPress = false;
			}
		}

		// Token: 0x0400019D RID: 413
		private TextEntry widget;

		// Token: 0x0400019E RID: 414
		private string oldEntryValue = string.Empty;

		// Token: 0x0400019F RID: 415
		private bool isKeyPress = false;
	}
}

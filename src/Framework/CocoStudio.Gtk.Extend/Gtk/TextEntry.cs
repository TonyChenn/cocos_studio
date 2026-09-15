using System;
using Gdk;
using MonoDevelop.Components.Commands;

namespace Gtk
{
	public class TextEntry : Entry
	{
		public event EventHandler ValueSet;

		public void SetToSubState()
		{
			base.Text = "-";
			this.isSubState = true;
		}

		private void SetValue()
		{
			if (this.ValueSet != null)
			{
				this.ValueSet(this, new EventArgs());
			}
			this.isSubState = false;
		}

		protected override void OnTextInserted(string text, ref int position)
		{
			this.isSubState = false;
			base.OnTextInserted(text, ref position);
		}

		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if (KeyboardExtend.IsEnterKey(evnt.Key))
			{
				this.SetValue();
			}
			return base.OnKeyReleaseEvent(evnt);
		}

		protected override bool OnFocusInEvent(EventFocus evnt)
		{
			if (this.isSubState)
			{
				base.Text = string.Empty;
				this.isSubState = true;
			}
			return base.OnFocusInEvent(evnt);
		}

		protected override bool OnFocusOutEvent(EventFocus evnt)
		{
			base.SelectRegion(0, 0);
			if (this.isSubState)
			{
				this.SetToSubState();
			}
			else
			{
				this.SetValue();
			}
			return base.OnFocusOutEvent(evnt);
		}

		[CommandHandler("CocoStudio.Core.Commands.CmdEnum.RedoCmd")]
		[CommandHandler("CocoStudio.Core.Commands.CmdEnum.UndoCmd")]
		private void OnUndoHandle()
		{
		}

		protected bool isSubState = false;
	}
}

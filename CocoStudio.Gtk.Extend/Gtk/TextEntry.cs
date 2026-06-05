using System;
using Gdk;
using MonoDevelop.Components.Commands;

namespace Gtk
{
	// Token: 0x02000060 RID: 96
	public class TextEntry : Entry
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000205 RID: 517 RVA: 0x00009168 File Offset: 0x00007368
		// (remove) Token: 0x06000206 RID: 518 RVA: 0x000091A4 File Offset: 0x000073A4
		public event EventHandler ValueSet;

		// Token: 0x06000207 RID: 519 RVA: 0x000091E0 File Offset: 0x000073E0
		public void SetToSubState()
		{
			base.Text = "-";
			this.isSubState = true;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000091F8 File Offset: 0x000073F8
		private void SetValue()
		{
			if (this.ValueSet != null)
			{
				this.ValueSet(this, new EventArgs());
			}
			this.isSubState = false;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000922C File Offset: 0x0000742C
		protected override void OnTextInserted(string text, ref int position)
		{
			this.isSubState = false;
			base.OnTextInserted(text, ref position);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009240 File Offset: 0x00007440
		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if (KeyboardExtend.IsEnterKey(evnt.Key))
			{
				this.SetValue();
			}
			return base.OnKeyReleaseEvent(evnt);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009274 File Offset: 0x00007474
		protected override bool OnFocusInEvent(EventFocus evnt)
		{
			if (this.isSubState)
			{
				base.Text = string.Empty;
				this.isSubState = true;
			}
			return base.OnFocusInEvent(evnt);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000092B0 File Offset: 0x000074B0
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

		// Token: 0x0600020D RID: 525 RVA: 0x000092EF File Offset: 0x000074EF
		[CommandHandler("CocoStudio.Core.Commands.CmdEnum.RedoCmd")]
		[CommandHandler("CocoStudio.Core.Commands.CmdEnum.UndoCmd")]
		private void OnUndoHandle()
		{
		}

		// Token: 0x04000305 RID: 773
		protected bool isSubState = false;
	}
}

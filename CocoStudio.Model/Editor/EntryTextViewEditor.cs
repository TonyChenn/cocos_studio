using System;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000053 RID: 83
	internal class EntryTextViewEditor : BaseEditor
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00009C90 File Offset: 0x00007E90
		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00009CA4 File Offset: 0x00007EA4
		protected override Widget OnCreateWidget()
		{
			this.textView = new TextView();
			this.textView.WrapMode = WrapMode.Char;
			this.textView.HeightRequest = 80;
			base.SetControl();
			this.textView.KeyReleaseEvent += this.textView_KeyReleaseEvent;
			this.textView.FocusOutEvent += this.textView_FocusOutEvent;
			EventBox eventBox = new EventBox();
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment.Add(this.textView);
			eventBox.Add(alignment);
			alignment.BorderWidth = 1U;
			eventBox.ModifyBg(StateType.Normal, PropertyPadStyle.darkBorderColor);
			eventBox.ShowAll();
			return eventBox;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00009D68 File Offset: 0x00007F68
		protected override void OnSetControl()
		{
			if (base.PropertyItem != null)
			{
				object obj = base.PropertyItem.Values[0];
				if (obj != null)
				{
					this.textView.Buffer.Text = obj.ToString();
				}
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00009DBC File Offset: 0x00007FBC
		private void textView_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (!this.isKeyPress)
			{
				base.UpdatePropertyValue(this.textView.Buffer.Text, null);
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00009DF4 File Offset: 0x00007FF4
		private void textView_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && this.textView.IsFocus)
			{
				this.isKeyPress = true;
				base.UpdatePropertyValue(this.textView.Buffer.Text, null);
				this.isKeyPress = false;
			}
		}

		// Token: 0x04000136 RID: 310
		private TextView textView;

		// Token: 0x04000137 RID: 311
		private bool isKeyPress = false;
	}
}

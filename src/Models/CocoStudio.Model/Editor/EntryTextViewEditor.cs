using System;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class EntryTextViewEditor : BaseEditor
	{
		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

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

		private void textView_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (!this.isKeyPress)
			{
				base.UpdatePropertyValue(this.textView.Buffer.Text, null);
			}
		}

		private void textView_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && this.textView.IsFocus)
			{
				this.isKeyPress = true;
				base.UpdatePropertyValue(this.textView.Buffer.Text, null);
				this.isKeyPress = false;
			}
		}

		private TextView textView;

		private bool isKeyPress = false;
	}
}

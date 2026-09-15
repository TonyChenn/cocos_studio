using System;
using CocoStudio.Core;
using CocoStudio.Model.Visiter;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	public class ValidTextEditor : BaseEditor
	{
		public override bool IsShowLabel
		{
			get
			{
				return false;
			}
		}

		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		protected override Widget OnCreateWidget()
		{
			this.widget = new NoUndoEntry();
			if (PropertyItem.Objects.Count > 1)
			{
				this.widget.Sensitive = false;
			}
			else
			{
				base.SetControl();
				this.widget.KeyReleaseEvent += this.widget_KeyReleaseEvent;
				this.widget.FocusOutEvent += this.widget_FocusOutEvent;
			}
			return this.widget;
		}

		private void widget_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (!this.isKeyPress)
			{
				this.SetText(this.widget.Text.Trim());
			}
		}

		private void widget_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && this.widget.IsFocus)
			{
				this.isKeyPress = true;
				this.SetText(this.widget.Text.Trim());
				this.isKeyPress = false;
			}
		}

		private void SetText(string value)
		{
			if (this.oldStr == value)
			{
				this.widget.Text = value;
			}
			else if (string.IsNullOrEmpty(value))
			{
				this.widget.Text = this.oldStr;
			}
			else if (!Services.ProjectOperations.CurrentSelectedProject.IsObjectNameStandardized(value))
			{
				this.widget.Text = this.oldStr;
			}
			else
			{
				Services.ProjectOperations.CurrentSelectedProject.RemoveName(this.oldStr);
				base.UpdatePropertyValue(this.widget.Text.Trim(), null);
				this.oldStr = this.widget.Text.Trim();
				this.widget.Text = this.oldStr;
				Services.ProjectOperations.CurrentSelectedProject.AddName(this.oldStr);
			}
		}

		protected override void OnSetControl()
		{
			object obj = base.PropertyItem.Values[0];
			if (obj != null)
			{
				this.widget.Text = obj.ToString();
				this.oldStr = this.widget.Text;
			}
		}

		private Entry widget;

		private bool isKeyPress = false;

		private string oldStr;
	}
}

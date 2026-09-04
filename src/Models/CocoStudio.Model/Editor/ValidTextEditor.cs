using System;
using CocoStudio.Core;
using CocoStudio.Model.Visiter;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200006C RID: 108
	public class ValidTextEditor : BaseEditor
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00011850 File Offset: 0x0000FA50
		public override bool IsShowLabel
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00011864 File Offset: 0x0000FA64
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00011878 File Offset: 0x0000FA78
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

		// Token: 0x060003AA RID: 938 RVA: 0x000118FC File Offset: 0x0000FAFC
		private void widget_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (!this.isKeyPress)
			{
				this.SetText(this.widget.Text.Trim());
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00011930 File Offset: 0x0000FB30
		private void widget_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && this.widget.IsFocus)
			{
				this.isKeyPress = true;
				this.SetText(this.widget.Text.Trim());
				this.isKeyPress = false;
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00011990 File Offset: 0x0000FB90
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

		// Token: 0x060003AD RID: 941 RVA: 0x00011A84 File Offset: 0x0000FC84
		protected override void OnSetControl()
		{
			object obj = base.PropertyItem.Values[0];
			if (obj != null)
			{
				this.widget.Text = obj.ToString();
				this.oldStr = this.widget.Text;
			}
		}

		// Token: 0x040001DF RID: 479
		private Entry widget;

		// Token: 0x040001E0 RID: 480
		private bool isKeyPress = false;

		// Token: 0x040001E1 RID: 481
		private string oldStr;
	}
}

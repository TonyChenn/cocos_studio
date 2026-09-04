using System;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	// Token: 0x0200000A RID: 10
	public class IconRadioButton : IconToggleButton
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000030B8 File Offset: 0x000012B8
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000030CF File Offset: 0x000012CF
		public string GroupName { get; set; }

		// Token: 0x06000051 RID: 81 RVA: 0x000030D8 File Offset: 0x000012D8
		public IconRadioButton(XwtImage normalIcon) : base(normalIcon, null)
		{
			RadioButtonManager.Instance.Add(this);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000030F1 File Offset: 0x000012F1
		protected override void OnMousePressed(ButtonPressEventArgs args)
		{
			base.IsChecked = true;
		}
	}
}

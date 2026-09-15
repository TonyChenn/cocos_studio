using System;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	public class IconRadioButton : IconToggleButton
	{
		public string GroupName { get; set; }

		public IconRadioButton(XwtImage normalIcon) : base(normalIcon, null)
		{
			RadioButtonManager.Instance.Add(this);
		}

		protected override void OnMousePressed(ButtonPressEventArgs args)
		{
			base.IsChecked = true;
		}
	}
}

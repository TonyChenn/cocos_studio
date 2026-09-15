using System;
using Gdk;
using MonoDevelop.Components;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Gtk
{
	public class TriangleComboButton : EventBox
	{
		public event EventHandler Clicked;

		public TriangleComboButton()
		{
			base.ModifyBg(StateType.Normal, new Gdk.Color(50, 50, 54));
			base.ModifyBg(StateType.Insensitive, new Gdk.Color(62, 62, 66));
			if (Platform.IsWindows)
			{
				this.normalImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.TriangleButton.WinNormal.png");
				this.hoverImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.TriangleButton.WinHover.png");
			}
			else
			{
				this.normalImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.TriangleButton.MacNormal.png");
				this.hoverImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.TriangleButton.MacHover.png");
			}
			this.imageView = new ImageView(this.normalImg);
			base.Add(this.imageView);
			base.ShowAll();
			base.EnterNotifyEvent += this.MouseEnterEventHandler;
			base.LeaveNotifyEvent += this.MouseLeaveEventHandler;
			base.ButtonReleaseEvent += this.ButtonReleasedHandler;
		}

		private void MouseEnterEventHandler(object o, EnterNotifyEventArgs args)
		{
			this.imageView.Image = this.hoverImg;
			this.isMouseIn = true;
		}

		private void MouseLeaveEventHandler(object o, LeaveNotifyEventArgs args)
		{
			this.imageView.Image = this.normalImg;
			this.isMouseIn = false;
		}

		private void ButtonReleasedHandler(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U || this.isMouseIn)
			{
				if (this.Clicked != null)
				{
					this.imageView.Image = this.normalImg;
					this.Clicked(this, new EventArgs());
				}
			}
		}

		private bool isMouseIn = false;

		private ImageView imageView;

		private Xwt.Drawing.Image normalImg;

		private Xwt.Drawing.Image hoverImg;
	}
}

using System;
using System.ComponentModel;
using Gdk;
using Gtk;

namespace Cocos.Launcher.Control
{
	[ToolboxItem(true)]
	public class ImageButtonView : EventBox
	{
		private string NormalNotifyPath { get; set; }

		private string EnterNotifyPath { get; set; }

		private string PressNotifyPath { get; set; }

		public bool IsCursor { get; set; }

		public ImageButtonView()
		{
			this.image_button = new ImageBin();
			base.VisibleWindow = false;
			base.Add(this.image_button);
			this.InitEvent();
		}

		private void InitEvent()
		{
			base.EnterNotifyEvent += this.LauncherButton_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.LauncherButton_LeaveNotifyEvent;
			base.ButtonPressEvent += this.LauncherButton_ButtonPressEvent;
		}

		public void SetNormalBack(string path)
		{
			this.NormalNotifyPath = path;
			this.image_button.SetImageView(ImageIcon.GetIcon(path));
			base.ShowAll();
		}

		public void SetMoveBack(string path)
		{
			this.EnterNotifyPath = path;
		}

		public void SetPressBack(string path)
		{
			this.PressNotifyPath = path;
		}

		private void LauncherButton_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if (this.IsCursor)
			{
				base.GdkWindow.Cursor = null;
			}
			if (!string.IsNullOrEmpty(this.NormalNotifyPath))
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.NormalNotifyPath));
				base.ShowAll();
			}
		}

		private void LauncherButton_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			if (this.IsCursor)
			{
				base.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
			}
			if (!string.IsNullOrEmpty(this.EnterNotifyPath))
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.EnterNotifyPath));
				base.ShowAll();
			}
		}

		private void LauncherButton_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (args.Event.GetMouseButton() != MouseButton.Left)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.PressNotifyPath))
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.PressNotifyPath));
				base.ShowAll();
			}
			args.RetVal = true;
		}

		private ImageBin image_button;
	}
}

using System;
using System.Drawing;
using System.Windows.Forms;
using Gtk;
using Xwt.Backends;

namespace CocoStudio.WindowsPlatform
{
	internal class CSWinStatusIconBackend : IStatusIconBackend, IBackend, IDisposable, IStatusIcon
	{
		private NotifyIcon notifyIcon;

		private ContextMenu menu;

		public object PopupMenu
		{
			get
			{
				return notifyIcon.ContextMenu;
			}
			set
			{
				notifyIcon.ContextMenu = value as ContextMenu;
			}
		}

		public string IconPath
		{
			get
			{
				return null;
			}
			set
			{
				notifyIcon.Icon = new System.Drawing.Icon(value);
			}
		}

		public string Tooltip
		{
			get
			{
				return notifyIcon.Text;
			}
			set
			{
				notifyIcon.Text = value;
			}
		}

		public event EventHandler<EventArgs> Action;

		public void SetMenu(object menuBackend)
		{
		}

		public void SetImage(ImageDescription img)
		{
		}

		public void InitializeBackend(object frontend, Xwt.Backends.ApplicationContext context)
		{
			notifyIcon = new NotifyIcon();
			notifyIcon.Visible = true;
			notifyIcon.DoubleClick += notifyIcon_DoubleClick;
		}

		private void notifyIcon_DoubleClick(object sender, EventArgs e)
		{
			if (Action != null)
			{
				Action(sender, e);
			}
		}

		public void EnableEvent(object eventId)
		{
			notifyIcon.Visible = true;
		}

		public void DisableEvent(object eventId)
		{
			notifyIcon.Visible = false;
		}

		public void Dispose()
		{
			if (notifyIcon != null)
			{
				notifyIcon.Visible = false;
				notifyIcon = null;
			}
		}
	}
}

using System;
using Xwt;
using Xwt.Backends;

namespace Gtk
{
	[BackendType(typeof(IStatusIconBackend))]
	public class StatusIconTray : XwtComponent, IStatusIcon
	{
		private IStatusIcon statusIconBackend
		{
			get
			{
				return base.BackendHost.Backend as IStatusIcon;
			}
		}

		public object PopupMenu
		{
			get
			{
				return this.statusIconBackend.PopupMenu;
			}
			set
			{
				this.statusIconBackend.PopupMenu = value;
			}
		}

		public string IconPath
		{
			get
			{
				return this.statusIconBackend.IconPath;
			}
			set
			{
				this.statusIconBackend.IconPath = value;
			}
		}

		protected override void Dispose(bool disposing)
		{
			this.statusIconBackend.Dispose();
			base.Dispose(disposing);
		}

		public event EventHandler<EventArgs> Action
		{
			add
			{
				this.statusIconBackend.Action += value;
			}
			remove
			{
				this.statusIconBackend.Action -= value;
			}
		}

		public string Tooltip
		{
			get
			{
				return this.statusIconBackend.Tooltip;
			}
			set
			{
				this.statusIconBackend.Tooltip = value;
			}
		}
	}
}

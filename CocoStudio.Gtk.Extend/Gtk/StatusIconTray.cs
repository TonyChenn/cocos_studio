using System;
using Xwt;
using Xwt.Backends;

namespace Gtk
{
	// Token: 0x0200006C RID: 108
	[BackendType(typeof(IStatusIconBackend))]
	public class StatusIconTray : XwtComponent, IStatusIcon
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000A218 File Offset: 0x00008418
		private IStatusIcon statusIconBackend
		{
			get
			{
				return base.BackendHost.Backend as IStatusIcon;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000A23C File Offset: 0x0000843C
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000A259 File Offset: 0x00008459
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000A26C File Offset: 0x0000846C
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000A289 File Offset: 0x00008489
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

		// Token: 0x06000264 RID: 612 RVA: 0x0000A299 File Offset: 0x00008499
		protected override void Dispose(bool disposing)
		{
			this.statusIconBackend.Dispose();
			base.Dispose(disposing);
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000265 RID: 613 RVA: 0x0000A2B0 File Offset: 0x000084B0
		// (remove) Token: 0x06000266 RID: 614 RVA: 0x0000A2C0 File Offset: 0x000084C0
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000A2D0 File Offset: 0x000084D0
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000A2ED File Offset: 0x000084ED
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

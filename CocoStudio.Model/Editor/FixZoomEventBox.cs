using System;
using Gdk;
using Gtk;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000058 RID: 88
	public class FixZoomEventBox : EventBox
	{
		// Token: 0x0600030D RID: 781 RVA: 0x0000C4FB File Offset: 0x0000A6FB
		public FixZoomEventBox()
		{
			base.Add(this.left);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000C534 File Offset: 0x0000A734
		protected override bool OnButtonReleaseEvent(EventButton evnt)
		{
			if (evnt.Button == 1U && this.CanStrenth)
			{
				base.IsFocus = true;
				if (18.0 <= evnt.X && evnt.X <= 25.0)
				{
					this.isGayVer = !this.isGayVer;
					this.left.VType = (this.isGayVer ? 0 : 1);
					this.left.QueueDraw();
					if (this.HEventChanged != null)
					{
						this.HEventChanged(this, new FixZoomEventArgs(!this.isGayVer));
					}
					return base.OnButtonReleaseEvent(evnt);
				}
				if (18.0 <= evnt.Y && evnt.Y <= 25.0)
				{
					this.isGayHer = !this.isGayHer;
					this.left.HType = (this.isGayHer ? 0 : 1);
					this.left.QueueDraw();
					if (this.VEventChanged != null)
					{
						this.VEventChanged(this, new FixZoomEventArgs(!this.isGayHer));
					}
				}
			}
			return base.OnButtonReleaseEvent(evnt);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000C69C File Offset: 0x0000A89C
		public void SetLine(bool h, bool v)
		{
			this.isGayHer = !h;
			this.isGayVer = !v;
			this.left.VType = (this.isGayVer ? 0 : 1);
			this.left.HType = (this.isGayHer ? 0 : 1);
			this.left.QueueDraw();
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000C6FC File Offset: 0x0000A8FC
		// (set) Token: 0x06000311 RID: 785 RVA: 0x0000C714 File Offset: 0x0000A914
		public bool CanStrenth
		{
			get
			{
				return this.canStrenth;
			}
			set
			{
				this.canStrenth = value;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000312 RID: 786 RVA: 0x0000C720 File Offset: 0x0000A920
		// (remove) Token: 0x06000313 RID: 787 RVA: 0x0000C75C File Offset: 0x0000A95C
		public event EventHandler<FixZoomEventArgs> HEventChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000314 RID: 788 RVA: 0x0000C798 File Offset: 0x0000A998
		// (remove) Token: 0x06000315 RID: 789 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		public event EventHandler<FixZoomEventArgs> VEventChanged;

		// Token: 0x0400016B RID: 363
		private FixZoomDrawArea left = new FixZoomDrawArea();

		// Token: 0x0400016C RID: 364
		private bool isGayHer = true;

		// Token: 0x0400016D RID: 365
		private bool isGayVer = true;

		// Token: 0x0400016E RID: 366
		private bool canStrenth = true;
	}
}

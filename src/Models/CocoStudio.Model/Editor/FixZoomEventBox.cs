using System;
using Gdk;
using Gtk;

namespace CocoStudio.Model.Editor
{
	public class FixZoomEventBox : EventBox
	{
		public FixZoomEventBox()
		{
			base.Add(this.left);
		}

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

		public void SetLine(bool h, bool v)
		{
			this.isGayHer = !h;
			this.isGayVer = !v;
			this.left.VType = (this.isGayVer ? 0 : 1);
			this.left.HType = (this.isGayHer ? 0 : 1);
			this.left.QueueDraw();
		}

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

		public event EventHandler<FixZoomEventArgs> HEventChanged;

		public event EventHandler<FixZoomEventArgs> VEventChanged;

		private FixZoomDrawArea left = new FixZoomDrawArea();

		private bool isGayHer = true;

		private bool isGayVer = true;

		private bool canStrenth = true;
	}
}

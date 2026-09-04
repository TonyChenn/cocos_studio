using System;
using GLib;
using MonoDevelop.Core;

namespace Gtk
{
	// Token: 0x02000091 RID: 145
	public class WidgetAnimationModel
	{
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000316 RID: 790 RVA: 0x0000CA90 File Offset: 0x0000AC90
		// (remove) Token: 0x06000317 RID: 791 RVA: 0x0000CACC File Offset: 0x0000ACCC
		public event EventHandler<EndAnimationClickEventArgs> EndAnimationClick;

		// Token: 0x06000319 RID: 793 RVA: 0x0000CC50 File Offset: 0x0000AE50
		public void StartPositionAnimation(Window window, int end_x, int end_y)
		{
			double t = 0.0;
			int start_x;
			int start_y;
			window.GetPosition(out start_x, out start_y);
			double cp_x = (double)((end_x + start_x) / 2);
			double cp_y = (double)(start_y - 350);
			double tempnum = 0.015;
			if (Platform.IsMac)
			{
				tempnum = 0.02;
			}
			Timeout.Add(6U, delegate
			{
				double num = (double)start_x + (cp_x - (double)start_x) * t;
				double num2 = (double)start_y + (cp_y - (double)start_y) * t;
				double num3 = cp_x + ((double)end_x - cp_x) * t;
				double num4 = cp_y + ((double)end_y - cp_y) * t;
				double d = num + (num3 - num) * t;
				double d2 = num2 + (num4 - num2) * t;
				int x = (int)Math.Floor(d);
				int y = (int)Math.Floor(d2);
				window.Move(x, y);
				bool result;
				if (t >= 1.0)
				{
					if (this.EndAnimationClick != null)
					{
						this.EndAnimationClick(this, new EndAnimationClickEventArgs(window));
					}
					result = false;
				}
				else
				{
					t += tempnum;
					result = true;
				}
				return result;
			});
		}
	}
}

using System;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x0200000E RID: 14
	public class IconTrackPointButton : IconButton
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003688 File Offset: 0x00001888
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000036A0 File Offset: 0x000018A0
		public bool IsShowTrackPoint
		{
			get
			{
				return this._IsShowTrackPoint;
			}
			set
			{
				this._IsShowTrackPoint = value;
				if (this._IsShowTrackPoint)
				{
					this.trackPntImg.PointType = TrackPointType.Point;
				}
				else
				{
					this.trackPntImg.PointType = TrackPointType.None;
				}
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000036E0 File Offset: 0x000018E0
		public IconTrackPointButton(Image icon)
		{
			this.normalIcon = icon;
			this.trackPntImg = new TrackPointImage();
			this.trackPntImg.SetImage(icon);
			this.imgView = this.trackPntImg.CurrentImageView;
			this.bgBox.Add(this.trackPntImg);
			this.OnSetStyle();
			this.OnRefreshUI();
			base.ShowAll();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003750 File Offset: 0x00001950
		protected override void OnRefreshUI()
		{
			base.OnRefreshUI();
			if (this.trackPntImg != null)
			{
				if (base.State == StateType.Insensitive)
				{
					this.trackPntImg.PointType = TrackPointType.None;
				}
				else
				{
					this.IsShowTrackPoint = this.IsShowTrackPoint;
				}
			}
		}

		// Token: 0x0400002A RID: 42
		private TrackPointImage trackPntImg;

		// Token: 0x0400002B RID: 43
		private bool _IsShowTrackPoint;
	}
}

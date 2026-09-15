using System;
using Xwt.Drawing;

namespace Gtk
{
	public class IconTrackPointButton : IconButton
	{
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

		public IconTrackPointButton(Xwt.Drawing.Image icon)
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

		private TrackPointImage trackPntImg;

		private bool _IsShowTrackPoint;
	}
}

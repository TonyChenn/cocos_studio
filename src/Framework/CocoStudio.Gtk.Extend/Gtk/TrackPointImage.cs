using System;
using System.ComponentModel;
using MonoDevelop.Components;
using Stetic;
using Xwt.Drawing;

namespace Gtk
{
	[ToolboxItem(true)]
	public class TrackPointImage : Bin
	{
		public ImageView CurrentImageView { get; private set; }

		public TrackPointType PointType
		{
			get
			{
				return this._PointType;
			}
			set
			{
				if (this._PointType != value)
				{
					this._PointType = value;
					this.SetTrackPointImage(this._PointType);
				}
			}
		}

		public TrackPointImage()
		{
			this.Build();
			base.SizeAllocated += this.SizeAllocatedHandler;
		}

		public void SetImage(Xwt.Drawing.Image image)
		{
			this.alignment_mainImage.RemoveChild();
			if (image == null)
			{
				base.WidthRequest = 0;
				base.HeightRequest = 0;
			}
			else
			{
				this.CurrentImageView = new ImageView(image);
				this.alignment_mainImage.Add(this.CurrentImageView);
				base.WidthRequest = (int)image.Width;
				base.HeightRequest = (int)image.Height;
				base.ShowAll();
			}
		}

		private void SetTrackPointImage(TrackPointType type)
		{
			if (this._PointType == TrackPointType.None)
			{
				this.alignment_point.RemoveChild();
			}
			else
			{
				string resourceID = string.Format("CocoStudio.DefaultResource.Images.TrackPoint.{0}.png", this._PointType);
                Xwt.Drawing.Image icon = ImageIcon.GetIcon(resourceID);
				this.alignment_point.Add(new ImageView(icon));
				this.alignment_point.WidthRequest = (int)icon.Width;
				this.alignment_point.HeightRequest = (int)icon.Height;
				Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.fixed_main[this.alignment_point];
				fixedChild.X = this.fixed_main.Allocation.Width - this.alignment_point.WidthRequest;
				this.alignment_point.ShowAll();
			}
		}

		private void SizeAllocatedHandler(object o, SizeAllocatedArgs args)
		{
			this.alignment_mainImage.WidthRequest = this.fixed_main.Allocation.Width;
			this.alignment_mainImage.HeightRequest = this.fixed_main.Allocation.Height;
			Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.fixed_main[this.alignment_point];
			int num = this.fixed_main.Allocation.Width - this.alignment_point.WidthRequest;
			if (fixedChild.X != num)
			{
				fixedChild.X = num;
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Gtk.TrackPointImage";
			this.fixed_main = new Fixed();
			this.fixed_main.Name = "fixed_main";
			this.fixed_main.HasWindow = false;
			this.alignment_mainImage = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_mainImage.WidthRequest = 100;
			this.alignment_mainImage.HeightRequest = 100;
			this.alignment_mainImage.Name = "alignment_mainImage";
			this.fixed_main.Add(this.alignment_mainImage);
			this.alignment_point = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_point.WidthRequest = 22;
			this.alignment_point.HeightRequest = 9;
			this.alignment_point.Name = "alignment_point";
			this.fixed_main.Add(this.alignment_point);
			Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.fixed_main[this.alignment_point];
			fixedChild.X = 78;
			base.Add(this.fixed_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private TrackPointType _PointType = TrackPointType.None;

		private Fixed fixed_main;

		private Alignment alignment_mainImage;

		private Alignment alignment_point;
	}
}

using System;
using System.ComponentModel;
using MonoDevelop.Components;
using Stetic;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x020000A7 RID: 167
	[ToolboxItem(true)]
	public class TrackPointImage : Bin
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00012F10 File Offset: 0x00011110
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00012F27 File Offset: 0x00011127
		public ImageView CurrentImageView { get; private set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00012F30 File Offset: 0x00011130
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00012F48 File Offset: 0x00011148
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

		// Token: 0x060003AF RID: 943 RVA: 0x00012F7C File Offset: 0x0001117C
		public TrackPointImage()
		{
			this.Build();
			base.SizeAllocated += this.SizeAllocatedHandler;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00012FA8 File Offset: 0x000111A8
		public void SetImage(Image image)
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

		// Token: 0x060003B1 RID: 945 RVA: 0x00013024 File Offset: 0x00011224
		private void SetTrackPointImage(TrackPointType type)
		{
			if (this._PointType == TrackPointType.None)
			{
				this.alignment_point.RemoveChild();
			}
			else
			{
				string resourceID = string.Format("CocoStudio.DefaultResource.Images.TrackPoint.{0}.png", this._PointType);
				Image icon = ImageIcon.GetIcon(resourceID);
				this.alignment_point.Add(new ImageView(icon));
				this.alignment_point.WidthRequest = (int)icon.Width;
				this.alignment_point.HeightRequest = (int)icon.Height;
				Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.fixed_main[this.alignment_point];
				fixedChild.X = this.fixed_main.Allocation.Width - this.alignment_point.WidthRequest;
				this.alignment_point.ShowAll();
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x000130F0 File Offset: 0x000112F0
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

		// Token: 0x060003B3 RID: 947 RVA: 0x00013180 File Offset: 0x00011380
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

		// Token: 0x04000453 RID: 1107
		private TrackPointType _PointType = TrackPointType.None;

		// Token: 0x04000454 RID: 1108
		private Fixed fixed_main;

		// Token: 0x04000455 RID: 1109
		private Alignment alignment_mainImage;

		// Token: 0x04000456 RID: 1110
		private Alignment alignment_point;
	}
}

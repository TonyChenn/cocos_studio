using System;
using MonoDevelop.Core;

namespace Gtk
{
	public class ImageLabelButton : ImageButton
	{
		public Label Label
		{
			get
			{
				return this.label_display;
			}
		}

		public string LabelText
		{
			get
			{
				return this.Label.Text;
			}
			set
			{
				this.Label.Text = value;
			}
		}

		public ImageLabelButton()
		{
			this.RemoveChild();
			this.fixed_base = new Fixed();
			this.fixed_base.HasWindow = false;
			this.alignment_image = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.fixed_base.Add(this.alignment_image);
			this.alignment_label = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.fixed_base.Add(this.alignment_label);
			base.Add(this.fixed_base);
			this.fixed_base.SizeAllocated += this.FixedSizeAllocatedHandler;
			this.alignment_image.Add(this.imageView);
			this.label_display = new Label();
			if (Platform.IsWindows)
			{
				this.label_display.SetFontSize(13.0);
			}
			else
			{
				this.label_display.SetFontSize(12.0);
			}
			this.alignment_label.Add(this.label_display);
			base.Child.ShowAll();
			base.RefreshUI();
		}

		protected override void OnRefreshUI()
		{
			base.OnRefreshUI();
			Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.fixed_base[this.alignment_label];
			if (base.Sensitive && base.CurrentState == ButtonState.Pressed)
			{
				fixedChild.X = (fixedChild.Y = 3);
			}
			else
			{
				fixedChild.X = (fixedChild.Y = 2);
			}
		}

		protected void FixedSizeAllocatedHandler(object o, SizeAllocatedArgs args)
		{
			int width = args.Allocation.Width;
			int height = args.Allocation.Height;
			this.alignment_image.WidthRequest = width;
			this.alignment_image.HeightRequest = height;
			int num = width - 4;
			int num2 = height - 4;
			if (num < 1)
			{
				num = 1;
			}
			if (num2 < 1)
			{
				num2 = 1;
			}
			this.alignment_label.WidthRequest = num;
			this.alignment_label.HeightRequest = num2;
		}

		private Fixed fixed_base;

		private Alignment alignment_image;

		private Alignment alignment_label;

		private Label label_display;
	}
}

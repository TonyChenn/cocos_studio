using System;
using MonoDevelop.Core;

namespace Gtk
{
	// Token: 0x02000011 RID: 17
	public class ImageLabelButton : ImageButton
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003A74 File Offset: 0x00001C74
		public Label Label
		{
			get
			{
				return this.label_display;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003A8C File Offset: 0x00001C8C
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003AA9 File Offset: 0x00001CA9
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

		// Token: 0x0600007C RID: 124 RVA: 0x00003ABC File Offset: 0x00001CBC
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

		// Token: 0x0600007D RID: 125 RVA: 0x00003BF4 File Offset: 0x00001DF4
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

		// Token: 0x0600007E RID: 126 RVA: 0x00003C68 File Offset: 0x00001E68
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

		// Token: 0x0400002E RID: 46
		private Fixed fixed_base;

		// Token: 0x0400002F RID: 47
		private Alignment alignment_image;

		// Token: 0x04000030 RID: 48
		private Alignment alignment_label;

		// Token: 0x04000031 RID: 49
		private Label label_display;
	}
}

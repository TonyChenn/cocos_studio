using System;
using System.ComponentModel;
using CocoStudio.Basic;
using MonoDevelop.Components;
using Stetic;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x0200008A RID: 138
	[ToolboxItem(true)]
	public class ImageBin : Bin
	{
		// Token: 0x060002FC RID: 764 RVA: 0x0000C29E File Offset: 0x0000A49E
		public ImageBin()
		{
			this.Build();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000C2B0 File Offset: 0x0000A4B0
		public void SetImageView(Image image)
		{
			try
			{
				this.alignment_img.RemoveChild();
				if (image != null)
				{
					this.CurrentImage = new ImageView(image);
					this.alignment_img.Add(this.CurrentImage);
					this.CurrentImage.Show();
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("设置图片时出错", exception);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000C330 File Offset: 0x0000A530
		public ImageView GetImageView()
		{
			return this.CurrentImage;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000C348 File Offset: 0x0000A548
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Gtk.ImageBin";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.alignment_top = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_top.Name = "alignment_top";
			this.vbox_main.Add(this.alignment_top);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.alignment_top];
			boxChild.Position = 0;
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.alignment_left = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_left.Name = "alignment_left";
			this.hbox_main.Add(this.alignment_left);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_main[this.alignment_left];
			boxChild2.Position = 0;
			this.alignment_img = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_img.Name = "alignment_img";
			this.hbox_main.Add(this.alignment_img);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_main[this.alignment_img];
			boxChild3.Position = 1;
			this.alignment_right = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_right.Name = "alignment_right";
			this.hbox_main.Add(this.alignment_right);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_main[this.alignment_right];
			boxChild4.Position = 2;
			this.vbox_main.Add(this.hbox_main);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_main[this.hbox_main];
			boxChild5.Position = 1;
			this.alignment_bottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bottom.Name = "alignment_bottom";
			this.vbox_main.Add(this.alignment_bottom);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_main[this.alignment_bottom];
			boxChild6.Position = 2;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x04000370 RID: 880
		private ImageView CurrentImage;

		// Token: 0x04000371 RID: 881
		private VBox vbox_main;

		// Token: 0x04000372 RID: 882
		private Alignment alignment_top;

		// Token: 0x04000373 RID: 883
		private HBox hbox_main;

		// Token: 0x04000374 RID: 884
		private Alignment alignment_left;

		// Token: 0x04000375 RID: 885
		private Alignment alignment_img;

		// Token: 0x04000376 RID: 886
		private Alignment alignment_right;

		// Token: 0x04000377 RID: 887
		private Alignment alignment_bottom;
	}
}

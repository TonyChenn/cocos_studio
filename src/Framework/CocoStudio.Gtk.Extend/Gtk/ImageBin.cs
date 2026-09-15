using System;
using System.ComponentModel;
using CocoStudio.Basic;
using MonoDevelop.Components;
using Stetic;
using MonoDevelopImageView = MonoDevelop.Components.ImageView;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	[ToolboxItem(true)]
	public class ImageBin : Bin
	{
		public ImageBin()
		{
			this.Build();
		}

		public void SetImageView(XwtImage image)
		{
			try
			{
				this.alignment_img.RemoveChild();
				if (image != null)
				{
					this.CurrentImage = new MonoDevelopImageView(image);
					this.alignment_img.Add(this.CurrentImage);
					this.CurrentImage.Show();
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("设置图片时出错", exception);
			}
		}

		public MonoDevelopImageView GetImageView()
		{
			return this.CurrentImage;
		}

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

		private MonoDevelopImageView CurrentImage;

		private VBox vbox_main;

		private Alignment alignment_top;

		private HBox hbox_main;

		private Alignment alignment_left;

		private Alignment alignment_img;

		private Alignment alignment_right;

		private Alignment alignment_bottom;
	}
}

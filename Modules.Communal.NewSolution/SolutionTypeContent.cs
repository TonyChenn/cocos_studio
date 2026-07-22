using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Components;
using Pango;
using Stetic;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	// Token: 0x0200001D RID: 29
	[ToolboxItem(true)]
	public class SolutionTypeContent : Bin, IRadioItemContent
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00007BCC File Offset: 0x00005DCC
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00007BD4 File Offset: 0x00005DD4
		public SolutionTypeInfo Info { get; private set; }

		// Token: 0x060000DF RID: 223 RVA: 0x00007BDD File Offset: 0x00005DDD
		public SolutionTypeContent()
		{
			throw new Exception();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007BEC File Offset: 0x00005DEC
		public SolutionTypeContent(SolutionTypeInfo info)
		{
			this.Build();
			this.Info = info;
			this.label_title.Text = this.Info.Name;
			this.label_title.LineWrapMode = Pango.WrapMode.WordChar;
			this.label_title.SetFontSize((double)LanguageAdapter.GetDefaultFontSize());
			Xwt.Drawing.Image image;
			if (info.Image != null)
			{
				image = info.Image;
			}
			else
			{
				image = ImageIcon.GetIcon("Modules.Communal.NewSolution.Resource.ItemIcon_Default.png");
			}
			ImageView imageView = new ImageView(image);
			this.alignment_imgBorder.Add(imageView);
			imageView.Show();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00007C74 File Offset: 0x00005E74
		public Widget GetGtkWidget()
		{
			return this;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007C77 File Offset: 0x00005E77
		public void RefreshUI(bool isSelect, ButtonState currentState)
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007C7C File Offset: 0x00005E7C
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.NewSolution.SolutionTypeContent";
			this.vbox_main = new VBox();
			this.vbox_main.WidthRequest = 94;
			this.vbox_main.Name = "vbox_main";
			this.alignment_image = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_image.Name = "alignment_image";
			this.alignment_image.TopPadding = 10U;
			this.vbox_image = new VBox();
			this.vbox_image.HeightRequest = 52;
			this.vbox_image.Name = "vbox_image";
			this.alignment_imageTop = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageTop.Name = "alignment_imageTop";
			this.vbox_image.Add(this.alignment_imageTop);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_image[this.alignment_imageTop];
			boxChild.Position = 0;
			this.hbox_image = new HBox();
			this.hbox_image.Name = "hbox_image";
			this.alignment_imageLeft = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageLeft.Name = "alignment_imageLeft";
			this.hbox_image.Add(this.alignment_imageLeft);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_image[this.alignment_imageLeft];
			boxChild2.Position = 0;
			this.alignment_imgBorder = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imgBorder.Name = "alignment_imgBorder";
			this.hbox_image.Add(this.alignment_imgBorder);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_image[this.alignment_imgBorder];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			this.alignment_imageRight = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageRight.Name = "alignment_imageRight";
			this.hbox_image.Add(this.alignment_imageRight);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_image[this.alignment_imageRight];
			boxChild4.Position = 2;
			this.vbox_image.Add(this.hbox_image);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_image[this.hbox_image];
			boxChild5.Position = 1;
			this.alignment_imageBottom = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_imageBottom.Name = "alignment_imageBottom";
			this.vbox_image.Add(this.alignment_imageBottom);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_image[this.alignment_imageBottom];
			boxChild6.Position = 2;
			this.alignment_image.Add(this.vbox_image);
			this.vbox_main.Add(this.alignment_image);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_main[this.alignment_image];
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			this.hbox_title = new HBox();
			this.hbox_title.Name = "hbox_title";
			this.alignment_left = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_left.Name = "alignment_left";
			this.hbox_title.Add(this.alignment_left);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox_title[this.alignment_left];
			boxChild8.Position = 0;
			this.alignment_title = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_title.Name = "alignment_title";
			this.alignment_title.TopPadding = 5U;
			this.alignment_title.BottomPadding = 6U;
			this.label_title = new Label();
			this.label_title.WidthRequest = 94;
			this.label_title.Name = "label_title";
			this.label_title.LabelProp = Catalog.GetString("空白完整项目");
			this.label_title.Wrap = true;
			this.label_title.Justify = Justification.Center;
			this.label_title.WidthChars = 88;
			this.alignment_title.Add(this.label_title);
			this.hbox_title.Add(this.alignment_title);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_title[this.alignment_title];
			boxChild9.Position = 1;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.alignment_right = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_right.Name = "alignment_right";
			this.hbox_title.Add(this.alignment_right);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_title[this.alignment_right];
			boxChild10.Position = 2;
			this.vbox_main.Add(this.hbox_title);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_main[this.hbox_title];
			boxChild11.Position = 1;
			boxChild11.Expand = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x040000A6 RID: 166
		private VBox vbox_main;

		// Token: 0x040000A7 RID: 167
		private Gtk.Alignment alignment_image;

		// Token: 0x040000A8 RID: 168
		private VBox vbox_image;

		// Token: 0x040000A9 RID: 169
		private Gtk.Alignment alignment_imageTop;

		// Token: 0x040000AA RID: 170
		private HBox hbox_image;

		// Token: 0x040000AB RID: 171
		private Gtk.Alignment alignment_imageLeft;

		// Token: 0x040000AC RID: 172
		private Gtk.Alignment alignment_imgBorder;

		// Token: 0x040000AD RID: 173
		private Gtk.Alignment alignment_imageRight;

		// Token: 0x040000AE RID: 174
		private Gtk.Alignment alignment_imageBottom;

		// Token: 0x040000AF RID: 175
		private HBox hbox_title;

		// Token: 0x040000B0 RID: 176
		private Gtk.Alignment alignment_left;

		// Token: 0x040000B1 RID: 177
		private Gtk.Alignment alignment_title;

		// Token: 0x040000B2 RID: 178
		private Label label_title;

		// Token: 0x040000B3 RID: 179
		private Gtk.Alignment alignment_right;
	}
}

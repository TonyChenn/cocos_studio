using System;
using System.ComponentModel;
using Gdk;
using Gtk;
using Xwt.Drawing;

namespace Cocos.Launcher.Control
{
	// Token: 0x0200000E RID: 14
	[ToolboxItem(true)]
	public class LinkView : EventBox
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000085 RID: 133 RVA: 0x000030C0 File Offset: 0x000012C0
		// (remove) Token: 0x06000086 RID: 134 RVA: 0x000030F8 File Offset: 0x000012F8
		public event EventHandler<LinkClickedEventArgs> LinkClicked;

		// Token: 0x06000087 RID: 135 RVA: 0x0000312D File Offset: 0x0000132D
		public LinkView()
		{
			this.label_display = new Label();
			this.label_display.Xalign = 0f;
			base.Add(this.label_display);
			this.InitEvent();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003164 File Offset: 0x00001364
		public LinkView(string imagePath)
		{
			base.VisibleWindow = false;
			HBox hbox = new HBox();
			hbox.Spacing = 8;
			ImageBin imageBin = new ImageBin();
			imageBin.SetImageView(ImageIcon.GetIcon(imagePath));
			hbox.Add(imageBin);
			Box.BoxChild boxChild = (Box.BoxChild)hbox[imageBin];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.label_display = new Label();
			this.label_display.Xalign = 0f;
			this.label_display.Yalign = 0.5f;
			hbox.Add(this.label_display);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox[this.label_display];
			boxChild2.Position = 1;
			boxChild2.Expand = true;
			boxChild2.Fill = true;
			base.Add(hbox);
			this.InitEvent();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003231 File Offset: 0x00001431
		public LinkView(Xwt.Drawing.Image imagePath)
		{
			this.imageBin = new ImageBin();
			this.imageBin.SetImageView(imagePath);
			base.Add(this.imageBin);
			this.InitEvent();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003264 File Offset: 0x00001464
		public LinkView(Xwt.Drawing.Image imagePath, int width, int height)
		{
			double scaleX = (double)width / imagePath.Width;
			double scaleY = (double)height / imagePath.Height;
			this.imageBin = new ImageBin();
			this.imageBin.SetImageView(imagePath.Scale(scaleX, scaleY));
			base.Add(this.imageBin);
			base.HeightRequest = height;
			base.WidthRequest = width;
			this.InitEvent();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000032CC File Offset: 0x000014CC
		public void SetImageView(string imagePath)
		{
			Xwt.Drawing.Image iconFromFile = ImageIcon.GetIconFromFile(imagePath.ToString());
			ImageBin imageBin = new ImageBin();
			imageBin.SetImageView(iconFromFile);
			base.Add(imageBin);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000032F9 File Offset: 0x000014F9
		public void SetImage(Xwt.Drawing.Image image)
		{
			this.imageBin.SetImageView(image);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003307 File Offset: 0x00001507
		private void InitEvent()
		{
			base.EnterNotifyEvent += this.LauncherLink_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.LauncherLink_LeaveNotifyEvent;
			base.ButtonReleaseEvent += this.LauncherLink_ButtonReleaseEvent;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000333F File Offset: 0x0000153F
		private void LauncherLink_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (this.LinkClicked != null && args.Event.Button == 1U)
			{
				this.LinkClicked(o, new LinkClickedEventArgs(this.Tag));
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000336E File Offset: 0x0000156E
		private void LauncherLink_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = null;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000337C File Offset: 0x0000157C
		private void LauncherLink_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003390 File Offset: 0x00001590
		public void SetLableText(string text)
		{
			this.label_display.Text = text;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000339E File Offset: 0x0000159E
		public void SetLabelAlign(float x, float y)
		{
			this.label_display.Xalign = x;
			this.label_display.Yalign = y;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000033B8 File Offset: 0x000015B8
		public void SetLabelSize(int width, int height)
		{
			base.SetSizeRequest(width, height);
			this.label_display.SetSizeRequest(width, height);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000033CF File Offset: 0x000015CF
		public void SetWidthRequest(int width)
		{
			base.WidthRequest = width;
			this.label_display.WidthRequest = width;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000033E4 File Offset: 0x000015E4
		public void SetHeightRequest(int height)
		{
			base.HeightRequest = height;
			this.label_display.HeightRequest = height;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000033F9 File Offset: 0x000015F9
		public void SetTooltip(string tooltip)
		{
			base.TooltipText = tooltip;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003402 File Offset: 0x00001602
		public void SetFontSize(double size)
		{
			this.label_display.SetFontSize(size);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003410 File Offset: 0x00001610
		public void SetBackGroundColor(Gdk.Color bgColor)
		{
			base.ModifyBg(StateType.Normal, bgColor);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000341A File Offset: 0x0000161A
		public void SetForeGroundColor(Gdk.Color fgColor)
		{
			this.label_display.ModifyFg(StateType.Normal, fgColor);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003429 File Offset: 0x00001629
		public void SetLinkText(string text)
		{
			this.SetForeGroundColor(ConstantConfig.Colors.TabFontPressColor);
			this.SetLableText(text);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003442 File Offset: 0x00001642
		public void SetTag(object tag)
		{
			this.Tag = tag;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000344B File Offset: 0x0000164B
		public object GetTag()
		{
			return this.Tag;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003453 File Offset: 0x00001653
		public string GetLableText()
		{
			return this.label_display.Text;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003460 File Offset: 0x00001660
		public void SetLableTextWrap(bool wrap)
		{
			this.label_display.Wrap = wrap;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000346E File Offset: 0x0000166E
		internal void SetLableTextWidth(int width)
		{
			this.label_display.WidthRequest = width;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000347C File Offset: 0x0000167C
		public void SetLableTextHeight(int height)
		{
			this.label_display.HeightRequest = height;
		}

		// Token: 0x0400004A RID: 74
		private Label label_display;

		// Token: 0x0400004B RID: 75
		private object Tag;

		// Token: 0x0400004D RID: 77
		private ImageBin imageBin;
	}
}

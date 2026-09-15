using System;
using System.ComponentModel;
using Gdk;
using Gtk;
using Xwt.Drawing;

namespace Cocos.Launcher.Control
{
	[ToolboxItem(true)]
	public class LinkView : EventBox
	{
		public event EventHandler<LinkClickedEventArgs> LinkClicked;

		public LinkView()
		{
			this.label_display = new Label();
			this.label_display.Xalign = 0f;
			base.Add(this.label_display);
			this.InitEvent();
		}

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

		public LinkView(Xwt.Drawing.Image imagePath)
		{
			this.imageBin = new ImageBin();
			this.imageBin.SetImageView(imagePath);
			base.Add(this.imageBin);
			this.InitEvent();
		}

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

		public void SetImageView(string imagePath)
		{
			Xwt.Drawing.Image iconFromFile = ImageIcon.GetIconFromFile(imagePath.ToString());
			ImageBin imageBin = new ImageBin();
			imageBin.SetImageView(iconFromFile);
			base.Add(imageBin);
		}

		public void SetImage(Xwt.Drawing.Image image)
		{
			this.imageBin.SetImageView(image);
		}

		private void InitEvent()
		{
			base.EnterNotifyEvent += this.LauncherLink_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.LauncherLink_LeaveNotifyEvent;
			base.ButtonReleaseEvent += this.LauncherLink_ButtonReleaseEvent;
		}

		private void LauncherLink_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (this.LinkClicked != null && args.Event.Button == 1U)
			{
				this.LinkClicked(o, new LinkClickedEventArgs(this.Tag));
			}
		}

		private void LauncherLink_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = null;
		}

		private void LauncherLink_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
		}

		public void SetLableText(string text)
		{
			this.label_display.Text = text;
		}

		public void SetLabelAlign(float x, float y)
		{
			this.label_display.Xalign = x;
			this.label_display.Yalign = y;
		}

		public void SetLabelSize(int width, int height)
		{
			base.SetSizeRequest(width, height);
			this.label_display.SetSizeRequest(width, height);
		}

		public void SetWidthRequest(int width)
		{
			base.WidthRequest = width;
			this.label_display.WidthRequest = width;
		}

		public void SetHeightRequest(int height)
		{
			base.HeightRequest = height;
			this.label_display.HeightRequest = height;
		}

		public void SetTooltip(string tooltip)
		{
			base.TooltipText = tooltip;
		}

		public void SetFontSize(double size)
		{
			this.label_display.SetFontSize(size);
		}

		public void SetBackGroundColor(Gdk.Color bgColor)
		{
			base.ModifyBg(StateType.Normal, bgColor);
		}

		public void SetForeGroundColor(Gdk.Color fgColor)
		{
			this.label_display.ModifyFg(StateType.Normal, fgColor);
		}

		public void SetLinkText(string text)
		{
			this.SetForeGroundColor(ConstantConfig.Colors.TabFontPressColor);
			this.SetLableText(text);
		}

		public void SetTag(object tag)
		{
			this.Tag = tag;
		}

		public object GetTag()
		{
			return this.Tag;
		}

		public string GetLableText()
		{
			return this.label_display.Text;
		}

		public void SetLableTextWrap(bool wrap)
		{
			this.label_display.Wrap = wrap;
		}

		internal void SetLableTextWidth(int width)
		{
			this.label_display.WidthRequest = width;
		}

		public void SetLableTextHeight(int height)
		{
			this.label_display.HeightRequest = height;
		}

		private Label label_display;

		private object Tag;

		private ImageBin imageBin;
	}
}

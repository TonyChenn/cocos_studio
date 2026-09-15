using System;
using System.ComponentModel;
using Gdk;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using Stetic;
using Xwt.Drawing;

namespace Gtk
{
	[ToolboxItem(true)]
	public class CustomTitleBar : Bin
	{
		public string Title
		{
			get
			{
				return this.label_title.Text;
			}
			set
			{
				this.label_title.Text = value;
			}
		}

		public event EventHandler<EventArgs> CloseClicked;

		public CustomTitleBar()
		{
			this.Build();
			if (Platform.IsMac)
			{
				ImageButton imageButton = new ImageButton();
				imageButton.WidthRequest = (imageButton.HeightRequest = 16);
				imageButton.NormalImage = ImageIcon.GetIcon("CocoStudio.DefaultResource.WindowResource.macClose_normal.png");
				imageButton.HoverImage = ImageIcon.GetIcon("CocoStudio.DefaultResource.WindowResource.macClose_hover.png");
				imageButton.PressedImage = ImageIcon.GetIcon("CocoStudio.DefaultResource.WindowResource.macClose_pressed.png");
				imageButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.OnCloseButtonClicked);
				this.alignment_macCloseBtn.Add(imageButton);
				imageButton.Show();
			}
			else if (Platform.IsWindows)
			{
				ImageButton imageButton2 = new ImageButton();
				imageButton2.WidthRequest = 47;
				imageButton2.HeightRequest = 18;
				imageButton2.NormalImage = ImageIcon.GetIcon("CocoStudio.DefaultResource.WindowResource.winClose_normal.png");
				imageButton2.HoverImage = ImageIcon.GetIcon("CocoStudio.DefaultResource.WindowResource.winClose_hover.png");
				imageButton2.PressedImage = ImageIcon.GetIcon("CocoStudio.DefaultResource.WindowResource.winClose_pressed.png");
				imageButton2.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.OnCloseButtonClicked);
				this.alignment_winCloseBtn.Add(imageButton2);
				imageButton2.Show();
			}
			this.label_title.ModifyFg(StateType.Normal, new Gdk.Color(byte.MaxValue, byte.MaxValue, byte.MaxValue));
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("CocoStudio.DefaultResource.WindowResource.customTitleBg.png");
			ImageView imageView = new ImageView(icon);
			this.alignment_bg.Add(imageView);
			imageView.Show();
		}

		public void SetParentWindow(Window parentWnd)
		{
			this.parentWindow = parentWnd;
		}

		public void DisableClose()
		{
			this.alignment_macCloseBtn.RemoveChild();
			this.alignment_winCloseBtn.RemoveChild();
		}

		protected void OnMousePressed(object o, ButtonPressEventArgs args)
		{
			this.isDragging = true;
			this.pressedPosX = args.Event.X;
			this.pressedPosY = args.Event.Y;
		}

		protected void OnMouseReleased(object o, ButtonReleaseEventArgs args)
		{
			this.isDragging = false;
		}

		protected void OnMouseMove(object o, MotionNotifyEventArgs args)
		{
			if (this.isDragging)
			{
				double num = args.Event.X - this.pressedPosX;
				double num2 = args.Event.Y - this.pressedPosY;
				int num3;
				int num4;
				this.parentWindow.GdkWindow.GetOrigin(out num3, out num4);
				int x = (int)((double)num3 + num);
				int num5 = (int)((double)num4 + num2);
				if (Platform.IsMac)
				{
					if (num5 < 23)
					{
						num5 = 23;
					}
				}
				this.parentWindow.Move(x, num5);
			}
		}

		protected void OnFixedSizeAllocated(object o, SizeAllocatedArgs args)
		{
			int width = args.Allocation.Width;
			int height = args.Allocation.Height;
			this.hbox_main.WidthRequest = width;
			this.hbox_main.HeightRequest = height;
			this.alignment_bg.WidthRequest = width;
			this.alignment_bg.HeightRequest = height;
		}

		private void OnCloseButtonClicked(object sender, EventArgs args)
		{
			if (this.CloseClicked != null)
			{
				this.CloseClicked(this, new EventArgs());
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Gtk.CustomTitleBar";
			this.evtbx_base = new EventBox();
			this.evtbx_base.Name = "evtbx_base";
			this.fixed_main = new Fixed();
			this.fixed_main.Name = "fixed_main";
			this.fixed_main.HasWindow = false;
			this.alignment_bg = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_bg.WidthRequest = 300;
			this.alignment_bg.HeightRequest = 50;
			this.alignment_bg.Name = "alignment_bg";
			this.fixed_main.Add(this.alignment_bg);
			this.hbox_main = new HBox();
			this.hbox_main.WidthRequest = 300;
			this.hbox_main.HeightRequest = 50;
			this.hbox_main.Name = "hbox_main";
			this.vbox_macCloseBtn = new VBox();
			this.vbox_macCloseBtn.Name = "vbox_macCloseBtn";
			this.alignment_macCloseTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_macCloseTop.Name = "alignment_macCloseTop";
			this.vbox_macCloseBtn.Add(this.alignment_macCloseTop);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_macCloseBtn[this.alignment_macCloseTop];
			boxChild.Position = 0;
			this.alignment_macCloseBtn = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_macCloseBtn.WidthRequest = 0;
			this.alignment_macCloseBtn.Name = "alignment_macCloseBtn";
			this.alignment_macCloseBtn.LeftPadding = 5U;
			this.vbox_macCloseBtn.Add(this.alignment_macCloseBtn);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_macCloseBtn[this.alignment_macCloseBtn];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.alignment_macCloseBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_macCloseBottom.Name = "alignment_macCloseBottom";
			this.vbox_macCloseBtn.Add(this.alignment_macCloseBottom);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_macCloseBtn[this.alignment_macCloseBottom];
			boxChild3.Position = 2;
			this.hbox_main.Add(this.vbox_macCloseBtn);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_main[this.vbox_macCloseBtn];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.hbox_title = new HBox();
			this.hbox_title.Name = "hbox_title";
			this.hbox_title.Spacing = 6;
			this.alignment_titleLeft = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_titleLeft.Name = "alignment_titleLeft";
			this.hbox_title.Add(this.alignment_titleLeft);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_title[this.alignment_titleLeft];
			boxChild5.Position = 0;
			this.label_title = new Label();
			this.label_title.Name = "label_title";
			this.label_title.LabelProp = Catalog.GetString("标题");
			this.hbox_title.Add(this.label_title);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_title[this.label_title];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.alignment_titleRight = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_titleRight.Name = "alignment_titleRight";
			this.hbox_title.Add(this.alignment_titleRight);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_title[this.alignment_titleRight];
			boxChild7.Position = 2;
			this.hbox_main.Add(this.hbox_title);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox_main[this.hbox_title];
			boxChild8.Position = 1;
			this.vbox_winCloseBtn = new VBox();
			this.vbox_winCloseBtn.Name = "vbox_winCloseBtn";
			this.alignment_winCloseBtn = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_winCloseBtn.Name = "alignment_winCloseBtn";
			this.alignment_winCloseBtn.RightPadding = 5U;
			this.vbox_winCloseBtn.Add(this.alignment_winCloseBtn);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_winCloseBtn[this.alignment_winCloseBtn];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.hbox_main.Add(this.vbox_winCloseBtn);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_main[this.vbox_winCloseBtn];
			boxChild10.Position = 2;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.fixed_main.Add(this.hbox_main);
			this.evtbx_base.Add(this.fixed_main);
			base.Add(this.evtbx_base);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.evtbx_base.ButtonPressEvent += this.OnMousePressed;
			this.evtbx_base.ButtonReleaseEvent += this.OnMouseReleased;
			this.evtbx_base.MotionNotifyEvent += this.OnMouseMove;
			this.fixed_main.SizeAllocated += this.OnFixedSizeAllocated;
		}

		private Window parentWindow;

		private bool isDragging = false;

		private double pressedPosX;

		private double pressedPosY;

		private EventBox evtbx_base;

		private Fixed fixed_main;

		private Alignment alignment_bg;

		private HBox hbox_main;

		private VBox vbox_macCloseBtn;

		private Alignment alignment_macCloseTop;

		private Alignment alignment_macCloseBtn;

		private Alignment alignment_macCloseBottom;

		private HBox hbox_title;

		private Alignment alignment_titleLeft;

		private Label label_title;

		private Alignment alignment_titleRight;

		private VBox vbox_winCloseBtn;

		private Alignment alignment_winCloseBtn;
	}
}

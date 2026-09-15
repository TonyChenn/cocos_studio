using System;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.ResourcePanel
{
	public class PreviewControl : Gtk.Window
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WindowPosition = WindowPosition.CenterOnParent;
			this.backgroundeventbox = new EventBox();
			this.backgroundeventbox.Name = "backgroundeventbox";
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.vbox2.BorderWidth = (uint)this.BorderWidth;
			this.vbox1 = new VBox();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			this.vbox3 = new VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			this.vbox1.Add(this.vbox3);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox1[this.vbox3];
			boxChild.Position = 0;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.hbox2 = new HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.hbox1.Add(this.hbox2);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox1[this.hbox2];
			boxChild2.Position = 0;
			this.image2 = new Gtk.Image();
			this.image2.Name = "image2";
			this.hbox1.Add(this.image2);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox1[this.image2];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.hbox3 = new HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			this.hbox1.Add(this.hbox3);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox1[this.hbox3];
			boxChild4.Position = 2;
			this.vbox1.Add(this.hbox1);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox1[this.hbox1];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.vbox4 = new VBox();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			this.vbox1.Add(this.vbox4);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox1[this.vbox4];
			boxChild6.Position = 2;
			this.vbox2.Add(this.vbox1);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox2[this.vbox1];
			boxChild7.Position = 0;
			this.imgsizelabel = new Label();
			this.imgsizelabel.HeightRequest = 20;
			this.imgsizelabel.CanFocus = true;
			this.imgsizelabel.Name = "imgsizelabel";
			this.imgsizelabel.Xalign = 0.9f;
			this.imgsizelabel.Yalign = 0.8f;
			this.imgsizelabel.Justify = Justification.Right;
			this.vbox2.Add(this.imgsizelabel);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox2[this.imgsizelabel];
			boxChild8.PackType = PackType.End;
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.backgroundeventbox.Add(this.vbox2);
			base.Add(this.backgroundeventbox);
		}

		public bool IsShown { get; private set; }

		public int Height { get; private set; }

		public int Width { get; private set; }

		public PreviewControl(Gdk.Window parentWindow) : base(Gtk.WindowType.Popup)
		{
			this.InitViewControl();
			this.InitStyle();
			base.ParentWindow = parentWindow;
		}

		protected virtual void InitStyle()
		{
			base.ModifyBg(StateType.Normal, PreviewControl.BGK);
			this.backgroundeventbox.ModifyBg(StateType.Normal, PreviewControl.BGK);
			base.CanFocus = false;
			base.Resizable = false;
		}

		protected virtual void InitViewControl()
		{
			this.Build();
		}

		public PreviewImageInfo ShowImagePath
		{
			get
			{
				return this.showImagePath;
			}
			set
			{
				if (this.showImagePath == value)
				{
					return;
				}
				this.showImagePath = value;
				bool flag = false;
				try
				{
					Pixbuf pixbuf = null;
					if (value != null)
					{
						pixbuf = value.Image;
					}
					if (pixbuf != null && pixbuf != null)
					{
						int width = pixbuf.Width;
						int height = pixbuf.Height;
						this.image2.Pixbuf = pixbuf;
						this.imgsizelabel.LabelProp = value.Size.Width + " * " + value.Size.Height;
						this.image2.WidthRequest = width;
						this.image2.HeightRequest = height;
						int num = width + 10;
						int num2 = height + 10 + 20;
						num = ((num < 110) ? 110 : num);
						num2 = ((num2 < 130) ? 130 : num2);
						this.Width = (base.WidthRequest = num);
						this.Height = (base.HeightRequest = num2);
						flag = true;
						this.IsShown = true;
					}
				}
				catch
				{
					base.Hide();
					this.IsShown = false;
					return;
				}
				if (!flag)
				{
					base.Hide();
					this.IsShown = false;
					return;
				}
				if (Platform.IsMac)
				{
					base.Hide();
					base.ShowAll();
					base.Show();
					base.ShowNow();
				}
			}
		}

		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			base.Hide();
			return base.OnLeaveNotifyEvent(evnt);
		}

		private EventBox backgroundeventbox;

		private VBox vbox2;

		private VBox vbox1;

		private VBox vbox3;

		private HBox hbox1;

		private HBox hbox2;

		private Gtk.Image image2;

		private HBox hbox3;

		private VBox vbox4;

		private Label imgsizelabel;

		private static readonly Color BGK = new Color(81, 81, 81);

		public int ShowMaxValue = 400;

		public new int BorderWidth = 5;

		private PreviewImageInfo showImagePath;
	}
}

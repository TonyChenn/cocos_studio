using System;
using System.ComponentModel;
using CocoStudio.Model;
using Gdk;
using Gtk;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.ResourcePanel
{
	[ToolboxItem(true)]
	public class FileTypeItem : Bin
	{
		public bool IsSelected { get; private set; }

		public IProjectFileCreator FileView { get; private set; }

		public event EventHandler<EventArgs> Selected;

		public event EventHandler<EventArgs> DoubleClicked;

		public FileTypeItem(IProjectFileCreator view)
		{
			this.Build();
			this.FileView = view;
			this.imagebin_icon.SetImage(view.Icon);
			if (view.IsShowTrackPoint)
			{
				this.imagebin_icon.PointType = TrackPointType.New;
			}
			this.InitLabel(view.LabelName);
			this.evtbx_root.ButtonPressEvent += this.ButtonPressEventHandler;
		}

		private void InitLabel(string labelText)
		{
			string[] array = labelText.Split(new char[]
			{
				'#'
			});
			if (array.Length > 1)
			{
				this.label_firstLine.Text = array[0];
				this.label_secondLine.Text = array[1];
				return;
			}
			this.vbox_labelContent.Remove(this.label_secondLine);
			this.label_firstLine.Text = array[0];
		}

		public void Select()
		{
			this.IsSelected = true;
			this.RefreshUI();
			if (this.Selected != null)
			{
				this.Selected(this, new EventArgs());
			}
		}

		public void UnSelect()
		{
			this.IsSelected = false;
			this.RefreshUI();
		}

		private void RefreshUI()
		{
			if (this.IsSelected)
			{
				this.label_firstLine.Sensitive = true;
				this.label_secondLine.Sensitive = true;
				this.evtbx_imageBorder.ModifyBg(StateType.Normal, this.color_border);
				this.evtbx_imageBg.ModifyBg(StateType.Normal, this.color_selected);
				return;
			}
			this.label_firstLine.Sensitive = false;
			this.label_secondLine.Sensitive = false;
			this.evtbx_imageBorder.ModifyBg(StateType.Normal, this.color_normal);
			this.evtbx_imageBg.ModifyBg(StateType.Normal, this.color_normal);
		}

		protected void HandleButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (!this.IsSelected)
			{
				this.Select();
			}
		}

		private void ButtonPressEventHandler(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Type == EventType.TwoButtonPress && this.DoubleClicked != null)
			{
				this.DoubleClicked(this, new EventArgs());
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.WidthRequest = 64;
			base.Name = "Modules.Communal.ResourcePanel.FileTypeItem";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.TopPadding = 4U;
			this.evtbx_root = new EventBox();
			this.evtbx_root.WidthRequest = 72;
			this.evtbx_root.Name = "evtbx_root";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 4;
			this.hbox_topMain = new HBox();
			this.hbox_topMain.Name = "hbox_topMain";
			this.alignment_left = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_left.Name = "alignment_left";
			this.hbox_topMain.Add(this.alignment_left);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_topMain[this.alignment_left];
			boxChild.Position = 0;
			this.evtbx_imageBorder = new EventBox();
			this.evtbx_imageBorder.WidthRequest = 48;
			this.evtbx_imageBorder.HeightRequest = 48;
			this.evtbx_imageBorder.Name = "evtbx_imageBorder";
			this.evtbx_imageBg = new EventBox();
			this.evtbx_imageBg.Name = "evtbx_imageBg";
			this.evtbx_imageBg.BorderWidth = 1U;
			this.vbox_picMain = new VBox();
			this.vbox_picMain.Name = "vbox_picMain";
			this.vbox_picMain.Spacing = 6;
			this.vbox_top = new VBox();
			this.vbox_top.Name = "vbox_top";
			this.vbox_top.Spacing = 6;
			this.vbox_picMain.Add(this.vbox_top);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_picMain[this.vbox_top];
			boxChild2.Position = 0;
			this.hbox_picMain = new HBox();
			this.hbox_picMain.WidthRequest = 48;
			this.hbox_picMain.Name = "hbox_picMain";
			this.hbox_picMain.Spacing = 6;
			this.hbox_left = new HBox();
			this.hbox_left.Name = "hbox_left";
			this.hbox_left.Spacing = 6;
			this.hbox_picMain.Add(this.hbox_left);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_picMain[this.hbox_left];
			boxChild3.Position = 0;
			this.imagebin_icon = new TrackPointImage();
			this.imagebin_icon.Events = EventMask.ButtonPressMask;
			this.imagebin_icon.Name = "imagebin_icon";
			this.hbox_picMain.Add(this.imagebin_icon);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_picMain[this.imagebin_icon];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.hbox_rightOccupy = new HBox();
			this.hbox_rightOccupy.Name = "hbox_rightOccupy";
			this.hbox_rightOccupy.Spacing = 6;
			this.hbox_picMain.Add(this.hbox_rightOccupy);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_picMain[this.hbox_rightOccupy];
			boxChild5.Position = 2;
			this.vbox_picMain.Add(this.hbox_picMain);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_picMain[this.hbox_picMain];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.vbox_bottom = new VBox();
			this.vbox_bottom.Name = "vbox_bottom";
			this.vbox_bottom.Spacing = 6;
			this.vbox_picMain.Add(this.vbox_bottom);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_picMain[this.vbox_bottom];
			boxChild7.Position = 2;
			this.evtbx_imageBg.Add(this.vbox_picMain);
			this.evtbx_imageBorder.Add(this.evtbx_imageBg);
			this.hbox_topMain.Add(this.evtbx_imageBorder);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox_topMain[this.evtbx_imageBorder];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			this.alignment_right = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_right.Name = "alignment_right";
			this.hbox_topMain.Add(this.alignment_right);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_topMain[this.alignment_right];
			boxChild9.Position = 2;
			this.vbox_main.Add(this.hbox_topMain);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.vbox_main[this.hbox_topMain];
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			this.vbox_labelMain = new VBox();
			this.vbox_labelMain.Name = "vbox_labelMain";
			this.alignment_labelTop = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_labelTop.Name = "alignment_labelTop";
			this.vbox_labelMain.Add(this.alignment_labelTop);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.vbox_labelMain[this.alignment_labelTop];
			boxChild11.Position = 0;
			this.vbox_labelContent = new VBox();
			this.vbox_labelContent.Name = "vbox_labelContent";
			this.label_firstLine = new Label();
			this.label_firstLine.Name = "label_firstLine";
			this.label_firstLine.LabelProp = Catalog.GetString("动画");
			this.vbox_labelContent.Add(this.label_firstLine);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.vbox_labelContent[this.label_firstLine];
			boxChild12.Position = 0;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.label_secondLine = new Label();
			this.label_secondLine.Name = "label_secondLine";
			this.label_secondLine.LabelProp = Catalog.GetString("（节点）");
			this.vbox_labelContent.Add(this.label_secondLine);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.vbox_labelContent[this.label_secondLine];
			boxChild13.Position = 1;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			this.vbox_labelMain.Add(this.vbox_labelContent);
			Box.BoxChild boxChild14 = (Box.BoxChild)this.vbox_labelMain[this.vbox_labelContent];
			boxChild14.Position = 1;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			this.alignment_labelBottom = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_labelBottom.Name = "alignment_labelBottom";
			this.vbox_labelMain.Add(this.alignment_labelBottom);
			Box.BoxChild boxChild15 = (Box.BoxChild)this.vbox_labelMain[this.alignment_labelBottom];
			boxChild15.Position = 2;
			this.vbox_main.Add(this.vbox_labelMain);
			Box.BoxChild boxChild16 = (Box.BoxChild)this.vbox_main[this.vbox_labelMain];
			boxChild16.Position = 1;
			this.evtbx_root.Add(this.vbox_main);
			this.alignment_main.Add(this.evtbx_root);
			base.Add(this.alignment_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.evtbx_root.ButtonReleaseEvent += this.HandleButtonReleaseEvent;
		}

		private Color color_normal = new Color(66, 65, 71);

		private Color color_border = new Color(7, 114, 244);

		private Color color_selected = new Color(51, 50, 55);

		private Alignment alignment_main;

		private EventBox evtbx_root;

		private VBox vbox_main;

		private HBox hbox_topMain;

		private Alignment alignment_left;

		private EventBox evtbx_imageBorder;

		private EventBox evtbx_imageBg;

		private VBox vbox_picMain;

		private VBox vbox_top;

		private HBox hbox_picMain;

		private HBox hbox_left;

		private TrackPointImage imagebin_icon;

		private HBox hbox_rightOccupy;

		private VBox vbox_bottom;

		private Alignment alignment_right;

		private VBox vbox_labelMain;

		private Alignment alignment_labelTop;

		private VBox vbox_labelContent;

		private Label label_firstLine;

		private Label label_secondLine;

		private Alignment alignment_labelBottom;
	}
}

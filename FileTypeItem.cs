using System;
using System.ComponentModel;
using CocoStudio.Model;
using Gdk;
using Gtk;
using Mono.Unix;
using Stetic;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200002C RID: 44
	[ToolboxItem(true)]
	public class FileTypeItem : Bin
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000091AB File Offset: 0x000073AB
		// (set) Token: 0x060001AD RID: 429 RVA: 0x000091B3 File Offset: 0x000073B3
		public bool IsSelected { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000091BC File Offset: 0x000073BC
		// (set) Token: 0x060001AF RID: 431 RVA: 0x000091C4 File Offset: 0x000073C4
		public IProjectFileCreator FileView { get; private set; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060001B0 RID: 432 RVA: 0x000091D0 File Offset: 0x000073D0
		// (remove) Token: 0x060001B1 RID: 433 RVA: 0x00009208 File Offset: 0x00007408
		public event EventHandler<EventArgs> Selected;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060001B2 RID: 434 RVA: 0x00009240 File Offset: 0x00007440
		// (remove) Token: 0x060001B3 RID: 435 RVA: 0x00009278 File Offset: 0x00007478
		public event EventHandler<EventArgs> DoubleClicked;

		// Token: 0x060001B4 RID: 436 RVA: 0x000092B0 File Offset: 0x000074B0
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

		// Token: 0x060001B5 RID: 437 RVA: 0x00009350 File Offset: 0x00007550
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

		// Token: 0x060001B6 RID: 438 RVA: 0x000093B3 File Offset: 0x000075B3
		public void Select()
		{
			this.IsSelected = true;
			this.RefreshUI();
			if (this.Selected != null)
			{
				this.Selected(this, new EventArgs());
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000093DB File Offset: 0x000075DB
		public void UnSelect()
		{
			this.IsSelected = false;
			this.RefreshUI();
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000093EC File Offset: 0x000075EC
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

		// Token: 0x060001B9 RID: 441 RVA: 0x0000947A File Offset: 0x0000767A
		protected void HandleButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (!this.IsSelected)
			{
				this.Select();
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000948A File Offset: 0x0000768A
		private void ButtonPressEventHandler(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Type == EventType.TwoButtonPress && this.DoubleClicked != null)
			{
				this.DoubleClicked(this, new EventArgs());
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000094B4 File Offset: 0x000076B4
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

		// Token: 0x04000077 RID: 119
		private Color color_normal = new Color(66, 65, 71);

		// Token: 0x04000078 RID: 120
		private Color color_border = new Color(7, 114, 244);

		// Token: 0x04000079 RID: 121
		private Color color_selected = new Color(51, 50, 55);

		// Token: 0x0400007C RID: 124
		private Alignment alignment_main;

		// Token: 0x0400007D RID: 125
		private EventBox evtbx_root;

		// Token: 0x0400007E RID: 126
		private VBox vbox_main;

		// Token: 0x0400007F RID: 127
		private HBox hbox_topMain;

		// Token: 0x04000080 RID: 128
		private Alignment alignment_left;

		// Token: 0x04000081 RID: 129
		private EventBox evtbx_imageBorder;

		// Token: 0x04000082 RID: 130
		private EventBox evtbx_imageBg;

		// Token: 0x04000083 RID: 131
		private VBox vbox_picMain;

		// Token: 0x04000084 RID: 132
		private VBox vbox_top;

		// Token: 0x04000085 RID: 133
		private HBox hbox_picMain;

		// Token: 0x04000086 RID: 134
		private HBox hbox_left;

		// Token: 0x04000087 RID: 135
		private TrackPointImage imagebin_icon;

		// Token: 0x04000088 RID: 136
		private HBox hbox_rightOccupy;

		// Token: 0x04000089 RID: 137
		private VBox vbox_bottom;

		// Token: 0x0400008A RID: 138
		private Alignment alignment_right;

		// Token: 0x0400008B RID: 139
		private VBox vbox_labelMain;

		// Token: 0x0400008C RID: 140
		private Alignment alignment_labelTop;

		// Token: 0x0400008D RID: 141
		private VBox vbox_labelContent;

		// Token: 0x0400008E RID: 142
		private Label label_firstLine;

		// Token: 0x0400008F RID: 143
		private Label label_secondLine;

		// Token: 0x04000090 RID: 144
		private Alignment alignment_labelBottom;
	}
}

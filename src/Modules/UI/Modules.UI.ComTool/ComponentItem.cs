using System;
using System.ComponentModel;
using CocoStudio.Model;
using CocoStudio.UserStatistics;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.UI.ComTool.Model;
using Mono.Unix;
using MonoDevelop.Components;
using Stetic;
using Xwt.Drawing;

namespace Modules.UI.ComTool
{
	[ToolboxItem(true)]
	public class ComponentItem : Bin
	{
		public ComponentItem()
		{
			this.Build();
			this.evtbx_Detail.ButtonReleaseEvent += delegate(object o, ButtonReleaseEventArgs args)
			{
				this.ShowTooltip();
			};
			base.WidthRequest = ComponentItem.ComSize;
			base.HeightRequest = ComponentItem.ComSize;
			this.evtbx_bg.Events = EventMask.PointerMotionMask;
			this.evtbx_bg.EnterNotifyEvent += this.HandleEnterNotifyEvent;
			this.evtbx_bg.LeaveNotifyEvent += this.HandleLeaveNotifyEvent;
			base.MotionNotifyEvent += this.HandleMotionNotifyEvent;
			this.evtbx_bg.ModifyBg(StateType.Normal, this.UnSelectedColor);
			this.evtbx_Detail.ModifyBg(StateType.Normal, this.UnSelectedColor);
			this.vbox_labels.Remove(this.label_secondLine);
		}

		private void BuildImage()
		{
			if (this.imgDetail == null)
			{
				this.imgDetail = new ImageBin();
				Xwt.Drawing.Image icon = ImageIcon.GetIcon("CocoStudio.DefaultResource.ComponentResource.Help.png");
				this.imgDetail.SetImageView(icon);
				this.evtbx_Detail.Add(this.imgDetail);
				this.imgDetail.ShowAll();
			}
		}

		private void HandleMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			if (this.UIControlToolItem != null)
			{
				Gtk.Drag.SourceSet(this, ModifierType.Button1Mask, ComponentItem.source_table, DragAction.Copy);
				this.uiToolDragData = this.UIControlToolItem;
			}
		}

		private void ShowTooltip()
		{
			if (this.tooltipWin == null)
			{
				if (this.tooltipWin == null)
				{
					this.tooltipWin = new ToolTipWindow();
				}
				this.tooltipWin.InitiToolTip(this);
				this.tooltipWin.LeaveNotifyEvent += delegate(object o, LeaveNotifyEventArgs args)
				{
					if (args.Event.Detail != NotifyType.Inferior)
					{
						if (!this.IsSelected(args.Event.XRoot, args.Event.YRoot))
						{
							this.UnSelectedMode();
						}
					}
				};
				this.PositioningToolTip();
				this.tooltipWin.ShowAll();
			}
		}

		private void PositioningToolTip()
		{
			int num;
			int num2;
			base.GdkWindow.GetOrigin(out num, out num2);
			num += base.Allocation.Width + base.Allocation.Location.X;
			num2 += base.Allocation.Location.Y;
			int width = base.Screen.Width;
			int width2 = this.tooltipWin.SizeRequest().Width;
			int num3;
			int num4;
			base.GdkWindow.GetOrigin(out num3, out num4);
			num4 += base.Allocation.Y;
			num3 += base.Allocation.X;
			if (num + width2 > width)
			{
				num = num - ComponentItem.ComSize - width2;
				this.tooltipWin.ToolOrientation = EnumToolTipOrientation.Left;
				num -= ComponentItem.TooltipOffset.X;
				if (num + width2 > num3)
				{
					int num5 = num3 - num - width2;
					num -= num5;
				}
				num = num3 + base.Allocation.Width - ComponentItem.TooltipOffset.X - width2;
			}
			else
			{
				num -= ComponentItem.TooltipOffset.X;
			}
			num2 += ComponentItem.TooltipOffset.Y;
			this.tooltipWin.Move(num, num2);
			this.toolX = num;
			this.toolY = num2;
		}

		private void HandleLeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if (args.Event.Detail != NotifyType.Inferior)
			{
				if (!this.IsSelected(args.Event.XRoot, args.Event.YRoot))
				{
					this.UnSelectedMode();
				}
			}
		}

		private bool IsSelected(double mouseX, double mouseY)
		{
			if (base.GdkWindow != null)
			{
				int num;
				int num2;
				base.GdkWindow.GetOrigin(out num, out num2);
				num2 += base.Allocation.Y;
				num += base.Allocation.X;
				if (this.tooltipWin != null)
				{
					int width = this.tooltipWin.SizeRequest().Width;
					int height = this.tooltipWin.SizeRequest().Height;
					if ((mouseX > (double)num && mouseX < (double)(num + base.Allocation.Width) && mouseY > (double)num2 && mouseY < (double)(num2 + base.Allocation.Height)) || (mouseX > (double)this.toolX && mouseX < (double)(this.toolX + width) && mouseY > (double)this.toolY && mouseY < (double)(this.toolY + height)))
					{
						return true;
					}
				}
			}
			return false;
		}

		[ConnectBefore]
		private void HandleEnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			this.BuildImage();
			this.SelectedMode();
		}

		public ControlToolItem UIControlToolItem { get; set; }

		public void InitiCom(ControlToolItem com)
		{
			this.imgIcon.SetImageView(com.BitImage);
			string text = com.DisplayName;
			string[] array = text.Split(new char[]
			{
				' '
			});
			if (array.Length > 1)
			{
				text = array[0];
				this.label_secondLine.Text = array[1];
				this.vbox_labels.Add(this.label_secondLine);
				this.vbox_labels.Spacing = -2;
				this.vbox_main.Spacing = 4;
			}
			int defaultFontSize = LanguageAdapter.GetDefaultFontSize();
			this.labComName.SetFontSize((double)defaultFontSize);
			this.label_secondLine.SetFontSize((double)defaultFontSize);
			this.labComName.LabelProp = text;
			this.imgIcon.ShowAll();
			this.UIControlToolItem = com;
		}

		public void SelectedMode()
		{
			this.evtbx_bg.ModifyBg(StateType.Normal, this.SelectedColor);
			this.evtbx_Detail.ModifyBg(StateType.Normal, this.SelectedColor);
			if (!this.imgDetail.Visible)
			{
				this.imgDetail.Visible = true;
			}
			base.Show();
		}

		public void UnSelectedMode()
		{
			this.evtbx_bg.ModifyBg(StateType.Normal, this.UnSelectedColor);
			this.evtbx_Detail.ModifyBg(StateType.Normal, this.UnSelectedColor);
			if (this.imgDetail != null && this.imgDetail.Visible)
			{
				this.imgDetail.Visible = false;
			}
			if (this.tooltipWin != null)
			{
				this.tooltipWin.Destroy();
				this.tooltipWin.Dispose();
				this.tooltipWin = null;
			}
		}

		protected override void OnDragBegin(DragContext context)
		{
			if (this.uiToolDragData != null)
			{
				Gtk.Drag.SetIconPixbuf(context, this.uiToolDragData.BitImage.ToPixbuf(), 0, 0);
				context.SetDragData(new ModelDragData(this.uiToolDragData.ModelMedaData));
				this.UnSelectedMode();
			}
			base.OnDragBegin(context);
		}

		private void AddFeatureInfo()
		{
			string text = this.uiToolDragData.ModelMedaData.Type.Name;
			if (this.uiToolDragData.ModelMedaData.IsDefault)
			{
				int length = (text.Length - 6 > 0) ? (text.Length - 6) : text.Length;
				text = text.Substring(0, length);
			}
			Tracker.Add(ViewRegions.ResourcePanel, "New" + text, "", "");
		}

		protected override bool OnDragFailed(DragContext drag_context, DragResult drag_result)
		{
			ComponentItem.isDragSucceed = false;
			return base.OnDragFailed(drag_context, drag_result);
		}

		protected override void OnDragEnd(DragContext context)
		{
			if (ComponentItem.isDragSucceed)
			{
				this.AddFeatureInfo();
			}
			ComponentItem.isDragSucceed = true;
			base.OnDragEnd(context);
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.WidthRequest = 72;
			base.HeightRequest = 72;
			base.Name = "Modules.UI.ComTool.ComponetItem";
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.vbox_main = new VBox();
			this.vbox_main.WidthRequest = 70;
			this.vbox_main.HeightRequest = 100;
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.hbox_top = new HBox();
			this.hbox_top.Name = "hbox_top";
			this.hbox_top.Spacing = 6;
			this.alignment_mainIcon = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_mainIcon.Name = "alignment_mainIcon";
			this.alignment_mainIcon.LeftPadding = 20U;
			this.alignment_mainIcon.TopPadding = 6U;
			this.imgIcon = new ImageBin();
			this.imgIcon.WidthRequest = 32;
			this.imgIcon.HeightRequest = 32;
			this.imgIcon.Events = EventMask.ButtonPressMask;
			this.imgIcon.Name = "imgIcon";
			this.alignment_mainIcon.Add(this.imgIcon);
			this.hbox_top.Add(this.alignment_mainIcon);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_top[this.alignment_mainIcon];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.vbox_info = new VBox();
			this.vbox_info.Name = "vbox_info";
			this.vbox_info.Spacing = 6;
			this.evtbx_Detail = new EventBox();
			this.evtbx_Detail.WidthRequest = 12;
			this.evtbx_Detail.HeightRequest = 12;
			this.evtbx_Detail.Name = "evtbx_Detail";
			this.vbox_info.Add(this.evtbx_Detail);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_info[this.evtbx_Detail];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			this.hbox_top.Add(this.vbox_info);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_top[this.vbox_info];
			boxChild3.Position = 1;
			this.vbox_main.Add(this.hbox_top);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.hbox_top];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.vbox_topOccupy = new VBox();
			this.vbox_topOccupy.Name = "vbox_topOccupy";
			this.vbox_topOccupy.Spacing = 6;
			this.vbox_main.Add(this.vbox_topOccupy);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox_main[this.vbox_topOccupy];
			boxChild5.Position = 1;
			this.vbox_labels = new VBox();
			this.vbox_labels.Name = "vbox_labels";
			this.vbox_labels.Spacing = 1;
			this.labComName = new Label();
			this.labComName.WidthRequest = 70;
			this.labComName.Name = "labComName";
			this.labComName.LabelProp = Catalog.GetString("自定义字体");
			this.vbox_labels.Add(this.labComName);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox_labels[this.labComName];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.label_secondLine = new Label();
			this.label_secondLine.Name = "label_secondLine";
			this.label_secondLine.LabelProp = Catalog.GetString("第二行");
			this.vbox_labels.Add(this.label_secondLine);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.vbox_labels[this.label_secondLine];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.vbox_main.Add(this.vbox_labels);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.vbox_main[this.vbox_labels];
			boxChild8.Position = 2;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.vbox_bottomOccupy = new VBox();
			this.vbox_bottomOccupy.Name = "vbox_bottomOccupy";
			this.vbox_bottomOccupy.Spacing = 6;
			this.vbox_main.Add(this.vbox_bottomOccupy);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.vbox_main[this.vbox_bottomOccupy];
			boxChild9.Position = 3;
			this.evtbx_bg.Add(this.vbox_main);
			base.Add(this.evtbx_bg);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private Gdk.Color SelectedColor = new Gdk.Color(50, 50, 54);

		private Gdk.Color UnSelectedColor = WindowStyle.WindowPanelColor;

		public static int ComSize = 70;

		public static Point TooltipOffset = new Point(10, 15);

		private static bool isDragSucceed = true;

		private ImageBin imgDetail;

		private static TargetEntry[] source_table = new TargetEntry[]
		{
			DragTargetType.CocoStudioTarget
		};

		private ControlToolItem uiToolDragData;

		private int toolX;

		private int toolY;

		private ToolTipWindow tooltipWin;

		private EventBox evtbx_bg;

		private VBox vbox_main;

		private HBox hbox_top;

		private Alignment alignment_mainIcon;

		private ImageBin imgIcon;

		private VBox vbox_info;

		private EventBox evtbx_Detail;

		private VBox vbox_topOccupy;

		private VBox vbox_labels;

		private Label labComName;

		private Label label_secondLine;

		private VBox vbox_bottomOccupy;
	}
}

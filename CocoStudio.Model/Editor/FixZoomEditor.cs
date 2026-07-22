using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Timers;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;
using Xwt.Drawing;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000054 RID: 84
	internal class FixZoomEditor : BaseEditor
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00009E64 File Offset: 0x00008064
		public override bool IsShowLabel
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00009E78 File Offset: 0x00008078
		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00009E8C File Offset: 0x0000808C
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00009EA0 File Offset: 0x000080A0
		protected override Widget OnCreateWidget()
		{
			Table table = new Table(2U, 2U, false);
			table.ColumnSpacing = 6U;
			table.RowSpacing = 2U;
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.position_pin_over.png");
			Xwt.Drawing.Image icon2 = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.position_pin_nor.png");
			Xwt.Drawing.Image normal = this.RotationImage(icon2, PixbufRotation.Clockwise);
			Xwt.Drawing.Image check = this.RotationImage(icon, PixbufRotation.Clockwise);
			this.topButton = new IconOnlyToggleButton(normal, check);
			this.topButton.Name = "top";
			Xwt.Drawing.Image normal2 = this.RotationImage(icon2, PixbufRotation.Counterclockwise);
			Xwt.Drawing.Image check2 = this.RotationImage(icon, PixbufRotation.Counterclockwise);
			this.bottomButton = new IconOnlyToggleButton(normal2, check2);
			this.bottomButton.Name = "bottom";
			Xwt.Drawing.Image normal3 = this.RotationImage(icon2, PixbufRotation.None);
			Xwt.Drawing.Image check3 = this.RotationImage(icon, PixbufRotation.None);
			this.leftButton = new IconOnlyToggleButton(normal3, check3);
			this.leftButton.Name = "left";
			Xwt.Drawing.Image normal4 = this.RotationImage(icon2, PixbufRotation.Upsidedown);
			Xwt.Drawing.Image check4 = this.RotationImage(icon, PixbufRotation.Upsidedown);
			this.rightButton = new IconOnlyToggleButton(normal4, check4);
			this.rightButton.Name = "right";
			this.topEntry = new NoUndoNumEntry();
			this.bottomEntry = new NoUndoNumEntry();
			this.leftEntry = new NoUndoNumEntry();
			this.rightEntry = new NoUndoNumEntry();
			this.topEntry.WidthRequest = (this.bottomEntry.WidthRequest = (this.leftEntry.WidthRequest = (this.rightEntry.WidthRequest = 40)));
			this.topEntry.Value = (this.bottomEntry.Value = (this.leftEntry.Value = (this.rightEntry.Value = 0f)));
			this.leftRend.WidthRequest = 45;
			this.leftRend.HeightRequest = 45;
			Table table2 = new Table(5U, 5U, false);
			table2.Attach(this.leftEntry, 0U, 1U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 1U);
			table2.Attach(this.rightEntry, 4U, 5U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 1U);
			table2.Attach(this.topEntry, 2U, 3U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 1U, 0U);
			table2.Attach(this.bottomEntry, 2U, 3U, 4U, 5U, AttachOptions.Fill, AttachOptions.Fill, 1U, 0U);
			VBox vbox = new VBox();
			vbox.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f));
			vbox.PackStart(this.leftButton, false, false, 0U);
			vbox.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f));
			table2.Attach(vbox, 1U, 2U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 1U);
			VBox vbox2 = new VBox();
			vbox2.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f));
			vbox2.PackStart(this.rightButton, false, false, 0U);
			vbox2.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f));
			table2.Attach(vbox2, 3U, 4U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 1U);
			HBox hbox = new HBox();
			hbox.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f));
			hbox.PackStart(this.topButton, false, false, 0U);
			hbox.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f));
			table2.Attach(hbox, 2U, 3U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 1U, 0U);
			HBox hbox2 = new HBox();
			hbox2.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f));
			hbox2.PackStart(this.bottomButton, false, false, 0U);
			hbox2.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f));
			table2.Attach(hbox2, 2U, 3U, 3U, 4U, AttachOptions.Fill, AttachOptions.Fill, 1U, 0U);
			this.leftEntry.EntryValueChanged += this.leftEntry_EntryValueChanged;
			this.rightEntry.EntryValueChanged += this.rightEntry_EntryValueChanged;
			this.topEntry.EntryValueChanged += this.topEntry_EntryValueChanged;
			this.bottomEntry.EntryValueChanged += this.bottomEntry_EntryValueChanged;
			this.leftEntry.SetEntryProperty(false, 1, 0.1f);
			this.rightEntry.SetEntryProperty(false, 1, 0.1f);
			this.topEntry.SetEntryProperty(false, 1, 0.1f);
			this.bottomEntry.SetEntryProperty(false, 1, 0.1f);
			EventBox eventBox = new EventBox();
			this.align.TopPadding = 1U;
			this.align.LeftPadding = 1U;
			this.align.RightPadding = 1U;
			this.align.BottomPadding = 1U;
			System.Drawing.Color color = System.Drawing.Color.FromArgb(255, 171, 174, 183);
			eventBox.ModifyBg(StateType.Normal, new Gdk.Color(color.R, color.G, color.B));
			foreach (object obj in PropertyItem.Objects)
			{
				IStretchSize stretchSize = obj as IStretchSize;
				if (stretchSize == null || !stretchSize.CanShowStretch)
				{
					this.showStretch = false;
					break;
				}
				ISizeType sizeType = obj as ISizeType;
				if (sizeType != null)
				{
					if (!sizeType.IsCustomSize)
					{
						this.showStretch = false;
						break;
					}
				}
				FileNodeObject fileNodeObject = obj as FileNodeObject;
				if (fileNodeObject != null)
				{
					if (fileNodeObject.Project == null)
					{
						this.showStretch = false;
						break;
					}
					string fileType = fileNodeObject.Project.GetFileType();
					if (fileType != "Layer")
					{
						this.showStretch = false;
						break;
					}
				}
			}
			if (this.showStretch)
			{
				this.align.Add(this.leftRend);
			}
			else
			{
				EventBox eventBox2 = new EventBox();
				this.align.Add(eventBox2);
				eventBox2.WidthRequest = 45;
				eventBox2.HeightRequest = 45;
			}
			this.leftRend.HEventChanged += this.leftRend_HEventChanged;
			this.leftRend.VEventChanged += this.leftRend_VEventChanged;
			eventBox.Add(this.align);
			table2.Attach(eventBox, 2U, 3U, 2U, 3U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill, 0U, 0U);
			VBox vbox3 = new VBox();
			vbox3.Add(new Alignment(0.5f, 0.5f, 1f, 1f));
			vbox3.PackStart(this.animation, false, false, 0U);
			vbox3.Add(new Alignment(0.5f, 0.5f, 1f, 1f));
			this.animation.WidthRequest = 150;
			this.animation.HeightRequest = 75;
			this.eventBoxFir.Add(table2);
			this.eventBoxSec.Add(vbox3);
			table.Attach(this.eventBoxFir, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			table.Attach(this.eventBoxSec, 1U, 2U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			Label label = new Label();
			Label label2 = new Label();
			table.Attach(label, 0U, 1U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			table.Attach(label2, 1U, 2U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			label.Text = LanguageInfo.Property_PinAndSizing;
			label.SetFontSize(10.0);
			label.ModifyFg(StateType.Normal, WindowStyle.LableToolTipColor);
			label2.Text = LanguageInfo.Property_Preview;
			label2.SetFontSize(10.0);
			label2.ModifyFg(StateType.Normal, WindowStyle.LableToolTipColor);
			base.SetControl();
			this.topButton.CheckChanged += this.button_Clicked;
			this.bottomButton.CheckChanged += this.button_Clicked;
			this.leftButton.CheckChanged += this.button_Clicked;
			this.rightButton.CheckChanged += this.button_Clicked;
			this.eventBoxFir.EnterNotifyEvent += this.eventBox_EnterNotifyEvent;
			this.eventBoxFir.LeaveNotifyEvent += this.eventBox_LeaveNotifyEvent;
			this.eventBoxSec.EnterNotifyEvent += this.eventBox_EnterNotifyEvent;
			this.eventBoxSec.LeaveNotifyEvent += this.eventBox_LeaveNotifyEvent;
			this.timer = new Timer();
			this.timer.Elapsed += this.timer_Elapsed;
			this.timer.Interval = 100.0;
			this.timer1 = new Timer();
			this.timer1.Elapsed += this.timer1_Elapsed;
			this.timer1.Interval = 100.0;
			HBox hbox3 = new HBox();
			hbox3.Add(new Alignment(0.5f, 0.5f, 1f, 1f));
			hbox3.PackStart(table, false, false, 0U);
			hbox3.Add(new Alignment(0.5f, 0.5f, 1f, 1f));
			hbox3.ShowAll();
			return hbox3;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000A854 File Offset: 0x00008A54
		private void leftEntry_EntryValueChanged(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					(obj as NodeObject).LeftMargin = e.Value;
				}
				base.ReportUserData("Layout");
			}
			base.SetControl();
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		private void rightEntry_EntryValueChanged(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					(obj as NodeObject).RightMargin = e.Value;
				}
				base.ReportUserData("Layout");
			}
			base.SetControl();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000A994 File Offset: 0x00008B94
		private void topEntry_EntryValueChanged(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					(obj as NodeObject).TopMargin = e.Value;
				}
				base.ReportUserData("Layout");
			}
			base.SetControl();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000AA34 File Offset: 0x00008C34
		private void bottomEntry_EntryValueChanged(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					(obj as NodeObject).BottomMargin = e.Value;
				}
				base.ReportUserData("Layout");
			}
			base.SetControl();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000AAD4 File Offset: 0x00008CD4
		protected override void OnSetSensitive(bool isSensitive)
		{
			Widget widget = this.leftButton;
			Widget widget2 = this.rightButton;
			Widget widget3 = this.topButton;
			this.bottomButton.Sensitive = isSensitive;
			widget3.Sensitive = isSensitive;
			widget2.Sensitive = isSensitive;
			widget.Sensitive = isSensitive;
			this.eventBoxSec.Sensitive = isSensitive;
			this.align.Sensitive = isSensitive;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000AB36 File Offset: 0x00008D36
		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.animation.TimerTick();
				this.animation.QueueDrawArea(0, 0, 150, 75);
				return false;
			});
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000AB4C File Offset: 0x00008D4C
		private void timer1_Elapsed(object sender, ElapsedEventArgs e)
		{
			GLib.Timeout.Add(0U, delegate
			{
				double num = this.animation.TimerTick();
				this.animation.QueueDrawArea(0, 0, 150, 75);
				if (num == 75.0)
				{
					this.timer1.Stop();
				}
				return false;
			});
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000AB64 File Offset: 0x00008D64
		private void eventBox_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if (args.Event.Detail != NotifyType.Inferior)
			{
				if (this.eventBoxSec.Sensitive)
				{
					this.timer.Stop();
					this.timer1.Start();
				}
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000ABB0 File Offset: 0x00008DB0
		private void eventBox_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			if (this.eventBoxSec.Sensitive)
			{
				this.timer1.Stop();
				this.timer.Start();
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000ABE8 File Offset: 0x00008DE8
		private Xwt.Drawing.Image RotationImage(Xwt.Drawing.Image image, PixbufRotation rotation)
		{
			return Xwt.Drawing.Image.FromStream(new MemoryStream(image.GetPixbuf().RotateSimple(rotation).SaveToBuffer("png")));
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000AC1C File Offset: 0x00008E1C
		private void leftRend_VEventChanged(object sender, FixZoomEventArgs e)
		{
			using (base.GetLock(true))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					(obj as IStretchSize).StretchWidthEnable = e.Type;
				}
			}
			base.SetControl();
			base.ReportUserData("Layout");
			this.animation.IsWidthCenter = e.Type;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000ACD0 File Offset: 0x00008ED0
		private void leftRend_HEventChanged(object sender, FixZoomEventArgs e)
		{
			using (base.GetLock(true))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					(obj as IStretchSize).StretchHeightEnable = e.Type;
				}
			}
			base.SetControl();
			base.ReportUserData("Layout");
			this.animation.IsHeightCenter = e.Type;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000AD84 File Offset: 0x00008F84
		private void button_Clicked(object sender, EventArgs e)
		{
			if (!base.IsSettingControl)
			{
				IconToggleButton iconToggleButton = sender as IconToggleButton;
				string name = iconToggleButton.Name;
				if (name != null)
				{
					if (!(name == "top") && !(name == "bottom"))
					{
						if (name == "left" || name == "right")
						{
							HorizontalBerthEdge horizontalEdge;
							if (this.leftButton.IsChecked && this.rightButton.IsChecked)
							{
								horizontalEdge = HorizontalBerthEdge.BothEdge;
							}
							else if (!this.leftButton.IsChecked && this.rightButton.IsChecked)
							{
								horizontalEdge = HorizontalBerthEdge.RightEdge;
							}
							else if (this.leftButton.IsChecked && !this.rightButton.IsChecked)
							{
								horizontalEdge = HorizontalBerthEdge.LeftEdge;
							}
							else
							{
								horizontalEdge = HorizontalBerthEdge.None;
							}
							using (base.GetLock(true))
							{
								foreach (object obj in PropertyItem.Objects)
								{
									(obj as NodeObject).HorizontalEdge = horizontalEdge;
								}
							}
						}
					}
					else
					{
						VerticalBerthEdge verticalEdge;
						if (this.topButton.IsChecked && this.bottomButton.IsChecked)
						{
							verticalEdge = VerticalBerthEdge.BothEdge;
						}
						else if (!this.topButton.IsChecked && this.bottomButton.IsChecked)
						{
							verticalEdge = VerticalBerthEdge.BottomEdge;
						}
						else if (this.topButton.IsChecked && !this.bottomButton.IsChecked)
						{
							verticalEdge = VerticalBerthEdge.TopEdge;
						}
						else
						{
							verticalEdge = VerticalBerthEdge.None;
						}
						using (base.GetLock(true))
						{
							foreach (object obj in PropertyItem.Objects)
							{
								(obj as NodeObject).VerticalEdge = verticalEdge;
							}
						}
					}
				}
				this.animation.IsLeft = this.leftButton.IsChecked;
				this.animation.IsRight = this.rightButton.IsChecked;
				this.animation.IsTop = this.topButton.IsChecked;
				this.animation.IsBottom = this.bottomButton.IsChecked;
				base.ReportUserData("Layout");
				base.SetControl();
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000B094 File Offset: 0x00009294
		protected override void OnSetControl()
		{
			NodeObject nodeObject = PropertyItem.FirstObject as NodeObject;
			bool flag = false;
			bool flag2 = false;
			HorizontalBerthEdge horizontalBerthEdge = nodeObject.HorizontalEdge;
			VerticalBerthEdge verticalBerthEdge = nodeObject.VerticalEdge;
			IStretchSize stretchSize = PropertyItem.FirstObject as IStretchSize;
			if (stretchSize != null && stretchSize.CanShowStretch)
			{
				bool flag3 = (bool)PropertyItem.FirstObject.GetType().GetProperty("StretchWidthEnable").GetValue(PropertyItem.FirstObject, null);
				bool flag4 = (bool)PropertyItem.FirstObject.GetType().GetProperty("StretchHeightEnable").GetValue(PropertyItem.FirstObject, null);
				this.animation.IsWidthCenter = flag3;
				this.animation.IsHeightCenter = flag4;
				this.leftRend.SetLine(flag3, flag4);
				flag = stretchSize.StretchWidthEnable;
				flag2 = stretchSize.StretchHeightEnable;
			}
			if (PropertyItem.Objects.Count > 1)
			{
				bool flag5 = true;
				bool flag6 = true;
				Func<NodeObject, NodeObject, bool> func = (NodeObject a, NodeObject b) => (a.HorizontalEdge == HorizontalBerthEdge.LeftEdge || a.HorizontalEdge == HorizontalBerthEdge.BothEdge) && (b.HorizontalEdge == HorizontalBerthEdge.LeftEdge || b.HorizontalEdge == HorizontalBerthEdge.BothEdge);
				if (base.IsWhipNode<NodeObject>(func))
				{
					flag5 = false;
				}
				Func<NodeObject, NodeObject, bool> func2 = (NodeObject a, NodeObject b) => (a.HorizontalEdge == HorizontalBerthEdge.RightEdge || a.HorizontalEdge == HorizontalBerthEdge.BothEdge) && (b.HorizontalEdge == HorizontalBerthEdge.RightEdge || b.HorizontalEdge == HorizontalBerthEdge.BothEdge);
				if (base.IsWhipNode<NodeObject>(func2))
				{
					flag6 = false;
				}
				if (flag5 && flag6)
				{
					horizontalBerthEdge = HorizontalBerthEdge.BothEdge;
				}
				else if (flag5)
				{
					horizontalBerthEdge = HorizontalBerthEdge.LeftEdge;
				}
				else if (flag6)
				{
					horizontalBerthEdge = HorizontalBerthEdge.RightEdge;
				}
				else
				{
					horizontalBerthEdge = HorizontalBerthEdge.None;
				}
				bool flag7 = true;
				bool flag8 = true;
				Func<NodeObject, NodeObject, bool> func3 = (NodeObject a, NodeObject b) => a.VerticalEdge == b.VerticalEdge;
				if (base.IsWhipNode<NodeObject>(func3))
				{
				}
				Func<NodeObject, NodeObject, bool> func4 = (NodeObject a, NodeObject b) => (a.VerticalEdge == VerticalBerthEdge.TopEdge || a.VerticalEdge == VerticalBerthEdge.BothEdge) && (b.VerticalEdge == VerticalBerthEdge.TopEdge || b.VerticalEdge == VerticalBerthEdge.BothEdge);
				if (base.IsWhipNode<NodeObject>(func4))
				{
					flag7 = false;
				}
				Func<NodeObject, NodeObject, bool> func5 = (NodeObject a, NodeObject b) => (a.VerticalEdge == VerticalBerthEdge.BottomEdge || a.VerticalEdge == VerticalBerthEdge.BothEdge) && (b.VerticalEdge == VerticalBerthEdge.BottomEdge || b.VerticalEdge == VerticalBerthEdge.BothEdge);
				if (base.IsWhipNode<NodeObject>(func5))
				{
					flag8 = false;
				}
				if (flag7 && flag8)
				{
					verticalBerthEdge = VerticalBerthEdge.BothEdge;
				}
				else if (flag7)
				{
					verticalBerthEdge = VerticalBerthEdge.TopEdge;
				}
				else if (flag8)
				{
					verticalBerthEdge = VerticalBerthEdge.BottomEdge;
				}
				else
				{
					verticalBerthEdge = VerticalBerthEdge.None;
				}
				if (this.showStretch)
				{
					Func<IStretchSize, IStretchSize, bool> func6 = (IStretchSize a, IStretchSize b) => a.StretchWidthEnable == b.StretchWidthEnable;
					Func<IStretchSize, IStretchSize, bool> func7 = (IStretchSize a, IStretchSize b) => a.StretchHeightEnable == b.StretchHeightEnable;
					if (base.IsWhipNode<IStretchSize>(func6))
					{
						flag = false;
					}
					if (base.IsWhipNode<IStretchSize>(func7))
					{
						flag2 = false;
					}
					this.animation.IsWidthCenter = flag;
					this.animation.IsHeightCenter = flag2;
					this.leftRend.SetLine(flag, flag2);
				}
				Func<NodeObject, NodeObject, bool> func8 = (NodeObject a, NodeObject b) => a.LeftMargin == b.LeftMargin;
				Func<NodeObject, NodeObject, bool> func9 = (NodeObject a, NodeObject b) => a.RightMargin == b.RightMargin;
				Func<NodeObject, NodeObject, bool> func10 = (NodeObject a, NodeObject b) => a.TopMargin == b.TopMargin;
				Func<NodeObject, NodeObject, bool> func11 = (NodeObject a, NodeObject b) => a.BottomMargin == b.BottomMargin;
				if (base.IsWhipNode<NodeObject>(func8))
				{
					this.leftEntry.SetToSubState();
				}
				else
				{
					this.leftEntry.Value = (PropertyItem.FirstObject as NodeObject).LeftMargin;
				}
				if (base.IsWhipNode<NodeObject>(func9))
				{
					this.rightEntry.SetToSubState();
				}
				else
				{
					this.rightEntry.Value = (PropertyItem.FirstObject as NodeObject).RightMargin;
				}
				if (base.IsWhipNode<NodeObject>(func10))
				{
					this.topEntry.SetToSubState();
				}
				else
				{
					this.topEntry.Value = (PropertyItem.FirstObject as NodeObject).TopMargin;
				}
				if (base.IsWhipNode<NodeObject>(func11))
				{
					this.bottomEntry.SetToSubState();
				}
				else
				{
					this.bottomEntry.Value = (PropertyItem.FirstObject as NodeObject).BottomMargin;
				}
			}
			else
			{
				this.leftEntry.Value = (float)PropertyItem.FirstObject.GetType().GetProperty("LeftMargin").GetValue(PropertyItem.FirstObject, null);
				this.rightEntry.Value = (float)PropertyItem.FirstObject.GetType().GetProperty("RightMargin").GetValue(PropertyItem.FirstObject, null);
				this.topEntry.Value = (float)PropertyItem.FirstObject.GetType().GetProperty("TopMargin").GetValue(PropertyItem.FirstObject, null);
				this.bottomEntry.Value = (float)PropertyItem.FirstObject.GetType().GetProperty("BottomMargin").GetValue(PropertyItem.FirstObject, null);
			}
			switch (horizontalBerthEdge)
			{
			case HorizontalBerthEdge.None:
				this.leftButton.IsChecked = (this.rightButton.IsChecked = false);
				this.leftEntry.Sensitive = false;
				this.rightEntry.Sensitive = false;
				break;
			case HorizontalBerthEdge.LeftEdge:
				this.leftButton.IsChecked = true;
				this.rightButton.IsChecked = false;
				this.leftEntry.Sensitive = true;
				this.rightEntry.Sensitive = false;
				break;
			case HorizontalBerthEdge.RightEdge:
				this.leftButton.IsChecked = false;
				this.rightButton.IsChecked = true;
				this.leftEntry.Sensitive = false;
				this.rightEntry.Sensitive = true;
				break;
			case HorizontalBerthEdge.BothEdge:
				this.leftButton.IsChecked = true;
				this.rightButton.IsChecked = true;
				this.leftEntry.Sensitive = flag;
				this.rightEntry.Sensitive = flag;
				break;
			}
			switch (verticalBerthEdge)
			{
			case VerticalBerthEdge.None:
				this.topButton.IsChecked = (this.bottomButton.IsChecked = false);
				this.topEntry.Sensitive = false;
				this.bottomEntry.Sensitive = false;
				break;
			case VerticalBerthEdge.BottomEdge:
				this.topButton.IsChecked = false;
				this.bottomButton.IsChecked = true;
				this.topEntry.Sensitive = false;
				this.bottomEntry.Sensitive = true;
				break;
			case VerticalBerthEdge.TopEdge:
				this.topButton.IsChecked = true;
				this.bottomButton.IsChecked = false;
				this.topEntry.Sensitive = true;
				this.bottomEntry.Sensitive = false;
				break;
			case VerticalBerthEdge.BothEdge:
				this.topButton.IsChecked = true;
				this.bottomButton.IsChecked = true;
				this.topEntry.Sensitive = flag2;
				this.bottomEntry.Sensitive = flag2;
				break;
			}
			this.animation.IsLeft = this.leftButton.IsChecked;
			this.animation.IsRight = this.rightButton.IsChecked;
			this.animation.IsTop = this.topButton.IsChecked;
			this.animation.IsBottom = this.bottomButton.IsChecked;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000B890 File Offset: 0x00009A90
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsCustomSize")
			{
				ISizeType sizeType = PropertyItem.FirstObject as ISizeType;
				if (sizeType != null)
				{
					if (this.align == null || this.align.Children == null || this.align.Children.Count<Widget>() < 1)
					{
						return;
					}
					this.RefreshStretchView(sizeType.IsCustomSize);
				}
				this.OnSetSensitive((PropertyItem.FirstObject as IOperationMask).OperationFlag.HasFlag(OperationMask.AnchorMoveFlag));
			}
			else
			{
				if (e.PropertyName == "FileData")
				{
					bool showStretchView = this.showStretch;
					foreach (object obj in PropertyItem.Objects)
					{
						FileNodeObject fileNodeObject = obj as FileNodeObject;
						if (fileNodeObject != null)
						{
							if (fileNodeObject.Project != null)
							{
								string fileType = fileNodeObject.Project.GetFileType();
								showStretchView = (fileType == "Layer");
							}
							else
							{
								showStretchView = false;
							}
							break;
						}
					}
					this.RefreshStretchView(showStretchView);
				}
				if (e.PropertyName == "Position" || e.PropertyName == "Size" || e.PropertyName == "LayoutState")
				{
					base.SetControl();
				}
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000BA54 File Offset: 0x00009C54
		private void RefreshStretchView(bool showStretchView)
		{
			this.align.Remove(this.align.Children[0]);
			if (showStretchView)
			{
				this.align.Add(this.leftRend);
			}
			else
			{
				EventBox eventBox = new EventBox();
				this.align.Add(eventBox);
				eventBox.WidthRequest = 45;
				eventBox.HeightRequest = 45;
			}
			this.align.ShowAll();
		}

		// Token: 0x04000138 RID: 312
		private Timer timer;

		// Token: 0x04000139 RID: 313
		private Timer timer1;

		// Token: 0x0400013A RID: 314
		private IconToggleButton topButton;

		// Token: 0x0400013B RID: 315
		private IconToggleButton bottomButton;

		// Token: 0x0400013C RID: 316
		private IconToggleButton leftButton;

		// Token: 0x0400013D RID: 317
		private IconToggleButton rightButton;

		// Token: 0x0400013E RID: 318
		private NoUndoNumEntry topEntry;

		// Token: 0x0400013F RID: 319
		private NoUndoNumEntry bottomEntry;

		// Token: 0x04000140 RID: 320
		private NoUndoNumEntry leftEntry;

		// Token: 0x04000141 RID: 321
		private NoUndoNumEntry rightEntry;

		// Token: 0x04000142 RID: 322
		private Alignment align = new Alignment(0.5f, 0.5f, 1f, 1f);

		// Token: 0x04000143 RID: 323
		private bool showStretch = true;

		// Token: 0x04000144 RID: 324
		private FixZoomRectangle animation = new FixZoomRectangle();

		// Token: 0x04000145 RID: 325
		private FixZoomEventBox leftRend = new FixZoomEventBox();

		// Token: 0x04000146 RID: 326
		private EventBox eventBoxFir = new EventBox();

		// Token: 0x04000147 RID: 327
		private EventBox eventBoxSec = new EventBox();
	}
}

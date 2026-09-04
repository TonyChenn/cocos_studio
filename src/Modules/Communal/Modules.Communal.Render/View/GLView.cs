using System;
using System.Threading;
using System.Timers;
using CocoStudio.Basic;
using CocoStudio.Core.Commands;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.Render.ExtensionModel;
using Modules.Communal.Render.Model;
using Mono.TextEditor;
using MonoDevelop.Components.Commands;
using Xwt.GtkBackend;

namespace Modules.Communal.Render.View
{
	// Token: 0x02000034 RID: 52
	public class GLView : EventBox, IGLView, ICommandRouter
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		private IObjectContextMenu ContextMenu
		{
			get
			{
				IObjectContextMenu result;
				if (this.ViewMode == null)
				{
					result = null;
				}
				else
				{
					result = this.ViewMode.GetContextMenu();
				}
				return result;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000C728 File Offset: 0x0000A928
		private ICommandService CommandService
		{
			get
			{
				return ViewModeManager.Instance.Current.GetCommandService();
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000C74C File Offset: 0x0000A94C
		private IViewMode ViewMode
		{
			get
			{
				return ViewModeManager.Instance.Current;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000C768 File Offset: 0x0000A968
		public GameWindow GameWindow
		{
			get
			{
				return GameWindow.Current;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C780 File Offset: 0x0000A980
		public GLView(IEventAggregator eventAggregator)
		{
			Gtk.Drag.DestSet(this, DestDefaults.All, GLView.target_table, DragAction.Copy | DragAction.Move | DragAction.Link);
			base.DoubleBuffered = false;
			this.Initialize(eventAggregator);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
		private void Initialize(IEventAggregator eventAggregator)
		{
			base.Events = EventMask.PointerMotionMask;
			if (eventAggregator == null)
			{
				base.Sensitive = false;
			}
			this.InitializeCommand(eventAggregator);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C804 File Offset: 0x0000AA04
		private void ControlContextMenu(ButtonReleaseEventArgs e)
		{
			if (Xwt.GtkBackend.GtkWorkarounds.IsContextMenuButton(e.Event) && this.ContextMenu != null)
			{
				Point p = new Point((int)e.Event.X, (int)e.Event.Y);
				PointF scenePoint = this.ConvertControlToScene(p);
				bool flag = this.ViewMode.CanShowContextMenu(scenePoint);
				if (flag)
				{
					KeyboardExtend.ForceClear();
					Xwt.GtkBackend.GtkWorkarounds.ShowContextMenu((Menu)this.ContextMenu, this, e.Event);
					((Menu)this.ContextMenu).ShowAll();
					this.pasteObjectPosition = p;
				}
				else
				{
					this.pasteObjectPosition = null;
				}
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000C8C0 File Offset: 0x0000AAC0
		private void InitializeTimer()
		{
			this.timer = new System.Timers.Timer(30.0);
			this.timer.Elapsed += this.Timer_Elapsed;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000C918 File Offset: 0x0000AB18
		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			int num = Interlocked.CompareExchange(ref this.lockTag, 1, 0);
			if (num == 0)
			{
				GLib.Timeout.Add(0U, delegate
				{
					this.DrawView();
					Interlocked.CompareExchange(ref this.lockTag, 0, 1);
					return false;
				});
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000C960 File Offset: 0x0000AB60
		private void DrawView()
		{
			try
			{
				if (this.GameWindow != null)
				{
					this.GameWindow.Draw();
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Draw scene failed,  something wrong when draw the game scene, please contact cocosstudio@chukong-inc.com", ex);
				for (Exception innerException = ex.InnerException; innerException != null; innerException = innerException.InnerException)
				{
					LogConfig.Logger.Error("Draw scene innerException", innerException);
				}
				this.StopTimer();
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
		private void StartTimer()
		{
			if (this.timer != null && !this.timer.Enabled)
			{
				this.timer.Start();
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000CA28 File Offset: 0x0000AC28
		private void StopTimer()
		{
			if (this.timer != null && this.timer.Enabled)
			{
				this.timer.Stop();
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000CA60 File Offset: 0x0000AC60
		private void SizeAllocatedHandle(object o, SizeAllocatedArgs args)
		{
			if (this.GameWindow != null)
			{
				this.StopTimer();
				this.ResizeView(args.Allocation);
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000CA94 File Offset: 0x0000AC94
		private void ResizeView(Rectangle sizeInfo)
		{
			bool isEmpty = this.viewSize.IsEmpty;
			this.viewSize.Width = sizeInfo.Width;
			this.viewSize.Height = sizeInfo.Height;
			int x;
			int y;
			base.TranslateCoordinates(ApplicationCurrent.MainWindow, 0, 0, out x, out y);
			this.GameWindow.SetViewRect(x, y, this.viewSize.Width, this.viewSize.Height);
			if (isEmpty && this.ViewMode != null)
			{
				this.ViewMode.ResetView();
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000CB2C File Offset: 0x0000AD2C
		protected override bool OnExposeEvent(EventExpose evnt)
		{
			bool result;
			if (this.NeedRedraw || this.timer == null || !this.timer.Enabled)
			{
				base.OnExposeEvent(evnt);
				this.NeedRedraw = false;
				this.StartTimer();
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000CB80 File Offset: 0x0000AD80
		public bool CreateView()
		{
			bool result;
			try
			{
				if (base.GdkWindow == null)
				{
					result = false;
				}
				else
				{
					GameWindow gameWindow = new GameWindow(base.GdkWindow);
					this.InitializeTimer();
					this.InitializeEvent();
					result = true;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Failed to init GameWindow", exception);
				string info = string.Format("OpenGL 2.0 or higher is required, your version is too old. Please upgrade the driver of your video card. The CocosStudio will exit.", new object[0]);
				MessageBox.Show(info, null, MessageBoxImage.Error, null, EnumMainButton.Yes, null);
				Environment.Exit(0);
				result = false;
			}
			return result;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000CC14 File Offset: 0x0000AE14
		public PointF ConvertControlToScene(PointF controlPoint)
		{
			controlPoint.Y = (float)this.viewSize.Height - controlPoint.Y;
			return controlPoint;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000CC44 File Offset: 0x0000AE44
		public PointF ConvertSceneToControl(PointF sencePoint)
		{
			sencePoint.Y = (float)this.viewSize.Height - sencePoint.Y;
			return sencePoint;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000CC74 File Offset: 0x0000AE74
		public PointF ConvertScreenToScene(PointF screenPoint)
		{
			int num;
			int num2;
			base.GdkWindow.GetOrigin(out num, out num2);
			screenPoint.X -= (float)num;
			screenPoint.Y -= (float)num2;
			screenPoint.Y = (float)this.viewSize.Height - screenPoint.Y;
			return screenPoint;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000CCD1 File Offset: 0x0000AED1
		public void OnCanvasSizeChanged()
		{
			this.NeedRedraw = true;
			this.ViewMode.OnCanvasSizeChanged();
		}

		// Token: 0x1700004F RID: 79
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		public Cursor Cursor
		{
			set
			{
				if (base.GdkWindow != null)
				{
					base.GdkWindow.Cursor = value;
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000CD10 File Offset: 0x0000AF10
		public float ActualWidth
		{
			get
			{
				return (float)this.viewSize.Width;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000CD30 File Offset: 0x0000AF30
		public float ActualHeight
		{
			get
			{
				return (float)this.viewSize.Height;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000CD50 File Offset: 0x0000AF50
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000CD68 File Offset: 0x0000AF68
		internal bool NeedRedraw
		{
			get
			{
				return this.needRedraw;
			}
			set
			{
				this.needRedraw = value;
				if (this.needRedraw)
				{
					base.QueueDraw();
				}
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000CD94 File Offset: 0x0000AF94
		internal void SwitchView(bool isShowing)
		{
			if (!isShowing)
			{
				this.StopTimer();
			}
			this.NeedRedraw = true;
			if (isShowing)
			{
				this.StartTimer();
			}
			if (this.GameWindow != null)
			{
				this.GameWindow.UpdateGLContext(isShowing, base.GdkWindow);
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
		private void InitializeEvent()
		{
			base.GdkWindow.Events = EventMask.AllEventsMask;
			base.CanFocus = true;
			base.SizeAllocated += this.SizeAllocatedHandle;
			base.FocusInEvent += this.FocusInEventHandle;
			base.FocusOutEvent += this.FocusOutEventHandle;
			base.ButtonPressEvent += this.ButtonPressHandle;
			base.ButtonReleaseEvent += this.ButtonReleaseHandle;
			base.MotionNotifyEvent += this.MotionNotifyHandle;
			base.EnterNotifyEvent += this.EnterNotifyHandle;
			base.LeaveNotifyEvent += this.LeaveNotifyHandle;
			base.ScrollEvent += this.ScrollEventHandle;
			base.KeyPressEvent += this.KeyPressHandle;
			base.KeyReleaseEvent += this.KeyReleaseHandle;
			base.DragBegin += this.DragBeginHandle;
			base.DragMotion += this.DragMotionHandle;
			base.DragLeave += this.DragLeaveHandle;
			base.DragDrop += this.DragDropHandle;
			base.DragDataReceived += this.DragDataReceivedHandle;
			if (GtkGestures.IsSupported)
			{
				this.RigesterGesturesEvent();
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000CF54 File Offset: 0x0000B154
		private void FocusInEventHandle(object o, FocusInEventArgs args)
		{
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000CF58 File Offset: 0x0000B158
		private void FocusOutEventHandle(object o, FocusOutEventArgs args)
		{
			if (this.isMouseDown && this.IsMouseEventArgsValid(this.lastButtonReleaseEventArgs) && this.ViewMode != null)
			{
				this.ViewMode.OnMouseUp(this.lastButtonReleaseEventArgs);
				this.isMouseDown = false;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000CFA8 File Offset: 0x0000B1A8
		private void LeaveNotifyHandle(object o, LeaveNotifyEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseLeave(args);
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000CFD0 File Offset: 0x0000B1D0
		private void EnterNotifyHandle(object o, EnterNotifyEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseEnter(args);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000CFF8 File Offset: 0x0000B1F8
		private void MotionNotifyHandle(object o, MotionNotifyEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseMove(args);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000D020 File Offset: 0x0000B220
		private void ButtonReleaseHandle(object o, ButtonReleaseEventArgs args)
		{
			this.isMouseDown = false;
			this.lastButtonReleaseEventArgs = args;
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseUp(args);
			}
			if (!args.CheckRetval())
			{
				this.ControlContextMenu(args);
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D068 File Offset: 0x0000B268
		private void ButtonPressHandle(object o, ButtonPressEventArgs args)
		{
			this.isMouseDown = true;
			if (!base.IsFocus)
			{
				base.IsFocus = true;
				base.GrabFocus();
			}
			if (this.ViewMode != null)
			{
				if (args.Event.Type == EventType.TwoButtonPress || args.Event.Type == EventType.ThreeButtonPress)
				{
					if (this.IsMouseEventArgsValid(this.lastButtonReleaseEventArgs))
					{
						this.ViewMode.OnMouseUp(this.lastButtonReleaseEventArgs);
					}
					this.ViewMode.OnMouseDoubleClick(args);
				}
				else
				{
					this.ViewMode.OnMouseDown(args);
					this.pasteObjectPosition = new Point((int)args.Event.X, (int)args.Event.Y);
				}
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D144 File Offset: 0x0000B344
		private void ScrollEventHandle(object o, ScrollEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseWheel(args);
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D16C File Offset: 0x0000B36C
		private bool IsMouseEventArgsValid(ButtonReleaseEventArgs args)
		{
			return this.lastButtonReleaseEventArgs != null && !double.IsNaN(this.lastButtonReleaseEventArgs.Event.X) && !double.IsNaN(this.lastButtonReleaseEventArgs.Event.Y);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D1BC File Offset: 0x0000B3BC
		private void KeyReleaseHandle(object o, KeyReleaseEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnKeyUp(args);
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000D1E4 File Offset: 0x0000B3E4
		private void KeyPressHandle(object o, KeyPressEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnKeyDown(args);
			}
			args.RetVal = true;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000D21C File Offset: 0x0000B41C
		private void DragDropHandle(object o, DragDropArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnDragDrop(args);
			}
			if (args.CheckRetval())
			{
				base.IsFocus = true;
				base.GrabFocus();
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000D264 File Offset: 0x0000B464
		private void DragLeaveHandle(object o, DragLeaveArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnDragLeave(args);
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000D28C File Offset: 0x0000B48C
		private void DragMotionHandle(object o, DragMotionArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnDragOver(args);
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		private void DragBeginHandle(object o, DragBeginArgs args)
		{
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D2B8 File Offset: 0x0000B4B8
		private void DragDataReceivedHandle(object o, DragDataReceivedArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.DragDataReceived(args);
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000D2E0 File Offset: 0x0000B4E0
		private void RigesterGesturesEvent()
		{
			this.AddGestureMagnifyHandler(new EventHandler<GestureMagnifyEventArgs>(this.GestureMagnifyEventHandle));
			this.AddGestureRotateHandler(new EventHandler<GestureRotateEventArgs>(this.GestureRotateEventHandle));
			this.AddGestureSwipeHandler(new EventHandler<GestureSwipeEventArgs>(this.GestureSwipeEventHandle));
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000D31C File Offset: 0x0000B51C
		private void GestureMagnifyEventHandle(object sender, GestureMagnifyEventArgs args)
		{
			double zoom = args.Magnification * 4.0;
			MouseGesturesEventArgs args2 = new MouseGesturesEventArgs(args.X, args.Y, zoom);
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseGestures(args2);
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000D368 File Offset: 0x0000B568
		private void GestureRotateEventHandle(object sender, GestureRotateEventArgs args)
		{
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000D36B File Offset: 0x0000B56B
		private void GestureSwipeEventHandle(object sender, GestureSwipeEventArgs args)
		{
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000D370 File Offset: 0x0000B570
		private void InitializeCommand(IEventAggregator eventAggregator)
		{
			if (eventAggregator == null)
			{
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000D38B File Offset: 0x0000B58B
		[CommandHandler(CmdEnum.DeleteCmd2)]
		[CommandHandler(CmdEnum.DeleteCmd)]
		private void DeleteFramd_Execute()
		{
			this.CommandService.DeleteObject();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000D39A File Offset: 0x0000B59A
		[CommandHandler(CmdEnum.CopyCmd)]
		private void CopyFramd_Execute()
		{
			this.CommandService.CopyObject();
			this.pasteObjectPosition = null;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000D3B0 File Offset: 0x0000B5B0
		[CommandHandler(CmdEnum.CutCmd)]
		private void CutFramd_Execute()
		{
			this.CommandService.CutObject();
			this.pasteObjectPosition = null;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000D3C8 File Offset: 0x0000B5C8
		[CommandHandler(CmdEnum.PasteCmd)]
		private void PasteFramd_Execute()
		{
			PasteObjectsChangeEventArgs args = null;
			if (this.pasteObjectPosition != null)
			{
				PointF controlPoint = this.pasteObjectPosition.Clone() as PointF;
				args = new PasteObjectsChangeEventArgs(this.ConvertControlToScene(controlPoint));
			}
			if (this.CommandService != null)
			{
				this.CommandService.PasteObject(args);
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000D428 File Offset: 0x0000B628
		public object GetNextCommandTarget()
		{
			return this.ViewMode.GetContextMenu();
		}

		// Token: 0x040000A3 RID: 163
		private const int imageMargin = 0;

		// Token: 0x040000A4 RID: 164
		private System.Timers.Timer timer;

		// Token: 0x040000A5 RID: 165
		private PointF pasteObjectPosition;

		// Token: 0x040000A6 RID: 166
		private int lockTag = 0;

		// Token: 0x040000A7 RID: 167
		private Size viewSize = new Size(0, 0);

		// Token: 0x040000A8 RID: 168
		private static TargetEntry[] target_table = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		// Token: 0x040000A9 RID: 169
		private bool needRedraw;

		// Token: 0x040000AA RID: 170
		private ButtonReleaseEventArgs lastButtonReleaseEventArgs;

		// Token: 0x040000AB RID: 171
		private bool isMouseDown = false;
	}
}

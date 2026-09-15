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
	public class GLView : EventBox, IGLView, ICommandRouter
	{
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

		private ICommandService CommandService
		{
			get
			{
				return ViewModeManager.Instance.Current.GetCommandService();
			}
		}

		private IViewMode ViewMode
		{
			get
			{
				return ViewModeManager.Instance.Current;
			}
		}

		public GameWindow GameWindow
		{
			get
			{
				return GameWindow.Current;
			}
		}

		public GLView(IEventAggregator eventAggregator)
		{
			Gtk.Drag.DestSet(this, DestDefaults.All, GLView.target_table, DragAction.Copy | DragAction.Move | DragAction.Link);
			base.DoubleBuffered = false;
			this.Initialize(eventAggregator);
		}

		private void Initialize(IEventAggregator eventAggregator)
		{
			base.Events = EventMask.PointerMotionMask;
			if (eventAggregator == null)
			{
				base.Sensitive = false;
			}
			this.InitializeCommand(eventAggregator);
		}

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

		private void InitializeTimer()
		{
			this.timer = new System.Timers.Timer(30.0);
			this.timer.Elapsed += this.Timer_Elapsed;
		}

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

		private void StartTimer()
		{
			if (this.timer != null && !this.timer.Enabled)
			{
				this.timer.Start();
			}
		}

		private void StopTimer()
		{
			if (this.timer != null && this.timer.Enabled)
			{
				this.timer.Stop();
			}
		}

		private void SizeAllocatedHandle(object o, SizeAllocatedArgs args)
		{
			if (this.GameWindow != null)
			{
				this.StopTimer();
				this.ResizeView(args.Allocation);
			}
		}

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

		public PointF ConvertControlToScene(PointF controlPoint)
		{
			controlPoint.Y = (float)this.viewSize.Height - controlPoint.Y;
			return controlPoint;
		}

		public PointF ConvertSceneToControl(PointF sencePoint)
		{
			sencePoint.Y = (float)this.viewSize.Height - sencePoint.Y;
			return sencePoint;
		}

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

		public void OnCanvasSizeChanged()
		{
			this.NeedRedraw = true;
			this.ViewMode.OnCanvasSizeChanged();
		}

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

		public float ActualWidth
		{
			get
			{
				return (float)this.viewSize.Width;
			}
		}

		public float ActualHeight
		{
			get
			{
				return (float)this.viewSize.Height;
			}
		}

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

		private void FocusInEventHandle(object o, FocusInEventArgs args)
		{
		}

		private void FocusOutEventHandle(object o, FocusOutEventArgs args)
		{
			if (this.isMouseDown && this.IsMouseEventArgsValid(this.lastButtonReleaseEventArgs) && this.ViewMode != null)
			{
				this.ViewMode.OnMouseUp(this.lastButtonReleaseEventArgs);
				this.isMouseDown = false;
			}
		}

		private void LeaveNotifyHandle(object o, LeaveNotifyEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseLeave(args);
			}
		}

		private void EnterNotifyHandle(object o, EnterNotifyEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseEnter(args);
			}
		}

		private void MotionNotifyHandle(object o, MotionNotifyEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseMove(args);
			}
		}

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

		private void ScrollEventHandle(object o, ScrollEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseWheel(args);
			}
		}

		private bool IsMouseEventArgsValid(ButtonReleaseEventArgs args)
		{
			return this.lastButtonReleaseEventArgs != null && !double.IsNaN(this.lastButtonReleaseEventArgs.Event.X) && !double.IsNaN(this.lastButtonReleaseEventArgs.Event.Y);
		}

		private void KeyReleaseHandle(object o, KeyReleaseEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnKeyUp(args);
			}
		}

		private void KeyPressHandle(object o, KeyPressEventArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnKeyDown(args);
			}
			args.RetVal = true;
		}

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

		private void DragLeaveHandle(object o, DragLeaveArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnDragLeave(args);
			}
		}

		private void DragMotionHandle(object o, DragMotionArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.OnDragOver(args);
			}
		}

		private void DragBeginHandle(object o, DragBeginArgs args)
		{
		}

		private void DragDataReceivedHandle(object o, DragDataReceivedArgs args)
		{
			if (this.ViewMode != null)
			{
				this.ViewMode.DragDataReceived(args);
			}
		}

		private void RigesterGesturesEvent()
		{
			this.AddGestureMagnifyHandler(new EventHandler<GestureMagnifyEventArgs>(this.GestureMagnifyEventHandle));
			this.AddGestureRotateHandler(new EventHandler<GestureRotateEventArgs>(this.GestureRotateEventHandle));
			this.AddGestureSwipeHandler(new EventHandler<GestureSwipeEventArgs>(this.GestureSwipeEventHandle));
		}

		private void GestureMagnifyEventHandle(object sender, GestureMagnifyEventArgs args)
		{
			double zoom = args.Magnification * 4.0;
			MouseGesturesEventArgs args2 = new MouseGesturesEventArgs(args.X, args.Y, zoom);
			if (this.ViewMode != null)
			{
				this.ViewMode.OnMouseGestures(args2);
			}
		}

		private void GestureRotateEventHandle(object sender, GestureRotateEventArgs args)
		{
		}

		private void GestureSwipeEventHandle(object sender, GestureSwipeEventArgs args)
		{
		}

		private void InitializeCommand(IEventAggregator eventAggregator)
		{
			if (eventAggregator == null)
			{
			}
		}

		[CommandHandler(CmdEnum.DeleteCmd2)]
		[CommandHandler(CmdEnum.DeleteCmd)]
		private void DeleteFramd_Execute()
		{
			this.CommandService.DeleteObject();
		}

		[CommandHandler(CmdEnum.CopyCmd)]
		private void CopyFramd_Execute()
		{
			this.CommandService.CopyObject();
			this.pasteObjectPosition = null;
		}

		[CommandHandler(CmdEnum.CutCmd)]
		private void CutFramd_Execute()
		{
			this.CommandService.CutObject();
			this.pasteObjectPosition = null;
		}

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

		public object GetNextCommandTarget()
		{
			return this.ViewMode.GetContextMenu();
		}

		private const int imageMargin = 0;

		private System.Timers.Timer timer;

		private PointF pasteObjectPosition;

		private int lockTag = 0;

		private Size viewSize = new Size(0, 0);

		private static TargetEntry[] target_table = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		private bool needRedraw;

		private ButtonReleaseEventArgs lastButtonReleaseEventArgs;

		private bool isMouseDown = false;
	}
}

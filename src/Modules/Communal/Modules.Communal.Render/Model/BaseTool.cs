using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.Render.Model
{
	public abstract class BaseTool : ITool, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, ICoordinateMapping
	{
		public abstract Xwt.Drawing.Image Icon { get; }

		public abstract string Tooltip { get; }

		public abstract Gdk.Key ShortcutKey { get; }

		public virtual bool HasSeparator
		{
			get
			{
				return false;
			}
		}

		public virtual ToolType Type
		{
			get
			{
				return ToolType.Radio;
			}
		}

		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled != value)
				{
					this.enabled = value;
					this.OnEnabledChanged();
				}
			}
		}

		public event EventHandler EnabledChanged;

		public event EventHandler SelectedChanged;

		public virtual ToolGroup Group { get; set; }

		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					this.OnSelectedChanged();
				}
			}
		}

		public virtual Widget CustomWidget
		{
			get
			{
				return null;
			}
		}

		protected virtual void OnSelectedChanged()
		{
			if (this.SelectedChanged != null)
			{
				this.SelectedChanged(this, new EventArgs());
			}
		}

		protected virtual void OnEnabledChanged()
		{
			if (this.EnabledChanged != null)
			{
				this.EnabledChanged(this, new EventArgs());
			}
		}

		public virtual void Initialize()
		{
		}

		public virtual void Load()
		{
		}

		public virtual void UnLoad()
		{
		}

		public bool ResponseKeyPress(KeyPressEventArgs args)
		{
			string b = args.Event.Key.ToString().ToUpperInvariant();
			bool result;
			if (this.ShortcutKey.ToString().ToUpperInvariant() != b)
			{
				result = false;
			}
			else if (!this.Enabled)
			{
				args.RetVal = true;
				result = true;
			}
			else
			{
				this.OnKeyDown(args);
				if (this.Type == ToolType.Radio)
				{
					this.Group.Current = this;
				}
				else if (this.Type == ToolType.Toggle)
				{
					this.IsSelected = !this.IsSelected;
				}
				args.RetVal = true;
				result = true;
			}
			return result;
		}

		public virtual void OnMouseMove(MotionNotifyEventArgs args)
		{
		}

		public virtual void OnMouseUp(ButtonReleaseEventArgs args)
		{
		}

		public virtual void OnMouseDown(ButtonPressEventArgs args)
		{
		}

		public virtual void OnMouseEnter(EnterNotifyEventArgs args)
		{
		}

		public virtual void OnMouseLeave(LeaveNotifyEventArgs args)
		{
		}

		public virtual void OnMouseWheel(ScrollEventArgs args)
		{
		}

		public virtual void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
		}

		public virtual void OnMouseGestures(MouseGesturesEventArgs args)
		{
		}

		public virtual void OnKeyDown(KeyPressEventArgs args)
		{
		}

		public virtual void OnKeyUp(KeyReleaseEventArgs args)
		{
		}

		public virtual PointF ConvertCoordinate(PointF point)
		{
			return GameWindow.Current.ConvertControlToScene(point);
		}

		protected static void SetCursor(Cursor cursor)
		{
			if (GameWindow.Current.GdkWindow != null)
			{
				GameWindow.Current.GdkWindow.Cursor = cursor;
			}
		}

		protected static bool IsMouseMoved(PointF originPoint, PointF currentPoint)
		{
			return originPoint.IsEmpty || Math.Abs(originPoint.X - currentPoint.X) > 2f || Math.Abs(originPoint.Y - currentPoint.Y) > 2f;
		}

		private bool enabled = true;

		private bool _isSelected;
	}
}

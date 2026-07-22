using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.Render.Model
{
	// Token: 0x0200000F RID: 15
	public abstract class BaseTool : ITool, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, ICoordinateMapping
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007D RID: 125
		public abstract Xwt.Drawing.Image Icon { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007E RID: 126
		public abstract string Tooltip { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007F RID: 127
		public abstract Gdk.Key ShortcutKey { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00005870 File Offset: 0x00003A70
		public virtual bool HasSeparator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00005884 File Offset: 0x00003A84
		public virtual ToolType Type
		{
			get
			{
				return ToolType.Radio;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00005898 File Offset: 0x00003A98
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000058B0 File Offset: 0x00003AB0
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

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000084 RID: 132 RVA: 0x000058E0 File Offset: 0x00003AE0
		// (remove) Token: 0x06000085 RID: 133 RVA: 0x0000591C File Offset: 0x00003B1C
		public event EventHandler EnabledChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000086 RID: 134 RVA: 0x00005958 File Offset: 0x00003B58
		// (remove) Token: 0x06000087 RID: 135 RVA: 0x00005994 File Offset: 0x00003B94
		public event EventHandler SelectedChanged;

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000059D0 File Offset: 0x00003BD0
		// (set) Token: 0x06000089 RID: 137 RVA: 0x000059E7 File Offset: 0x00003BE7
		public virtual ToolGroup Group { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000059F0 File Offset: 0x00003BF0
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00005A08 File Offset: 0x00003C08
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00005A38 File Offset: 0x00003C38
		public virtual Widget CustomWidget
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005A4C File Offset: 0x00003C4C
		protected virtual void OnSelectedChanged()
		{
			if (this.SelectedChanged != null)
			{
				this.SelectedChanged(this, new EventArgs());
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005A7C File Offset: 0x00003C7C
		protected virtual void OnEnabledChanged()
		{
			if (this.EnabledChanged != null)
			{
				this.EnabledChanged(this, new EventArgs());
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005AA9 File Offset: 0x00003CA9
		public virtual void Initialize()
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005AAC File Offset: 0x00003CAC
		public virtual void Load()
		{
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005AAF File Offset: 0x00003CAF
		public virtual void UnLoad()
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005AB4 File Offset: 0x00003CB4
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

		// Token: 0x06000093 RID: 147 RVA: 0x00005B7C File Offset: 0x00003D7C
		public virtual void OnMouseMove(MotionNotifyEventArgs args)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005B7F File Offset: 0x00003D7F
		public virtual void OnMouseUp(ButtonReleaseEventArgs args)
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005B82 File Offset: 0x00003D82
		public virtual void OnMouseDown(ButtonPressEventArgs args)
		{
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005B85 File Offset: 0x00003D85
		public virtual void OnMouseEnter(EnterNotifyEventArgs args)
		{
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005B88 File Offset: 0x00003D88
		public virtual void OnMouseLeave(LeaveNotifyEventArgs args)
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005B8B File Offset: 0x00003D8B
		public virtual void OnMouseWheel(ScrollEventArgs args)
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005B8E File Offset: 0x00003D8E
		public virtual void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005B91 File Offset: 0x00003D91
		public virtual void OnMouseGestures(MouseGesturesEventArgs args)
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005B94 File Offset: 0x00003D94
		public virtual void OnKeyDown(KeyPressEventArgs args)
		{
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005B97 File Offset: 0x00003D97
		public virtual void OnKeyUp(KeyReleaseEventArgs args)
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005B9C File Offset: 0x00003D9C
		public virtual PointF ConvertCoordinate(PointF point)
		{
			return GameWindow.Current.ConvertControlToScene(point);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005BBC File Offset: 0x00003DBC
		protected static void SetCursor(Cursor cursor)
		{
			if (GameWindow.Current.GdkWindow != null)
			{
				GameWindow.Current.GdkWindow.Cursor = cursor;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005BEC File Offset: 0x00003DEC
		protected static bool IsMouseMoved(PointF originPoint, PointF currentPoint)
		{
			return originPoint.IsEmpty || Math.Abs(originPoint.X - currentPoint.X) > 2f || Math.Abs(originPoint.Y - currentPoint.Y) > 2f;
		}

		// Token: 0x04000019 RID: 25
		private bool enabled = true;

		// Token: 0x0400001C RID: 28
		private bool _isSelected;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Projects;
using Gtk;

namespace Modules.Communal.Render.Model
{
	public class ToolGroup : IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl
	{
		public BaseTool[] Items
		{
			get
			{
				return this.items;
			}
		}

		public ITool Current
		{
			get
			{
				return this._currentTool;
			}
			set
			{
				if (this._currentTool != value)
				{
					if (!this.items.Contains(value))
					{
						throw new InvalidOperationException("This group did not have this tool.");
					}
					ToolGroup.OnCurrentToolChanged(this._currentTool as BaseTool, value as BaseTool);
					this._currentTool = value;
					if (this.CurrentChanged != null)
					{
						this.CurrentChanged(this, new CurrentToolChangedEventArgs(value));
					}
				}
			}
		}

		public event EventHandler<CurrentToolChangedEventArgs> CurrentChanged;

		public ToolGroup(IEnumerable<BaseTool> toolList)
		{
			this.items = toolList.ToArray<BaseTool>();
			foreach (BaseTool baseTool in toolList)
			{
				baseTool.Group = this;
			}
		}

		private static void OnCurrentToolChanged(BaseTool oldValue, BaseTool newValue)
		{
			if (oldValue != null)
			{
				oldValue.IsSelected = false;
			}
			if (newValue != null)
			{
				newValue.IsSelected = true;
			}
		}

		public void Initialize(IGLView glView)
		{
			if (this.items != null)
			{
				foreach (BaseTool baseTool in this.items)
				{
					baseTool.Initialize();
				}
			}
		}

		public void OnMouseMove(MotionNotifyEventArgs args)
		{
			this.Current.OnMouseMove(args);
		}

		public void OnMouseUp(ButtonReleaseEventArgs args)
		{
			this.Current.OnMouseUp(args);
		}

		public void OnMouseDown(ButtonPressEventArgs args)
		{
			this.Current.OnMouseDown(args);
		}

		public void OnMouseEnter(EnterNotifyEventArgs args)
		{
			this.Current.OnMouseEnter(args);
		}

		public void OnMouseLeave(LeaveNotifyEventArgs args)
		{
			this.Current.OnMouseLeave(args);
		}

		public void OnMouseWheel(ScrollEventArgs args)
		{
			this.Current.OnMouseWheel(args);
		}

		public void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
			this.Current.OnMouseDoubleClick(args);
		}

		public void OnMouseGestures(MouseGesturesEventArgs args)
		{
			this.Current.OnMouseGestures(args);
		}

		public void OnKeyDown(KeyPressEventArgs args)
		{
			bool flag = false;
			foreach (BaseTool baseTool in this.items)
			{
				if (baseTool.ResponseKeyPress(args))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.Current.OnKeyDown(args);
			}
		}

		public void OnKeyUp(KeyReleaseEventArgs args)
		{
			this.Current.OnKeyUp(args);
		}

		public virtual void Activated(CocosItem cocosItem)
		{
			foreach (BaseTool baseTool in this.items)
			{
				baseTool.Load();
			}
		}

		public virtual void Deactivated()
		{
			foreach (BaseTool baseTool in this.items)
			{
				baseTool.UnLoad();
			}
		}

		public T GetTool<T>() where T : BaseTool
		{
			return this.items.FirstOrDefault((BaseTool a) => a.GetType() == typeof(T)) as T;
		}

		private BaseTool[] items;

		private ITool _currentTool;
	}
}

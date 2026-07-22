using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Projects;
using Gtk;

namespace Modules.Communal.Render.Model
{
	// Token: 0x0200002B RID: 43
	public class ToolGroup : IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00009938 File Offset: 0x00007B38
		public BaseTool[] Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00009950 File Offset: 0x00007B50
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00009968 File Offset: 0x00007B68
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

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000199 RID: 409 RVA: 0x000099E4 File Offset: 0x00007BE4
		// (remove) Token: 0x0600019A RID: 410 RVA: 0x00009A20 File Offset: 0x00007C20
		public event EventHandler<CurrentToolChangedEventArgs> CurrentChanged;

		// Token: 0x0600019B RID: 411 RVA: 0x00009A5C File Offset: 0x00007C5C
		public ToolGroup(IEnumerable<BaseTool> toolList)
		{
			this.items = toolList.ToArray<BaseTool>();
			foreach (BaseTool baseTool in toolList)
			{
				baseTool.Group = this;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00009AC8 File Offset: 0x00007CC8
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

		// Token: 0x0600019D RID: 413 RVA: 0x00009AF8 File Offset: 0x00007CF8
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

		// Token: 0x0600019E RID: 414 RVA: 0x00009B3F File Offset: 0x00007D3F
		public void OnMouseMove(MotionNotifyEventArgs args)
		{
			this.Current.OnMouseMove(args);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00009B4F File Offset: 0x00007D4F
		public void OnMouseUp(ButtonReleaseEventArgs args)
		{
			this.Current.OnMouseUp(args);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00009B5F File Offset: 0x00007D5F
		public void OnMouseDown(ButtonPressEventArgs args)
		{
			this.Current.OnMouseDown(args);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00009B6F File Offset: 0x00007D6F
		public void OnMouseEnter(EnterNotifyEventArgs args)
		{
			this.Current.OnMouseEnter(args);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00009B7F File Offset: 0x00007D7F
		public void OnMouseLeave(LeaveNotifyEventArgs args)
		{
			this.Current.OnMouseLeave(args);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00009B8F File Offset: 0x00007D8F
		public void OnMouseWheel(ScrollEventArgs args)
		{
			this.Current.OnMouseWheel(args);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00009B9F File Offset: 0x00007D9F
		public void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
			this.Current.OnMouseDoubleClick(args);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00009BAF File Offset: 0x00007DAF
		public void OnMouseGestures(MouseGesturesEventArgs args)
		{
			this.Current.OnMouseGestures(args);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00009BC0 File Offset: 0x00007DC0
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

		// Token: 0x060001A7 RID: 423 RVA: 0x00009C1D File Offset: 0x00007E1D
		public void OnKeyUp(KeyReleaseEventArgs args)
		{
			this.Current.OnKeyUp(args);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009C30 File Offset: 0x00007E30
		public virtual void Activated(CocosItem cocosItem)
		{
			foreach (BaseTool baseTool in this.items)
			{
				baseTool.Load();
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009C68 File Offset: 0x00007E68
		public virtual void Deactivated()
		{
			foreach (BaseTool baseTool in this.items)
			{
				baseTool.UnLoad();
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009CC8 File Offset: 0x00007EC8
		public T GetTool<T>() where T : BaseTool
		{
			return this.items.FirstOrDefault((BaseTool a) => a.GetType() == typeof(T)) as T;
		}

		// Token: 0x0400006C RID: 108
		private BaseTool[] items;

		// Token: 0x0400006D RID: 109
		private ITool _currentTool;
	}
}

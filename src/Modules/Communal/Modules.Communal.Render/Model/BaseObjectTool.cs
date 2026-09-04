using System;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using Gdk;
using Gtk;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000010 RID: 16
	public abstract class BaseObjectTool : BaseTool
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00005C58 File Offset: 0x00003E58
		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			this.clickPoint = args.Event.GetPoint();
			if (args.Event.GetMouseButton() == MouseButton.Left && !args.Event.State.HasFlag(ModifierType.ControlMask) && !args.Event.State.HasFlag(ModifierType.Mod2Mask))
			{
				if (this.controlNode != null && this.controlNode.Visible)
				{
					if (this.HitTest(this.clickPoint))
					{
						Services.TaskService.BeginCompositeTask("ObjectTool MouseDown");
						MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, this);
						this.controlNode.MouseDown(args2);
						args.RetVal = true;
					}
				}
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005D34 File Offset: 0x00003F34
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			if (this.controlNode != null)
			{
				MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, this);
				this.controlNode.MouseUp(args2);
				if (Services.TaskService.IsRunningCompositeTask && !Services.TaskService.IsEmptyCompositeTask)
				{
					SelectService.Instance.BeginTask();
					SelectService.Instance.EndTask();
				}
				Services.TaskService.EndCompositeTask();
				if (BaseTool.IsMouseMoved(this.clickPoint, args.Event.GetPoint()))
				{
					args.RetVal = true;
				}
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005DE0 File Offset: 0x00003FE0
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (this.controlNode != null)
			{
				if (args.Event.State != ModifierType.None)
				{
					MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, this);
					this.controlNode.MouseMove(args2);
					args.RetVal = true;
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005E3C File Offset: 0x0000403C
		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (this.controlNode != null)
			{
				if (this.controlNode.Visible)
				{
					this.controlNode.KeyDown(args);
				}
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005E80 File Offset: 0x00004080
		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (this.controlNode != null)
			{
				if (this.controlNode.Visible)
				{
					this.controlNode.KeyUp(args);
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005EC4 File Offset: 0x000040C4
		public override void Load()
		{
			if (this.controlNode != null)
			{
				this.controlNode.Visible = true;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005EF4 File Offset: 0x000040F4
		public override void UnLoad()
		{
			if (this.controlNode != null)
			{
				this.controlNode.Visible = false;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005F24 File Offset: 0x00004124
		protected virtual bool HitTest(PointF widgetPoint)
		{
			bool result;
			if (this.controlNode == null)
			{
				result = false;
			}
			else
			{
				PointF point = this.ConvertCoordinate(widgetPoint);
				HitTestResult hitTestResult = this.controlNode.HitTest(point);
				result = (hitTestResult != null && hitTestResult.HitVisual != null);
			}
			return result;
		}

		// Token: 0x0400001E RID: 30
		protected VisualObject controlNode;

		// Token: 0x0400001F RID: 31
		protected PointF clickPoint = PointF.Empty;
	}
}

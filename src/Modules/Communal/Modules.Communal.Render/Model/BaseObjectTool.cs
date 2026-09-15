using System;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using Gdk;
using Gtk;

namespace Modules.Communal.Render.Model
{
	public abstract class BaseObjectTool : BaseTool
	{
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

		public override void Load()
		{
			if (this.controlNode != null)
			{
				this.controlNode.Visible = true;
			}
		}

		public override void UnLoad()
		{
			if (this.controlNode != null)
			{
				this.controlNode.Visible = false;
			}
		}

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

		protected VisualObject controlNode;

		protected PointF clickPoint = PointF.Empty;
	}
}

using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.Render;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render3D.Model
{
	internal class Light3DTool : IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl, IDocumentEventHandler
	{
		public void OnMouseDown(ButtonPressEventArgs args)
		{
			this.activedLight = null;
			IReadOnlyList<VisualObject> selectedObjectList = SelectService.Instance.SelectedObjectList;
			if (selectedObjectList.Count == 0)
			{
				return;
			}
			foreach (VisualObject visualObject in selectedObjectList)
			{
				Light3DObject light3DObject = visualObject as Light3DObject;
				if (light3DObject == null || light3DObject.Type == LightType.AMBIENT || light3DObject.Type == LightType.DIRECTIONAL)
				{
					return;
				}
				MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, null);
				if (light3DObject.HitTestControlNode(args2))
				{
					this.activedLight = light3DObject;
					this.activedLight.MouseDown(args2);
					break;
				}
			}
			if (this.activedLight != null)
			{
				args.RetVal = true;
			}
		}

		public void OnMouseUp(ButtonReleaseEventArgs args)
		{
			if (this.activedLight != null)
			{
				MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, null);
				this.activedLight.MouseUp(args2);
				args.RetVal = true;
				this.activedLight = null;
			}
		}

		public void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (this.activedLight != null)
			{
				MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, null);
				this.activedLight.MouseMove(args2);
				args.RetVal = true;
			}
		}

		public void Initialize(IGLView glView)
		{
		}

		public void Activated(CocosItem cocosItem)
		{
		}

		public void Deactivated()
		{
		}

		public void OnMouseEnter(EnterNotifyEventArgs args)
		{
		}

		public void OnMouseLeave(LeaveNotifyEventArgs args)
		{
		}

		public void OnMouseWheel(ScrollEventArgs args)
		{
		}

		public void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
		}

		public void OnMouseGestures(MouseGesturesEventArgs args)
		{
		}

		public void OnKeyDown(KeyPressEventArgs args)
		{
		}

		public void OnKeyUp(KeyReleaseEventArgs args)
		{
		}

		public void OnDocumentChanged(CocosItem cocosItem)
		{
			Light3DHelper.Instance.Clear();
			if (this.currentDocument != null)
			{
				GameNode3DObject node = this.currentDocument.GetRootNode() as GameNode3DObject;
				this.RefreshLightState(node, false);
			}
			if (cocosItem != null)
			{
				Light3DHelper.Instance.Enbaled = false;
				GameNode3DObject node2 = cocosItem.GetRootNode() as GameNode3DObject;
				this.RefreshLightState(node2, true);
				Light3DHelper.Instance.Enbaled = true;
			}
			this.currentDocument = cocosItem;
		}

		public void OnDocumentSaved(CocosItem cocosItem)
		{
		}

		public void OnDocumentBeforeSave(CocosItem cocosItem)
		{
		}

		public void OnDocumentClosed(CocosItem cocosItem)
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				this.currentDocument = null;
			}
		}

		private void RefreshLightState(AbstractNodeObject node, bool enabled)
		{
			if (node == null || node.Children == null)
			{
				return;
			}
			foreach (AbstractNodeObject abstractNodeObject in node.Children)
			{
				Light3DObject light3DObject = abstractNodeObject as Light3DObject;
				if (light3DObject != null)
				{
					light3DObject.RefreshLightState(enabled);
					if (enabled)
					{
						Light3DHelper.Instance.AddLight(light3DObject);
					}
				}
				this.RefreshLightState(abstractNodeObject, enabled);
			}
		}

		private Light3DObject activedLight;

		private CocosItem currentDocument;
	}
}

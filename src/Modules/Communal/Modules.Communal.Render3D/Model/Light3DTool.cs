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
	// Token: 0x02000007 RID: 7
	internal class Light3DTool : IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl, IDocumentEventHandler
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002A44 File Offset: 0x00000C44
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

		// Token: 0x0600002E RID: 46 RVA: 0x00002B08 File Offset: 0x00000D08
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

		// Token: 0x0600002F RID: 47 RVA: 0x00002B4C File Offset: 0x00000D4C
		public void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (this.activedLight != null)
			{
				MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, null);
				this.activedLight.MouseMove(args2);
				args.RetVal = true;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B86 File Offset: 0x00000D86
		public void Initialize(IGLView glView)
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002B88 File Offset: 0x00000D88
		public void Activated(CocosItem cocosItem)
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002B8A File Offset: 0x00000D8A
		public void Deactivated()
		{
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002B8C File Offset: 0x00000D8C
		public void OnMouseEnter(EnterNotifyEventArgs args)
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B8E File Offset: 0x00000D8E
		public void OnMouseLeave(LeaveNotifyEventArgs args)
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B90 File Offset: 0x00000D90
		public void OnMouseWheel(ScrollEventArgs args)
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002B92 File Offset: 0x00000D92
		public void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B94 File Offset: 0x00000D94
		public void OnMouseGestures(MouseGesturesEventArgs args)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B96 File Offset: 0x00000D96
		public void OnKeyDown(KeyPressEventArgs args)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002B98 File Offset: 0x00000D98
		public void OnKeyUp(KeyReleaseEventArgs args)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B9C File Offset: 0x00000D9C
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

		// Token: 0x0600003B RID: 59 RVA: 0x00002C08 File Offset: 0x00000E08
		public void OnDocumentSaved(CocosItem cocosItem)
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002C0A File Offset: 0x00000E0A
		public void OnDocumentBeforeSave(CocosItem cocosItem)
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002C0C File Offset: 0x00000E0C
		public void OnDocumentClosed(CocosItem cocosItem)
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				this.currentDocument = null;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002C24 File Offset: 0x00000E24
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

		// Token: 0x0400000E RID: 14
		private Light3DObject activedLight;

		// Token: 0x0400000F RID: 15
		private CocosItem currentDocument;
	}
}

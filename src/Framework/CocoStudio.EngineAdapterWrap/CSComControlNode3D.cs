using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000035 RID: 53
	public class CSComControlNode3D : CSVisualObject
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x0000A9EC File Offset: 0x00008BEC
		public CSComControlNode3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0000AA0C File Offset: 0x00008C0C
		public static HandleRef getCPtr(CSComControlNode3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0000AA38 File Offset: 0x00008C38
		~CSComControlNode3D()
		{
			this.Dispose();
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0000AA9C File Offset: 0x00008C9C
		public override void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						HandleRef handle = new HandleRef(null, this.swigCPtr.Handle);
						if (this.IsContainOpenGLResource())
						{
							GtkInvokeHelp.BeginInvoke(delegate
							{
								this.swigCPtr = handle;
								CocoStudioEngineAdapterPINVOKE.delete_CSComControlNode3D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSComControlNode3D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0000AB9C File Offset: 0x00008D9C
		public CSComControlNode3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSComControlNode3D(), true)
		{
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000ABAD File Offset: 0x00008DAD
		public void SetOpt(ControlOpt.Opt o)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SetOpt(this.swigCPtr, (int)o);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0000ABC0 File Offset: 0x00008DC0
		public ControlOpt.Opt GetOpt()
		{
			return (ControlOpt.Opt)CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_GetOpt(this.swigCPtr);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0000ABDF File Offset: 0x00008DDF
		public void SetSpace(bool isGlobal)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SetSpace(this.swigCPtr, isGlobal);
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0000ABF0 File Offset: 0x00008DF0
		public bool GetSpace()
		{
			return CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_GetSpace(this.swigCPtr);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0000AC0F File Offset: 0x00008E0F
		public void SetSelect(bool select)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SetSelect(this.swigCPtr, select);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0000AC20 File Offset: 0x00008E20
		public bool OnSelect(PointF screenPoint)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_OnSelect(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0000AC69 File Offset: 0x00008E69
		public void SetTarget(CSVisualObject targetObject)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SetTarget(this.swigCPtr, CSVisualObject.getCPtr(targetObject));
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0000AC80 File Offset: 0x00008E80
		public bool OnMouseDown(PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_OnMouseDown(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0000ACCC File Offset: 0x00008ECC
		public ControlResult OnMouseMove(PointF point)
		{
			ControlResult result = new ControlResult(CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_OnMouseMove(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y))), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0000AD1C File Offset: 0x00008F1C
		public void OnMouseUp(PointF point)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_OnMouseUp(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0000AD60 File Offset: 0x00008F60
		public void RefreshSelectable()
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_RefreshSelectable(this.swigCPtr);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0000AD6F File Offset: 0x00008F6F
		public void SelectShiftKey(bool isSelect)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SelectShiftKey(this.swigCPtr, isSelect);
		}

		// Token: 0x04000058 RID: 88
		private HandleRef swigCPtr;
	}
}

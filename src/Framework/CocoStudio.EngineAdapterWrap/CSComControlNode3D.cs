using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSComControlNode3D : CSVisualObject
	{
		public CSComControlNode3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSComControlNode3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSComControlNode3D()
		{
			this.Dispose();
		}

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

		public CSComControlNode3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSComControlNode3D(), true)
		{
		}

		public void SetOpt(ControlOpt.Opt o)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SetOpt(this.swigCPtr, (int)o);
		}

		public ControlOpt.Opt GetOpt()
		{
			return (ControlOpt.Opt)CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_GetOpt(this.swigCPtr);
		}

		public void SetSpace(bool isGlobal)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SetSpace(this.swigCPtr, isGlobal);
		}

		public bool GetSpace()
		{
			return CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_GetSpace(this.swigCPtr);
		}

		public void SetSelect(bool select)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SetSelect(this.swigCPtr, select);
		}

		public bool OnSelect(PointF screenPoint)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_OnSelect(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void SetTarget(CSVisualObject targetObject)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SetTarget(this.swigCPtr, CSVisualObject.getCPtr(targetObject));
		}

		public bool OnMouseDown(PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_OnMouseDown(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public ControlResult OnMouseMove(PointF point)
		{
			ControlResult result = new ControlResult(CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_OnMouseMove(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y))), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void OnMouseUp(PointF point)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_OnMouseUp(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void RefreshSelectable()
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_RefreshSelectable(this.swigCPtr);
		}

		public void SelectShiftKey(bool isSelect)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode3D_SelectShiftKey(this.swigCPtr, isSelect);
		}

		private HandleRef swigCPtr;
	}
}

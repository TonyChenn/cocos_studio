using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSTranslateNodeDrawPen : CSControlNodeDrawPen
	{
		public CSTranslateNodeDrawPen(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSTranslateNodeDrawPen obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSTranslateNodeDrawPen()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSTranslateNodeDrawPen(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSTranslateNodeDrawPen(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSTranslateNodeDrawPen() : this(CocoStudioEngineAdapterPINVOKE.new_CSTranslateNodeDrawPen(), true)
		{
		}

		public void SetXYAreaSize(float xysize)
		{
			CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_SetXYAreaSize(this.swigCPtr, xysize);
		}

		public float GetXYAreaSize()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_GetXYAreaSize(this.swigCPtr);
		}

		public override void DrawControl()
		{
			CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_DrawControl(this.swigCPtr);
		}

		public override CSControlNodeDrawPen.OperateState PointAtControl(PointF point)
		{
			CSControlNodeDrawPen.OperateState result = (CSControlNodeDrawPen.OperateState)CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_PointAtControl(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private HandleRef swigCPtr;
	}
}

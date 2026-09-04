using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000048 RID: 72
	public class CSTranslateNodeDrawPen : CSControlNodeDrawPen
	{
		// Token: 0x060009AA RID: 2474 RVA: 0x0000DAFF File Offset: 0x0000BCFF
		public CSTranslateNodeDrawPen(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0000DB20 File Offset: 0x0000BD20
		public static HandleRef getCPtr(CSTranslateNodeDrawPen obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		~CSTranslateNodeDrawPen()
		{
			this.Dispose();
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
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

		// Token: 0x060009AE RID: 2478 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
		public CSTranslateNodeDrawPen() : this(CocoStudioEngineAdapterPINVOKE.new_CSTranslateNodeDrawPen(), true)
		{
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0000DCC1 File Offset: 0x0000BEC1
		public void SetXYAreaSize(float xysize)
		{
			CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_SetXYAreaSize(this.swigCPtr, xysize);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0000DCD4 File Offset: 0x0000BED4
		public float GetXYAreaSize()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_GetXYAreaSize(this.swigCPtr);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0000DCF3 File Offset: 0x0000BEF3
		public override void DrawControl()
		{
			CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_DrawControl(this.swigCPtr);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0000DD04 File Offset: 0x0000BF04
		public override CSControlNodeDrawPen.OperateState PointAtControl(PointF point)
		{
			CSControlNodeDrawPen.OperateState result = (CSControlNodeDrawPen.OperateState)CocoStudioEngineAdapterPINVOKE.CSTranslateNodeDrawPen_PointAtControl(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0400007F RID: 127
		private HandleRef swigCPtr;
	}
}

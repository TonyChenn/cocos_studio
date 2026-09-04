using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000049 RID: 73
	public class CSScaleNodeDrawPen : CSTranslateNodeDrawPen
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x0000DD4D File Offset: 0x0000BF4D
		public CSScaleNodeDrawPen(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSScaleNodeDrawPen_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		public static HandleRef getCPtr(CSScaleNodeDrawPen obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0000DD98 File Offset: 0x0000BF98
		~CSScaleNodeDrawPen()
		{
			this.Dispose();
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0000DDFC File Offset: 0x0000BFFC
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
								CocoStudioEngineAdapterPINVOKE.delete_CSScaleNodeDrawPen(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSScaleNodeDrawPen(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0000DEFC File Offset: 0x0000C0FC
		public override void DrawControl()
		{
			CocoStudioEngineAdapterPINVOKE.CSScaleNodeDrawPen_DrawControl(this.swigCPtr);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0000DF0C File Offset: 0x0000C10C
		public override CSControlNodeDrawPen.OperateState PointAtControl(PointF point)
		{
			CSControlNodeDrawPen.OperateState result = (CSControlNodeDrawPen.OperateState)CocoStudioEngineAdapterPINVOKE.CSScaleNodeDrawPen_PointAtControl(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0000DF58 File Offset: 0x0000C158
		public void StretchLine(PointF deltaXY)
		{
			CocoStudioEngineAdapterPINVOKE.CSScaleNodeDrawPen_StretchLine(this.swigCPtr, Vec2.getCPtr(new Vec2(deltaXY.X, deltaXY.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0000DF9C File Offset: 0x0000C19C
		public void ResetLine()
		{
			CocoStudioEngineAdapterPINVOKE.CSScaleNodeDrawPen_ResetLine(this.swigCPtr);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0000DFAB File Offset: 0x0000C1AB
		public CSScaleNodeDrawPen() : this(CocoStudioEngineAdapterPINVOKE.new_CSScaleNodeDrawPen(), true)
		{
		}

		// Token: 0x04000080 RID: 128
		private HandleRef swigCPtr;
	}
}

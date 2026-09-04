using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000044 RID: 68
	public class CSControlNodeDrawPen : IDisposable
	{
		// Token: 0x06000981 RID: 2433 RVA: 0x0000D2BF File Offset: 0x0000B4BF
		public CSControlNodeDrawPen(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0000D2E0 File Offset: 0x0000B4E0
		public static HandleRef getCPtr(CSControlNodeDrawPen obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0000D30C File Offset: 0x0000B50C
		~CSControlNodeDrawPen()
		{
			this.Dispose();
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0000D370 File Offset: 0x0000B570
		public virtual void Dispose()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSControlNodeDrawPen(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSControlNodeDrawPen(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0000D468 File Offset: 0x0000B668
		public CSControlNodeDrawPen() : this(CocoStudioEngineAdapterPINVOKE.new_CSControlNodeDrawPen__SWIG_0(), true)
		{
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0000D479 File Offset: 0x0000B679
		public CSControlNodeDrawPen(CSDrawNode drawnode) : this(CocoStudioEngineAdapterPINVOKE.new_CSControlNodeDrawPen__SWIG_1(CSDrawNode.getCPtr(drawnode)), true)
		{
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0000D490 File Offset: 0x0000B690
		public void SetDrawNodePen(CSDrawNode drawnode)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetDrawNodePen(this.swigCPtr, CSDrawNode.getCPtr(drawnode));
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0000D4A5 File Offset: 0x0000B6A5
		public void ClearDraw()
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_ClearDraw(this.swigCPtr);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
		public void SetCenterPoint(PointF axiscenter)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetCenterPoint(this.swigCPtr, Vec2.getCPtr(new Vec2(axiscenter.X, axiscenter.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0000D4F8 File Offset: 0x0000B6F8
		public void SetOperateState(CSControlNodeDrawPen.OperateState state)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetOperateState(this.swigCPtr, (int)state);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0000D508 File Offset: 0x0000B708
		public CSControlNodeDrawPen.OperateState GetOperateState()
		{
			return (CSControlNodeDrawPen.OperateState)CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetOperateState(this.swigCPtr);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0000D527 File Offset: 0x0000B727
		public void SetCollideLineWidth(float width)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetCollideLineWidth(this.swigCPtr, width);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0000D538 File Offset: 0x0000B738
		public float GetCollideLine()
		{
			return CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetCollideLine(this.swigCPtr);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0000D557 File Offset: 0x0000B757
		public void SetControlXLength(float length)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetControlXLength(this.swigCPtr, length);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0000D568 File Offset: 0x0000B768
		public float GetControlXLength()
		{
			return CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetControlXLength(this.swigCPtr);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0000D587 File Offset: 0x0000B787
		public void SetControlYLength(float length)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetControlYLength(this.swigCPtr, length);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0000D598 File Offset: 0x0000B798
		public float GetControlYLength()
		{
			return CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetControlYLength(this.swigCPtr);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0000D5B7 File Offset: 0x0000B7B7
		public virtual void DrawControl()
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_DrawControl(this.swigCPtr);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		public virtual CSControlNodeDrawPen.OperateState PointAtControl(PointF point)
		{
			CSControlNodeDrawPen.OperateState result = (CSControlNodeDrawPen.OperateState)CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_PointAtControl(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0000D614 File Offset: 0x0000B814
		public CSRect GetSizeRectToWorldTransfrom(CSVisualObject obj)
		{
			return new CSRect(CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetSizeRectToWorldTransfrom(this.swigCPtr, CSVisualObject.getCPtr(obj)), true);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0000D63F File Offset: 0x0000B83F
		public void GetNodeRotateToPen(CSVisualObject obj)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetNodeRotateToPen(this.swigCPtr, CSVisualObject.getCPtr(obj));
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0000D654 File Offset: 0x0000B854
		public void ResetDrawPenScale()
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_ResetDrawPenScale(this.swigCPtr);
		}

		// Token: 0x04000075 RID: 117
		private HandleRef swigCPtr;

		// Token: 0x04000076 RID: 118
		protected bool swigCMemOwn;

		// Token: 0x02000045 RID: 69
		public enum OperateState
		{
			// Token: 0x04000078 RID: 120
			NONE,
			// Token: 0x04000079 RID: 121
			XLINE,
			// Token: 0x0400007A RID: 122
			YLINE,
			// Token: 0x0400007B RID: 123
			XYLINE
		}
	}
}

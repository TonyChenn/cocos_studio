using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSControlNodeDrawPen : IDisposable
	{
		public CSControlNodeDrawPen(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSControlNodeDrawPen obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSControlNodeDrawPen()
		{
			this.Dispose();
		}

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

		public CSControlNodeDrawPen() : this(CocoStudioEngineAdapterPINVOKE.new_CSControlNodeDrawPen__SWIG_0(), true)
		{
		}

		public CSControlNodeDrawPen(CSDrawNode drawnode) : this(CocoStudioEngineAdapterPINVOKE.new_CSControlNodeDrawPen__SWIG_1(CSDrawNode.getCPtr(drawnode)), true)
		{
		}

		public void SetDrawNodePen(CSDrawNode drawnode)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetDrawNodePen(this.swigCPtr, CSDrawNode.getCPtr(drawnode));
		}

		public void ClearDraw()
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_ClearDraw(this.swigCPtr);
		}

		public void SetCenterPoint(PointF axiscenter)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetCenterPoint(this.swigCPtr, Vec2.getCPtr(new Vec2(axiscenter.X, axiscenter.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void SetOperateState(CSControlNodeDrawPen.OperateState state)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetOperateState(this.swigCPtr, (int)state);
		}

		public CSControlNodeDrawPen.OperateState GetOperateState()
		{
			return (CSControlNodeDrawPen.OperateState)CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetOperateState(this.swigCPtr);
		}

		public void SetCollideLineWidth(float width)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetCollideLineWidth(this.swigCPtr, width);
		}

		public float GetCollideLine()
		{
			return CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetCollideLine(this.swigCPtr);
		}

		public void SetControlXLength(float length)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetControlXLength(this.swigCPtr, length);
		}

		public float GetControlXLength()
		{
			return CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetControlXLength(this.swigCPtr);
		}

		public void SetControlYLength(float length)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_SetControlYLength(this.swigCPtr, length);
		}

		public float GetControlYLength()
		{
			return CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetControlYLength(this.swigCPtr);
		}

		public virtual void DrawControl()
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_DrawControl(this.swigCPtr);
		}

		public virtual CSControlNodeDrawPen.OperateState PointAtControl(PointF point)
		{
			CSControlNodeDrawPen.OperateState result = (CSControlNodeDrawPen.OperateState)CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_PointAtControl(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public CSRect GetSizeRectToWorldTransfrom(CSVisualObject obj)
		{
			return new CSRect(CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetSizeRectToWorldTransfrom(this.swigCPtr, CSVisualObject.getCPtr(obj)), true);
		}

		public void GetNodeRotateToPen(CSVisualObject obj)
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_GetNodeRotateToPen(this.swigCPtr, CSVisualObject.getCPtr(obj));
		}

		public void ResetDrawPenScale()
		{
			CocoStudioEngineAdapterPINVOKE.CSControlNodeDrawPen_ResetDrawPenScale(this.swigCPtr);
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		public enum OperateState
		{
			NONE,
			XLINE,
			YLINE,
			XYLINE
		}
	}
}

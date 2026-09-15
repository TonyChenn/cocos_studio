using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSBoneRackDrawPen : CSControlNodeDrawPen
	{
		public CSBoneRackDrawPen(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSBoneRackDrawPen_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSBoneRackDrawPen obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSBoneRackDrawPen()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSBoneRackDrawPen(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSBoneRackDrawPen(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public void DrawBoneRack(PointF start, PointF end, float headWidth)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneRackDrawPen_DrawBoneRack(this.swigCPtr, Vec2.getCPtr(new Vec2(start.X, start.Y)), Vec2.getCPtr(new Vec2(end.X, end.Y)), headWidth);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void DrawArrowLine(PointF start, PointF end)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneRackDrawPen_DrawArrowLine__SWIG_0(this.swigCPtr, Vec2.getCPtr(new Vec2(start.X, start.Y)), Vec2.getCPtr(new Vec2(end.X, end.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void DrawArrowLine(PointF start, PointF end, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneRackDrawPen_DrawArrowLine__SWIG_1(this.swigCPtr, Vec2.getCPtr(new Vec2(start.X, start.Y)), Vec2.getCPtr(new Vec2(end.X, end.Y)), Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSBoneRackDrawPen() : this(CocoStudioEngineAdapterPINVOKE.new_CSBoneRackDrawPen(), true)
		{
		}

		private HandleRef swigCPtr;
	}
}

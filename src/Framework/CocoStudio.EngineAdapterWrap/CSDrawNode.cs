using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSDrawNode : CSVisualObject
	{
		public CSDrawNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSDrawNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSDrawNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSDrawNode()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSDrawNode(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSDrawNode(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSDrawNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSDrawNode(), true)
		{
		}

		public void DrawDot(PointF pos, float radius, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawDot(this.swigCPtr, Vec2.getCPtr(new Vec2(pos.X, pos.Y)), radius, Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void DrawSegment(PointF from, PointF to, float radius, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawSegment(this.swigCPtr, Vec2.getCPtr(new Vec2(from.X, from.Y)), Vec2.getCPtr(new Vec2(to.X, to.Y)), radius, Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void DrawTriangle(PointF p1, PointF p2, PointF p3, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawTriangle(this.swigCPtr, Vec2.getCPtr(new Vec2(p1.X, p1.Y)), Vec2.getCPtr(new Vec2(p2.X, p2.Y)), Vec2.getCPtr(new Vec2(p3.X, p3.Y)), Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void DrawLine(PointF origin, PointF destination, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawLine(this.swigCPtr, Vec2.getCPtr(new Vec2(origin.X, origin.Y)), Vec2.getCPtr(new Vec2(destination.X, destination.Y)), Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void DrawRectangle(PointF leftop, PointF rightbottom, float radius, Color4F color, bool isfill)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawRectangle(this.swigCPtr, Vec2.getCPtr(new Vec2(leftop.X, leftop.Y)), Vec2.getCPtr(new Vec2(rightbottom.X, rightbottom.Y)), radius, Color4F.getCPtr(color), isfill);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void DrawQuadraticBezier(PointF from, PointF control, PointF to, int segments, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawQuadraticBezier(this.swigCPtr, Vec2.getCPtr(new Vec2(from.X, from.Y)), Vec2.getCPtr(new Vec2(control.X, control.Y)), Vec2.getCPtr(new Vec2(to.X, to.Y)), segments, Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_Clear(this.swigCPtr);
		}

		public virtual void SetBlendFunc(BlendFuncValue blendFunc)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_SetBlendFunc(this.swigCPtr, CSBlendFunc.getCPtr(new CSBlendFunc((uint)blendFunc.BlendSrc, (uint)blendFunc.BlendDst)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual BlendFuncValue GetBlendFunc()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSDrawNode_GetBlendFunc(this.swigCPtr);
			CSBlendFunc csblendFunc = new CSBlendFunc(cPtr, true);
			return new BlendFuncValue((BlendSrc)csblendFunc.Src, (BlendDst)csblendFunc.Dst);
		}

		private HandleRef swigCPtr;
	}
}

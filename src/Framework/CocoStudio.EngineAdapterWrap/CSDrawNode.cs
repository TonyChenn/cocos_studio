using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200000C RID: 12
	public class CSDrawNode : CSVisualObject
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00004012 File Offset: 0x00002212
		public CSDrawNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSDrawNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004034 File Offset: 0x00002234
		public static HandleRef getCPtr(CSDrawNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004060 File Offset: 0x00002260
		~CSDrawNode()
		{
			this.Dispose();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000040C4 File Offset: 0x000022C4
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

		// Token: 0x060000A4 RID: 164 RVA: 0x000041C4 File Offset: 0x000023C4
		public CSDrawNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSDrawNode(), true)
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000041D8 File Offset: 0x000023D8
		public void DrawDot(PointF pos, float radius, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawDot(this.swigCPtr, Vec2.getCPtr(new Vec2(pos.X, pos.Y)), radius, Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004224 File Offset: 0x00002424
		public void DrawSegment(PointF from, PointF to, float radius, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawSegment(this.swigCPtr, Vec2.getCPtr(new Vec2(from.X, from.Y)), Vec2.getCPtr(new Vec2(to.X, to.Y)), radius, Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004288 File Offset: 0x00002488
		public void DrawTriangle(PointF p1, PointF p2, PointF p3, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawTriangle(this.swigCPtr, Vec2.getCPtr(new Vec2(p1.X, p1.Y)), Vec2.getCPtr(new Vec2(p2.X, p2.Y)), Vec2.getCPtr(new Vec2(p3.X, p3.Y)), Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004304 File Offset: 0x00002504
		public void DrawLine(PointF origin, PointF destination, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawLine(this.swigCPtr, Vec2.getCPtr(new Vec2(origin.X, origin.Y)), Vec2.getCPtr(new Vec2(destination.X, destination.Y)), Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004368 File Offset: 0x00002568
		public void DrawRectangle(PointF leftop, PointF rightbottom, float radius, Color4F color, bool isfill)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawRectangle(this.swigCPtr, Vec2.getCPtr(new Vec2(leftop.X, leftop.Y)), Vec2.getCPtr(new Vec2(rightbottom.X, rightbottom.Y)), radius, Color4F.getCPtr(color), isfill);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000043D0 File Offset: 0x000025D0
		public void DrawQuadraticBezier(PointF from, PointF control, PointF to, int segments, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_DrawQuadraticBezier(this.swigCPtr, Vec2.getCPtr(new Vec2(from.X, from.Y)), Vec2.getCPtr(new Vec2(control.X, control.Y)), Vec2.getCPtr(new Vec2(to.X, to.Y)), segments, Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000444D File Offset: 0x0000264D
		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_Clear(this.swigCPtr);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000445C File Offset: 0x0000265C
		public virtual void SetBlendFunc(BlendFuncValue blendFunc)
		{
			CocoStudioEngineAdapterPINVOKE.CSDrawNode_SetBlendFunc(this.swigCPtr, CSBlendFunc.getCPtr(new CSBlendFunc((uint)blendFunc.BlendSrc, (uint)blendFunc.BlendDst)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000044A0 File Offset: 0x000026A0
		public virtual BlendFuncValue GetBlendFunc()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSDrawNode_GetBlendFunc(this.swigCPtr);
			CSBlendFunc csblendFunc = new CSBlendFunc(cPtr, true);
			return new BlendFuncValue((BlendSrc)csblendFunc.Src, (BlendDst)csblendFunc.Dst);
		}

		// Token: 0x04000012 RID: 18
		private HandleRef swigCPtr;
	}
}

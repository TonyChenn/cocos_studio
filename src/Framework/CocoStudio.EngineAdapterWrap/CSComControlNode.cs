using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000034 RID: 52
	public class CSComControlNode : CSVisualObject
	{
		// Token: 0x060008BA RID: 2234 RVA: 0x0000A338 File Offset: 0x00008538
		public CSComControlNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSComControlNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0000A358 File Offset: 0x00008558
		public static HandleRef getCPtr(CSComControlNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0000A384 File Offset: 0x00008584
		~CSComControlNode()
		{
			this.Dispose();
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0000A3E8 File Offset: 0x000085E8
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
								CocoStudioEngineAdapterPINVOKE.delete_CSComControlNode(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSComControlNode(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0000A4E8 File Offset: 0x000086E8
		public CSComControlNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSComControlNode(), true)
		{
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0000A4FC File Offset: 0x000086FC
		public override int HitTest(PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSComControlNode_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0000A545 File Offset: 0x00008745
		public virtual void SetCanvasObject(CSVisualObject canvas)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetCanvasObject(this.swigCPtr, CSVisualObject.getCPtr(canvas));
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0000A55C File Offset: 0x0000875C
		public virtual RectF RectApplyTransform(RectF rect, CSMatrix mat)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSComControlNode_RectApplyTransform(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)), CSMatrix.getCPtr(mat));
			Rect rect2 = new Rect(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			if (rect2.size.width < 0f || rect2.size.height < 0f)
			{
				rect2.origin.x = 0f;
				rect2.origin.y = 0f;
				rect2.size.width = 0f;
				rect2.size.height = 0f;
			}
			return new RectF(rect2.origin.x, rect2.origin.y, rect2.size.width, rect2.size.height);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0000A668 File Offset: 0x00008868
		public virtual void SetMat(CSMatrix mat)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetMat(this.swigCPtr, CSMatrix.getCPtr(mat));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0000A69C File Offset: 0x0000889C
		public virtual CSMatrix GetMat()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetMat(this.swigCPtr), true);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0000A6C1 File Offset: 0x000088C1
		public virtual void SetEnable(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetEnable(this.swigCPtr, enable);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0000A6D4 File Offset: 0x000088D4
		public virtual bool IsEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSComControlNode_IsEnable(this.swigCPtr);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0000A6F4 File Offset: 0x000088F4
		public virtual int GetControlPointType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetControlPointType(this.swigCPtr);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0000A714 File Offset: 0x00008914
		public virtual CSMatrix GetMatrixWithoutReCalculate()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetMatrixWithoutReCalculate(this.swigCPtr), true);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0000A73C File Offset: 0x0000893C
		public virtual CSMatrix Mat4Multiply(CSMatrix pM1, CSMatrix pM2)
		{
			CSMatrix result = new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_Mat4Multiply(this.swigCPtr, CSMatrix.getCPtr(pM1), CSMatrix.getCPtr(pM2)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0000A780 File Offset: 0x00008980
		public virtual CSMatrix Mat4Inverse(CSMatrix pM)
		{
			CSMatrix result = new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_Mat4Inverse(this.swigCPtr, CSMatrix.getCPtr(pM)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0000A7C0 File Offset: 0x000089C0
		public virtual CSMatrix Mat4Identity(CSMatrix pM)
		{
			CSMatrix result = new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_Mat4Identity(this.swigCPtr, CSMatrix.getCPtr(pM)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0000A800 File Offset: 0x00008A00
		public virtual MatrixNode Mat4ToMatrixNode(CSMatrix mat)
		{
			MatrixNode result = new MatrixNode(CocoStudioEngineAdapterPINVOKE.CSComControlNode_Mat4ToMatrixNode(this.swigCPtr, CSMatrix.getCPtr(mat)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0000A840 File Offset: 0x00008A40
		public virtual PointF TransformPoint(PointF p, CSMatrix mat)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSComControlNode_TransformPoint(this.swigCPtr, Vec2.getCPtr(new Vec2(p.X, p.Y)), CSMatrix.getCPtr(mat));
			Vec2 vec = new Vec2(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new PointF(vec.x, vec.y);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0000A8AB File Offset: 0x00008AAB
		public virtual void SetAnchorPointVisible(bool isVisiable)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetAnchorPointVisible(this.swigCPtr, isVisiable);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0000A8BB File Offset: 0x00008ABB
		public virtual void SetControlPointVisible(bool isVisiable)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetControlPointVisible(this.swigCPtr, isVisiable);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0000A8CB File Offset: 0x00008ACB
		public virtual void SetAttachNode(CSVisualObject node)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetAttachNode(this.swigCPtr, CSVisualObject.getCPtr(node));
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0000A8E0 File Offset: 0x00008AE0
		public override PointF GetAnchorPointInPoints()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetAnchorPointInPoints(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new PointF(vec.x, vec.y);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0000A91C File Offset: 0x00008B1C
		public virtual RectF GetBoundingBox()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetBoundingBox(this.swigCPtr);
			Rect rect = new Rect(cPtr, true);
			if (rect.size.width < 0f || rect.size.height < 0f)
			{
				rect.origin.x = 0f;
				rect.origin.y = 0f;
				rect.size.width = 0f;
				rect.size.height = 0f;
			}
			return new RectF(rect.origin.x, rect.origin.y, rect.size.width, rect.size.height);
		}

		// Token: 0x04000057 RID: 87
		private HandleRef swigCPtr;
	}
}

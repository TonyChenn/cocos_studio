using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSComControlNode : CSVisualObject
	{
		public CSComControlNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSComControlNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSComControlNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSComControlNode()
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

		public CSComControlNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSComControlNode(), true)
		{
		}

		public override int HitTest(PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSComControlNode_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public virtual void SetCanvasObject(CSVisualObject canvas)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetCanvasObject(this.swigCPtr, CSVisualObject.getCPtr(canvas));
		}

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

		public virtual void SetMat(CSMatrix mat)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetMat(this.swigCPtr, CSMatrix.getCPtr(mat));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual CSMatrix GetMat()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetMat(this.swigCPtr), true);
		}

		public virtual void SetEnable(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetEnable(this.swigCPtr, enable);
		}

		public virtual bool IsEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSComControlNode_IsEnable(this.swigCPtr);
		}

		public virtual int GetControlPointType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetControlPointType(this.swigCPtr);
		}

		public virtual CSMatrix GetMatrixWithoutReCalculate()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetMatrixWithoutReCalculate(this.swigCPtr), true);
		}

		public virtual CSMatrix Mat4Multiply(CSMatrix pM1, CSMatrix pM2)
		{
			CSMatrix result = new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_Mat4Multiply(this.swigCPtr, CSMatrix.getCPtr(pM1), CSMatrix.getCPtr(pM2)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public virtual CSMatrix Mat4Inverse(CSMatrix pM)
		{
			CSMatrix result = new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_Mat4Inverse(this.swigCPtr, CSMatrix.getCPtr(pM)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public virtual CSMatrix Mat4Identity(CSMatrix pM)
		{
			CSMatrix result = new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSComControlNode_Mat4Identity(this.swigCPtr, CSMatrix.getCPtr(pM)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public virtual MatrixNode Mat4ToMatrixNode(CSMatrix mat)
		{
			MatrixNode result = new MatrixNode(CocoStudioEngineAdapterPINVOKE.CSComControlNode_Mat4ToMatrixNode(this.swigCPtr, CSMatrix.getCPtr(mat)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

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

		public virtual void SetAnchorPointVisible(bool isVisiable)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetAnchorPointVisible(this.swigCPtr, isVisiable);
		}

		public virtual void SetControlPointVisible(bool isVisiable)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetControlPointVisible(this.swigCPtr, isVisiable);
		}

		public virtual void SetAttachNode(CSVisualObject node)
		{
			CocoStudioEngineAdapterPINVOKE.CSComControlNode_SetAttachNode(this.swigCPtr, CSVisualObject.getCPtr(node));
		}

		public override PointF GetAnchorPointInPoints()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSComControlNode_GetAnchorPointInPoints(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new PointF(vec.x, vec.y);
		}

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

		private HandleRef swigCPtr;
	}
}

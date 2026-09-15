using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSNode3D : CSNode
	{
		public CSNode3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSNode3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSNode3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSNode3D()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSNode3D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSNode3D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSNode3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSNode3D(), true)
		{
		}

		public virtual bool IsFlipped()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode3D_IsFlipped(this.swigCPtr);
		}

		public virtual void RefreshObjectFace()
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_RefreshObjectFace(this.swigCPtr);
		}

		public virtual void SetPixelRenderMode(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_SetPixelRenderMode(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_RestoreRenderMode(this.swigCPtr);
		}

		public virtual void SetNodeCameraMask(uint mask, bool applyChildren)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_SetNodeCameraMask(this.swigCPtr, mask, applyChildren);
		}

		public virtual uint GetNodeCameraMask()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode3D_GetNodeCameraMask(this.swigCPtr);
		}

		public virtual void Roll(float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Roll__SWIG_0(this.swigCPtr, degree, (int)space);
		}

		public virtual void Roll(float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Roll__SWIG_1(this.swigCPtr, degree);
		}

		public virtual void Pitch(float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Pitch__SWIG_0(this.swigCPtr, degree, (int)space);
		}

		public virtual void Pitch(float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Pitch__SWIG_1(this.swigCPtr, degree);
		}

		public virtual void Yaw(float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Yaw__SWIG_0(this.swigCPtr, degree, (int)space);
		}

		public virtual void Yaw(float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Yaw__SWIG_1(this.swigCPtr, degree);
		}

		public virtual void Rotate3D(Point3F axis, float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_0(this.swigCPtr, Vec3.getCPtr(new Vec3(axis.X, axis.Y, axis.Z)), degree, (int)space);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void Rotate3D(Point3F axis, float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_1(this.swigCPtr, Vec3.getCPtr(new Vec3(axis.X, axis.Y, axis.Z)), degree);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void Rotate3D(Quaternion q, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_2(this.swigCPtr, Quaternion.getCPtr(q), (int)space);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void Rotate3D(Quaternion q)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_3(this.swigCPtr, Quaternion.getCPtr(q));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void Rotate3D(Quaternion q, CSNode3D target)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_4(this.swigCPtr, Quaternion.getCPtr(q), CSNode3D.getCPtr(target));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void SetObjectState(CSVisualObject.ObjectState boxState)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_SetObjectState(this.swigCPtr, (int)boxState);
		}

		public override float HitTest3D(CocoStudio.Model.PointF screenPoint)
		{
			float result = CocoStudioEngineAdapterPINVOKE.CSNode3D_HitTest3D(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public override bool RectTest3D(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSNode3D_RectTest3D(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private HandleRef swigCPtr;
	}
}

using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSCamera : CSVisualObject
	{
		public CSCamera(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSCamera_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSCamera obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSCamera()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSCamera(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSCamera(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSCamera() : this(CocoStudioEngineAdapterPINVOKE.new_CSCamera(), true)
		{
		}

		public virtual void SetCameraFlag(ushort flag)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetCameraFlag(this.swigCPtr, flag);
		}

		public virtual void LookAt(Point3F target, Point3F up)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_LookAt(this.swigCPtr, Vec3.getCPtr(new Vec3(target.X, target.Y, target.Z)), Vec3.getCPtr(new Vec3(up.X, up.Y, up.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual SizeF GetScreenSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetScreenSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		public virtual void SetScreenSize(SizeF size)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetScreenSize(this.swigCPtr, Size.getCPtr(new Size(size.Width, size.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public float GetNearPlane()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCamera_GetNearPlane(this.swigCPtr);
		}

		public void SetNearPlane(float nearPlane)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetNearPlane(this.swigCPtr, nearPlane);
		}

		public float GetFarPlane()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCamera_GetFarPlane(this.swigCPtr);
		}

		public void SetFarPlane(float farPlane)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetFarPlane(this.swigCPtr, farPlane);
		}

		public float GetFov()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCamera_GetFov(this.swigCPtr);
		}

		public void SetFov(float fov)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetFov(this.swigCPtr, fov);
		}

		public Point3F GetUp()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetUp(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public Point3F GetRight()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetRight(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public Point3F GetLookAt()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetLookAt(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public virtual void SetProjection(CSCamera.Projection p, bool force)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetProjection__SWIG_0(this.swigCPtr, (int)p, force);
		}

		public virtual void SetProjection(CSCamera.Projection p)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetProjection__SWIG_1(this.swigCPtr, (int)p);
		}

		public Point3F Unproject(PointF screenPoint)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_Unproject(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			Vec3 vec = new Vec3(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public Point3F ScreenToWorld(PointF screenPoint, float distance)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_ScreenToWorld__SWIG_0(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)), distance);
			Vec3 vec = new Vec3(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public Point3F ScreenToWorld(PointF screenPoint, Point3F projectionPlane)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_ScreenToWorld__SWIG_1(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)), Vec3.getCPtr(new Vec3(projectionPlane.X, projectionPlane.Y, projectionPlane.Z)));
			Vec3 vec = new Vec3(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public Point3F ScreenToWorld(PointF screenPoint, Point3F projectionPlane, Point3F normal)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_ScreenToWorld__SWIG_2(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)), Vec3.getCPtr(new Vec3(projectionPlane.X, projectionPlane.Y, projectionPlane.Z)), Vec3.getCPtr(new Vec3(normal.X, normal.Y, normal.Z)));
			Vec3 vec = new Vec3(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public void UpdateFrustumPlane(RectF rect)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_UpdateFrustumPlane(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void SetSkyBox(CSSkyBox brush)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetSkyBox(this.swigCPtr, CSSkyBox.getCPtr(brush));
		}

		public CSSkyBox GetSkyBox()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetSkyBox(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSSkyBox(intPtr, false);
		}

		private HandleRef swigCPtr;

		public enum Projection
		{
			Perspective,
			Orthographic
		}
	}
}

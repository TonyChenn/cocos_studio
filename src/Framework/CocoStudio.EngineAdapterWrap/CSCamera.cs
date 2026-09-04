using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000030 RID: 48
	public class CSCamera : CSVisualObject
	{
		// Token: 0x0600087C RID: 2172 RVA: 0x000093C2 File Offset: 0x000075C2
		public CSCamera(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSCamera_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000093E4 File Offset: 0x000075E4
		public static HandleRef getCPtr(CSCamera obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00009410 File Offset: 0x00007610
		~CSCamera()
		{
			this.Dispose();
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00009474 File Offset: 0x00007674
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

		// Token: 0x06000880 RID: 2176 RVA: 0x00009574 File Offset: 0x00007774
		public CSCamera() : this(CocoStudioEngineAdapterPINVOKE.new_CSCamera(), true)
		{
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00009585 File Offset: 0x00007785
		public virtual void SetCameraFlag(ushort flag)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetCameraFlag(this.swigCPtr, flag);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00009598 File Offset: 0x00007798
		public virtual void LookAt(Point3F target, Point3F up)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_LookAt(this.swigCPtr, Vec3.getCPtr(new Vec3(target.X, target.Y, target.Z)), Vec3.getCPtr(new Vec3(up.X, up.Y, up.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x000095FC File Offset: 0x000077FC
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

		// Token: 0x06000884 RID: 2180 RVA: 0x00009678 File Offset: 0x00007878
		public virtual void SetScreenSize(SizeF size)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetScreenSize(this.swigCPtr, Size.getCPtr(new Size(size.Width, size.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x000096BC File Offset: 0x000078BC
		public float GetNearPlane()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCamera_GetNearPlane(this.swigCPtr);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x000096DB File Offset: 0x000078DB
		public void SetNearPlane(float nearPlane)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetNearPlane(this.swigCPtr, nearPlane);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000096EC File Offset: 0x000078EC
		public float GetFarPlane()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCamera_GetFarPlane(this.swigCPtr);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000970B File Offset: 0x0000790B
		public void SetFarPlane(float farPlane)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetFarPlane(this.swigCPtr, farPlane);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0000971C File Offset: 0x0000791C
		public float GetFov()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCamera_GetFov(this.swigCPtr);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0000973B File Offset: 0x0000793B
		public void SetFov(float fov)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetFov(this.swigCPtr, fov);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0000974C File Offset: 0x0000794C
		public Point3F GetUp()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetUp(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0000978C File Offset: 0x0000798C
		public Point3F GetRight()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetRight(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x000097CC File Offset: 0x000079CC
		public Point3F GetLookAt()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetLookAt(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0000980B File Offset: 0x00007A0B
		public virtual void SetProjection(CSCamera.Projection p, bool force)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetProjection__SWIG_0(this.swigCPtr, (int)p, force);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0000981C File Offset: 0x00007A1C
		public virtual void SetProjection(CSCamera.Projection p)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetProjection__SWIG_1(this.swigCPtr, (int)p);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0000982C File Offset: 0x00007A2C
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

		// Token: 0x06000891 RID: 2193 RVA: 0x00009898 File Offset: 0x00007A98
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

		// Token: 0x06000892 RID: 2194 RVA: 0x00009904 File Offset: 0x00007B04
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

		// Token: 0x06000893 RID: 2195 RVA: 0x0000998C File Offset: 0x00007B8C
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

		// Token: 0x06000894 RID: 2196 RVA: 0x00009A30 File Offset: 0x00007C30
		public void UpdateFrustumPlane(RectF rect)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_UpdateFrustumPlane(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00009A7E File Offset: 0x00007C7E
		public void SetSkyBox(CSSkyBox brush)
		{
			CocoStudioEngineAdapterPINVOKE.CSCamera_SetSkyBox(this.swigCPtr, CSSkyBox.getCPtr(brush));
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00009A94 File Offset: 0x00007C94
		public CSSkyBox GetSkyBox()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSCamera_GetSkyBox(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSSkyBox(intPtr, false);
		}

		// Token: 0x04000051 RID: 81
		private HandleRef swigCPtr;

		// Token: 0x02000031 RID: 49
		public enum Projection
		{
			// Token: 0x04000053 RID: 83
			Perspective,
			// Token: 0x04000054 RID: 84
			Orthographic
		}
	}
}

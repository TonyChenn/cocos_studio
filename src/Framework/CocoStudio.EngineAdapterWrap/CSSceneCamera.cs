using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200004B RID: 75
	public class CSSceneCamera : CSCamera
	{
		// Token: 0x060009C3 RID: 2499 RVA: 0x0000E1C8 File Offset: 0x0000C3C8
		public CSSceneCamera(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
		public static HandleRef getCPtr(CSSceneCamera obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0000E238 File Offset: 0x0000C438
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
						if (!this.IsContainOpenGLResource())
						{
							throw new MethodAccessException("C++ destructor does not have public access");
						}
						GtkInvokeHelp.BeginInvoke(delegate
						{
							this.swigCPtr = handle;
							throw new MethodAccessException("C++ destructor does not have public access");
						});
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0000E338 File Offset: 0x0000C538
		public void SetMode(CSSceneCamera.CameraMode mode)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetMode(this.swigCPtr, (int)mode);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0000E348 File Offset: 0x0000C548
		public void MoveTo(CSVisualObject target)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_MoveTo(this.swigCPtr, CSVisualObject.getCPtr(target));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0000E37A File Offset: 0x0000C57A
		public void StartMove()
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_StartMove(this.swigCPtr);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0000E389 File Offset: 0x0000C589
		public void Paused()
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_Paused(this.swigCPtr);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0000E398 File Offset: 0x0000C598
		public void SetMoveSpeed(float speed, bool setCurrent)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetMoveSpeed__SWIG_0(this.swigCPtr, speed, setCurrent);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0000E3A9 File Offset: 0x0000C5A9
		public void SetMoveSpeed(float speed)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetMoveSpeed__SWIG_1(this.swigCPtr, speed);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0000E3BC File Offset: 0x0000C5BC
		public float GetMoveSpeed()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSceneCamera_GetMoveSpeed(this.swigCPtr);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0000E3DB File Offset: 0x0000C5DB
		public void SetMoveAccelerate(float acce)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetMoveAccelerate(this.swigCPtr, acce);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		public void MoveRelative(Point3F position, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_MoveRelative__SWIG_0(this.swigCPtr, Vec3.getCPtr(new Vec3(position.X, position.Y, position.Z)), (int)space);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0000E438 File Offset: 0x0000C638
		public void MoveRelative(Point3F position)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_MoveRelative__SWIG_1(this.swigCPtr, Vec3.getCPtr(new Vec3(position.X, position.Y, position.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0000E480 File Offset: 0x0000C680
		public void MoveDirect(float distance)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_MoveDirect(this.swigCPtr, distance);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0000E490 File Offset: 0x0000C690
		public void AddMoveFlag(ushort flag, bool g)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_AddMoveFlag(this.swigCPtr, flag, g);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0000E4A1 File Offset: 0x0000C6A1
		public void RemoveFlag(ushort flag)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_RemoveFlag(this.swigCPtr, flag);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
		public ushort GetMoveFlag()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSceneCamera_GetMoveFlag(this.swigCPtr);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0000E4D3 File Offset: 0x0000C6D3
		public void ClearFlag()
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_ClearFlag(this.swigCPtr);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0000E4E2 File Offset: 0x0000C6E2
		public void Update(float delta)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_Update(this.swigCPtr, delta);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0000E4F2 File Offset: 0x0000C6F2
		public void OnViewSizeChange(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_OnViewSizeChange(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0000E508 File Offset: 0x0000C708
		public void Rotate3D(Point3F axis, float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_Rotate3D__SWIG_0(this.swigCPtr, Vec3.getCPtr(new Vec3(axis.X, axis.Y, axis.Z)), degree, (int)space);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0000E554 File Offset: 0x0000C754
		public void Rotate3D(Point3F axis, float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_Rotate3D__SWIG_1(this.swigCPtr, Vec3.getCPtr(new Vec3(axis.X, axis.Y, axis.Z)), degree);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public override void SetPosition3D(Point3F pos)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetPosition3D(this.swigCPtr, Vec3.getCPtr(new Vec3(pos.X, pos.Y, pos.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
		public override void SetRotation3D(Point3F rot)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetRotation3D(this.swigCPtr, Vec3.getCPtr(new Vec3(rot.X, rot.Y, rot.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0000E630 File Offset: 0x0000C830
		public CSUserCameraPreview GetUserCameraPreview()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSSceneCamera_GetUserCameraPreview(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSUserCameraPreview(intPtr, false);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0000E668 File Offset: 0x0000C868
		public Color GetPickColor(CocoStudio.Model.PointF screenPoint)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSceneCamera_GetPickColor(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			Color3B color3B = new Color3B(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0000E6D3 File Offset: 0x0000C8D3
		public void OnMove(float detailX, float detailY)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_OnMove(this.swigCPtr, detailX, detailY);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0000E6E4 File Offset: 0x0000C8E4
		public void OnChangeViewpoint(CocoStudio.Model.PointF screenPoint)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_OnChangeViewpoint(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0000E728 File Offset: 0x0000C928
		public void RefreshLookAt()
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_RefreshLookAt(this.swigCPtr);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0000E737 File Offset: 0x0000C937
		public void SetRectDrawNode(CSDrawNode drawNode)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetRectDrawNode(this.swigCPtr, CSDrawNode.getCPtr(drawNode));
		}

		// Token: 0x04000082 RID: 130
		private HandleRef swigCPtr;

		// Token: 0x0200004C RID: 76
		public enum MoveFlag
		{
			// Token: 0x04000084 RID: 132
			None,
			// Token: 0x04000085 RID: 133
			Left,
			// Token: 0x04000086 RID: 134
			Right,
			// Token: 0x04000087 RID: 135
			Foward = 4,
			// Token: 0x04000088 RID: 136
			Back = 8,
			// Token: 0x04000089 RID: 137
			Up = 16,
			// Token: 0x0400008A RID: 138
			Down = 32
		}

		// Token: 0x0200004D RID: 77
		public enum CameraMode
		{
			// Token: 0x0400008C RID: 140
			Mode2D,
			// Token: 0x0400008D RID: 141
			Mode3D
		}
	}
}

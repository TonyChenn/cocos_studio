using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSSceneCamera : CSCamera
	{
		public CSSceneCamera(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSSceneCamera obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
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

		public void SetMode(CSSceneCamera.CameraMode mode)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetMode(this.swigCPtr, (int)mode);
		}

		public void MoveTo(CSVisualObject target)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_MoveTo(this.swigCPtr, CSVisualObject.getCPtr(target));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void StartMove()
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_StartMove(this.swigCPtr);
		}

		public void Paused()
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_Paused(this.swigCPtr);
		}

		public void SetMoveSpeed(float speed, bool setCurrent)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetMoveSpeed__SWIG_0(this.swigCPtr, speed, setCurrent);
		}

		public void SetMoveSpeed(float speed)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetMoveSpeed__SWIG_1(this.swigCPtr, speed);
		}

		public float GetMoveSpeed()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSceneCamera_GetMoveSpeed(this.swigCPtr);
		}

		public void SetMoveAccelerate(float acce)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetMoveAccelerate(this.swigCPtr, acce);
		}

		public void MoveRelative(Point3F position, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_MoveRelative__SWIG_0(this.swigCPtr, Vec3.getCPtr(new Vec3(position.X, position.Y, position.Z)), (int)space);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void MoveRelative(Point3F position)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_MoveRelative__SWIG_1(this.swigCPtr, Vec3.getCPtr(new Vec3(position.X, position.Y, position.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void MoveDirect(float distance)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_MoveDirect(this.swigCPtr, distance);
		}

		public void AddMoveFlag(ushort flag, bool g)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_AddMoveFlag(this.swigCPtr, flag, g);
		}

		public void RemoveFlag(ushort flag)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_RemoveFlag(this.swigCPtr, flag);
		}

		public ushort GetMoveFlag()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSceneCamera_GetMoveFlag(this.swigCPtr);
		}

		public void ClearFlag()
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_ClearFlag(this.swigCPtr);
		}

		public void Update(float delta)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_Update(this.swigCPtr, delta);
		}

		public void OnViewSizeChange(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_OnViewSizeChange(this.swigCPtr, x, y, width, height);
		}

		public void Rotate3D(Point3F axis, float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_Rotate3D__SWIG_0(this.swigCPtr, Vec3.getCPtr(new Vec3(axis.X, axis.Y, axis.Z)), degree, (int)space);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Rotate3D(Point3F axis, float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_Rotate3D__SWIG_1(this.swigCPtr, Vec3.getCPtr(new Vec3(axis.X, axis.Y, axis.Z)), degree);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void SetPosition3D(Point3F pos)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetPosition3D(this.swigCPtr, Vec3.getCPtr(new Vec3(pos.X, pos.Y, pos.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void SetRotation3D(Point3F rot)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetRotation3D(this.swigCPtr, Vec3.getCPtr(new Vec3(rot.X, rot.Y, rot.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSUserCameraPreview GetUserCameraPreview()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSSceneCamera_GetUserCameraPreview(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSUserCameraPreview(intPtr, false);
		}

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

		public void OnMove(float detailX, float detailY)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_OnMove(this.swigCPtr, detailX, detailY);
		}

		public void OnChangeViewpoint(CocoStudio.Model.PointF screenPoint)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_OnChangeViewpoint(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void RefreshLookAt()
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_RefreshLookAt(this.swigCPtr);
		}

		public void SetRectDrawNode(CSDrawNode drawNode)
		{
			CocoStudioEngineAdapterPINVOKE.CSSceneCamera_SetRectDrawNode(this.swigCPtr, CSDrawNode.getCPtr(drawNode));
		}

		private HandleRef swigCPtr;

		public enum MoveFlag
		{
			None,
			Left,
			Right,
			Foward = 4,
			Back = 8,
			Up = 16,
			Down = 32
		}

		public enum CameraMode
		{
			Mode2D,
			Mode3D
		}
	}
}

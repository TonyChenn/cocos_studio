using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSLight : CSNode3D
	{
		public CSLight(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSLight_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSLight obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSLight()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSLight(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSLight(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSLight() : this(CocoStudioEngineAdapterPINVOKE.new_CSLight(), true)
		{
		}

		public void SetLightType(int type)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetLightType(this.swigCPtr, type);
		}

		public int GetLightType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetLightType(this.swigCPtr);
		}

		public void SetLightFlag(int flag)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetLightFlag(this.swigCPtr, flag);
		}

		public int GetLightFlag()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetLightFlag(this.swigCPtr);
		}

		public void SetIntensity(float intensity)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetIntensity(this.swigCPtr, intensity);
		}

		public float GetIntensity()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetIntensity(this.swigCPtr);
		}

		public void SetEnabled(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetEnabled(this.swigCPtr, enable);
		}

		public bool IsEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_IsEnabled(this.swigCPtr);
		}

		public void SetRange(float range)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetRange(this.swigCPtr, range);
		}

		public float GetRange()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetRange(this.swigCPtr);
		}

		public void SetInnerAngle(float angle)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetInnerAngle(this.swigCPtr, angle);
		}

		public float GetInnerAngle()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetInnerAngle(this.swigCPtr);
		}

		public void SetOuterAngle(float angle)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetOuterAngle(this.swigCPtr, angle);
		}

		public float GetOuterAngle()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetOuterAngle(this.swigCPtr);
		}

		public bool OnMouseMove(CocoStudio.Model.PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSLight_OnMouseMove(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool OnMouseDown(CocoStudio.Model.PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSLight_OnMouseDown(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool OnMouseUp(CocoStudio.Model.PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSLight_OnMouseUp(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool HitControlPoint(CocoStudio.Model.PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSLight_HitControlPoint(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void RefreshLightState(bool enabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_RefreshLightState(this.swigCPtr, enabled);
		}

		public void RefreshLightIndex(int index)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_RefreshLightIndex(this.swigCPtr, index);
		}

		public override Color GetDisplayColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSLight_GetDisplayColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public override void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_RestoreRenderMode(this.swigCPtr);
		}

		public override void SetPixelRenderMode(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetPixelRenderMode(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}

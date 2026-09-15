using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSUserCamera : CSNode3D
	{
		public CSUserCamera(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSUserCamera_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSUserCamera obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSUserCamera()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSUserCamera(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSUserCamera(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSUserCamera() : this(CocoStudioEngineAdapterPINVOKE.new_CSUserCamera(), true)
		{
		}

		public uint GetCameraMask()
		{
			return CocoStudioEngineAdapterPINVOKE.CSUserCamera_GetCameraMask(this.swigCPtr);
		}

		public void SetCameraMask(uint mask)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetCameraMask(this.swigCPtr, mask);
		}

		public override void SetObjectState(CSVisualObject.ObjectState boxState)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetObjectState(this.swigCPtr, (int)boxState);
		}

		public void SetProjection(CSCamera.Projection p, bool force)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetProjection__SWIG_0(this.swigCPtr, (int)p, force);
		}

		public void SetProjection(CSCamera.Projection p)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetProjection__SWIG_1(this.swigCPtr, (int)p);
		}

		public void ResetFrustum()
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_ResetFrustum(this.swigCPtr);
		}

		public CSCamera GetCSCamera()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSUserCamera_GetCSCamera(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSCamera(intPtr, false);
		}

		public override void SetPixelRenderMode(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetPixelRenderMode(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_RestoreRenderMode(this.swigCPtr);
		}

		public override Color GetDisplayColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSUserCamera_GetDisplayColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public CSSkyBox GetCSSkyBox()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSUserCamera_GetCSSkyBox(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSSkyBox(intPtr, false);
		}

		private HandleRef swigCPtr;

		public class CameraListener : IDisposable
		{
			public CameraListener(IntPtr cPtr, bool cMemoryOwn)
			{
				this.swigCMemOwn = cMemoryOwn;
				this.swigCPtr = new HandleRef(this, cPtr);
			}

			public static HandleRef getCPtr(CSUserCamera.CameraListener obj)
			{
				return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
			}

			~CameraListener()
			{
				this.Dispose();
			}

			public virtual void Dispose()
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
									CocoStudioEngineAdapterPINVOKE.delete_CSUserCamera_CameraListener(this.swigCPtr);
								});
							}
							else
							{
								CocoStudioEngineAdapterPINVOKE.delete_CSUserCamera_CameraListener(this.swigCPtr);
							}
						}
						this.swigCPtr = new HandleRef(null, IntPtr.Zero);
					}
					GC.SuppressFinalize(this);
				}
			}

			public virtual void OnSizeChange()
			{
				CocoStudioEngineAdapterPINVOKE.CSUserCamera_CameraListener_OnSizeChange(this.swigCPtr);
			}

			private HandleRef swigCPtr;

			protected bool swigCMemOwn;
		}
	}
}

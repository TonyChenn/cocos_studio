using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200005C RID: 92
	public class CSUserCamera : CSNode3D
	{
		// Token: 0x06000ACC RID: 2764 RVA: 0x0001178D File Offset: 0x0000F98D
		public CSUserCamera(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSUserCamera_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x000117AC File Offset: 0x0000F9AC
		public static HandleRef getCPtr(CSUserCamera obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x000117D8 File Offset: 0x0000F9D8
		~CSUserCamera()
		{
			this.Dispose();
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0001183C File Offset: 0x0000FA3C
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

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0001193C File Offset: 0x0000FB3C
		public CSUserCamera() : this(CocoStudioEngineAdapterPINVOKE.new_CSUserCamera(), true)
		{
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00011950 File Offset: 0x0000FB50
		public uint GetCameraMask()
		{
			return CocoStudioEngineAdapterPINVOKE.CSUserCamera_GetCameraMask(this.swigCPtr);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0001196F File Offset: 0x0000FB6F
		public void SetCameraMask(uint mask)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetCameraMask(this.swigCPtr, mask);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0001197F File Offset: 0x0000FB7F
		public override void SetObjectState(CSVisualObject.ObjectState boxState)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetObjectState(this.swigCPtr, (int)boxState);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0001198F File Offset: 0x0000FB8F
		public void SetProjection(CSCamera.Projection p, bool force)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetProjection__SWIG_0(this.swigCPtr, (int)p, force);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000119A0 File Offset: 0x0000FBA0
		public void SetProjection(CSCamera.Projection p)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetProjection__SWIG_1(this.swigCPtr, (int)p);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000119B0 File Offset: 0x0000FBB0
		public void ResetFrustum()
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_ResetFrustum(this.swigCPtr);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x000119C0 File Offset: 0x0000FBC0
		public CSCamera GetCSCamera()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSUserCamera_GetCSCamera(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSCamera(intPtr, false);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000119F8 File Offset: 0x0000FBF8
		public override void SetPixelRenderMode(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_SetPixelRenderMode(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00011A43 File Offset: 0x0000FC43
		public override void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCamera_RestoreRenderMode(this.swigCPtr);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00011A54 File Offset: 0x0000FC54
		public override Color GetDisplayColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSUserCamera_GetDisplayColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00011A94 File Offset: 0x0000FC94
		public CSSkyBox GetCSSkyBox()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSUserCamera_GetCSSkyBox(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSSkyBox(intPtr, false);
		}

		// Token: 0x040000A0 RID: 160
		private HandleRef swigCPtr;

		// Token: 0x0200005D RID: 93
		public class CameraListener : IDisposable
		{
			// Token: 0x06000ADC RID: 2780 RVA: 0x00011ACC File Offset: 0x0000FCCC
			public CameraListener(IntPtr cPtr, bool cMemoryOwn)
			{
				this.swigCMemOwn = cMemoryOwn;
				this.swigCPtr = new HandleRef(this, cPtr);
			}

			// Token: 0x06000ADD RID: 2781 RVA: 0x00011AEC File Offset: 0x0000FCEC
			public static HandleRef getCPtr(CSUserCamera.CameraListener obj)
			{
				return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
			}

			// Token: 0x06000ADE RID: 2782 RVA: 0x00011B18 File Offset: 0x0000FD18
			~CameraListener()
			{
				this.Dispose();
			}

			// Token: 0x06000ADF RID: 2783 RVA: 0x00011B7C File Offset: 0x0000FD7C
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

			// Token: 0x06000AE0 RID: 2784 RVA: 0x00011C74 File Offset: 0x0000FE74
			public virtual void OnSizeChange()
			{
				CocoStudioEngineAdapterPINVOKE.CSUserCamera_CameraListener_OnSizeChange(this.swigCPtr);
			}

			// Token: 0x040000A1 RID: 161
			private HandleRef swigCPtr;

			// Token: 0x040000A2 RID: 162
			protected bool swigCMemOwn;
		}
	}
}

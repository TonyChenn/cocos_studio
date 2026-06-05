using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200003F RID: 63
	public class CSParticle3D : CSNode3D
	{
		// Token: 0x0600094E RID: 2382 RVA: 0x0000C6AA File Offset: 0x0000A8AA
		public CSParticle3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSParticle3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0000C6CC File Offset: 0x0000A8CC
		public static HandleRef getCPtr(CSParticle3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0000C6F8 File Offset: 0x0000A8F8
		~CSParticle3D()
		{
			this.Dispose();
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0000C75C File Offset: 0x0000A95C
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
								CocoStudioEngineAdapterPINVOKE.delete_CSParticle3D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSParticle3D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0000C85C File Offset: 0x0000AA5C
		public CSParticle3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSParticle3D(), true)
		{
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0000C870 File Offset: 0x0000AA70
		public void InitParticle3DSystem(string file)
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_InitParticle3DSystem(this.swigCPtr, file);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0000C89D File Offset: 0x0000AA9D
		public void StartParticleIfPossible()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_StartParticleIfPossible(this.swigCPtr);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0000C8AC File Offset: 0x0000AAAC
		public override void SetPixelRenderMode(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_SetPixelRenderMode(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0000C8F7 File Offset: 0x0000AAF7
		public override void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_RestoreRenderMode(this.swigCPtr);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0000C908 File Offset: 0x0000AB08
		public override Color GetDisplayColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSParticle3D_GetDisplayColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0000C947 File Offset: 0x0000AB47
		public void startParticle()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_startParticle(this.swigCPtr);
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0000C956 File Offset: 0x0000AB56
		public void stopParticle()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_stopParticle(this.swigCPtr);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0000C968 File Offset: 0x0000AB68
		public bool isWorldPosition()
		{
			return CocoStudioEngineAdapterPINVOKE.CSParticle3D_isWorldPosition(this.swigCPtr);
		}

		// Token: 0x04000069 RID: 105
		private HandleRef swigCPtr;
	}
}

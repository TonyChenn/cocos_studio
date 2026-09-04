using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000047 RID: 71
	public class CSScale : IDisposable
	{
		// Token: 0x0600099E RID: 2462 RVA: 0x0000D87D File Offset: 0x0000BA7D
		public CSScale(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0000D89C File Offset: 0x0000BA9C
		public static HandleRef getCPtr(CSScale obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
		~CSScale()
		{
			this.Dispose();
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0000D92C File Offset: 0x0000BB2C
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
								CocoStudioEngineAdapterPINVOKE.delete_CSScale(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSScale(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0000DA24 File Offset: 0x0000BC24
		public CSScale() : this(CocoStudioEngineAdapterPINVOKE.new_CSScale__SWIG_0(), true)
		{
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0000DA38 File Offset: 0x0000BC38
		public CSScale(ScaleValue scale) : this(CocoStudioEngineAdapterPINVOKE.new_CSScale__SWIG_1(CSScale.getCPtr(new CSScale(scale.ScaleX, scale.ScaleY))), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0000DA7C File Offset: 0x0000BC7C
		public CSScale(float scaleX, float scaleY) : this(CocoStudioEngineAdapterPINVOKE.new_CSScale__SWIG_2(scaleX, scaleY), true)
		{
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0000DA8F File Offset: 0x0000BC8F
		public void SetScale(float scaleX, float scaleY)
		{
			CocoStudioEngineAdapterPINVOKE.CSScale_SetScale(this.swigCPtr, scaleX, scaleY);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
		public void SetScaleX(float scaleX)
		{
			CocoStudioEngineAdapterPINVOKE.CSScale_SetScaleX(this.swigCPtr, scaleX);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0000DAB0 File Offset: 0x0000BCB0
		public float GetScaleX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScale_GetScaleX(this.swigCPtr);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0000DACF File Offset: 0x0000BCCF
		public void SetScaleY(float scaleY)
		{
			CocoStudioEngineAdapterPINVOKE.CSScale_SetScaleY(this.swigCPtr, scaleY);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0000DAE0 File Offset: 0x0000BCE0
		public float GetScaleY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScale_GetScaleY(this.swigCPtr);
		}

		// Token: 0x0400007D RID: 125
		private HandleRef swigCPtr;

		// Token: 0x0400007E RID: 126
		protected bool swigCMemOwn;
	}
}

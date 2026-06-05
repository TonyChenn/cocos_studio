using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000039 RID: 57
	public class CSLayer : CSNode2D
	{
		// Token: 0x060008F9 RID: 2297 RVA: 0x0000B2C8 File Offset: 0x000094C8
		public CSLayer(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSLayer_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0000B2E8 File Offset: 0x000094E8
		public static HandleRef getCPtr(CSLayer obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0000B314 File Offset: 0x00009514
		~CSLayer()
		{
			this.Dispose();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0000B378 File Offset: 0x00009578
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
								CocoStudioEngineAdapterPINVOKE.delete_CSLayer(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSLayer(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0000B478 File Offset: 0x00009678
		public CSLayer() : this(CocoStudioEngineAdapterPINVOKE.new_CSLayer(), true)
		{
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0000B48C File Offset: 0x0000968C
		public virtual bool IsTouchEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLayer_IsTouchEnabled(this.swigCPtr);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0000B4AB File Offset: 0x000096AB
		public virtual void SetTouchEnabled(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSLayer_SetTouchEnabled(this.swigCPtr, isEnabled);
		}

		// Token: 0x04000063 RID: 99
		private HandleRef swigCPtr;
	}
}

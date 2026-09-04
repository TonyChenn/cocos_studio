using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000080 RID: 128
	public class V3F_C4B_T2F_Quad : IDisposable
	{
		// Token: 0x06000D10 RID: 3344 RVA: 0x00019279 File Offset: 0x00017479
		public V3F_C4B_T2F_Quad(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00019298 File Offset: 0x00017498
		public static HandleRef getCPtr(V3F_C4B_T2F_Quad obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000192C4 File Offset: 0x000174C4
		~V3F_C4B_T2F_Quad()
		{
			this.Dispose();
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00019328 File Offset: 0x00017528
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
								CocoStudioEngineAdapterPINVOKE.delete_V3F_C4B_T2F_Quad(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V3F_C4B_T2F_Quad(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00019438 File Offset: 0x00017638
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x00019420 File Offset: 0x00017620
		public V3F_C4B_T2F tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_Quad_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_Quad_tl_set(this.swigCPtr, V3F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x00019488 File Offset: 0x00017688
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x00019470 File Offset: 0x00017670
		public V3F_C4B_T2F bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_Quad_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_Quad_bl_set(this.swigCPtr, V3F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x000194D8 File Offset: 0x000176D8
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x000194C0 File Offset: 0x000176C0
		public V3F_C4B_T2F tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_Quad_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_Quad_tr_set(this.swigCPtr, V3F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00019528 File Offset: 0x00017728
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00019510 File Offset: 0x00017710
		public V3F_C4B_T2F br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_Quad_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_Quad_br_set(this.swigCPtr, V3F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00019560 File Offset: 0x00017760
		public V3F_C4B_T2F_Quad() : this(CocoStudioEngineAdapterPINVOKE.new_V3F_C4B_T2F_Quad(), true)
		{
		}

		// Token: 0x040000F5 RID: 245
		private HandleRef swigCPtr;

		// Token: 0x040000F6 RID: 246
		protected bool swigCMemOwn;
	}
}

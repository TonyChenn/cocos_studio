using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200007B RID: 123
	public class V2F_C4B_T2F_Quad : IDisposable
	{
		// Token: 0x06000CD5 RID: 3285 RVA: 0x00018491 File Offset: 0x00016691
		public V2F_C4B_T2F_Quad(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000184B0 File Offset: 0x000166B0
		public static HandleRef getCPtr(V2F_C4B_T2F_Quad obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000184DC File Offset: 0x000166DC
		~V2F_C4B_T2F_Quad()
		{
			this.Dispose();
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00018540 File Offset: 0x00016740
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
								CocoStudioEngineAdapterPINVOKE.delete_V2F_C4B_T2F_Quad(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V2F_C4B_T2F_Quad(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00018650 File Offset: 0x00016850
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x00018638 File Offset: 0x00016838
		public V2F_C4B_T2F bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Quad_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Quad_bl_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000186A0 File Offset: 0x000168A0
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x00018688 File Offset: 0x00016888
		public V2F_C4B_T2F br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Quad_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Quad_br_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x000186F0 File Offset: 0x000168F0
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x000186D8 File Offset: 0x000168D8
		public V2F_C4B_T2F tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Quad_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Quad_tl_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00018740 File Offset: 0x00016940
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x00018728 File Offset: 0x00016928
		public V2F_C4B_T2F tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Quad_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Quad_tr_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00018778 File Offset: 0x00016978
		public V2F_C4B_T2F_Quad() : this(CocoStudioEngineAdapterPINVOKE.new_V2F_C4B_T2F_Quad(), true)
		{
		}

		// Token: 0x040000EB RID: 235
		private HandleRef swigCPtr;

		// Token: 0x040000EC RID: 236
		protected bool swigCMemOwn;
	}
}

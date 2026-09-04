using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000082 RID: 130
	public class V3F_T2F_Quad : IDisposable
	{
		// Token: 0x06000D26 RID: 3366 RVA: 0x000197C9 File Offset: 0x000179C9
		public V3F_T2F_Quad(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x000197E8 File Offset: 0x000179E8
		public static HandleRef getCPtr(V3F_T2F_Quad obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00019814 File Offset: 0x00017A14
		~V3F_T2F_Quad()
		{
			this.Dispose();
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00019878 File Offset: 0x00017A78
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
								CocoStudioEngineAdapterPINVOKE.delete_V3F_T2F_Quad(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V3F_T2F_Quad(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00019988 File Offset: 0x00017B88
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00019970 File Offset: 0x00017B70
		public V3F_T2F bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_bl_set(this.swigCPtr, V3F_T2F.getCPtr(value));
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x000199D8 File Offset: 0x00017BD8
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x000199C0 File Offset: 0x00017BC0
		public V3F_T2F br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_br_set(this.swigCPtr, V3F_T2F.getCPtr(value));
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00019A28 File Offset: 0x00017C28
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x00019A10 File Offset: 0x00017C10
		public V3F_T2F tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_tl_set(this.swigCPtr, V3F_T2F.getCPtr(value));
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00019A78 File Offset: 0x00017C78
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00019A60 File Offset: 0x00017C60
		public V3F_T2F tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_tr_set(this.swigCPtr, V3F_T2F.getCPtr(value));
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00019AB0 File Offset: 0x00017CB0
		public V3F_T2F_Quad() : this(CocoStudioEngineAdapterPINVOKE.new_V3F_T2F_Quad(), true)
		{
		}

		// Token: 0x040000F9 RID: 249
		private HandleRef swigCPtr;

		// Token: 0x040000FA RID: 250
		protected bool swigCMemOwn;
	}
}

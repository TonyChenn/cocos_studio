using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200007E RID: 126
	public class V2F_C4F_T2F_Quad : IDisposable
	{
		// Token: 0x06000CF8 RID: 3320 RVA: 0x00018CD9 File Offset: 0x00016ED9
		public V2F_C4F_T2F_Quad(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00018CF8 File Offset: 0x00016EF8
		public static HandleRef getCPtr(V2F_C4F_T2F_Quad obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00018D24 File Offset: 0x00016F24
		~V2F_C4F_T2F_Quad()
		{
			this.Dispose();
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00018D88 File Offset: 0x00016F88
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
								CocoStudioEngineAdapterPINVOKE.delete_V2F_C4F_T2F_Quad(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V2F_C4F_T2F_Quad(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00018E98 File Offset: 0x00017098
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x00018E80 File Offset: 0x00017080
		public V2F_C4F_T2F bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_Quad_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_Quad_bl_set(this.swigCPtr, V2F_C4F_T2F.getCPtr(value));
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00018EE8 File Offset: 0x000170E8
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x00018ED0 File Offset: 0x000170D0
		public V2F_C4F_T2F br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_Quad_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_Quad_br_set(this.swigCPtr, V2F_C4F_T2F.getCPtr(value));
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00018F38 File Offset: 0x00017138
		// (set) Token: 0x06000D00 RID: 3328 RVA: 0x00018F20 File Offset: 0x00017120
		public V2F_C4F_T2F tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_Quad_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_Quad_tl_set(this.swigCPtr, V2F_C4F_T2F.getCPtr(value));
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00018F88 File Offset: 0x00017188
		// (set) Token: 0x06000D02 RID: 3330 RVA: 0x00018F70 File Offset: 0x00017170
		public V2F_C4F_T2F tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_Quad_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_Quad_tr_set(this.swigCPtr, V2F_C4F_T2F.getCPtr(value));
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00018FC0 File Offset: 0x000171C0
		public V2F_C4F_T2F_Quad() : this(CocoStudioEngineAdapterPINVOKE.new_V2F_C4F_T2F_Quad(), true)
		{
		}

		// Token: 0x040000F1 RID: 241
		private HandleRef swigCPtr;

		// Token: 0x040000F2 RID: 242
		protected bool swigCMemOwn;
	}
}

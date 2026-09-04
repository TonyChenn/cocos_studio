using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200007C RID: 124
	public class V2F_C4B_T2F_Triangle : IDisposable
	{
		// Token: 0x06000CE2 RID: 3298 RVA: 0x00018789 File Offset: 0x00016989
		public V2F_C4B_T2F_Triangle(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x000187A8 File Offset: 0x000169A8
		public static HandleRef getCPtr(V2F_C4B_T2F_Triangle obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x000187D4 File Offset: 0x000169D4
		~V2F_C4B_T2F_Triangle()
		{
			this.Dispose();
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00018838 File Offset: 0x00016A38
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
								CocoStudioEngineAdapterPINVOKE.delete_V2F_C4B_T2F_Triangle(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V2F_C4B_T2F_Triangle(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00018948 File Offset: 0x00016B48
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x00018930 File Offset: 0x00016B30
		public V2F_C4B_T2F a
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_a_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_a_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00018998 File Offset: 0x00016B98
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00018980 File Offset: 0x00016B80
		public V2F_C4B_T2F b
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_b_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_b_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x000189E8 File Offset: 0x00016BE8
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x000189D0 File Offset: 0x00016BD0
		public V2F_C4B_T2F c
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_c_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_c_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00018A20 File Offset: 0x00016C20
		public V2F_C4B_T2F_Triangle() : this(CocoStudioEngineAdapterPINVOKE.new_V2F_C4B_T2F_Triangle(), true)
		{
		}

		// Token: 0x040000ED RID: 237
		private HandleRef swigCPtr;

		// Token: 0x040000EE RID: 238
		protected bool swigCMemOwn;
	}
}

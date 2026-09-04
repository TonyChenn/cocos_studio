using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000081 RID: 129
	public class V3F_T2F : IDisposable
	{
		// Token: 0x06000D1D RID: 3357 RVA: 0x00019571 File Offset: 0x00017771
		public V3F_T2F(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00019590 File Offset: 0x00017790
		public static HandleRef getCPtr(V3F_T2F obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x000195BC File Offset: 0x000177BC
		~V3F_T2F()
		{
			this.Dispose();
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00019620 File Offset: 0x00017820
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
								CocoStudioEngineAdapterPINVOKE.delete_V3F_T2F(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V3F_T2F(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x00019730 File Offset: 0x00017930
		// (set) Token: 0x06000D21 RID: 3361 RVA: 0x00019718 File Offset: 0x00017918
		public Vec3 vertices
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_vertices_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_vertices_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00019780 File Offset: 0x00017980
		// (set) Token: 0x06000D23 RID: 3363 RVA: 0x00019768 File Offset: 0x00017968
		public Tex2F texCoords
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_texCoords_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_texCoords_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x000197B8 File Offset: 0x000179B8
		public V3F_T2F() : this(CocoStudioEngineAdapterPINVOKE.new_V3F_T2F(), true)
		{
		}

		// Token: 0x040000F7 RID: 247
		private HandleRef swigCPtr;

		// Token: 0x040000F8 RID: 248
		protected bool swigCMemOwn;
	}
}

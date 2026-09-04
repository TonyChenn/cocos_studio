using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200007A RID: 122
	public class V2F_C4B_T2F : IDisposable
	{
		// Token: 0x06000CCA RID: 3274 RVA: 0x000181E7 File Offset: 0x000163E7
		public V2F_C4B_T2F(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00018208 File Offset: 0x00016408
		public static HandleRef getCPtr(V2F_C4B_T2F obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00018234 File Offset: 0x00016434
		~V2F_C4B_T2F()
		{
			this.Dispose();
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00018298 File Offset: 0x00016498
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
								CocoStudioEngineAdapterPINVOKE.delete_V2F_C4B_T2F(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V2F_C4B_T2F(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x000183A8 File Offset: 0x000165A8
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00018390 File Offset: 0x00016590
		public Vec2 vertices
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_vertices_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_vertices_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x000183F8 File Offset: 0x000165F8
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x000183E0 File Offset: 0x000165E0
		public Color4B colors
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_colors_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_colors_set(this.swigCPtr, Color4B.getCPtr(value));
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00018448 File Offset: 0x00016648
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x00018430 File Offset: 0x00016630
		public Tex2F texCoords
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_texCoords_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_texCoords_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00018480 File Offset: 0x00016680
		public V2F_C4B_T2F() : this(CocoStudioEngineAdapterPINVOKE.new_V2F_C4B_T2F(), true)
		{
		}

		// Token: 0x040000E9 RID: 233
		private HandleRef swigCPtr;

		// Token: 0x040000EA RID: 234
		protected bool swigCMemOwn;
	}
}

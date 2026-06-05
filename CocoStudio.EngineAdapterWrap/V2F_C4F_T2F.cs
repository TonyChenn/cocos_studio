using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200007D RID: 125
	public class V2F_C4F_T2F : IDisposable
	{
		// Token: 0x06000CED RID: 3309 RVA: 0x00018A31 File Offset: 0x00016C31
		public V2F_C4F_T2F(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00018A50 File Offset: 0x00016C50
		public static HandleRef getCPtr(V2F_C4F_T2F obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00018A7C File Offset: 0x00016C7C
		~V2F_C4F_T2F()
		{
			this.Dispose();
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00018AE0 File Offset: 0x00016CE0
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
								CocoStudioEngineAdapterPINVOKE.delete_V2F_C4F_T2F(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V2F_C4F_T2F(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00018BF0 File Offset: 0x00016DF0
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x00018BD8 File Offset: 0x00016DD8
		public Vec2 vertices
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_vertices_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_vertices_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00018C40 File Offset: 0x00016E40
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x00018C28 File Offset: 0x00016E28
		public Color4F colors
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_colors_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_colors_set(this.swigCPtr, Color4F.getCPtr(value));
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00018C90 File Offset: 0x00016E90
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x00018C78 File Offset: 0x00016E78
		public Tex2F texCoords
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_texCoords_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4F_T2F_texCoords_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00018CC8 File Offset: 0x00016EC8
		public V2F_C4F_T2F() : this(CocoStudioEngineAdapterPINVOKE.new_V2F_C4F_T2F(), true)
		{
		}

		// Token: 0x040000EF RID: 239
		private HandleRef swigCPtr;

		// Token: 0x040000F0 RID: 240
		protected bool swigCMemOwn;
	}
}

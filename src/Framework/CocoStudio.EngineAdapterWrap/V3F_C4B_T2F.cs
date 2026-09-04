using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200007F RID: 127
	public class V3F_C4B_T2F : IDisposable
	{
		// Token: 0x06000D05 RID: 3333 RVA: 0x00018FD1 File Offset: 0x000171D1
		public V3F_C4B_T2F(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00018FF0 File Offset: 0x000171F0
		public static HandleRef getCPtr(V3F_C4B_T2F obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0001901C File Offset: 0x0001721C
		~V3F_C4B_T2F()
		{
			this.Dispose();
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00019080 File Offset: 0x00017280
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
								CocoStudioEngineAdapterPINVOKE.delete_V3F_C4B_T2F(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V3F_C4B_T2F(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00019190 File Offset: 0x00017390
		// (set) Token: 0x06000D09 RID: 3337 RVA: 0x00019178 File Offset: 0x00017378
		public Vec3 vertices
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_vertices_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_vertices_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x000191E0 File Offset: 0x000173E0
		// (set) Token: 0x06000D0B RID: 3339 RVA: 0x000191C8 File Offset: 0x000173C8
		public Color4B colors
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_colors_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_colors_set(this.swigCPtr, Color4B.getCPtr(value));
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00019230 File Offset: 0x00017430
		// (set) Token: 0x06000D0D RID: 3341 RVA: 0x00019218 File Offset: 0x00017418
		public Tex2F texCoords
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_texCoords_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_C4B_T2F_texCoords_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00019268 File Offset: 0x00017468
		public V3F_C4B_T2F() : this(CocoStudioEngineAdapterPINVOKE.new_V3F_C4B_T2F(), true)
		{
		}

		// Token: 0x040000F3 RID: 243
		private HandleRef swigCPtr;

		// Token: 0x040000F4 RID: 244
		protected bool swigCMemOwn;
	}
}

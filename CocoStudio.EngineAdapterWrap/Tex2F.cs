using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000077 RID: 119
	public class Tex2F : IDisposable
	{
		// Token: 0x06000CC0 RID: 3264 RVA: 0x00017FBD File Offset: 0x000161BD
		public Tex2F(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00017FDC File Offset: 0x000161DC
		public static HandleRef getCPtr(Tex2F obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00018008 File Offset: 0x00016208
		~Tex2F()
		{
			this.Dispose();
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0001806C File Offset: 0x0001626C
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
								CocoStudioEngineAdapterPINVOKE.delete_Tex2F(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Tex2F(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00018164 File Offset: 0x00016364
		public Tex2F(float _u, float _v) : this(CocoStudioEngineAdapterPINVOKE.new_Tex2F__SWIG_0(_u, _v), true)
		{
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00018177 File Offset: 0x00016377
		public Tex2F() : this(CocoStudioEngineAdapterPINVOKE.new_Tex2F__SWIG_1(), true)
		{
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00018198 File Offset: 0x00016398
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x00018188 File Offset: 0x00016388
		public float u
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Tex2F_u_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Tex2F_u_set(this.swigCPtr, value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x000181C8 File Offset: 0x000163C8
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x000181B7 File Offset: 0x000163B7
		public float v
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Tex2F_v_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Tex2F_v_set(this.swigCPtr, value);
			}
		}

		// Token: 0x040000DF RID: 223
		private HandleRef swigCPtr;

		// Token: 0x040000E0 RID: 224
		protected bool swigCMemOwn;
	}
}

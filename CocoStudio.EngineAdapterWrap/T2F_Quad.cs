using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000076 RID: 118
	public class T2F_Quad : IDisposable
	{
		// Token: 0x06000CB3 RID: 3251 RVA: 0x00017CC4 File Offset: 0x00015EC4
		public T2F_Quad(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00017CE4 File Offset: 0x00015EE4
		public static HandleRef getCPtr(T2F_Quad obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00017D10 File Offset: 0x00015F10
		~T2F_Quad()
		{
			this.Dispose();
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00017D74 File Offset: 0x00015F74
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
								CocoStudioEngineAdapterPINVOKE.delete_T2F_Quad(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_T2F_Quad(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00017E84 File Offset: 0x00016084
		// (set) Token: 0x06000CB7 RID: 3255 RVA: 0x00017E6C File Offset: 0x0001606C
		public Tex2F bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.T2F_Quad_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.T2F_Quad_bl_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00017ED4 File Offset: 0x000160D4
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x00017EBC File Offset: 0x000160BC
		public Tex2F br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.T2F_Quad_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.T2F_Quad_br_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00017F24 File Offset: 0x00016124
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x00017F0C File Offset: 0x0001610C
		public Tex2F tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.T2F_Quad_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.T2F_Quad_tl_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00017F74 File Offset: 0x00016174
		// (set) Token: 0x06000CBD RID: 3261 RVA: 0x00017F5C File Offset: 0x0001615C
		public Tex2F tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.T2F_Quad_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.T2F_Quad_tr_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00017FAC File Offset: 0x000161AC
		public T2F_Quad() : this(CocoStudioEngineAdapterPINVOKE.new_T2F_Quad(), true)
		{
		}

		// Token: 0x040000DD RID: 221
		private HandleRef swigCPtr;

		// Token: 0x040000DE RID: 222
		protected bool swigCMemOwn;
	}
}

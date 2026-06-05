using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200006E RID: 110
	public class Quad2 : IDisposable
	{
		// Token: 0x06000C27 RID: 3111 RVA: 0x00015EB4 File Offset: 0x000140B4
		public Quad2(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00015ED4 File Offset: 0x000140D4
		public static HandleRef getCPtr(Quad2 obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00015F00 File Offset: 0x00014100
		~Quad2()
		{
			this.Dispose();
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00015F64 File Offset: 0x00014164
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
								CocoStudioEngineAdapterPINVOKE.delete_Quad2(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Quad2(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x00016074 File Offset: 0x00014274
		// (set) Token: 0x06000C2B RID: 3115 RVA: 0x0001605C File Offset: 0x0001425C
		public Vec2 tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad2_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad2_tl_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x000160C4 File Offset: 0x000142C4
		// (set) Token: 0x06000C2D RID: 3117 RVA: 0x000160AC File Offset: 0x000142AC
		public Vec2 tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad2_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad2_tr_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00016114 File Offset: 0x00014314
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x000160FC File Offset: 0x000142FC
		public Vec2 bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad2_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad2_bl_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00016164 File Offset: 0x00014364
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x0001614C File Offset: 0x0001434C
		public Vec2 br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad2_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad2_br_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x0001619C File Offset: 0x0001439C
		public Quad2() : this(CocoStudioEngineAdapterPINVOKE.new_Quad2(), true)
		{
		}

		// Token: 0x040000D0 RID: 208
		private HandleRef swigCPtr;

		// Token: 0x040000D1 RID: 209
		protected bool swigCMemOwn;
	}
}

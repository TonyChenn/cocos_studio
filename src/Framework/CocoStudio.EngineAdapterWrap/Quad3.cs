using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200006F RID: 111
	public class Quad3 : IDisposable
	{
		// Token: 0x06000C34 RID: 3124 RVA: 0x000161AD File Offset: 0x000143AD
		public Quad3(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000161CC File Offset: 0x000143CC
		public static HandleRef getCPtr(Quad3 obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000161F8 File Offset: 0x000143F8
		~Quad3()
		{
			this.Dispose();
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x0001625C File Offset: 0x0001445C
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
								CocoStudioEngineAdapterPINVOKE.delete_Quad3(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Quad3(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0001636C File Offset: 0x0001456C
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x00016354 File Offset: 0x00014554
		public Vec3 bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad3_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad3_bl_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x000163BC File Offset: 0x000145BC
		// (set) Token: 0x06000C3A RID: 3130 RVA: 0x000163A4 File Offset: 0x000145A4
		public Vec3 br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad3_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad3_br_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x0001640C File Offset: 0x0001460C
		// (set) Token: 0x06000C3C RID: 3132 RVA: 0x000163F4 File Offset: 0x000145F4
		public Vec3 tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad3_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad3_tl_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x0001645C File Offset: 0x0001465C
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x00016444 File Offset: 0x00014644
		public Vec3 tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad3_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad3_tr_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00016494 File Offset: 0x00014694
		public Quad3() : this(CocoStudioEngineAdapterPINVOKE.new_Quad3(), true)
		{
		}

		// Token: 0x040000D2 RID: 210
		private HandleRef swigCPtr;

		// Token: 0x040000D3 RID: 211
		protected bool swigCMemOwn;
	}
}

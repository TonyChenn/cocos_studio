using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200001F RID: 31
	public class Acceleration : IDisposable
	{
		// Token: 0x06000192 RID: 402 RVA: 0x00006EA3 File Offset: 0x000050A3
		public Acceleration(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006EC4 File Offset: 0x000050C4
		public static HandleRef getCPtr(Acceleration obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006EF0 File Offset: 0x000050F0
		~Acceleration()
		{
			this.Dispose();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006F54 File Offset: 0x00005154
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
								CocoStudioEngineAdapterPINVOKE.delete_Acceleration(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Acceleration(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000705C File Offset: 0x0000525C
		// (set) Token: 0x06000196 RID: 406 RVA: 0x0000704C File Offset: 0x0000524C
		public double x
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Acceleration_x_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Acceleration_x_set(this.swigCPtr, value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000708C File Offset: 0x0000528C
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000707B File Offset: 0x0000527B
		public double y
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Acceleration_y_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Acceleration_y_set(this.swigCPtr, value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000070BC File Offset: 0x000052BC
		// (set) Token: 0x0600019A RID: 410 RVA: 0x000070AB File Offset: 0x000052AB
		public double z
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Acceleration_z_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Acceleration_z_set(this.swigCPtr, value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000070EC File Offset: 0x000052EC
		// (set) Token: 0x0600019C RID: 412 RVA: 0x000070DB File Offset: 0x000052DB
		public double timestamp
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Acceleration_timestamp_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Acceleration_timestamp_set(this.swigCPtr, value);
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000710B File Offset: 0x0000530B
		public Acceleration() : this(CocoStudioEngineAdapterPINVOKE.new_Acceleration(), true)
		{
		}

		// Token: 0x04000022 RID: 34
		private HandleRef swigCPtr;

		// Token: 0x04000023 RID: 35
		protected bool swigCMemOwn;
	}
}

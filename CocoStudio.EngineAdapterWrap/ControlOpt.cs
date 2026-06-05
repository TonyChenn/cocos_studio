using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200002B RID: 43
	public class ControlOpt : IDisposable
	{
		// Token: 0x06000834 RID: 2100 RVA: 0x000084F6 File Offset: 0x000066F6
		public ControlOpt(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00008518 File Offset: 0x00006718
		public static HandleRef getCPtr(ControlOpt obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00008544 File Offset: 0x00006744
		~ControlOpt()
		{
			this.Dispose();
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000085A8 File Offset: 0x000067A8
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
								CocoStudioEngineAdapterPINVOKE.delete_ControlOpt(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_ControlOpt(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x000086A0 File Offset: 0x000068A0
		public ControlOpt() : this(CocoStudioEngineAdapterPINVOKE.new_ControlOpt(), true)
		{
		}

		// Token: 0x04000045 RID: 69
		private HandleRef swigCPtr;

		// Token: 0x04000046 RID: 70
		protected bool swigCMemOwn;

		// Token: 0x0200002C RID: 44
		public enum Opt
		{
			// Token: 0x04000048 RID: 72
			Invalid,
			// Token: 0x04000049 RID: 73
			Translate,
			// Token: 0x0400004A RID: 74
			Rotate,
			// Token: 0x0400004B RID: 75
			Scale,
			// Token: 0x0400004C RID: 76
			Move
		}
	}
}

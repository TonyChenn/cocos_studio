using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200002D RID: 45
	public class ControlResult : IDisposable
	{
		// Token: 0x06000839 RID: 2105 RVA: 0x000086B1 File Offset: 0x000068B1
		public ControlResult(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000086D0 File Offset: 0x000068D0
		public static HandleRef getCPtr(ControlResult obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000086FC File Offset: 0x000068FC
		~ControlResult()
		{
			this.Dispose();
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00008760 File Offset: 0x00006960
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
								CocoStudioEngineAdapterPINVOKE.delete_ControlResult(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_ControlResult(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00008858 File Offset: 0x00006A58
		public ControlResult() : this(CocoStudioEngineAdapterPINVOKE.new_ControlResult(), true)
		{
		}

		// Token: 0x0400004D RID: 77
		private HandleRef swigCPtr;

		// Token: 0x0400004E RID: 78
		protected bool swigCMemOwn;
	}
}

using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000003 RID: 3
	public class CSObject : IDisposable
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000028B0 File Offset: 0x00000AB0
		protected internal CSObject()
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028BC File Offset: 0x00000ABC
		protected virtual bool IsContainOpenGLResource()
		{
			return true;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000028CF File Offset: 0x00000ACF
		public CSObject(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000028F0 File Offset: 0x00000AF0
		public static HandleRef getCPtr(CSObject obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002940 File Offset: 0x00000B40
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
						if (!this.IsContainOpenGLResource())
						{
							throw new MethodAccessException("C++ destructor does not have public access");
						}
						GtkInvokeHelp.BeginInvoke(delegate
						{
							this.swigCPtr = handle;
							throw new MethodAccessException("C++ destructor does not have public access");
						});
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000003 RID: 3
		private HandleRef swigCPtr;

		// Token: 0x04000004 RID: 4
		protected bool swigCMemOwn;
	}
}

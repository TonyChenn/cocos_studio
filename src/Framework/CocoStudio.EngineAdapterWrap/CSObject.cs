using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSObject : IDisposable
	{
		protected internal CSObject()
		{
		}

		protected virtual bool IsContainOpenGLResource()
		{
			return true;
		}

		public CSObject(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSObject obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

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

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

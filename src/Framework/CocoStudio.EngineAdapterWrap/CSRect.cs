using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSRect : IDisposable
	{
		public CSRect(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSRect obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSRect()
		{
			this.Dispose();
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
						if (this.IsContainOpenGLResource())
						{
							GtkInvokeHelp.BeginInvoke(delegate
							{
								this.swigCPtr = handle;
								CocoStudioEngineAdapterPINVOKE.delete_CSRect(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSRect(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public CSRect(float x, float y, float width, float height) : this(CocoStudioEngineAdapterPINVOKE.new_CSRect__SWIG_0(x, y, width, height), true)
		{
		}

		public CSRect() : this(CocoStudioEngineAdapterPINVOKE.new_CSRect__SWIG_1(), true)
		{
		}

		public float MinX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSRect_MinX(this.swigCPtr);
		}

		public float MinY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSRect_MinY(this.swigCPtr);
		}

		public float MaxX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSRect_MaxX(this.swigCPtr);
		}

		public float MaxY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSRect_MaxY(this.swigCPtr);
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

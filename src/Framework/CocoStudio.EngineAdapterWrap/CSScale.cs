using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSScale : IDisposable
	{
		public CSScale(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSScale obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSScale()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSScale(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSScale(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public CSScale() : this(CocoStudioEngineAdapterPINVOKE.new_CSScale__SWIG_0(), true)
		{
		}

		public CSScale(ScaleValue scale) : this(CocoStudioEngineAdapterPINVOKE.new_CSScale__SWIG_1(CSScale.getCPtr(new CSScale(scale.ScaleX, scale.ScaleY))), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSScale(float scaleX, float scaleY) : this(CocoStudioEngineAdapterPINVOKE.new_CSScale__SWIG_2(scaleX, scaleY), true)
		{
		}

		public void SetScale(float scaleX, float scaleY)
		{
			CocoStudioEngineAdapterPINVOKE.CSScale_SetScale(this.swigCPtr, scaleX, scaleY);
		}

		public void SetScaleX(float scaleX)
		{
			CocoStudioEngineAdapterPINVOKE.CSScale_SetScaleX(this.swigCPtr, scaleX);
		}

		public float GetScaleX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScale_GetScaleX(this.swigCPtr);
		}

		public void SetScaleY(float scaleY)
		{
			CocoStudioEngineAdapterPINVOKE.CSScale_SetScaleY(this.swigCPtr, scaleY);
		}

		public float GetScaleY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScale_GetScaleY(this.swigCPtr);
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

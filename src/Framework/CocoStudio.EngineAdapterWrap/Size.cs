using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	internal class Size : IDisposable
	{
		public Size(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Size obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Size()
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
								CocoStudioEngineAdapterPINVOKE.delete_Size(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Size(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public float width
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Size_width_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Size_width_set(this.swigCPtr, value);
			}
		}

		public float height
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Size_height_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Size_height_set(this.swigCPtr, value);
			}
		}

		public Size() : this(CocoStudioEngineAdapterPINVOKE.new_Size__SWIG_0(), true)
		{
		}

		public Size(float width, float height) : this(CocoStudioEngineAdapterPINVOKE.new_Size__SWIG_1(width, height), true)
		{
		}

		public Size(Size other) : this(CocoStudioEngineAdapterPINVOKE.new_Size__SWIG_2(Size.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Size(Vec2 point) : this(CocoStudioEngineAdapterPINVOKE.new_Size__SWIG_3(Vec2.getCPtr(point)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void setSize(float width, float height)
		{
			CocoStudioEngineAdapterPINVOKE.Size_setSize(this.swigCPtr, width, height);
		}

		public bool equals(Size target)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Size_equals(this.swigCPtr, Size.getCPtr(target));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Size ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Size_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Size(intPtr, false);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

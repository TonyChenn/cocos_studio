using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Tex2F : IDisposable
	{
		public Tex2F(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Tex2F obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Tex2F()
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
								CocoStudioEngineAdapterPINVOKE.delete_Tex2F(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Tex2F(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Tex2F(float _u, float _v) : this(CocoStudioEngineAdapterPINVOKE.new_Tex2F__SWIG_0(_u, _v), true)
		{
		}

		public Tex2F() : this(CocoStudioEngineAdapterPINVOKE.new_Tex2F__SWIG_1(), true)
		{
		}

		public float u
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Tex2F_u_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Tex2F_u_set(this.swigCPtr, value);
			}
		}

		public float v
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Tex2F_v_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Tex2F_v_set(this.swigCPtr, value);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class V2F_C4B_T2F_Triangle : IDisposable
	{
		public V2F_C4B_T2F_Triangle(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(V2F_C4B_T2F_Triangle obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~V2F_C4B_T2F_Triangle()
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
								CocoStudioEngineAdapterPINVOKE.delete_V2F_C4B_T2F_Triangle(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V2F_C4B_T2F_Triangle(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public V2F_C4B_T2F a
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_a_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_a_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		public V2F_C4B_T2F b
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_b_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_b_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		public V2F_C4B_T2F c
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_c_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V2F_C4B_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V2F_C4B_T2F_Triangle_c_set(this.swigCPtr, V2F_C4B_T2F.getCPtr(value));
			}
		}

		public V2F_C4B_T2F_Triangle() : this(CocoStudioEngineAdapterPINVOKE.new_V2F_C4B_T2F_Triangle(), true)
		{
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

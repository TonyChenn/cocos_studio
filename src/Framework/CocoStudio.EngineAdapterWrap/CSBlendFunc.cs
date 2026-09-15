using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSBlendFunc : IDisposable
	{
		public CSBlendFunc(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSBlendFunc obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSBlendFunc()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSBlendFunc(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSBlendFunc(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public uint Src
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.CSBlendFunc_Src_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.CSBlendFunc_Src_set(this.swigCPtr, value);
			}
		}

		public uint Dst
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.CSBlendFunc_Dst_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.CSBlendFunc_Dst_set(this.swigCPtr, value);
			}
		}

		public CSBlendFunc() : this(CocoStudioEngineAdapterPINVOKE.new_CSBlendFunc__SWIG_0(), true)
		{
		}

		public CSBlendFunc(uint src, uint dst) : this(CocoStudioEngineAdapterPINVOKE.new_CSBlendFunc__SWIG_1(src, dst), true)
		{
		}

		public static CSBlendFunc DISABLE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSBlendFunc_DISABLE_get();
				return (intPtr == IntPtr.Zero) ? null : new CSBlendFunc(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.CSBlendFunc_DISABLE_set(CSBlendFunc.getCPtr(value));
			}
		}

		public static CSBlendFunc ALPHA_PREMULTIPLIED
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSBlendFunc_ALPHA_PREMULTIPLIED_get();
				return (intPtr == IntPtr.Zero) ? null : new CSBlendFunc(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.CSBlendFunc_ALPHA_PREMULTIPLIED_set(CSBlendFunc.getCPtr(value));
			}
		}

		public static CSBlendFunc ALPHA_NON_PREMULTIPLIED
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSBlendFunc_ALPHA_NON_PREMULTIPLIED_get();
				return (intPtr == IntPtr.Zero) ? null : new CSBlendFunc(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.CSBlendFunc_ALPHA_NON_PREMULTIPLIED_set(CSBlendFunc.getCPtr(value));
			}
		}

		public static CSBlendFunc ADDITIVE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSBlendFunc_ADDITIVE_get();
				return (intPtr == IntPtr.Zero) ? null : new CSBlendFunc(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.CSBlendFunc_ADDITIVE_set(CSBlendFunc.getCPtr(value));
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

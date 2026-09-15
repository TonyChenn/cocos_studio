using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Color4B : IDisposable
	{
		public Color4B(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Color4B obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Color4B()
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
								CocoStudioEngineAdapterPINVOKE.delete_Color4B(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Color4B(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Color4B() : this(CocoStudioEngineAdapterPINVOKE.new_Color4B__SWIG_0(), true)
		{
		}

		public Color4B(byte _r, byte _g, byte _b, byte _a) : this(CocoStudioEngineAdapterPINVOKE.new_Color4B__SWIG_1(_r, _g, _b, _a), true)
		{
		}

		public Color4B(Color3B color) : this(CocoStudioEngineAdapterPINVOKE.new_Color4B__SWIG_2(Color3B.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Color4B(Color4F color) : this(CocoStudioEngineAdapterPINVOKE.new_Color4B__SWIG_3(Color4F.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public byte r
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color4B_r_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color4B_r_set(this.swigCPtr, value);
			}
		}

		public byte g
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color4B_g_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color4B_g_set(this.swigCPtr, value);
			}
		}

		public byte b
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color4B_b_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color4B_b_set(this.swigCPtr, value);
			}
		}

		public byte a
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color4B_a_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color4B_a_set(this.swigCPtr, value);
			}
		}

		public static Color4B WHITE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_WHITE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		public static Color4B YELLOW
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_YELLOW_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		public static Color4B BLUE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_BLUE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		public static Color4B GREEN
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_GREEN_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		public static Color4B RED
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_RED_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		public static Color4B MAGENTA
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_MAGENTA_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		public static Color4B BLACK
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_BLACK_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		public static Color4B ORANGE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_ORANGE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		public static Color4B GRAY
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_GRAY_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

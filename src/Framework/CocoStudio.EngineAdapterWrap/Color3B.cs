using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Color3B : IDisposable
	{
		public Color3B(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Color3B obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Color3B()
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
								CocoStudioEngineAdapterPINVOKE.delete_Color3B(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Color3B(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Color3B() : this(CocoStudioEngineAdapterPINVOKE.new_Color3B__SWIG_0(), true)
		{
		}

		public Color3B(byte _r, byte _g, byte _b) : this(CocoStudioEngineAdapterPINVOKE.new_Color3B__SWIG_1(_r, _g, _b), true)
		{
		}

		public Color3B(Color4B color) : this(CocoStudioEngineAdapterPINVOKE.new_Color3B__SWIG_2(Color4B.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Color3B(Color4F color) : this(CocoStudioEngineAdapterPINVOKE.new_Color3B__SWIG_3(Color4F.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool equals(Color3B other)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Color3B_equals(this.swigCPtr, Color3B.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public byte r
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color3B_r_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color3B_r_set(this.swigCPtr, value);
			}
		}

		public byte g
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color3B_g_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color3B_g_set(this.swigCPtr, value);
			}
		}

		public byte b
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color3B_b_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color3B_b_set(this.swigCPtr, value);
			}
		}

		public static Color3B WHITE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_WHITE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		public static Color3B YELLOW
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_YELLOW_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		public static Color3B BLUE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_BLUE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		public static Color3B GREEN
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_GREEN_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		public static Color3B RED
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_RED_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		public static Color3B MAGENTA
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_MAGENTA_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		public static Color3B BLACK
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_BLACK_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		public static Color3B ORANGE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_ORANGE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		public static Color3B GRAY
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_GRAY_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Color4F : IDisposable
	{
		public Color4F(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Color4F obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Color4F()
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
								CocoStudioEngineAdapterPINVOKE.delete_Color4F(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Color4F(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Color4F() : this(CocoStudioEngineAdapterPINVOKE.new_Color4F__SWIG_0(), true)
		{
		}

		public Color4F(float _r, float _g, float _b, float _a) : this(CocoStudioEngineAdapterPINVOKE.new_Color4F__SWIG_1(_r, _g, _b, _a), true)
		{
		}

		public Color4F(Color3B color) : this(CocoStudioEngineAdapterPINVOKE.new_Color4F__SWIG_2(Color3B.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Color4F(Color4B color) : this(CocoStudioEngineAdapterPINVOKE.new_Color4F__SWIG_3(Color4B.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool equals(Color4F other)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Color4F_equals(this.swigCPtr, Color4F.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float r
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color4F_r_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color4F_r_set(this.swigCPtr, value);
			}
		}

		public float g
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color4F_g_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color4F_g_set(this.swigCPtr, value);
			}
		}

		public float b
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color4F_b_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color4F_b_set(this.swigCPtr, value);
			}
		}

		public float a
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Color4F_a_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Color4F_a_set(this.swigCPtr, value);
			}
		}

		public static Color4F WHITE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_WHITE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		public static Color4F YELLOW
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_YELLOW_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		public static Color4F BLUE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_BLUE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		public static Color4F GREEN
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_GREEN_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		public static Color4F RED
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_RED_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		public static Color4F MAGENTA
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_MAGENTA_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		public static Color4F BLACK
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_BLACK_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		public static Color4F ORANGE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_ORANGE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		public static Color4F GRAY
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_GRAY_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

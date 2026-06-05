using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200002A RID: 42
	public class Color4F : IDisposable
	{
		// Token: 0x0600081A RID: 2074 RVA: 0x00007FF2 File Offset: 0x000061F2
		public Color4F(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00008014 File Offset: 0x00006214
		public static HandleRef getCPtr(Color4F obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00008040 File Offset: 0x00006240
		~Color4F()
		{
			this.Dispose();
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000080A4 File Offset: 0x000062A4
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

		// Token: 0x0600081E RID: 2078 RVA: 0x0000819C File Offset: 0x0000639C
		public Color4F() : this(CocoStudioEngineAdapterPINVOKE.new_Color4F__SWIG_0(), true)
		{
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000081AD File Offset: 0x000063AD
		public Color4F(float _r, float _g, float _b, float _a) : this(CocoStudioEngineAdapterPINVOKE.new_Color4F__SWIG_1(_r, _g, _b, _a), true)
		{
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x000081C4 File Offset: 0x000063C4
		public Color4F(Color3B color) : this(CocoStudioEngineAdapterPINVOKE.new_Color4F__SWIG_2(Color3B.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x000081F8 File Offset: 0x000063F8
		public Color4F(Color4B color) : this(CocoStudioEngineAdapterPINVOKE.new_Color4F__SWIG_3(Color4B.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0000822C File Offset: 0x0000642C
		public bool equals(Color4F other)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Color4F_equals(this.swigCPtr, Color4F.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00008274 File Offset: 0x00006474
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x00008263 File Offset: 0x00006463
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x000082A4 File Offset: 0x000064A4
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x00008293 File Offset: 0x00006493
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x000082D4 File Offset: 0x000064D4
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x000082C3 File Offset: 0x000064C3
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00008304 File Offset: 0x00006504
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x000082F3 File Offset: 0x000064F3
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00008324 File Offset: 0x00006524
		public static Color4F WHITE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_WHITE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00008358 File Offset: 0x00006558
		public static Color4F YELLOW
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_YELLOW_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0000838C File Offset: 0x0000658C
		public static Color4F BLUE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_BLUE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x000083C0 File Offset: 0x000065C0
		public static Color4F GREEN
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_GREEN_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x000083F4 File Offset: 0x000065F4
		public static Color4F RED
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_RED_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x00008428 File Offset: 0x00006628
		public static Color4F MAGENTA
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_MAGENTA_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0000845C File Offset: 0x0000665C
		public static Color4F BLACK
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_BLACK_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00008490 File Offset: 0x00006690
		public static Color4F ORANGE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_ORANGE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x000084C4 File Offset: 0x000066C4
		public static Color4F GRAY
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4F_GRAY_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4F(intPtr, false);
			}
		}

		// Token: 0x04000043 RID: 67
		private HandleRef swigCPtr;

		// Token: 0x04000044 RID: 68
		protected bool swigCMemOwn;
	}
}

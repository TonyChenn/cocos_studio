using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000028 RID: 40
	public class Color3B : IDisposable
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x00007653 File Offset: 0x00005853
		public Color3B(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00007674 File Offset: 0x00005874
		public static HandleRef getCPtr(Color3B obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000076A0 File Offset: 0x000058A0
		~Color3B()
		{
			this.Dispose();
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00007704 File Offset: 0x00005904
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

		// Token: 0x060007ED RID: 2029 RVA: 0x000077FC File Offset: 0x000059FC
		public Color3B() : this(CocoStudioEngineAdapterPINVOKE.new_Color3B__SWIG_0(), true)
		{
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0000780D File Offset: 0x00005A0D
		public Color3B(byte _r, byte _g, byte _b) : this(CocoStudioEngineAdapterPINVOKE.new_Color3B__SWIG_1(_r, _g, _b), true)
		{
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00007824 File Offset: 0x00005A24
		public Color3B(Color4B color) : this(CocoStudioEngineAdapterPINVOKE.new_Color3B__SWIG_2(Color4B.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00007858 File Offset: 0x00005A58
		public Color3B(Color4F color) : this(CocoStudioEngineAdapterPINVOKE.new_Color3B__SWIG_3(Color4F.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0000788C File Offset: 0x00005A8C
		public bool equals(Color3B other)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Color3B_equals(this.swigCPtr, Color3B.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x000078D4 File Offset: 0x00005AD4
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x000078C3 File Offset: 0x00005AC3
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

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00007904 File Offset: 0x00005B04
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x000078F3 File Offset: 0x00005AF3
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

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00007934 File Offset: 0x00005B34
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x00007923 File Offset: 0x00005B23
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x00007954 File Offset: 0x00005B54
		public static Color3B WHITE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_WHITE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00007988 File Offset: 0x00005B88
		public static Color3B YELLOW
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_YELLOW_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x000079BC File Offset: 0x00005BBC
		public static Color3B BLUE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_BLUE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x000079F0 File Offset: 0x00005BF0
		public static Color3B GREEN
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_GREEN_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00007A24 File Offset: 0x00005C24
		public static Color3B RED
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_RED_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x00007A58 File Offset: 0x00005C58
		public static Color3B MAGENTA
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_MAGENTA_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x00007A8C File Offset: 0x00005C8C
		public static Color3B BLACK
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_BLACK_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00007AC0 File Offset: 0x00005CC0
		public static Color3B ORANGE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_ORANGE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00007AF4 File Offset: 0x00005CF4
		public static Color3B GRAY
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color3B_GRAY_get();
				return (intPtr == IntPtr.Zero) ? null : new Color3B(intPtr, false);
			}
		}

		// Token: 0x0400003F RID: 63
		private HandleRef swigCPtr;

		// Token: 0x04000040 RID: 64
		protected bool swigCMemOwn;
	}
}

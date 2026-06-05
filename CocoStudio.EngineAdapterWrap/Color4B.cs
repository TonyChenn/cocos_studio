using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000029 RID: 41
	public class Color4B : IDisposable
	{
		// Token: 0x06000801 RID: 2049 RVA: 0x00007B26 File Offset: 0x00005D26
		public Color4B(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00007B48 File Offset: 0x00005D48
		public static HandleRef getCPtr(Color4B obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00007B74 File Offset: 0x00005D74
		~Color4B()
		{
			this.Dispose();
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00007BD8 File Offset: 0x00005DD8
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

		// Token: 0x06000805 RID: 2053 RVA: 0x00007CD0 File Offset: 0x00005ED0
		public Color4B() : this(CocoStudioEngineAdapterPINVOKE.new_Color4B__SWIG_0(), true)
		{
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00007CE1 File Offset: 0x00005EE1
		public Color4B(byte _r, byte _g, byte _b, byte _a) : this(CocoStudioEngineAdapterPINVOKE.new_Color4B__SWIG_1(_r, _g, _b, _a), true)
		{
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00007CF8 File Offset: 0x00005EF8
		public Color4B(Color3B color) : this(CocoStudioEngineAdapterPINVOKE.new_Color4B__SWIG_2(Color3B.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00007D2C File Offset: 0x00005F2C
		public Color4B(Color4F color) : this(CocoStudioEngineAdapterPINVOKE.new_Color4B__SWIG_3(Color4F.getCPtr(color)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x00007D70 File Offset: 0x00005F70
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x00007D60 File Offset: 0x00005F60
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00007DA0 File Offset: 0x00005FA0
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x00007D8F File Offset: 0x00005F8F
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00007DD0 File Offset: 0x00005FD0
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x00007DBF File Offset: 0x00005FBF
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x00007E00 File Offset: 0x00006000
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x00007DEF File Offset: 0x00005FEF
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x00007E20 File Offset: 0x00006020
		public static Color4B WHITE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_WHITE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00007E54 File Offset: 0x00006054
		public static Color4B YELLOW
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_YELLOW_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x00007E88 File Offset: 0x00006088
		public static Color4B BLUE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_BLUE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00007EBC File Offset: 0x000060BC
		public static Color4B GREEN
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_GREEN_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00007EF0 File Offset: 0x000060F0
		public static Color4B RED
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_RED_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00007F24 File Offset: 0x00006124
		public static Color4B MAGENTA
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_MAGENTA_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x00007F58 File Offset: 0x00006158
		public static Color4B BLACK
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_BLACK_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00007F8C File Offset: 0x0000618C
		public static Color4B ORANGE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_ORANGE_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00007FC0 File Offset: 0x000061C0
		public static Color4B GRAY
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Color4B_GRAY_get();
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
		}

		// Token: 0x04000041 RID: 65
		private HandleRef swigCPtr;

		// Token: 0x04000042 RID: 66
		protected bool swigCMemOwn;
	}
}

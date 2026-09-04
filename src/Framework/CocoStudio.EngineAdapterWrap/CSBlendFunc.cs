using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000008 RID: 8
	public class CSBlendFunc : IDisposable
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002EF4 File Offset: 0x000010F4
		public CSBlendFunc(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002F14 File Offset: 0x00001114
		public static HandleRef getCPtr(CSBlendFunc obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002F40 File Offset: 0x00001140
		~CSBlendFunc()
		{
			this.Dispose();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002FA4 File Offset: 0x000011A4
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

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000030AC File Offset: 0x000012AC
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000309C File Offset: 0x0000129C
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000030DC File Offset: 0x000012DC
		// (set) Token: 0x06000054 RID: 84 RVA: 0x000030CB File Offset: 0x000012CB
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

		// Token: 0x06000056 RID: 86 RVA: 0x000030FB File Offset: 0x000012FB
		public CSBlendFunc() : this(CocoStudioEngineAdapterPINVOKE.new_CSBlendFunc__SWIG_0(), true)
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000310C File Offset: 0x0000130C
		public CSBlendFunc(uint src, uint dst) : this(CocoStudioEngineAdapterPINVOKE.new_CSBlendFunc__SWIG_1(src, dst), true)
		{
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003130 File Offset: 0x00001330
		// (set) Token: 0x06000058 RID: 88 RVA: 0x0000311F File Offset: 0x0000131F
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

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003174 File Offset: 0x00001374
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00003162 File Offset: 0x00001362
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000031B8 File Offset: 0x000013B8
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000031A6 File Offset: 0x000013A6
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000031FC File Offset: 0x000013FC
		// (set) Token: 0x0600005E RID: 94 RVA: 0x000031EA File Offset: 0x000013EA
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

		// Token: 0x04000007 RID: 7
		private HandleRef swigCPtr;

		// Token: 0x04000008 RID: 8
		protected bool swigCMemOwn;
	}
}

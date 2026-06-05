using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000042 RID: 66
	public class CSResourceData : IDisposable
	{
		// Token: 0x06000971 RID: 2417 RVA: 0x0000CF2D File Offset: 0x0000B12D
		public CSResourceData(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0000CF4C File Offset: 0x0000B14C
		public static HandleRef getCPtr(CSResourceData obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0000CF78 File Offset: 0x0000B178
		~CSResourceData()
		{
			this.Dispose();
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0000CFDC File Offset: 0x0000B1DC
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
								CocoStudioEngineAdapterPINVOKE.delete_CSResourceData(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSResourceData(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
		public CSResourceData() : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_0(), true)
		{
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0000D0E8 File Offset: 0x0000B2E8
		public CSResourceData(string path) : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_1(path), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0000D118 File Offset: 0x0000B318
		public CSResourceData(string path, CSResourceData.CSEnumResourceType type) : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_2(path, (int)type), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0000D148 File Offset: 0x0000B348
		public CSResourceData(string path, CSResourceData.CSEnumResourceType type, string plist) : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_3(path, (int)type, plist), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0000D17C File Offset: 0x0000B37C
		public CSResourceData(CSResourceData other) : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_4(CSResourceData.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		public CSResourceData.CSEnumResourceType GetResourceType()
		{
			return (CSResourceData.CSEnumResourceType)CocoStudioEngineAdapterPINVOKE.CSResourceData_GetResourceType(this.swigCPtr);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		public string GetPath()
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_GetPath(this.swigCPtr);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		public string GetPathC()
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_GetPathC(this.swigCPtr);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0000D210 File Offset: 0x0000B410
		public string GetPlistFile()
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_GetPlistFile(this.swigCPtr);
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0000D230 File Offset: 0x0000B430
		public bool EndsWith(string suffix)
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_EndsWith(this.swigCPtr, suffix);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0000D250 File Offset: 0x0000B450
		public bool Equals(ResourceData other)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSResourceData_Equals(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(other.Path, (CSResourceData.CSEnumResourceType)other.Type, other.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		public bool IsEmpty()
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_IsEmpty(this.swigCPtr);
		}

		// Token: 0x0400006C RID: 108
		private HandleRef swigCPtr;

		// Token: 0x0400006D RID: 109
		protected bool swigCMemOwn;

		// Token: 0x02000043 RID: 67
		public enum CSEnumResourceType
		{
			// Token: 0x0400006F RID: 111
			None = -1,
			// Token: 0x04000070 RID: 112
			Normal,
			// Token: 0x04000071 RID: 113
			PlistSubImage,
			// Token: 0x04000072 RID: 114
			Default,
			// Token: 0x04000073 RID: 115
			MarkedSubImage,
			// Token: 0x04000074 RID: 116
			Addin
		}
	}
}

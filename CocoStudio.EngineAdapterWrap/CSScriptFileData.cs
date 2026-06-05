using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200004E RID: 78
	public class CSScriptFileData : IDisposable
	{
		// Token: 0x060009E1 RID: 2529 RVA: 0x0000E74C File Offset: 0x0000C94C
		public CSScriptFileData(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0000E76C File Offset: 0x0000C96C
		public static HandleRef getCPtr(CSScriptFileData obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0000E798 File Offset: 0x0000C998
		~CSScriptFileData()
		{
			this.Dispose();
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0000E7FC File Offset: 0x0000C9FC
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
								CocoStudioEngineAdapterPINVOKE.delete_CSScriptFileData(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSScriptFileData(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
		public CSScriptFileData() : this(CocoStudioEngineAdapterPINVOKE.new_CSScriptFileData__SWIG_0(), true)
		{
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0000E905 File Offset: 0x0000CB05
		public CSScriptFileData(string path, CSScriptFileData.CSScriptType type) : this(CocoStudioEngineAdapterPINVOKE.new_CSScriptFileData__SWIG_1(path, (int)type), true)
		{
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0000E918 File Offset: 0x0000CB18
		public CSScriptFileData(CSScriptFileData other) : this(CocoStudioEngineAdapterPINVOKE.new_CSScriptFileData__SWIG_2(CSScriptFileData.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0000E94C File Offset: 0x0000CB4C
		public CSScriptFileData.CSScriptType GetScriptType()
		{
			return (CSScriptFileData.CSScriptType)CocoStudioEngineAdapterPINVOKE.CSScriptFileData_GetScriptType(this.swigCPtr);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0000E96C File Offset: 0x0000CB6C
		public string GetPath()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScriptFileData_GetPath(this.swigCPtr);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0000E98C File Offset: 0x0000CB8C
		public bool EndsWith(string suffix)
		{
			return CocoStudioEngineAdapterPINVOKE.CSScriptFileData_EndsWith(this.swigCPtr, suffix);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0000E9AC File Offset: 0x0000CBAC
		public bool Equals(ScriptFileData other)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSScriptFileData_Equals(this.swigCPtr, CSScriptFileData.getCPtr(new CSScriptFileData(other.ScriptFile, (CSScriptFileData.CSScriptType)other.FileType)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
		public bool IsEmpty()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScriptFileData_IsEmpty(this.swigCPtr);
		}

		// Token: 0x0400008E RID: 142
		private HandleRef swigCPtr;

		// Token: 0x0400008F RID: 143
		protected bool swigCMemOwn;

		// Token: 0x0200004F RID: 79
		public enum CSScriptType
		{
			// Token: 0x04000091 RID: 145
			None = -1,
			// Token: 0x04000092 RID: 146
			Lua,
			// Token: 0x04000093 RID: 147
			JavaScript
		}
	}
}

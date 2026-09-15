using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSScriptFileData : IDisposable
	{
		public CSScriptFileData(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSScriptFileData obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSScriptFileData()
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

		public CSScriptFileData() : this(CocoStudioEngineAdapterPINVOKE.new_CSScriptFileData__SWIG_0(), true)
		{
		}

		public CSScriptFileData(string path, CSScriptFileData.CSScriptType type) : this(CocoStudioEngineAdapterPINVOKE.new_CSScriptFileData__SWIG_1(path, (int)type), true)
		{
		}

		public CSScriptFileData(CSScriptFileData other) : this(CocoStudioEngineAdapterPINVOKE.new_CSScriptFileData__SWIG_2(CSScriptFileData.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSScriptFileData.CSScriptType GetScriptType()
		{
			return (CSScriptFileData.CSScriptType)CocoStudioEngineAdapterPINVOKE.CSScriptFileData_GetScriptType(this.swigCPtr);
		}

		public string GetPath()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScriptFileData_GetPath(this.swigCPtr);
		}

		public bool EndsWith(string suffix)
		{
			return CocoStudioEngineAdapterPINVOKE.CSScriptFileData_EndsWith(this.swigCPtr, suffix);
		}

		public bool Equals(ScriptFileData other)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSScriptFileData_Equals(this.swigCPtr, CSScriptFileData.getCPtr(new CSScriptFileData(other.ScriptFile, (CSScriptFileData.CSScriptType)other.FileType)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool IsEmpty()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScriptFileData_IsEmpty(this.swigCPtr);
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		public enum CSScriptType
		{
			None = -1,
			Lua,
			JavaScript
		}
	}
}

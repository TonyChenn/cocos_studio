using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSResourceData : IDisposable
	{
		public CSResourceData(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSResourceData obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSResourceData()
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

		public CSResourceData() : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_0(), true)
		{
		}

		public CSResourceData(string path) : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_1(path), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSResourceData(string path, CSResourceData.CSEnumResourceType type) : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_2(path, (int)type), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSResourceData(string path, CSResourceData.CSEnumResourceType type, string plist) : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_3(path, (int)type, plist), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSResourceData(CSResourceData other) : this(CocoStudioEngineAdapterPINVOKE.new_CSResourceData__SWIG_4(CSResourceData.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSResourceData.CSEnumResourceType GetResourceType()
		{
			return (CSResourceData.CSEnumResourceType)CocoStudioEngineAdapterPINVOKE.CSResourceData_GetResourceType(this.swigCPtr);
		}

		public string GetPath()
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_GetPath(this.swigCPtr);
		}

		public string GetPathC()
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_GetPathC(this.swigCPtr);
		}

		public string GetPlistFile()
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_GetPlistFile(this.swigCPtr);
		}

		public bool EndsWith(string suffix)
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_EndsWith(this.swigCPtr, suffix);
		}

		public bool Equals(ResourceData other)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSResourceData_Equals(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(other.Path, (CSResourceData.CSEnumResourceType)other.Type, other.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool IsEmpty()
		{
			return CocoStudioEngineAdapterPINVOKE.CSResourceData_IsEmpty(this.swigCPtr);
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		public enum CSEnumResourceType
		{
			None = -1,
			Normal,
			PlistSubImage,
			Default,
			MarkedSubImage,
			Addin
		}
	}
}

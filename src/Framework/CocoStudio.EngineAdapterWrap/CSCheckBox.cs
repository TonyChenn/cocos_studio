using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSCheckBox : CSWidget
	{
		public CSCheckBox(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSCheckBox_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSCheckBox obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSCheckBox()
		{
			this.Dispose();
		}

		public override void Dispose()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSCheckBox(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSCheckBox(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSCheckBox() : this(CocoStudioEngineAdapterPINVOKE.new_CSCheckBox(), true)
		{
		}

		public override void ResetState()
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_ResetState(this.swigCPtr);
		}

		public virtual bool GetChecked()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetChecked(this.swigCPtr);
		}

		public virtual void SetChecked(bool bChecked)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetChecked(this.swigCPtr, bChecked);
		}

		public virtual ResourceData GetNormalGroundFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetNormalGroundFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetNormalGroudFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetNormalGroudFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetPressedGroundFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetPressedGroundFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetPressedGroudFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetPressedGroudFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetDisabledGroundFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetDisabledGroundFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetDisabledGroudFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetDisabledGroudFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetNormalNodeFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetNormalNodeFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetNormalNodeFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetNormalNodeFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetDisabledNodeFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetDisabledNodeFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetDisabledNodeFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetDisabledNodeFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}

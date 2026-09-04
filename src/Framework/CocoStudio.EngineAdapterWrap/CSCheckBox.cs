using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000033 RID: 51
	public class CSCheckBox : CSWidget
	{
		// Token: 0x060008A8 RID: 2216 RVA: 0x00009E91 File Offset: 0x00008091
		public CSCheckBox(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSCheckBox_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00009EB0 File Offset: 0x000080B0
		public static HandleRef getCPtr(CSCheckBox obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00009EDC File Offset: 0x000080DC
		~CSCheckBox()
		{
			this.Dispose();
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00009F40 File Offset: 0x00008140
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

		// Token: 0x060008AC RID: 2220 RVA: 0x0000A040 File Offset: 0x00008240
		public CSCheckBox() : this(CocoStudioEngineAdapterPINVOKE.new_CSCheckBox(), true)
		{
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0000A051 File Offset: 0x00008251
		public override void ResetState()
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_ResetState(this.swigCPtr);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000A060 File Offset: 0x00008260
		public virtual bool GetChecked()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetChecked(this.swigCPtr);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000A07F File Offset: 0x0000827F
		public virtual void SetChecked(bool bChecked)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetChecked(this.swigCPtr, bChecked);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0000A090 File Offset: 0x00008290
		public virtual ResourceData GetNormalGroundFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetNormalGroundFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0000A0D0 File Offset: 0x000082D0
		public virtual void SetNormalGroudFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetNormalGroudFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0000A118 File Offset: 0x00008318
		public virtual ResourceData GetPressedGroundFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetPressedGroundFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0000A158 File Offset: 0x00008358
		public virtual void SetPressedGroudFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetPressedGroudFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0000A1A0 File Offset: 0x000083A0
		public virtual ResourceData GetDisabledGroundFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetDisabledGroundFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0000A1E0 File Offset: 0x000083E0
		public virtual void SetDisabledGroudFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetDisabledGroudFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0000A228 File Offset: 0x00008428
		public virtual ResourceData GetNormalNodeFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetNormalNodeFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0000A268 File Offset: 0x00008468
		public virtual void SetNormalNodeFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetNormalNodeFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000A2B0 File Offset: 0x000084B0
		public virtual ResourceData GetDisabledNodeFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCheckBox_GetDisabledNodeFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0000A2F0 File Offset: 0x000084F0
		public virtual void SetDisabledNodeFile(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSCheckBox_SetDisabledNodeFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000056 RID: 86
		private HandleRef swigCPtr;
	}
}

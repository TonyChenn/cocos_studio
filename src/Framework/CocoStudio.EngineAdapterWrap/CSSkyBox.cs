using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSSkyBox : CSObject
	{
		public CSSkyBox(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSkyBox_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSSkyBox obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSSkyBox()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSkyBox(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSkyBox(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSSkyBox() : this(CocoStudioEngineAdapterPINVOKE.new_CSSkyBox(), true)
		{
		}

		public bool IsEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSkyBox_IsEnabled(this.swigCPtr);
		}

		public void SetEnabled(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSSkyBox_SetEnabled(this.swigCPtr, enable);
		}

		public void ResetSkyBox()
		{
			CocoStudioEngineAdapterPINVOKE.CSSkyBox_ResetSkyBox(this.swigCPtr);
		}

		public void RefreshSkyBox(ResourceData leftfile, ResourceData rightfile, ResourceData topfile, ResourceData bottomfile, ResourceData forwordfile, ResourceData backfile)
		{
			CocoStudioEngineAdapterPINVOKE.CSSkyBox_RefreshSkyBox(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(leftfile.Path, (CSResourceData.CSEnumResourceType)leftfile.Type, leftfile.Plist)), CSResourceData.getCPtr(new CSResourceData(rightfile.Path, (CSResourceData.CSEnumResourceType)rightfile.Type, rightfile.Plist)), CSResourceData.getCPtr(new CSResourceData(topfile.Path, (CSResourceData.CSEnumResourceType)topfile.Type, topfile.Plist)), CSResourceData.getCPtr(new CSResourceData(bottomfile.Path, (CSResourceData.CSEnumResourceType)bottomfile.Type, bottomfile.Plist)), CSResourceData.getCPtr(new CSResourceData(forwordfile.Path, (CSResourceData.CSEnumResourceType)forwordfile.Type, forwordfile.Plist)), CSResourceData.getCPtr(new CSResourceData(backfile.Path, (CSResourceData.CSEnumResourceType)backfile.Type, backfile.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}

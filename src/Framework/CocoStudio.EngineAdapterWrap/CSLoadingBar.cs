using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSLoadingBar : CSWidget
	{
		public CSLoadingBar(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSLoadingBar obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSLoadingBar()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSLoadingBar(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSLoadingBar(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSLoadingBar() : this(CocoStudioEngineAdapterPINVOKE.new_CSLoadingBar(), true)
		{
		}

		public virtual int GetProgressPercent()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLoadingBar_GetProgressPercent(this.swigCPtr);
		}

		public virtual void SetProgressPercent(int iInfo)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetProgressPercent(this.swigCPtr, iInfo);
		}

		public virtual int GetProgressType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLoadingBar_GetProgressType(this.swigCPtr);
		}

		public virtual void SetProgressType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetProgressType(this.swigCPtr, iType);
		}

		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLoadingBar_GetScale9Enabled(this.swigCPtr);
		}

		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		public virtual ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSLoadingBar_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}

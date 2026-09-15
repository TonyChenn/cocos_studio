using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSImageView : CSWidget
	{
		public CSImageView(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSImageView_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSImageView obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSImageView()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSImageView(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSImageView(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSImageView() : this(CocoStudioEngineAdapterPINVOKE.new_CSImageView(), true)
		{
		}

		public virtual bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSImageView_GetFlipX(this.swigCPtr);
		}

		public virtual void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetFlipX(this.swigCPtr, flip);
		}

		public virtual bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSImageView_GetFlipY(this.swigCPtr);
		}

		public virtual void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetFlipY(this.swigCPtr, flip);
		}

		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSImageView_GetScale9Enabled(this.swigCPtr);
		}

		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		public virtual ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSImageView_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}

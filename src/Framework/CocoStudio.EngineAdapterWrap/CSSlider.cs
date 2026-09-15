using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSSlider : CSWidget
	{
		public CSSlider(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSlider_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSSlider obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSSlider()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSlider(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSlider(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSSlider() : this(CocoStudioEngineAdapterPINVOKE.new_CSSlider(), true)
		{
		}

		public virtual int GetPercent()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlider_GetPercent(this.swigCPtr);
		}

		public virtual void SetPercent(int percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetPercent(this.swigCPtr, percent);
		}

		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlider_GetScale9Enabled(this.swigCPtr);
		}

		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		public virtual ResourceData GetGroundBarTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetGroundBarTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetGroundBarTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetGroundBarTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetProgressBarTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetProgressBarTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetProgressBarTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetProgressBarTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetBallNormalTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetBallNormalTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetBallNormalTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetBallNormalTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetBallPressedTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetBallPressedTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetBallPressedTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetBallPressedTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetBallDisabledTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetBallDisabledTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetBallDisabledTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetBallDisabledTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override SizeF GetWidgetAutoSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetWidgetAutoSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		private HandleRef swigCPtr;
	}
}

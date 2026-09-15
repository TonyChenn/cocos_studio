using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSButton : CSWidget
	{
		public CSButton(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSButton_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSButton obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSButton()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSButton(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSButton(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSButton() : this(CocoStudioEngineAdapterPINVOKE.new_CSButton(), true)
		{
		}

		public override CocoStudio.Model.SizeF GetWidgetAutoSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetWidgetAutoSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new CocoStudio.Model.SizeF(size.width, size.height);
		}

		public override void SetCustomSizeEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetCustomSizeEnabled(this.swigCPtr, bEnabled);
		}

		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetScale9Enabled(this.swigCPtr);
		}

		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		public virtual bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetFlipX(this.swigCPtr);
		}

		public virtual void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetFlipX(this.swigCPtr, flip);
		}

		public virtual bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetFlipY(this.swigCPtr);
		}

		public virtual void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetFlipY(this.swigCPtr, flip);
		}

		public virtual string GetText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetText(this.swigCPtr);
		}

		public virtual void SetText(string sText)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetText(this.swigCPtr, sText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual string GetFontName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetFontName(this.swigCPtr);
		}

		public virtual void SetFontName(string sName)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetFontName(this.swigCPtr, sName);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual int GetFontSize()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetFontSize(this.swigCPtr);
		}

		public virtual void SetFontSize(int sSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetFontSize(this.swigCPtr, sSize);
		}

		public virtual Color GetTextColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetTextColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public virtual void SetTextColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetTextColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetNormalFilePath()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetNormalFilePath(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetNormalFilePath(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetNormalFilePath(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetPressedFilePath()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetPressedFilePath(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetPressedFilePath(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetPressedFilePath(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetDisabledFilePath()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetDisabledFilePath(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetDisabledFilePath(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetDisabledFilePath(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void EnableShadow(Color4B shadowColor, CocoStudio.Model.SizeF offset, int blurRadius)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableShadow__SWIG_0(this.swigCPtr, Color4B.getCPtr(shadowColor), Size.getCPtr(new Size(offset.Width, offset.Height)), blurRadius);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void EnableShadow(Color4B shadowColor, CocoStudio.Model.SizeF offset)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableShadow__SWIG_1(this.swigCPtr, Color4B.getCPtr(shadowColor), Size.getCPtr(new Size(offset.Width, offset.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void EnableShadow(Color4B shadowColor)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableShadow__SWIG_2(this.swigCPtr, Color4B.getCPtr(shadowColor));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void EnableShadow()
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableShadow__SWIG_3(this.swigCPtr);
		}

		public virtual void EnableOutline(Color4B outlineColor, int outlineSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableOutline(this.swigCPtr, Color4B.getCPtr(outlineColor), outlineSize);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void DisabledEffect()
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_DisabledEffect(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}

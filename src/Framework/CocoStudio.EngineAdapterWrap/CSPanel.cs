using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSPanel : CSWidget
	{
		public CSPanel(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSPanel_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSPanel obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSPanel()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSPanel(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSPanel(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSPanel() : this(CocoStudioEngineAdapterPINVOKE.new_CSPanel(), true)
		{
		}

		public override CocoStudio.Model.SizeF GetWidgetAutoSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetWidgetAutoSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new CocoStudio.Model.SizeF(size.width, size.height);
		}

		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetScale9Enabled(this.swigCPtr);
		}

		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		public virtual bool GetClipAble()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetClipAble(this.swigCPtr);
		}

		public virtual void SetClipAble(bool bclip)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetClipAble(this.swigCPtr, bclip);
		}

		public virtual int GetGroundAlpha()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundAlpha(this.swigCPtr);
		}

		public virtual void SetGroundAlpha(int alpha)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundAlpha(this.swigCPtr, alpha);
		}

		public virtual int GetGroundColorType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundColorType(this.swigCPtr);
		}

		public virtual void SetGroundColorType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundColorType(this.swigCPtr, iType);
		}

		public virtual Color GetGroundSingleColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundSingleColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public virtual void SetGroundSingleColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundSingleColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual Color GetGroundLineStartColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundLineStartColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public virtual void SetGroundLineStartColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundLineStartColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual Color GetGroundLineEndColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundLineEndColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public virtual void SetGroundLineEndColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundLineEndColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ScaleValue GetGroundColorVector()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundColorVector(this.swigCPtr);
			CSScale csscale = new CSScale(cPtr, false);
			return new ScaleValue(csscale.GetScaleX(), csscale.GetScaleY(), 0.1, -99999999.0, 99999999.0);
		}

		public virtual void SetGroundColorVector(ScaleValue cVector)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundColorVector(this.swigCPtr, CSScale.getCPtr(new CSScale(cVector.ScaleX, cVector.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetFilePath()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetFilePath(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetFilePath(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetFilePath(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual int GetContainerLayoutType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetContainerLayoutType(this.swigCPtr);
		}

		public virtual void SetContainerLayoutType(int itype)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetContainerLayoutType(this.swigCPtr, itype);
		}

		public virtual void RemoveBackGroundFile()
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_RemoveBackGroundFile(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}

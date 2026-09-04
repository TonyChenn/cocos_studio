using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200003A RID: 58
	public class CSPanel : CSWidget
	{
		// Token: 0x06000900 RID: 2304 RVA: 0x0000B4BB File Offset: 0x000096BB
		public CSPanel(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSPanel_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0000B4DC File Offset: 0x000096DC
		public static HandleRef getCPtr(CSPanel obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0000B508 File Offset: 0x00009708
		~CSPanel()
		{
			this.Dispose();
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0000B56C File Offset: 0x0000976C
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

		// Token: 0x06000904 RID: 2308 RVA: 0x0000B66C File Offset: 0x0000986C
		public CSPanel() : this(CocoStudioEngineAdapterPINVOKE.new_CSPanel(), true)
		{
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0000B680 File Offset: 0x00009880
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

		// Token: 0x06000906 RID: 2310 RVA: 0x0000B6FC File Offset: 0x000098FC
		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetScale9Enabled(this.swigCPtr);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0000B71B File Offset: 0x0000991B
		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0000B72B File Offset: 0x0000992B
		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0000B740 File Offset: 0x00009940
		public virtual bool GetClipAble()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetClipAble(this.swigCPtr);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0000B75F File Offset: 0x0000995F
		public virtual void SetClipAble(bool bclip)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetClipAble(this.swigCPtr, bclip);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0000B770 File Offset: 0x00009970
		public virtual int GetGroundAlpha()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundAlpha(this.swigCPtr);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0000B78F File Offset: 0x0000998F
		public virtual void SetGroundAlpha(int alpha)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundAlpha(this.swigCPtr, alpha);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public virtual int GetGroundColorType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundColorType(this.swigCPtr);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0000B7BF File Offset: 0x000099BF
		public virtual void SetGroundColorType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundColorType(this.swigCPtr, iType);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0000B7D0 File Offset: 0x000099D0
		public virtual Color GetGroundSingleColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundSingleColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0000B810 File Offset: 0x00009A10
		public virtual void SetGroundSingleColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundSingleColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0000B85C File Offset: 0x00009A5C
		public virtual Color GetGroundLineStartColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundLineStartColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0000B89C File Offset: 0x00009A9C
		public virtual void SetGroundLineStartColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundLineStartColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0000B8E8 File Offset: 0x00009AE8
		public virtual Color GetGroundLineEndColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundLineEndColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0000B928 File Offset: 0x00009B28
		public virtual void SetGroundLineEndColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundLineEndColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0000B974 File Offset: 0x00009B74
		public virtual ScaleValue GetGroundColorVector()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetGroundColorVector(this.swigCPtr);
			CSScale csscale = new CSScale(cPtr, false);
			return new ScaleValue(csscale.GetScaleX(), csscale.GetScaleY(), 0.1, -99999999.0, 99999999.0);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		public virtual void SetGroundColorVector(ScaleValue cVector)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetGroundColorVector(this.swigCPtr, CSScale.getCPtr(new CSScale(cVector.ScaleX, cVector.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0000BA0C File Offset: 0x00009C0C
		public virtual ResourceData GetFilePath()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSPanel_GetFilePath(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0000BA4C File Offset: 0x00009C4C
		public virtual void SetFilePath(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetFilePath(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0000BA94 File Offset: 0x00009C94
		public virtual int GetContainerLayoutType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSPanel_GetContainerLayoutType(this.swigCPtr);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0000BAB3 File Offset: 0x00009CB3
		public virtual void SetContainerLayoutType(int itype)
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_SetContainerLayoutType(this.swigCPtr, itype);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0000BAC3 File Offset: 0x00009CC3
		public virtual void RemoveBackGroundFile()
		{
			CocoStudioEngineAdapterPINVOKE.CSPanel_RemoveBackGroundFile(this.swigCPtr);
		}

		// Token: 0x04000064 RID: 100
		private HandleRef swigCPtr;
	}
}

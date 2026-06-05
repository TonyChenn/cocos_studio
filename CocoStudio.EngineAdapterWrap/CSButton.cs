using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200002F RID: 47
	public class CSButton : CSWidget
	{
		// Token: 0x0600085A RID: 2138 RVA: 0x00008CBF File Offset: 0x00006EBF
		public CSButton(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSButton_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00008CE0 File Offset: 0x00006EE0
		public static HandleRef getCPtr(CSButton obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00008D0C File Offset: 0x00006F0C
		~CSButton()
		{
			this.Dispose();
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00008D70 File Offset: 0x00006F70
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

		// Token: 0x0600085E RID: 2142 RVA: 0x00008E70 File Offset: 0x00007070
		public CSButton() : this(CocoStudioEngineAdapterPINVOKE.new_CSButton(), true)
		{
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00008E84 File Offset: 0x00007084
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

		// Token: 0x06000860 RID: 2144 RVA: 0x00008EFE File Offset: 0x000070FE
		public override void SetCustomSizeEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetCustomSizeEnabled(this.swigCPtr, bEnabled);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00008F10 File Offset: 0x00007110
		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetScale9Enabled(this.swigCPtr);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00008F2F File Offset: 0x0000712F
		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00008F3F File Offset: 0x0000713F
		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00008F54 File Offset: 0x00007154
		public virtual bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetFlipX(this.swigCPtr);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00008F73 File Offset: 0x00007173
		public virtual void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetFlipX(this.swigCPtr, flip);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00008F84 File Offset: 0x00007184
		public virtual bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetFlipY(this.swigCPtr);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00008FA3 File Offset: 0x000071A3
		public virtual void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetFlipY(this.swigCPtr, flip);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00008FB4 File Offset: 0x000071B4
		public virtual string GetText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetText(this.swigCPtr);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00008FD4 File Offset: 0x000071D4
		public virtual void SetText(string sText)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetText(this.swigCPtr, sText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00009004 File Offset: 0x00007204
		public virtual string GetFontName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetFontName(this.swigCPtr);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00009024 File Offset: 0x00007224
		public virtual void SetFontName(string sName)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetFontName(this.swigCPtr, sName);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00009054 File Offset: 0x00007254
		public virtual int GetFontSize()
		{
			return CocoStudioEngineAdapterPINVOKE.CSButton_GetFontSize(this.swigCPtr);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00009073 File Offset: 0x00007273
		public virtual void SetFontSize(int sSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetFontSize(this.swigCPtr, sSize);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00009084 File Offset: 0x00007284
		public virtual Color GetTextColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetTextColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000090C4 File Offset: 0x000072C4
		public virtual void SetTextColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetTextColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00009110 File Offset: 0x00007310
		public virtual ResourceData GetNormalFilePath()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetNormalFilePath(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00009150 File Offset: 0x00007350
		public virtual void SetNormalFilePath(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetNormalFilePath(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00009198 File Offset: 0x00007398
		public virtual ResourceData GetPressedFilePath()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetPressedFilePath(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000091D8 File Offset: 0x000073D8
		public virtual void SetPressedFilePath(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetPressedFilePath(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00009220 File Offset: 0x00007420
		public virtual ResourceData GetDisabledFilePath()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSButton_GetDisabledFilePath(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00009260 File Offset: 0x00007460
		public virtual void SetDisabledFilePath(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_SetDisabledFilePath(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000092A8 File Offset: 0x000074A8
		public virtual void EnableShadow(Color4B shadowColor, CocoStudio.Model.SizeF offset, int blurRadius)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableShadow__SWIG_0(this.swigCPtr, Color4B.getCPtr(shadowColor), Size.getCPtr(new Size(offset.Width, offset.Height)), blurRadius);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000092F4 File Offset: 0x000074F4
		public virtual void EnableShadow(Color4B shadowColor, CocoStudio.Model.SizeF offset)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableShadow__SWIG_1(this.swigCPtr, Color4B.getCPtr(shadowColor), Size.getCPtr(new Size(offset.Width, offset.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0000933C File Offset: 0x0000753C
		public virtual void EnableShadow(Color4B shadowColor)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableShadow__SWIG_2(this.swigCPtr, Color4B.getCPtr(shadowColor));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0000936E File Offset: 0x0000756E
		public virtual void EnableShadow()
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableShadow__SWIG_3(this.swigCPtr);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00009380 File Offset: 0x00007580
		public virtual void EnableOutline(Color4B outlineColor, int outlineSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_EnableOutline(this.swigCPtr, Color4B.getCPtr(outlineColor), outlineSize);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000093B3 File Offset: 0x000075B3
		public virtual void DisabledEffect()
		{
			CocoStudioEngineAdapterPINVOKE.CSButton_DisabledEffect(this.swigCPtr);
		}

		// Token: 0x04000050 RID: 80
		private HandleRef swigCPtr;
	}
}

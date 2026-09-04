using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000059 RID: 89
	public class CSTextField : CSWidget
	{
		// Token: 0x06000A87 RID: 2695 RVA: 0x00010C74 File Offset: 0x0000EE74
		public CSTextField(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTextField_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00010C94 File Offset: 0x0000EE94
		public static HandleRef getCPtr(CSTextField obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00010CC0 File Offset: 0x0000EEC0
		~CSTextField()
		{
			this.Dispose();
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00010D24 File Offset: 0x0000EF24
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
								CocoStudioEngineAdapterPINVOKE.delete_CSTextField(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSTextField(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00010E24 File Offset: 0x0000F024
		public CSTextField() : this(CocoStudioEngineAdapterPINVOKE.new_CSTextField(), true)
		{
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00010E35 File Offset: 0x0000F035
		public override void SetCustomSizeEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetCustomSizeEnabled(this.swigCPtr, bEnabled);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00010E48 File Offset: 0x0000F048
		public virtual string GetFontName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetFontName(this.swigCPtr);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00010E67 File Offset: 0x0000F067
		public virtual void SetFontName(string sName)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetFontName(this.swigCPtr, sName);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00010E78 File Offset: 0x0000F078
		public virtual int GetFontSize()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetFontSize(this.swigCPtr);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00010E97 File Offset: 0x0000F097
		public virtual void SetFontSize(int iSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetFontSize(this.swigCPtr, iSize);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00010EA8 File Offset: 0x0000F0A8
		public virtual string GetLabelText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetLabelText(this.swigCPtr);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00010EC8 File Offset: 0x0000F0C8
		public virtual void SetLabelText(string sText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetLabelText(this.swigCPtr, sText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00010EF8 File Offset: 0x0000F0F8
		public virtual string GetPlaceHolderText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetPlaceHolderText(this.swigCPtr);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00010F18 File Offset: 0x0000F118
		public virtual void SetPlaceHolderText(string pText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetPlaceHolderText(this.swigCPtr, pText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00010F48 File Offset: 0x0000F148
		public virtual Color GetPlaceHolderTextColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSTextField_GetPlaceHolderTextColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00010F88 File Offset: 0x0000F188
		public virtual void SetPlaceHolderTextColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetPlaceHolderTextColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00010FD4 File Offset: 0x0000F1D4
		public virtual bool GetPassWordEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetPassWordEnabled(this.swigCPtr);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00010FF3 File Offset: 0x0000F1F3
		public virtual void SetPassWordEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetPassWordEnabled(this.swigCPtr, bEnabled);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00011004 File Offset: 0x0000F204
		public virtual string GetPasswordStyleText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetPasswordStyleText(this.swigCPtr);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00011023 File Offset: 0x0000F223
		public virtual void SetPasswordStyleText(string pText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetPasswordStyleText(this.swigCPtr, pText);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00011034 File Offset: 0x0000F234
		public virtual bool GetLengthLimited()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetLengthLimited(this.swigCPtr);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00011053 File Offset: 0x0000F253
		public virtual void SetLengthLimited(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetLengthLimited(this.swigCPtr, bEnabled);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00011064 File Offset: 0x0000F264
		public virtual int GetMaxLength()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetMaxLength(this.swigCPtr);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00011083 File Offset: 0x0000F283
		public virtual void SetMaxLength(int iLength)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetMaxLength(this.swigCPtr, iLength);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00011094 File Offset: 0x0000F294
		public override CocoStudio.Model.SizeF GetWidgetAutoSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSTextField_GetWidgetAutoSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new CocoStudio.Model.SizeF(size.width, size.height);
		}

		// Token: 0x0400009D RID: 157
		private HandleRef swigCPtr;
	}
}

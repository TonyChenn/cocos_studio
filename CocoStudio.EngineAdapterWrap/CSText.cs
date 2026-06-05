using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000056 RID: 86
	public class CSText : CSWidget
	{
		// Token: 0x06000A4E RID: 2638 RVA: 0x00010094 File Offset: 0x0000E294
		public CSText(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSText_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000100B4 File Offset: 0x0000E2B4
		public static HandleRef getCPtr(CSText obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000100E0 File Offset: 0x0000E2E0
		~CSText()
		{
			this.Dispose();
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00010144 File Offset: 0x0000E344
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
								CocoStudioEngineAdapterPINVOKE.delete_CSText(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSText(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00010244 File Offset: 0x0000E444
		public CSText() : this(CocoStudioEngineAdapterPINVOKE.new_CSText(), true)
		{
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00010258 File Offset: 0x0000E458
		public override SizeF GetWidgetAutoSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSText_GetWidgetAutoSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000102D2 File Offset: 0x0000E4D2
		public override void SetCustomSizeEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetCustomSizeEnabled(this.swigCPtr, bEnabled);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000102E4 File Offset: 0x0000E4E4
		public virtual bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetFlipX(this.swigCPtr);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00010303 File Offset: 0x0000E503
		public virtual void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetFlipX(this.swigCPtr, flip);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00010314 File Offset: 0x0000E514
		public virtual bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetFlipY(this.swigCPtr);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00010333 File Offset: 0x0000E533
		public virtual void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetFlipY(this.swigCPtr, flip);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00010344 File Offset: 0x0000E544
		public virtual string GetFontName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetFontName(this.swigCPtr);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00010363 File Offset: 0x0000E563
		public virtual void SetFontName(string sName)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetFontName(this.swigCPtr, sName);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00010374 File Offset: 0x0000E574
		public virtual int GetFontSize()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetFontSize(this.swigCPtr);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00010393 File Offset: 0x0000E593
		public virtual void SetFontSize(int iSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetFontSize(this.swigCPtr, iSize);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x000103A4 File Offset: 0x0000E5A4
		public virtual string GetLabelText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetLabelText(this.swigCPtr);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000103C4 File Offset: 0x0000E5C4
		public virtual void SetLabelText(string sText)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetLabelText(this.swigCPtr, sText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000103F4 File Offset: 0x0000E5F4
		public virtual int GetHorizontalAlignmentType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetHorizontalAlignmentType(this.swigCPtr);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00010413 File Offset: 0x0000E613
		public virtual void SetHorizontalAlignmentType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetHorizontalAlignmentType(this.swigCPtr, iType);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00010424 File Offset: 0x0000E624
		public virtual int GetVerticalAlignmentType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetVerticalAlignmentType(this.swigCPtr);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00010443 File Offset: 0x0000E643
		public virtual void SetVerticalAlignmentType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetVerticalAlignmentType(this.swigCPtr, iType);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00010454 File Offset: 0x0000E654
		public virtual bool GetTouchScaleChangeEanbleState()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetTouchScaleChangeEanbleState(this.swigCPtr);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00010473 File Offset: 0x0000E673
		public virtual void SetTouchScaleChangeEanbleState(bool bState)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetTouchScaleChangeEanbleState(this.swigCPtr, bState);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00010484 File Offset: 0x0000E684
		public virtual void EnableShadow(Color4B shadowColor, SizeF offset, int blurRadius)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableShadow__SWIG_0(this.swigCPtr, Color4B.getCPtr(shadowColor), Size.getCPtr(new Size(offset.Width, offset.Height)), blurRadius);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000104D0 File Offset: 0x0000E6D0
		public virtual void EnableShadow(Color4B shadowColor, SizeF offset)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableShadow__SWIG_1(this.swigCPtr, Color4B.getCPtr(shadowColor), Size.getCPtr(new Size(offset.Width, offset.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00010518 File Offset: 0x0000E718
		public virtual void EnableShadow(Color4B shadowColor)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableShadow__SWIG_2(this.swigCPtr, Color4B.getCPtr(shadowColor));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0001054C File Offset: 0x0000E74C
		public virtual void EnableShadow()
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableShadow__SWIG_3(this.swigCPtr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00010578 File Offset: 0x0000E778
		public virtual void EnableOutline(Color4B outlineColor, int outlineSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableOutline(this.swigCPtr, Color4B.getCPtr(outlineColor), outlineSize);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x000105AC File Offset: 0x0000E7AC
		public virtual void DisableEffect()
		{
			CocoStudioEngineAdapterPINVOKE.CSText_DisableEffect(this.swigCPtr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000105D8 File Offset: 0x0000E7D8
		public virtual void DisableShadow()
		{
			CocoStudioEngineAdapterPINVOKE.CSText_DisableShadow(this.swigCPtr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00010604 File Offset: 0x0000E804
		public virtual void DisableOutline()
		{
			CocoStudioEngineAdapterPINVOKE.CSText_DisableOutline(this.swigCPtr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00010630 File Offset: 0x0000E830
		public virtual void SetTextColor(Color4B color)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetTextColor(this.swigCPtr, Color4B.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00010664 File Offset: 0x0000E864
		public virtual Color4B GetTextColor()
		{
			return new Color4B(CocoStudioEngineAdapterPINVOKE.CSText_GetTextColor(this.swigCPtr), false);
		}

		// Token: 0x0400009A RID: 154
		private HandleRef swigCPtr;
	}
}

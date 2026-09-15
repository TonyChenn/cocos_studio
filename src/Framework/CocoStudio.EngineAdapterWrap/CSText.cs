using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSText : CSWidget
	{
		public CSText(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSText_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSText obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSText()
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

		public CSText() : this(CocoStudioEngineAdapterPINVOKE.new_CSText(), true)
		{
		}

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

		public override void SetCustomSizeEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetCustomSizeEnabled(this.swigCPtr, bEnabled);
		}

		public virtual bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetFlipX(this.swigCPtr);
		}

		public virtual void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetFlipX(this.swigCPtr, flip);
		}

		public virtual bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetFlipY(this.swigCPtr);
		}

		public virtual void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetFlipY(this.swigCPtr, flip);
		}

		public virtual string GetFontName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetFontName(this.swigCPtr);
		}

		public virtual void SetFontName(string sName)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetFontName(this.swigCPtr, sName);
		}

		public virtual int GetFontSize()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetFontSize(this.swigCPtr);
		}

		public virtual void SetFontSize(int iSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetFontSize(this.swigCPtr, iSize);
		}

		public virtual string GetLabelText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetLabelText(this.swigCPtr);
		}

		public virtual void SetLabelText(string sText)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetLabelText(this.swigCPtr, sText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual int GetHorizontalAlignmentType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetHorizontalAlignmentType(this.swigCPtr);
		}

		public virtual void SetHorizontalAlignmentType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetHorizontalAlignmentType(this.swigCPtr, iType);
		}

		public virtual int GetVerticalAlignmentType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetVerticalAlignmentType(this.swigCPtr);
		}

		public virtual void SetVerticalAlignmentType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetVerticalAlignmentType(this.swigCPtr, iType);
		}

		public virtual bool GetTouchScaleChangeEanbleState()
		{
			return CocoStudioEngineAdapterPINVOKE.CSText_GetTouchScaleChangeEanbleState(this.swigCPtr);
		}

		public virtual void SetTouchScaleChangeEanbleState(bool bState)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetTouchScaleChangeEanbleState(this.swigCPtr, bState);
		}

		public virtual void EnableShadow(Color4B shadowColor, SizeF offset, int blurRadius)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableShadow__SWIG_0(this.swigCPtr, Color4B.getCPtr(shadowColor), Size.getCPtr(new Size(offset.Width, offset.Height)), blurRadius);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void EnableShadow(Color4B shadowColor, SizeF offset)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableShadow__SWIG_1(this.swigCPtr, Color4B.getCPtr(shadowColor), Size.getCPtr(new Size(offset.Width, offset.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void EnableShadow(Color4B shadowColor)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableShadow__SWIG_2(this.swigCPtr, Color4B.getCPtr(shadowColor));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void EnableShadow()
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableShadow__SWIG_3(this.swigCPtr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void EnableOutline(Color4B outlineColor, int outlineSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_EnableOutline(this.swigCPtr, Color4B.getCPtr(outlineColor), outlineSize);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void DisableEffect()
		{
			CocoStudioEngineAdapterPINVOKE.CSText_DisableEffect(this.swigCPtr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void DisableShadow()
		{
			CocoStudioEngineAdapterPINVOKE.CSText_DisableShadow(this.swigCPtr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void DisableOutline()
		{
			CocoStudioEngineAdapterPINVOKE.CSText_DisableOutline(this.swigCPtr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void SetTextColor(Color4B color)
		{
			CocoStudioEngineAdapterPINVOKE.CSText_SetTextColor(this.swigCPtr, Color4B.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual Color4B GetTextColor()
		{
			return new Color4B(CocoStudioEngineAdapterPINVOKE.CSText_GetTextColor(this.swigCPtr), false);
		}

		private HandleRef swigCPtr;
	}
}

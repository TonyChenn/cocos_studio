using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSTextField : CSWidget
	{
		public CSTextField(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTextField_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSTextField obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSTextField()
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

		public CSTextField() : this(CocoStudioEngineAdapterPINVOKE.new_CSTextField(), true)
		{
		}

		public override void SetCustomSizeEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetCustomSizeEnabled(this.swigCPtr, bEnabled);
		}

		public virtual string GetFontName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetFontName(this.swigCPtr);
		}

		public virtual void SetFontName(string sName)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetFontName(this.swigCPtr, sName);
		}

		public virtual int GetFontSize()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetFontSize(this.swigCPtr);
		}

		public virtual void SetFontSize(int iSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetFontSize(this.swigCPtr, iSize);
		}

		public virtual string GetLabelText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetLabelText(this.swigCPtr);
		}

		public virtual void SetLabelText(string sText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetLabelText(this.swigCPtr, sText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual string GetPlaceHolderText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetPlaceHolderText(this.swigCPtr);
		}

		public virtual void SetPlaceHolderText(string pText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetPlaceHolderText(this.swigCPtr, pText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual Color GetPlaceHolderTextColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSTextField_GetPlaceHolderTextColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public virtual void SetPlaceHolderTextColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetPlaceHolderTextColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual bool GetPassWordEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetPassWordEnabled(this.swigCPtr);
		}

		public virtual void SetPassWordEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetPassWordEnabled(this.swigCPtr, bEnabled);
		}

		public virtual string GetPasswordStyleText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetPasswordStyleText(this.swigCPtr);
		}

		public virtual void SetPasswordStyleText(string pText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetPasswordStyleText(this.swigCPtr, pText);
		}

		public virtual bool GetLengthLimited()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetLengthLimited(this.swigCPtr);
		}

		public virtual void SetLengthLimited(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetLengthLimited(this.swigCPtr, bEnabled);
		}

		public virtual int GetMaxLength()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextField_GetMaxLength(this.swigCPtr);
		}

		public virtual void SetMaxLength(int iLength)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextField_SetMaxLength(this.swigCPtr, iLength);
		}

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

		private HandleRef swigCPtr;
	}
}

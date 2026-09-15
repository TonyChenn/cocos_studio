using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSWidget : CSNode2D
	{
		public CSWidget(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSWidget_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSWidget obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSWidget()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSWidget(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSWidget(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSWidget() : this(CocoStudioEngineAdapterPINVOKE.new_CSWidget(), true)
		{
		}

		public virtual bool GetCustomSizeEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetCustomSizeEnabled(this.swigCPtr);
		}

		public virtual void SetCustomSizeEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetCustomSizeEnabled(this.swigCPtr, bEnabled);
		}

		public virtual SizeF GetWidgetAutoSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSWidget_GetWidgetAutoSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		public virtual bool GetTouchEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetTouchEnabled(this.swigCPtr);
		}

		public virtual void SetTouchEnabled(bool select)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetTouchEnabled(this.swigCPtr, select);
		}

		public virtual int GetScale9Left()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Left(this.swigCPtr);
		}

		public virtual void SetScale9Left(int left)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Left(this.swigCPtr, left);
		}

		public virtual int GetScale9Right()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Right(this.swigCPtr);
		}

		public virtual void SetScale9Right(int right)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Right(this.swigCPtr, right);
		}

		public virtual int GetScale9Top()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Top(this.swigCPtr);
		}

		public virtual void SetScale9Top(int top)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Top(this.swigCPtr, top);
		}

		public virtual int GetScale9Bottom()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Bottom(this.swigCPtr);
		}

		public virtual void SetScale9Bottom(int bottom)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Bottom(this.swigCPtr, bottom);
		}

		public virtual int GetScale9OriginX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9OriginX(this.swigCPtr);
		}

		public virtual int GetScale9OriginY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9OriginY(this.swigCPtr);
		}

		public virtual int GetScale9Width()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Width(this.swigCPtr);
		}

		public virtual int GetScale9Height()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Height(this.swigCPtr);
		}

		public virtual bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Enabled(this.swigCPtr);
		}

		public virtual void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		public virtual void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		public virtual void CloneWidgetCustomProperty(CSWidget cWidget)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_CloneWidgetCustomProperty(this.swigCPtr, CSWidget.getCPtr(cWidget));
		}

		public virtual void ChangeState(bool isNormal)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_ChangeState(this.swigCPtr, isNormal);
		}

		public virtual void ResetState()
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_ResetState(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}

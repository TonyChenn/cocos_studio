using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSPageView : CSPanel
	{
		public CSPageView(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSPageView_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSPageView obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSPageView()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSPageView(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSPageView(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSPageView() : this(CocoStudioEngineAdapterPINVOKE.new_CSPageView(), true)
		{
		}

		public override void AddChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSPageView_AddChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		public override void RemoveChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSPageView_RemoveChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		public override void InsertChild(int index, CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSPageView_InsertChild(this.swigCPtr, index, CSVisualObject.getCPtr(child));
		}

		public override void SetSize(SizeF cPoint)
		{
			CocoStudioEngineAdapterPINVOKE.CSPageView_SetSize(this.swigCPtr, Size.getCPtr(new Size(cPoint.Width, cPoint.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}

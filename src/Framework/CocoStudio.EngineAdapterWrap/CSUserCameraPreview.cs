using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSUserCameraPreview : IDisposable
	{
		public CSUserCameraPreview(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSUserCameraPreview obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSUserCameraPreview()
		{
			this.Dispose();
		}

		public virtual void Dispose()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSUserCameraPreview(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSUserCameraPreview(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public CSUserCameraPreview() : this(CocoStudioEngineAdapterPINVOKE.new_CSUserCameraPreview(), true)
		{
		}

		public bool IsKeep()
		{
			return CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_IsKeep(this.swigCPtr);
		}

		public void SetCamera(CSCamera userCamera)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_SetCamera(this.swigCPtr, CSCamera.getCPtr(userCamera));
		}

		public void Render(bool start)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_Render(this.swigCPtr, start);
		}

		public void OnPreviewChanged(SizeF size)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_OnPreviewChanged(this.swigCPtr, Size.getCPtr(new Size(size.Width, size.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void SetVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_SetVisible(this.swigCPtr, visible);
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200005E RID: 94
	public class CSUserCameraPreview : IDisposable
	{
		// Token: 0x06000AE1 RID: 2785 RVA: 0x00011C83 File Offset: 0x0000FE83
		public CSUserCameraPreview(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00011CA4 File Offset: 0x0000FEA4
		public static HandleRef getCPtr(CSUserCameraPreview obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00011CD0 File Offset: 0x0000FED0
		~CSUserCameraPreview()
		{
			this.Dispose();
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00011D34 File Offset: 0x0000FF34
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

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00011E2C File Offset: 0x0001002C
		public CSUserCameraPreview() : this(CocoStudioEngineAdapterPINVOKE.new_CSUserCameraPreview(), true)
		{
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00011E40 File Offset: 0x00010040
		public bool IsKeep()
		{
			return CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_IsKeep(this.swigCPtr);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00011E5F File Offset: 0x0001005F
		public void SetCamera(CSCamera userCamera)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_SetCamera(this.swigCPtr, CSCamera.getCPtr(userCamera));
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00011E74 File Offset: 0x00010074
		public void Render(bool start)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_Render(this.swigCPtr, start);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00011E84 File Offset: 0x00010084
		public void OnPreviewChanged(SizeF size)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_OnPreviewChanged(this.swigCPtr, Size.getCPtr(new Size(size.Width, size.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00011EC6 File Offset: 0x000100C6
		public void SetVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSUserCameraPreview_SetVisible(this.swigCPtr, visible);
		}

		// Token: 0x040000A3 RID: 163
		private HandleRef swigCPtr;

		// Token: 0x040000A4 RID: 164
		protected bool swigCMemOwn;
	}
}

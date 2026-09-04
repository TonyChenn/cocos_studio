using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200006B RID: 107
	public class CSWindow : IDisposable
	{
		// Token: 0x06000C0F RID: 3087 RVA: 0x00015934 File Offset: 0x00013B34
		public CSWindow(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00015954 File Offset: 0x00013B54
		public static HandleRef getCPtr(CSWindow obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00015980 File Offset: 0x00013B80
		~CSWindow()
		{
			this.Dispose();
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000159E4 File Offset: 0x00013BE4
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
								CocoStudioEngineAdapterPINVOKE.delete_CSWindow(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSWindow(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00015ADC File Offset: 0x00013CDC
		public CSWindow(int windowHandle, int width, int heigth, string localPath, int multi) : this(CocoStudioEngineAdapterPINVOKE.new_CSWindow__SWIG_0(windowHandle, width, heigth, localPath, multi), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00015B14 File Offset: 0x00013D14
		public CSWindow(IntPtr windowHandle, IntPtr nsview, int width, int height, string localpath, int multi) : this(CocoStudioEngineAdapterPINVOKE.new_CSWindow__SWIG_1(windowHandle, nsview, width, height, localpath, multi), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00015B4C File Offset: 0x00013D4C
		public void Draw(int writeableBitmapPointer)
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_Draw(this.swigCPtr, writeableBitmapPointer);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00015B79 File Offset: 0x00013D79
		public void SetViewRect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_SetViewRect(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00015B8D File Offset: 0x00013D8D
		public void Close()
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_Close(this.swigCPtr);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00015B9C File Offset: 0x00013D9C
		public CSCanvas GetCanvas()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSWindow_GetCanvas(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSCanvas(intPtr, false);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00015BD4 File Offset: 0x00013DD4
		public CSScene GetScene()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSWindow_GetScene(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSScene(intPtr, false);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00015C0C File Offset: 0x00013E0C
		public void UpdateOpenGLContext(bool isShowing, int windowHandle)
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_UpdateOpenGLContext(this.swigCPtr, isShowing, windowHandle);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00015C1D File Offset: 0x00013E1D
		public void SetSceneMode(bool is2D)
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_SetSceneMode(this.swigCPtr, is2D);
		}

		// Token: 0x040000C9 RID: 201
		private HandleRef swigCPtr;

		// Token: 0x040000CA RID: 202
		protected bool swigCMemOwn;
	}
}

using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSWindow : IDisposable
	{
		public CSWindow(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSWindow obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSWindow()
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

		public CSWindow(int windowHandle, int width, int heigth, string localPath, int multi) : this(CocoStudioEngineAdapterPINVOKE.new_CSWindow__SWIG_0(windowHandle, width, heigth, localPath, multi), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CSWindow(IntPtr windowHandle, IntPtr nsview, int width, int height, string localpath, int multi) : this(CocoStudioEngineAdapterPINVOKE.new_CSWindow__SWIG_1(windowHandle, nsview, width, height, localpath, multi), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Draw(int writeableBitmapPointer)
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_Draw(this.swigCPtr, writeableBitmapPointer);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void SetViewRect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_SetViewRect(this.swigCPtr, x, y, width, height);
		}

		public void Close()
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_Close(this.swigCPtr);
		}

		public CSCanvas GetCanvas()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSWindow_GetCanvas(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSCanvas(intPtr, false);
		}

		public CSScene GetScene()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSWindow_GetScene(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSScene(intPtr, false);
		}

		public void UpdateOpenGLContext(bool isShowing, int windowHandle)
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_UpdateOpenGLContext(this.swigCPtr, isShowing, windowHandle);
		}

		public void SetSceneMode(bool is2D)
		{
			CocoStudioEngineAdapterPINVOKE.CSWindow_SetSceneMode(this.swigCPtr, is2D);
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}

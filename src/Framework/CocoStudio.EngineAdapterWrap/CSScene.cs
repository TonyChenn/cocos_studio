using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSScene : CSVisualObject
	{
		public CSScene(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSScene_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSScene obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSScene()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSScene(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSScene(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public void ChangeMode(bool b2D)
		{
			CocoStudioEngineAdapterPINVOKE.CSScene_ChangeMode(this.swigCPtr, b2D);
		}

		public CSSceneCamera GetCamera()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSScene_GetCamera(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSSceneCamera(intPtr, false);
		}

		public void OnViewSizeChange(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSScene_OnViewSizeChange(this.swigCPtr, x, y, width, height);
		}

		private HandleRef swigCPtr;
	}
}

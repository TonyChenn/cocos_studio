using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSExtensionFrame : CSTimelineFrame
	{
		public CSExtensionFrame(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSExtensionFrame_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSExtensionFrame obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSExtensionFrame()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSExtensionFrame(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSExtensionFrame(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSExtensionFrame(CSExtensionFrame csFrame) : this(CocoStudioEngineAdapterPINVOKE.new_CSExtensionFrame__SWIG_0(CSExtensionFrame.getCPtr(csFrame)), true)
		{
		}

		public CSExtensionFrame() : this(CocoStudioEngineAdapterPINVOKE.new_CSExtensionFrame__SWIG_1(), true)
		{
		}

		public virtual void SetFrameEnterCallBack(CSExtensionFrame.FrameEnterCallBack callback)
		{
			CocoStudioEngineAdapterPINVOKE.CSExtensionFrame_SetFrameEnterCallBack(this.swigCPtr, callback);
		}

		public virtual void SetFrameApplyCallBack(CSExtensionFrame.FrameApplyCallBack callback)
		{
			CocoStudioEngineAdapterPINVOKE.CSExtensionFrame_SetFrameApplyCallBack(this.swigCPtr, callback);
		}

		private HandleRef swigCPtr;

		public delegate void FrameEnterCallBack(int nextFrameIndex);

		public delegate void FrameApplyCallBack(float percent);
	}
}

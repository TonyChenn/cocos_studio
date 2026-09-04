using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000005 RID: 5
	public class CSExtensionFrame : CSTimelineFrame
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002CFA File Offset: 0x00000EFA
		public CSExtensionFrame(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSExtensionFrame_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002D1C File Offset: 0x00000F1C
		public static HandleRef getCPtr(CSExtensionFrame obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002D48 File Offset: 0x00000F48
		~CSExtensionFrame()
		{
			this.Dispose();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002DAC File Offset: 0x00000FAC
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

		// Token: 0x06000042 RID: 66 RVA: 0x00002EAC File Offset: 0x000010AC
		public CSExtensionFrame(CSExtensionFrame csFrame) : this(CocoStudioEngineAdapterPINVOKE.new_CSExtensionFrame__SWIG_0(CSExtensionFrame.getCPtr(csFrame)), true)
		{
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002EC3 File Offset: 0x000010C3
		public CSExtensionFrame() : this(CocoStudioEngineAdapterPINVOKE.new_CSExtensionFrame__SWIG_1(), true)
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002ED4 File Offset: 0x000010D4
		public virtual void SetFrameEnterCallBack(CSExtensionFrame.FrameEnterCallBack callback)
		{
			CocoStudioEngineAdapterPINVOKE.CSExtensionFrame_SetFrameEnterCallBack(this.swigCPtr, callback);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002EE4 File Offset: 0x000010E4
		public virtual void SetFrameApplyCallBack(CSExtensionFrame.FrameApplyCallBack callback)
		{
			CocoStudioEngineAdapterPINVOKE.CSExtensionFrame_SetFrameApplyCallBack(this.swigCPtr, callback);
		}

		// Token: 0x04000006 RID: 6
		private HandleRef swigCPtr;

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000047 RID: 71
		public delegate void FrameEnterCallBack(int nextFrameIndex);

		// Token: 0x02000007 RID: 7
		// (Invoke) Token: 0x0600004B RID: 75
		public delegate void FrameApplyCallBack(float percent);
	}
}

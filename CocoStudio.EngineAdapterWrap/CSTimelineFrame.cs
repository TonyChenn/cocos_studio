using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000004 RID: 4
	public class CSTimelineFrame : CSObject
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002A38 File Offset: 0x00000C38
		public CSTimelineFrame(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A58 File Offset: 0x00000C58
		public static HandleRef getCPtr(CSTimelineFrame obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002A84 File Offset: 0x00000C84
		~CSTimelineFrame()
		{
			this.Dispose();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002AE8 File Offset: 0x00000CE8
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
								CocoStudioEngineAdapterPINVOKE.delete_CSTimelineFrame(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSTimelineFrame(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public CSTimelineFrame() : this(CocoStudioEngineAdapterPINVOKE.new_CSTimelineFrame(), true)
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002BFC File Offset: 0x00000DFC
		public virtual int GetFrameIndex()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_GetFrameIndex(this.swigCPtr);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C1B File Offset: 0x00000E1B
		public virtual void SetFrameIndex(int iIndex)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SetFrameIndex(this.swigCPtr, iIndex);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002C2B File Offset: 0x00000E2B
		public virtual void SetTween(bool tween)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SetTween(this.swigCPtr, tween);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C3C File Offset: 0x00000E3C
		public virtual bool IsTween()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_IsTween(this.swigCPtr);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002C5B File Offset: 0x00000E5B
		public virtual void SetTweenType(int tweenType)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SetTweenType(this.swigCPtr, tweenType);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C6C File Offset: 0x00000E6C
		public virtual int GetTweenType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_GetTweenType(this.swigCPtr);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002C8C File Offset: 0x00000E8C
		public virtual void SetEasingParam(CSVectorFloat tweenParma)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SetEasingParam(this.swigCPtr, CSVectorFloat.getCPtr(tweenParma));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public virtual CSVectorFloat GetEasingParam()
		{
			return new CSVectorFloat(CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_GetEasingParam(this.swigCPtr), true);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002CE5 File Offset: 0x00000EE5
		public virtual void OnEnter(CSTimelineFrame nextFrame)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_OnEnter(this.swigCPtr, CSTimelineFrame.getCPtr(nextFrame));
		}

		// Token: 0x04000005 RID: 5
		private HandleRef swigCPtr;
	}
}

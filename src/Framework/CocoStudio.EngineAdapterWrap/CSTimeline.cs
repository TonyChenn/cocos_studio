using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200005A RID: 90
	public class CSTimeline : CSObject
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001110E File Offset: 0x0000F30E
		public CSTimeline(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTimeline_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00011130 File Offset: 0x0000F330
		public static HandleRef getCPtr(CSTimeline obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0001115C File Offset: 0x0000F35C
		~CSTimeline()
		{
			this.Dispose();
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000111C0 File Offset: 0x0000F3C0
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
								CocoStudioEngineAdapterPINVOKE.delete_CSTimeline(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSTimeline(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000112C0 File Offset: 0x0000F4C0
		public CSTimeline() : this(CocoStudioEngineAdapterPINVOKE.new_CSTimeline(), true)
		{
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000112D1 File Offset: 0x0000F4D1
		public virtual void SetActionTag(int tag)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_SetActionTag(this.swigCPtr, tag);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000112E4 File Offset: 0x0000F4E4
		public virtual int GetActionTag()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimeline_GetActionTag(this.swigCPtr);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00011303 File Offset: 0x0000F503
		public virtual void GotoFrame(int frameIndex)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_GotoFrame(this.swigCPtr, frameIndex);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00011313 File Offset: 0x0000F513
		public virtual void InsertFrame(int index, CSTimelineFrame frame)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_InsertFrame(this.swigCPtr, index, CSTimelineFrame.getCPtr(frame));
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00011329 File Offset: 0x0000F529
		public virtual void RemoveFrame(CSTimelineFrame frame)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_RemoveFrame(this.swigCPtr, CSTimelineFrame.getCPtr(frame));
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0001133E File Offset: 0x0000F53E
		public virtual void ClearSearchState()
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_ClearSearchState(this.swigCPtr);
		}

		// Token: 0x0400009E RID: 158
		private HandleRef swigCPtr;
	}
}

using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSTimeline : CSObject
	{
		public CSTimeline(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTimeline_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSTimeline obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSTimeline()
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

		public CSTimeline() : this(CocoStudioEngineAdapterPINVOKE.new_CSTimeline(), true)
		{
		}

		public virtual void SetActionTag(int tag)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_SetActionTag(this.swigCPtr, tag);
		}

		public virtual int GetActionTag()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimeline_GetActionTag(this.swigCPtr);
		}

		public virtual void GotoFrame(int frameIndex)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_GotoFrame(this.swigCPtr, frameIndex);
		}

		public virtual void InsertFrame(int index, CSTimelineFrame frame)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_InsertFrame(this.swigCPtr, index, CSTimelineFrame.getCPtr(frame));
		}

		public virtual void RemoveFrame(CSTimelineFrame frame)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_RemoveFrame(this.swigCPtr, CSTimelineFrame.getCPtr(frame));
		}

		public virtual void ClearSearchState()
		{
			CocoStudioEngineAdapterPINVOKE.CSTimeline_ClearSearchState(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}

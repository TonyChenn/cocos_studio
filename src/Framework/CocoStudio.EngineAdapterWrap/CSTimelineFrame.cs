using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSTimelineFrame : CSObject
	{
		public CSTimelineFrame(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSTimelineFrame obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSTimelineFrame()
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

		public CSTimelineFrame() : this(CocoStudioEngineAdapterPINVOKE.new_CSTimelineFrame(), true)
		{
		}

		public virtual int GetFrameIndex()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_GetFrameIndex(this.swigCPtr);
		}

		public virtual void SetFrameIndex(int iIndex)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SetFrameIndex(this.swigCPtr, iIndex);
		}

		public virtual void SetTween(bool tween)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SetTween(this.swigCPtr, tween);
		}

		public virtual bool IsTween()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_IsTween(this.swigCPtr);
		}

		public virtual void SetTweenType(int tweenType)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SetTweenType(this.swigCPtr, tweenType);
		}

		public virtual int GetTweenType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_GetTweenType(this.swigCPtr);
		}

		public virtual void SetEasingParam(CSVectorFloat tweenParma)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_SetEasingParam(this.swigCPtr, CSVectorFloat.getCPtr(tweenParma));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual CSVectorFloat GetEasingParam()
		{
			return new CSVectorFloat(CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_GetEasingParam(this.swigCPtr), true);
		}

		public virtual void OnEnter(CSTimelineFrame nextFrame)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineFrame_OnEnter(this.swigCPtr, CSTimelineFrame.getCPtr(nextFrame));
		}

		private HandleRef swigCPtr;
	}
}

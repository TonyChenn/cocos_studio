using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200005B RID: 91
	public class CSTimelineAction : CSObject
	{
		// Token: 0x06000AAB RID: 2731 RVA: 0x0001134D File Offset: 0x0000F54D
		public CSTimelineAction(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTimelineAction_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0001136C File Offset: 0x0000F56C
		public static HandleRef getCPtr(CSTimelineAction obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00011398 File Offset: 0x0000F598
		~CSTimelineAction()
		{
			this.Dispose();
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000113FC File Offset: 0x0000F5FC
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
								CocoStudioEngineAdapterPINVOKE.delete_CSTimelineAction(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSTimelineAction(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000114FC File Offset: 0x0000F6FC
		public bool IsNeedChangeState()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_IsNeedChangeState(this.swigCPtr);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0001151B File Offset: 0x0000F71B
		public void SetOnionSkinPreNum(int preSkinNum)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_SetOnionSkinPreNum(this.swigCPtr, preSkinNum);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0001152C File Offset: 0x0000F72C
		public int GetOnionSkinPreNum()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_GetOnionSkinPreNum(this.swigCPtr);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0001154B File Offset: 0x0000F74B
		public void SetOnionSkinSuffNum(int suffSkinNum)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_SetOnionSkinSuffNum(this.swigCPtr, suffSkinNum);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0001155C File Offset: 0x0000F75C
		public int GetOnionSkinSuffNum()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_GetOnionSkinSuffNum(this.swigCPtr);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0001157B File Offset: 0x0000F77B
		public void SetOnionSkinEnable(bool skinEnable)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_SetOnionSkinEnable(this.swigCPtr, skinEnable);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0001158C File Offset: 0x0000F78C
		public bool IsOnionSkinEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_IsOnionSkinEnable(this.swigCPtr);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000115AB File Offset: 0x0000F7AB
		public void AddOnionSkinKey(int frameIndex)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_AddOnionSkinKey(this.swigCPtr, frameIndex);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000115BB File Offset: 0x0000F7BB
		public void RemoveOnionSkinKey(int frameIndex)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_RemoveOnionSkinKey(this.swigCPtr, frameIndex);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000115CC File Offset: 0x0000F7CC
		public bool IsOnionKeyFrame(int frameIndex)
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_IsOnionKeyFrame(this.swigCPtr, frameIndex);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000115EC File Offset: 0x0000F7EC
		public CSTimelineAction(CSTimelineAction csAction) : this(CocoStudioEngineAdapterPINVOKE.new_CSTimelineAction__SWIG_0(CSTimelineAction.getCPtr(csAction)), true)
		{
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00011603 File Offset: 0x0000F803
		public CSTimelineAction() : this(CocoStudioEngineAdapterPINVOKE.new_CSTimelineAction__SWIG_1(), true)
		{
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00011614 File Offset: 0x0000F814
		public virtual void GotoFrame(int startIndex)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_GotoFrame(this.swigCPtr, startIndex);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00011624 File Offset: 0x0000F824
		public virtual void Play(int startIndex, int endIndex, int currentFrameIndex, bool loop)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_Play(this.swigCPtr, startIndex, endIndex, currentFrameIndex, loop);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00011638 File Offset: 0x0000F838
		public virtual void Pause()
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_Pause(this.swigCPtr);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00011647 File Offset: 0x0000F847
		public virtual void Resume()
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_Resume(this.swigCPtr);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00011656 File Offset: 0x0000F856
		public virtual void SetTimeSpeed(float speed)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_SetTimeSpeed(this.swigCPtr, speed);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00011668 File Offset: 0x0000F868
		public virtual float GetTimeSpeed()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_GetTimeSpeed(this.swigCPtr);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00011687 File Offset: 0x0000F887
		public virtual void SetDuration(int duration)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_SetDuration(this.swigCPtr, duration);
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00011698 File Offset: 0x0000F898
		public virtual int GetDuration()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_GetDuration(this.swigCPtr);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000116B7 File Offset: 0x0000F8B7
		public virtual void SetEndFrame(int endFrame)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_SetEndFrame(this.swigCPtr, endFrame);
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000116C8 File Offset: 0x0000F8C8
		public virtual int GetEndFrame()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_GetEndFrame(this.swigCPtr);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000116E7 File Offset: 0x0000F8E7
		public virtual void SetCurrentFrame(int frameIndex)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_SetCurrentFrame(this.swigCPtr, frameIndex);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x000116F8 File Offset: 0x0000F8F8
		public virtual int GetCurrentFrame()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_GetCurrentFrame(this.swigCPtr);
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00011717 File Offset: 0x0000F917
		public virtual void AddTimeline(CSTimeline timeline)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_AddTimeline(this.swigCPtr, CSTimeline.getCPtr(timeline));
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0001172C File Offset: 0x0000F92C
		public virtual void RemoveTimeline(CSTimeline timeline)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_RemoveTimeline(this.swigCPtr, CSTimeline.getCPtr(timeline));
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00011744 File Offset: 0x0000F944
		public virtual bool IsPlaying()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTimelineAction_IsPlaying(this.swigCPtr);
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00011763 File Offset: 0x0000F963
		public virtual void InitWithRootNode(CSVisualObject target)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_InitWithRootNode(this.swigCPtr, CSVisualObject.getCPtr(target));
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00011778 File Offset: 0x0000F978
		public virtual void ActiveAction(CSVisualObject target)
		{
			CocoStudioEngineAdapterPINVOKE.CSTimelineAction_ActiveAction(this.swigCPtr, CSVisualObject.getCPtr(target));
		}

		// Token: 0x0400009F RID: 159
		private HandleRef swigCPtr;
	}
}

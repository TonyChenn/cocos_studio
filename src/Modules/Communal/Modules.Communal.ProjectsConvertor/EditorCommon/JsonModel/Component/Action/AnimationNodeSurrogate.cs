using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;

namespace EditorCommon.JsonModel.Component.Action
{
	// Token: 0x02000007 RID: 7
	[DataContract]
	internal class AnimationNodeSurrogate : BaseEntitySurrogate
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003EF4 File Offset: 0x000020F4
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00003EFC File Offset: 0x000020FC
		[DataMember]
		public int ActionTag { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003F05 File Offset: 0x00002105
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003F0D File Offset: 0x0000210D
		[DataMember]
		private List<AnimationFrameSurrogate> actionframelist { get; set; }

		// Token: 0x06000050 RID: 80 RVA: 0x00003F16 File Offset: 0x00002116
		protected AnimationNodeSurrogate()
		{
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003F1E File Offset: 0x0000211E
		public AnimationNodeSurrogate(string className)
		{
			this.classname = this.classname;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003F34 File Offset: 0x00002134
		public override void SetValue(object obj)
		{
			TimelineActionData timelineActionData = obj as TimelineActionData;
			if (timelineActionData != null && this.actionframelist != null && this.actionframelist.Count > 0)
			{
				AnimationFrameSurrogate.LastBoneFrameIndex = timelineActionData.Duration;
				foreach (AnimationFrameSurrogate animationFrameSurrogate in this.actionframelist)
				{
					animationFrameSurrogate.SetValue(obj, this.ActionTag, timelineActionData.Duration);
					if (animationFrameSurrogate.frameid > AnimationNodeSurrogate.AnimationMaxFrame)
					{
						AnimationNodeSurrogate.AnimationMaxFrame = animationFrameSurrogate.frameid;
					}
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003FD8 File Offset: 0x000021D8
		public int getDuration()
		{
			int result = 0;
			if (this.actionframelist.Count > 0)
			{
				AnimationFrameSurrogate animationFrameSurrogate = this.actionframelist.ElementAt(this.actionframelist.Count - 1);
				result = animationFrameSurrogate.frameid;
			}
			return result;
		}

		// Token: 0x04000024 RID: 36
		public static int AnimationMaxFrame;
	}
}

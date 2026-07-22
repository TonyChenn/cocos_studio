using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;

namespace EditorCommon.JsonModel.Component.Action
{
	// Token: 0x02000008 RID: 8
	[DataContract]
	internal class AnimationSurrogate : BaseEntitySurrogate
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00004018 File Offset: 0x00002218
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00004020 File Offset: 0x00002220
		[DataMember]
		private bool loop { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00004029 File Offset: 0x00002229
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00004031 File Offset: 0x00002231
		[DataMember]
		private float unittime { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000403A File Offset: 0x0000223A
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00004042 File Offset: 0x00002242
		[DataMember]
		private List<AnimationNodeSurrogate> actionnodelist { get; set; }

		// Token: 0x0600005B RID: 91 RVA: 0x0000404B File Offset: 0x0000224B
		protected AnimationSurrogate()
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004053 File Offset: 0x00002253
		public AnimationSurrogate(string className)
		{
			this.classname = this.classname;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004068 File Offset: 0x00002268
		public override void SetValue(object obj)
		{
			TimelineActionData timelineActionData = obj as TimelineActionData;
			if (timelineActionData != null && this.actionnodelist != null && this.actionnodelist.Count > 0)
			{
				timelineActionData.Speed = 0.016666668f / this.unittime;
				AnimationNodeSurrogate.AnimationMaxFrame = 0;
				foreach (AnimationNodeSurrogate animationNodeSurrogate in this.actionnodelist)
				{
					animationNodeSurrogate.SetValue(timelineActionData);
				}
				timelineActionData.Duration += AnimationNodeSurrogate.AnimationMaxFrame;
				foreach (TimelineData timelineData in timelineActionData.Timelines)
				{
					if (timelineData != null && timelineData.Frames != null && timelineData.Frames.Count<FrameData>() > 0)
					{
						timelineData.Frames.Last<FrameData>().Tween = false;
					}
				}
				timelineActionData.Duration++;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004184 File Offset: 0x00002384
		public int getDuration()
		{
			int num = 0;
			foreach (AnimationNodeSurrogate animationNodeSurrogate in this.actionnodelist)
			{
				int duration = animationNodeSurrogate.getDuration();
				if (duration > num)
				{
					num = duration;
				}
			}
			return num;
		}
	}
}

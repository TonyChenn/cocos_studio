using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;

namespace EditorCommon.JsonModel.Component.Action
{
	[DataContract]
	internal class AnimationSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		private bool loop { get; set; }

		[DataMember]
		private float unittime { get; set; }

		[DataMember]
		private List<AnimationNodeSurrogate> actionnodelist { get; set; }

		protected AnimationSurrogate()
		{
		}

		public AnimationSurrogate(string className)
		{
			this.classname = this.classname;
		}

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

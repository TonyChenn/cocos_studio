using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;

namespace EditorCommon.JsonModel.Component.Action
{
	[DataContract]
	internal class AnimationNodeSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		public int ActionTag { get; set; }

		[DataMember]
		private List<AnimationFrameSurrogate> actionframelist { get; set; }

		protected AnimationNodeSurrogate()
		{
		}

		public AnimationNodeSurrogate(string className)
		{
			this.classname = this.classname;
		}

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

		public static int AnimationMaxFrame;
	}
}

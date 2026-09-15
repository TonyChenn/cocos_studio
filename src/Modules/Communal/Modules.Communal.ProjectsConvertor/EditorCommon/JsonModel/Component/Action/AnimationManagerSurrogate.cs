using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using Modules.Communal.ProjectsConvertor;

namespace EditorCommon.JsonModel.Component.Action
{
	[DataContract]
	internal class AnimationManagerSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		private List<AnimationSurrogate> actionlist { get; set; }

		protected AnimationManagerSurrogate()
		{
		}

		public AnimationManagerSurrogate(string className)
		{
			this.classname = this.classname;
		}

		public override void SetValue(object obj)
		{
			TimelineActionData timelineActionData = obj as TimelineActionData;
			if (timelineActionData != null && this.actionlist != null && this.actionlist.Count<AnimationSurrogate>() > 0)
			{
				foreach (AnimationSurrogate animationSurrogate in this.actionlist)
				{
					animationSurrogate.SetValue(timelineActionData);
				}
			}
			AnimationConveter.ClearRepeatFrames(timelineActionData);
		}

		public void SetActionList(GameFileData data)
		{
			int num = 0;
			foreach (AnimationSurrogate animationSurrogate in this.actionlist)
			{
				AnimationInfoData animationInfoData = new AnimationInfoData();
				animationInfoData.Name = animationSurrogate.name;
				animationInfoData.StartIndex = num;
				int duration = animationSurrogate.getDuration();
				animationInfoData.EndIndex = num + duration;
				num += duration + 1;
				animationInfoData.RenderColor = AnimationInfo.GetRandomColor();
				data.AnimationList.Add(animationInfoData);
			}
		}
	}
}

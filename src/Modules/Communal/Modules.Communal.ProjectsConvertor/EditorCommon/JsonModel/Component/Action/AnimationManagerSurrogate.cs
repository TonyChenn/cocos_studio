using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using Modules.Communal.ProjectsConvertor;

namespace EditorCommon.JsonModel.Component.Action
{
	// Token: 0x02000006 RID: 6
	[DataContract]
	internal class AnimationManagerSurrogate : BaseEntitySurrogate
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003DAE File Offset: 0x00001FAE
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003DB6 File Offset: 0x00001FB6
		[DataMember]
		private List<AnimationSurrogate> actionlist { get; set; }

		// Token: 0x06000048 RID: 72 RVA: 0x00003DBF File Offset: 0x00001FBF
		protected AnimationManagerSurrogate()
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003DC7 File Offset: 0x00001FC7
		public AnimationManagerSurrogate(string className)
		{
			this.classname = this.classname;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003DDC File Offset: 0x00001FDC
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

		// Token: 0x0600004B RID: 75 RVA: 0x00003E58 File Offset: 0x00002058
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

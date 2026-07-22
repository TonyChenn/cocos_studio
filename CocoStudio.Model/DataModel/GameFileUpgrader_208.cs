using System;
using System.Collections.Generic;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200000D RID: 13
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_208 : GameFileUpgrader
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000028CC File Offset: 0x00000ACC
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_208.version;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000028E4 File Offset: 0x00000AE4
		protected override bool OnUpgrade(GameFileData projectData)
		{
			bool flag = false;
			TimelineActionData animation = projectData.Animation;
			bool result;
			if (animation == null || animation.Timelines == null)
			{
				result = flag;
			}
			else
			{
				if (animation.Timelines.Count > 0)
				{
					flag = true;
				}
				List<TimelineData> list = new List<TimelineData>();
				foreach (TimelineData item in animation.Timelines)
				{
					this.ConvertFrameType(item, list);
				}
				animation.Timelines.AddRange(list);
				result = flag;
			}
			return result;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000029A4 File Offset: 0x00000BA4
		private void ConvertFrameType(TimelineData item, List<TimelineData> alphaTimelines)
		{
			if (item.FrameType == "PositionFrame")
			{
				item.Property = "Position";
			}
			else if (item.FrameType == "ScaleFrame")
			{
				item.Property = "Scale";
			}
			else if (item.FrameType == "RotationSkewFrame")
			{
				item.Property = "RotationSkew";
			}
			else if (item.FrameType == "ColorFrame")
			{
				item.Property = "CColor";
				TimelineData timelineData = new TimelineData();
				timelineData.ActionTag = item.ActionTag;
				timelineData.Property = "Alpha";
				foreach (FrameData frameData in item.Frames)
				{
					ColorFrameData colorFrameData = frameData as ColorFrameData;
					IntFrameData intFrameData = new IntFrameData();
					intFrameData.FrameIndex = colorFrameData.FrameIndex;
					intFrameData.Value = colorFrameData.Alpha;
					timelineData.Frames.Add(intFrameData);
				}
				if (timelineData.Frames.Count > 0)
				{
					alphaTimelines.Add(timelineData);
				}
			}
			else if (item.FrameType == "TextureFrame")
			{
				item.Property = "FileData";
			}
			else if (item.FrameType == "EventFrame")
			{
				item.Property = "FrameEvent";
			}
			else if (item.FrameType == "VisibleFrame")
			{
				item.Property = "VisibleForFrame";
			}
		}

		// Token: 0x04000015 RID: 21
		private static readonly Version version = new Version("2.0.8.0");
	}
}

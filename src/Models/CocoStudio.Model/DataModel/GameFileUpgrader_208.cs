using System;
using System.Collections.Generic;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	[Extension(Type = typeof(IFileUpgrader))]
	internal class GameFileUpgrader_208 : GameFileUpgrader
	{
		public override Version Version
		{
			get
			{
				return GameFileUpgrader_208.version;
			}
		}

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

		private static readonly Version version = new Version("2.0.8.0");
	}
}

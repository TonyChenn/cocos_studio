using System;
using System.Drawing;
using System.Runtime.Serialization;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Modules.Communal.ProjectsConvertor;

namespace EditorCommon.JsonModel.Component.Action
{
	[DataContract]
	internal class AnimationFrameSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		public int frameid { get; set; }

		[DataMember]
		public float starttime { get; set; }

		[DataMember]
		public float positionx { get; set; }

		[DataMember]
		public float positiony { get; set; }

		[DataMember]
		public float scalex { get; set; }

		[DataMember]
		public float scaley { get; set; }

		[DataMember]
		public float rotation { get; set; }

		[DataMember]
		public bool visible { get; set; }

		[DataMember]
		public int opacity { get; set; }

		[DataMember]
		public int colorr { get; set; }

		[DataMember]
		public int colorg { get; set; }

		[DataMember]
		public int colorb { get; set; }

		[DataMember]
		public int tweenType { get; set; }

		public AnimationFrameSurrogate(string className)
		{
			this.classname = this.classname;
		}

		public void SetValue(object obj, int actionTag, int startFrameIndex)
		{
			int num = startFrameIndex + this.frameid;
			TimelineActionData timelineActionData = obj as TimelineActionData;
			try
			{
				TimelineData nodeTimeline = AnimationConveter.GetNodeTimeline(timelineActionData, "CColor", actionTag);
				AnimationConveter.GetTimelineFrame(nodeTimeline, AnimationFrameSurrogate.LastBoneFrameIndex, typeof(ColorFrameData));
				AnimationConveter.SetFrameData(timelineActionData, num, actionTag, "Position", new CocoStudio.Model.PointF(this.positionx, this.positiony), true, null);
				AnimationConveter.SetFrameData(timelineActionData, num, actionTag, "Scale", new CocoStudio.Model.PointF(this.scalex, this.scaley), true, null);
				AnimationConveter.SetFrameData(timelineActionData, num, actionTag, "CColor", Color.FromArgb(this.opacity, this.colorr, this.colorg, this.colorb), true, null);
				AnimationConveter.SetFrameData(timelineActionData, num, actionTag, "Alpha", this.opacity, true, null);
				AnimationConveter.SetFrameData(timelineActionData, num, actionTag, "RotationSkew", new CocoStudio.Model.PointF(this.rotation, this.rotation), true, null);
				AnimationFrameSurrogate.LastBoneFrameIndex = num;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Convert UI Animation error", exception);
			}
		}

		public static int LastBoneFrameIndex;
	}
}

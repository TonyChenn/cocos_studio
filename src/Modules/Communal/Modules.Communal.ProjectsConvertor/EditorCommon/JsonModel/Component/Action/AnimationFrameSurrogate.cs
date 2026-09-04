using System;
using System.Drawing;
using System.Runtime.Serialization;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Modules.Communal.ProjectsConvertor;

namespace EditorCommon.JsonModel.Component.Action
{
	// Token: 0x02000005 RID: 5
	[DataContract]
	internal class AnimationFrameSurrogate : BaseEntitySurrogate
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00003BA0 File Offset: 0x00001DA0
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00003BA8 File Offset: 0x00001DA8
		[DataMember]
		public int frameid { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00003BB1 File Offset: 0x00001DB1
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00003BB9 File Offset: 0x00001DB9
		[DataMember]
		public float starttime { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00003BC2 File Offset: 0x00001DC2
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00003BCA File Offset: 0x00001DCA
		[DataMember]
		public float positionx { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00003BD3 File Offset: 0x00001DD3
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00003BDB File Offset: 0x00001DDB
		[DataMember]
		public float positiony { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003BE4 File Offset: 0x00001DE4
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00003BEC File Offset: 0x00001DEC
		[DataMember]
		public float scalex { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00003BF5 File Offset: 0x00001DF5
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00003BFD File Offset: 0x00001DFD
		[DataMember]
		public float scaley { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00003C06 File Offset: 0x00001E06
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00003C0E File Offset: 0x00001E0E
		[DataMember]
		public float rotation { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003C17 File Offset: 0x00001E17
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00003C1F File Offset: 0x00001E1F
		[DataMember]
		public bool visible { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003C28 File Offset: 0x00001E28
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00003C30 File Offset: 0x00001E30
		[DataMember]
		public int opacity { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003C39 File Offset: 0x00001E39
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00003C41 File Offset: 0x00001E41
		[DataMember]
		public int colorr { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003C4A File Offset: 0x00001E4A
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00003C52 File Offset: 0x00001E52
		[DataMember]
		public int colorg { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003C5B File Offset: 0x00001E5B
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00003C63 File Offset: 0x00001E63
		[DataMember]
		public int colorb { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003C6C File Offset: 0x00001E6C
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00003C74 File Offset: 0x00001E74
		[DataMember]
		public int tweenType { get; set; }

		// Token: 0x06000043 RID: 67 RVA: 0x00003C7D File Offset: 0x00001E7D
		public AnimationFrameSurrogate(string className)
		{
			this.classname = this.classname;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003C94 File Offset: 0x00001E94
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

		// Token: 0x04000015 RID: 21
		public static int LastBoneFrameIndex;
	}
}

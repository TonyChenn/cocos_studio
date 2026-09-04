using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	// Token: 0x0200000A RID: 10
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class BaseGameObjectSurrogate : VisualObjectSurrogate
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000041E8 File Offset: 0x000023E8
		// (set) Token: 0x06000061 RID: 97 RVA: 0x000041F0 File Offset: 0x000023F0
		[DataMember]
		public double x { get; protected set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000041F9 File Offset: 0x000023F9
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00004201 File Offset: 0x00002401
		[DataMember]
		public double y { get; protected set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000420A File Offset: 0x0000240A
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00004212 File Offset: 0x00002412
		[DataMember]
		public byte visible { get; protected set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000421B File Offset: 0x0000241B
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00004223 File Offset: 0x00002423
		[DataMember]
		public int zorder { get; protected set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000422C File Offset: 0x0000242C
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00004234 File Offset: 0x00002434
		[DataMember]
		public int objecttag { get; protected set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000423D File Offset: 0x0000243D
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00004245 File Offset: 0x00002445
		[DataMember]
		public float scalex { get; protected set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000424E File Offset: 0x0000244E
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00004256 File Offset: 0x00002456
		[DataMember]
		public float scaley { get; protected set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000425F File Offset: 0x0000245F
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00004267 File Offset: 0x00002467
		[DataMember]
		public float rotation { get; protected set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00004270 File Offset: 0x00002470
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00004278 File Offset: 0x00002478
		[DataMember]
		public bool canedit { get; protected set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00004281 File Offset: 0x00002481
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00004289 File Offset: 0x00002489
		[DataMember(Order = 51)]
		public List<BaseGameObjectSurrogate> gameobjects { get; private set; }

		// Token: 0x06000074 RID: 116 RVA: 0x00004292 File Offset: 0x00002492
		protected BaseGameObjectSurrogate()
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000429C File Offset: 0x0000249C
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			NodeObjectData nodeObjectData = obj as NodeObjectData;
			nodeObjectData.Position.X = (float)this.x;
			nodeObjectData.Position.Y = (float)this.y;
			nodeObjectData.VisibleForFrame = (this.visible != 0);
			nodeObjectData.ZOrder = this.zorder;
			nodeObjectData.Tag = this.objecttag;
			nodeObjectData.Scale.ScaleX = this.scalex;
			nodeObjectData.Scale.ScaleY = this.scaley;
			nodeObjectData.RotationSkewX = this.rotation;
			nodeObjectData.CanEdit = this.canedit;
			if (this.gameobjects != null && this.gameobjects.Count > 0)
			{
				if (nodeObjectData.Children == null)
				{
					nodeObjectData.Children = new List<AbstractNodeObjectData>();
				}
				foreach (BaseGameObjectSurrogate baseGameObjectSurrogate in this.gameobjects)
				{
					NodeObjectData item = (NodeObjectData)baseGameObjectSurrogate.ConvertToObject();
					nodeObjectData.Children.Add(item);
				}
			}
		}
	}
}

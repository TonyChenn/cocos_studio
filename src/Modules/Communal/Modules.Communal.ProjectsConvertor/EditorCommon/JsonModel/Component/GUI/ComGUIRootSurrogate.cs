using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using EditorCommon.JsonModel.Component.Action;
using Gdk;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000029 RID: 41
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComGUIRootSurrogate : BaseEntitySurrogate
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00007B5A File Offset: 0x00005D5A
		// (set) Token: 0x0600028E RID: 654 RVA: 0x00007B62 File Offset: 0x00005D62
		[DataMember]
		public string version { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00007B6B File Offset: 0x00005D6B
		// (set) Token: 0x06000290 RID: 656 RVA: 0x00007B73 File Offset: 0x00005D73
		[DataMember]
		public string cocos2dVersion { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00007B7C File Offset: 0x00005D7C
		// (set) Token: 0x06000292 RID: 658 RVA: 0x00007B84 File Offset: 0x00005D84
		[DataMember]
		public int designWidth { get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00007B8D File Offset: 0x00005D8D
		// (set) Token: 0x06000294 RID: 660 RVA: 0x00007B95 File Offset: 0x00005D95
		[DataMember]
		public int designHeight { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00007B9E File Offset: 0x00005D9E
		// (set) Token: 0x06000296 RID: 662 RVA: 0x00007BA6 File Offset: 0x00005DA6
		public Size desingSize { get; set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00007BAF File Offset: 0x00005DAF
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00007BB7 File Offset: 0x00005DB7
		[DataMember]
		public float dataScale { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00007BC0 File Offset: 0x00005DC0
		// (set) Token: 0x0600029A RID: 666 RVA: 0x00007BC8 File Offset: 0x00005DC8
		[DataMember]
		public List<string> textures { get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00007BD1 File Offset: 0x00005DD1
		// (set) Token: 0x0600029C RID: 668 RVA: 0x00007BD9 File Offset: 0x00005DD9
		[DataMember]
		public List<string> texturesPng { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00007BE2 File Offset: 0x00005DE2
		// (set) Token: 0x0600029E RID: 670 RVA: 0x00007BEA File Offset: 0x00005DEA
		[DataMember]
		public ComGUIExportSurrogate nodeTree { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00007BF3 File Offset: 0x00005DF3
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x00007BFB File Offset: 0x00005DFB
		[DataMember]
		public AnimationManagerSurrogate animation { get; set; }

		// Token: 0x060002A1 RID: 673 RVA: 0x00007C04 File Offset: 0x00005E04
		protected ComGUIRootSurrogate()
		{
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00007C0C File Offset: 0x00005E0C
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00007C18 File Offset: 0x00005E18
		internal void InitGameProjectData(GameFileData gameFileData)
		{
			if (this.cocos2dVersion == null || !(this.cocos2dVersion == "3.x"))
			{
				this.nodeTree.RefreshChildPropertyAfferInitFor3X();
			}
			AbstractNodeObjectData objectData = gameFileData.ObjectData;
			base.SetValue(objectData);
			objectData.Children = new List<AbstractNodeObjectData>();
			this.nodeTree.SetValue(objectData);
			this.nodeTree.ConvertWidgetPositionFromLayout(objectData.Children[0]);
			this.nodeTree.ConvertScrollViewChildrenPercentValue(objectData.Children[0]);
			this.animation.SetValue(gameFileData.Animation);
			this.animation.SetActionList(gameFileData);
		}
	}
}

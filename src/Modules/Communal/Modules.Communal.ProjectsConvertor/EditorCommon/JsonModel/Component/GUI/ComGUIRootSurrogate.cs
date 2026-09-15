using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using EditorCommon.JsonModel.Component.Action;
using Gdk;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComGUIRootSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		public string version { get; set; }

		[DataMember]
		public string cocos2dVersion { get; set; }

		[DataMember]
		public int designWidth { get; set; }

		[DataMember]
		public int designHeight { get; set; }

		public Size desingSize { get; set; }

		[DataMember]
		public float dataScale { get; set; }

		[DataMember]
		public List<string> textures { get; set; }

		[DataMember]
		public List<string> texturesPng { get; set; }

		[DataMember]
		public ComGUIExportSurrogate nodeTree { get; set; }

		[DataMember]
		public AnimationManagerSurrogate animation { get; set; }

		protected ComGUIRootSurrogate()
		{
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}

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

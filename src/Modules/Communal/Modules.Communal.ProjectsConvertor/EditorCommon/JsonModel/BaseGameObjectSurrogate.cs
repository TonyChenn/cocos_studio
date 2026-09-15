using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class BaseGameObjectSurrogate : VisualObjectSurrogate
	{
		[DataMember]
		public double x { get; protected set; }

		[DataMember]
		public double y { get; protected set; }

		[DataMember]
		public byte visible { get; protected set; }

		[DataMember]
		public int zorder { get; protected set; }

		[DataMember]
		public int objecttag { get; protected set; }

		[DataMember]
		public float scalex { get; protected set; }

		[DataMember]
		public float scaley { get; protected set; }

		[DataMember]
		public float rotation { get; protected set; }

		[DataMember]
		public bool canedit { get; protected set; }

		[DataMember(Order = 51)]
		public List<BaseGameObjectSurrogate> gameobjects { get; private set; }

		protected BaseGameObjectSurrogate()
		{
		}

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

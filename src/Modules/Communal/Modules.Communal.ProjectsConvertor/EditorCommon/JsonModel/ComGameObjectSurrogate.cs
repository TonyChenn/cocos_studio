using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using EditorCommon.JsonModel.Component;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComGameObjectSurrogate : BaseGameObjectSurrogate
	{
		[DataMember(Order = 50)]
		public List<BaseComSurrogate> components { get; private set; }

		protected ComGameObjectSurrogate()
		{
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			NodeObjectData nodeObjectData = obj as NodeObjectData;
			if (this.components != null && this.components.Count > 0 && nodeObjectData.Children == null)
			{
				nodeObjectData.Children = new List<AbstractNodeObjectData>();
			}
			foreach (BaseComSurrogate baseComSurrogate in this.components)
			{
				NodeObjectData item = (NodeObjectData)baseComSurrogate.ConvertToObject();
				nodeObjectData.Children.Add(item);
			}
		}
	}
}

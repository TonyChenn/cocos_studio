using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComGameMapSurrogate : ComRenderSurrogate
	{
		protected ComGameMapSurrogate()
		{
			this.classname = "CCTMXTiledMap";
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			GameMapObjectData gameMapObjectData = obj as GameMapObjectData;
			gameMapObjectData.FileData = new ResourceItemData((EnumResourceType)base.fileData.resourceType, base.fileData.path, base.fileData.plistFile);
			gameMapObjectData.IsAutoSize = true;
		}

		protected override object CreateModelObject()
		{
			return new GameMapObjectData();
		}
	}
}

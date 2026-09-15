using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComSpriteSurrogate : ComRenderSurrogate
	{
		protected ComSpriteSurrogate()
		{
			this.classname = "CCSprite";
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			SpriteObjectData spriteObjectData = obj as SpriteObjectData;
			spriteObjectData.FileData = new ResourceItemData((EnumResourceType)base.fileData.resourceType, base.fileData.path, base.fileData.plistFile);
			spriteObjectData.IsAutoSize = true;
		}

		protected override object CreateModelObject()
		{
			return new SpriteObjectData();
		}
	}
}

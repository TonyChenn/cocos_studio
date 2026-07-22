using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x02000017 RID: 23
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComSpriteSurrogate : ComRenderSurrogate
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00004A44 File Offset: 0x00002C44
		protected ComSpriteSurrogate()
		{
			this.classname = "CCSprite";
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004A58 File Offset: 0x00002C58
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			SpriteObjectData spriteObjectData = obj as SpriteObjectData;
			spriteObjectData.FileData = new ResourceItemData((EnumResourceType)base.fileData.resourceType, base.fileData.path, base.fileData.plistFile);
			spriteObjectData.IsAutoSize = true;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004AA8 File Offset: 0x00002CA8
		protected override object CreateModelObject()
		{
			return new SpriteObjectData();
		}
	}
}

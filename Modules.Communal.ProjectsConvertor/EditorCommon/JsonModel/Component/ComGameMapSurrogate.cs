using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x02000010 RID: 16
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComGameMapSurrogate : ComRenderSurrogate
	{
		// Token: 0x06000092 RID: 146 RVA: 0x000046FC File Offset: 0x000028FC
		protected ComGameMapSurrogate()
		{
			this.classname = "CCTMXTiledMap";
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004710 File Offset: 0x00002910
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			GameMapObjectData gameMapObjectData = obj as GameMapObjectData;
			gameMapObjectData.FileData = new ResourceItemData((EnumResourceType)base.fileData.resourceType, base.fileData.path, base.fileData.plistFile);
			gameMapObjectData.IsAutoSize = true;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004760 File Offset: 0x00002960
		protected override object CreateModelObject()
		{
			return new GameMapObjectData();
		}
	}
}

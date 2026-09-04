using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x02000016 RID: 22
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComSimpleAudioSurrogate : ComRenderSurrogate
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004987 File Offset: 0x00002B87
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x0000498F File Offset: 0x00002B8F
		[DataMember]
		public byte loop { get; set; }

		// Token: 0x060000A9 RID: 169 RVA: 0x00004998 File Offset: 0x00002B98
		protected ComSimpleAudioSurrogate()
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000049A0 File Offset: 0x00002BA0
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			SimpleAudioObjectData simpleAudioObjectData = obj as SimpleAudioObjectData;
			if (base.fileData.path != null && base.fileData.path != "")
			{
				simpleAudioObjectData.FileData = new ResourceItemData((EnumResourceType)base.fileData.resourceType, base.fileData.path, base.fileData.plistFile);
			}
			else
			{
				simpleAudioObjectData.FileData = null;
			}
			simpleAudioObjectData.Loop = (this.loop != 0);
			simpleAudioObjectData.PreSizeEnable = true;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004A30 File Offset: 0x00002C30
		protected override object CreateModelObject()
		{
			return new SimpleAudioObjectData();
		}
	}
}

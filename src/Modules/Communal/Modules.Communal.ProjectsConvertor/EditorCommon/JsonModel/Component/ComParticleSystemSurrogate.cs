using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x02000012 RID: 18
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComParticleSystemSurrogate : ComRenderSurrogate
	{
		// Token: 0x06000098 RID: 152 RVA: 0x00004870 File Offset: 0x00002A70
		protected ComParticleSystemSurrogate()
		{
			this.classname = "CCParticleSystemQuad";
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004884 File Offset: 0x00002A84
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ParticleObjectData particleObjectData = obj as ParticleObjectData;
			particleObjectData.FileData = new ResourceItemData((EnumResourceType)base.fileData.resourceType, base.fileData.path, base.fileData.plistFile);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000048CC File Offset: 0x00002ACC
		protected override object CreateModelObject()
		{
			return new ParticleObjectData();
		}
	}
}

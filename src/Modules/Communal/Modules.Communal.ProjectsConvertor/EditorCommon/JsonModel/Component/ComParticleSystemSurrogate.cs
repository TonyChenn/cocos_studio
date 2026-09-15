using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComParticleSystemSurrogate : ComRenderSurrogate
	{
		protected ComParticleSystemSurrogate()
		{
			this.classname = "CCParticleSystemQuad";
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ParticleObjectData particleObjectData = obj as ParticleObjectData;
			particleObjectData.FileData = new ResourceItemData((EnumResourceType)base.fileData.resourceType, base.fileData.path, base.fileData.plistFile);
		}

		protected override object CreateModelObject()
		{
			return new ParticleObjectData();
		}
	}
}

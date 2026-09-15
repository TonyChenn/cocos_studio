using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComSimpleAudioSurrogate : ComRenderSurrogate
	{
		[DataMember]
		public byte loop { get; set; }

		protected ComSimpleAudioSurrogate()
		{
		}

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

		protected override object CreateModelObject()
		{
			return new SimpleAudioObjectData();
		}
	}
}

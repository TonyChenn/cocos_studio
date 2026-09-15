using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using EditorCommon.JsonModel.Component;
using EditorCommon.ViewModel;
using Gdk;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class CanvasGameObjectSurrogate : BaseGameObjectSurrogate
	{
		[DataMember]
		public Size CanvasSize { get; set; }

		[DataMember]
		public string Version { get; set; }

		[DataMember]
		protected List<BaseComSurrogate> components { get; private set; }

		[DataMember]
		public ObservableCollection<TriggerModel> Triggers { get; set; }

		internal void InitGameFileData(GameFileData gameFileData)
		{
			AbstractNodeObjectData objectData = gameFileData.ObjectData;
			base.SetValue(objectData);
			objectData.Size = new SizeF((float)this.CanvasSize.Width, (float)this.CanvasSize.Height);
			if (this.components.Count > 1)
			{
				ComSimpleAudioSurrogate comSimpleAudioSurrogate = this.components[1] as ComSimpleAudioSurrogate;
				SimpleAudioObjectData simpleAudioObjectData = new SimpleAudioObjectData();
				simpleAudioObjectData.FileData = new ResourceItemData((EnumResourceType)comSimpleAudioSurrogate.fileData.resourceType, comSimpleAudioSurrogate.fileData.path, comSimpleAudioSurrogate.fileData.plistFile);
				simpleAudioObjectData.Loop = (comSimpleAudioSurrogate.loop != 0);
				if (simpleAudioObjectData.FileData.Path != "")
				{
					if (objectData.Children == null)
					{
						objectData.Children = new List<AbstractNodeObjectData>();
					}
					objectData.Children.Add(simpleAudioObjectData);
				}
			}
		}
	}
}

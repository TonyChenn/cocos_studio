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
	// Token: 0x0200000B RID: 11
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class CanvasGameObjectSurrogate : BaseGameObjectSurrogate
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000043C0 File Offset: 0x000025C0
		// (set) Token: 0x06000077 RID: 119 RVA: 0x000043C8 File Offset: 0x000025C8
		[DataMember]
		public Size CanvasSize { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000078 RID: 120 RVA: 0x000043D1 File Offset: 0x000025D1
		// (set) Token: 0x06000079 RID: 121 RVA: 0x000043D9 File Offset: 0x000025D9
		[DataMember]
		public string Version { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000043E2 File Offset: 0x000025E2
		// (set) Token: 0x0600007B RID: 123 RVA: 0x000043EA File Offset: 0x000025EA
		[DataMember]
		protected List<BaseComSurrogate> components { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000043FC File Offset: 0x000025FC
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000043F3 File Offset: 0x000025F3
		[DataMember]
		public ObservableCollection<TriggerModel> Triggers { get; set; }

		// Token: 0x0600007F RID: 127 RVA: 0x0000440C File Offset: 0x0000260C
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

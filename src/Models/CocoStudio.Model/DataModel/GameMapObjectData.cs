using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000018 RID: 24
	[DataModelExtension(typeof(GameMapObject))]
	public class GameMapObjectData : NodeObjectData
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003C50 File Offset: 0x00001E50
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00003C68 File Offset: 0x00001E68
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData
		{
			get
			{
				return this.fileData;
			}
			set
			{
				this.fileData = value;
				if (this.fileData == ResourceItemData.DefaultMarker)
				{
					this.fileData = GameMapObjectData.DefaultFile;
				}
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00003CAC File Offset: 0x00001EAC
		protected override void OnDataInitialize(VisualObject vObject)
		{
			GameMapObject gameMapObject = vObject as GameMapObject;
			if (gameMapObject != null)
			{
				if (this.FileData != null && gameMapObject.FileData.GetResourceData().Type != this.FileData.Type)
				{
					gameMapObject.FileData = null;
				}
			}
		}

		// Token: 0x04000071 RID: 113
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/defaultMap.tmx");

		// Token: 0x04000072 RID: 114
		private ResourceItemData fileData;
	}
}

using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(GameMapObject))]
	public class GameMapObjectData : NodeObjectData
	{
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

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/defaultMap.tmx");

		private ResourceItemData fileData;
	}
}

using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(SpriteObject))]
	public class SpriteObjectData : NodeObjectData
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool FlipX { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool FlipY { get; set; }

		[JsonProperty]
		[ItemProperty]
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
					this.fileData = SpriteObjectData.DefaultFile;
				}
			}
		}

		[JsonProperty]
		[ItemProperty]
		public BlendFuncValue BlendFunc { get; set; }

		public SpriteObjectData()
		{
			this.BlendFunc = new BlendFuncValue();
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			SpriteObject spriteObject = vObject as SpriteObject;
			if (spriteObject != null)
			{
				if (this.FileData != null && spriteObject.FileData.GetResourceData().Type != this.FileData.Type)
				{
					spriteObject.FileData = null;
				}
			}
		}

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/Sprite.png");

		private ResourceItemData fileData;
	}
}

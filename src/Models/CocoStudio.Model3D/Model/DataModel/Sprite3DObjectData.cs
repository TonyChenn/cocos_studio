using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(Sprite3DObject))]
	public class Sprite3DObjectData : Node3DObjectData
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
				if (this.fileData == null)
				{
					this.fileData = Sprite3DObjectData.DefaultFile;
					base.Size = Sprite3DObjectData.defaultSpriteSize;
				}
			}
		}

		[JsonProperty]
		[ItemProperty(DefaultValue = false)]
		public bool RunAction3D { get; set; }

		[ItemProperty(DefaultValue = false)]
		[JsonProperty]
		public bool IsFlipped { get; set; }

		[ItemProperty]
		[JsonProperty]
		public LightFlag LightFlag { get; set; }

		public Sprite3DObjectData()
		{
			this.LightFlag = LightFlag.LIGHT0;
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			Sprite3DObject sprite3DObject = vObject as Sprite3DObject;
			if (sprite3DObject != null && this.FileData != null && sprite3DObject.FileData.GetResourceData().Type != this.FileData.Type)
			{
				sprite3DObject.FileData = null;
			}
		}

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/empty.c3t");

		private static readonly SizeF defaultSpriteSize = new SizeF(1f, 1f);

		private ResourceItemData fileData;
	}
}

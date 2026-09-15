using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(Particle3DObject))]
	public class Particle3DObjectData : Node3DObjectData
	{
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
				if (this.fileData == null)
				{
					this.fileData = Particle3DObjectData.DefaultFile;
					base.Size = Particle3DObjectData.defaultSpriteSize;
				}
			}
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			Particle3DObject particle3DObject = vObject as Particle3DObject;
			if (particle3DObject != null && this.FileData != null && particle3DObject.FileData.GetResourceData().Type != this.FileData.Type)
			{
				particle3DObject.FileData = null;
			}
		}

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/scripts/default.pu");

		private static readonly SizeF defaultSpriteSize = new SizeF(1f, 1f);

		private ResourceItemData fileData;
	}
}

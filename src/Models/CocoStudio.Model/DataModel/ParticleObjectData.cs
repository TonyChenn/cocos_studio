using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(ParticleObject))]
	public class ParticleObjectData : NodeObjectData
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
				if (this.fileData == ResourceItemData.DefaultMarker)
				{
					this.fileData = ParticleObjectData.DefaultFile;
					this.BlendFunc = new BlendFuncValue(BlendSrc.GL_ONE_MINUS_DST_COLOR, BlendDst.GL_ONE);
					base.Size = ParticleObjectData.defaultParticleSize;
				}
			}
		}

		[JsonProperty]
		[ItemProperty]
		public BlendFuncValue BlendFunc { get; set; }

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/defaultParticle.plist");

		private static readonly SizeF defaultParticleSize = SizeF.Empty;

		private ResourceItemData fileData;
	}
}

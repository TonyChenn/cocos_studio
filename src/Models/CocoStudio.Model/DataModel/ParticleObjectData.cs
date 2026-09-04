using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200002A RID: 42
	[DataModelExtension(typeof(ParticleObject))]
	public class ParticleObjectData : NodeObjectData
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00006310 File Offset: 0x00004510
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00006328 File Offset: 0x00004528
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00006380 File Offset: 0x00004580
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x00006397 File Offset: 0x00004597
		[JsonProperty]
		[ItemProperty]
		public BlendFuncValue BlendFunc { get; set; }

		// Token: 0x040000C2 RID: 194
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/defaultParticle.plist");

		// Token: 0x040000C3 RID: 195
		private static readonly SizeF defaultParticleSize = SizeF.Empty;

		// Token: 0x040000C4 RID: 196
		private ResourceItemData fileData;
	}
}

using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000005 RID: 5
	[DataModelExtension(typeof(Particle3DObject))]
	public class Particle3DObjectData : Node3DObjectData
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000234E File Offset: 0x0000054E
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002356 File Offset: 0x00000556
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

		// Token: 0x06000034 RID: 52 RVA: 0x0000238C File Offset: 0x0000058C
		protected override void OnDataInitialize(VisualObject vObject)
		{
			Particle3DObject particle3DObject = vObject as Particle3DObject;
			if (particle3DObject != null && this.FileData != null && particle3DObject.FileData.GetResourceData().Type != this.FileData.Type)
			{
				particle3DObject.FileData = null;
			}
		}

		// Token: 0x04000014 RID: 20
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/scripts/default.pu");

		// Token: 0x04000015 RID: 21
		private static readonly SizeF defaultSpriteSize = new SizeF(1f, 1f);

		// Token: 0x04000016 RID: 22
		private ResourceItemData fileData;
	}
}

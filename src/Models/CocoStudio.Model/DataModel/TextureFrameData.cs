using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200003C RID: 60
	[DataModelExtension(typeof(TextureFrame))]
	[DataItem(Name = "TextureFrame")]
	public class TextureFrameData : FrameData
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00007174 File Offset: 0x00005374
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000718B File Offset: 0x0000538B
		[JsonProperty]
		[ItemProperty]
		public ResourceItemData TextureFile { get; set; }

		// Token: 0x06000257 RID: 599 RVA: 0x000071A0 File Offset: 0x000053A0
		public override bool FrameEquals(FrameData framedata)
		{
			TextureFrameData textureFrameData = framedata as TextureFrameData;
			bool flag = textureFrameData != null;
			return flag && base.FrameEquals(framedata) && this.TextureFile.Equals(textureFrameData.TextureFile);
		}
	}
}

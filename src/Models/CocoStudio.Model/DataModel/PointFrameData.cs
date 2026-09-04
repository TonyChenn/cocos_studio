using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200003A RID: 58
	[DataModelExtension(typeof(PointFrame))]
	[DataItem(Name = "PointFrame")]
	public class PointFrameData : FrameData
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00007014 File Offset: 0x00005214
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000702B File Offset: 0x0000522B
		[ItemProperty]
		[JsonProperty]
		public float X { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00007034 File Offset: 0x00005234
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000704B File Offset: 0x0000524B
		[ItemProperty]
		[JsonProperty]
		public float Y { get; set; }

		// Token: 0x0600024D RID: 589 RVA: 0x00007060 File Offset: 0x00005260
		public override bool FrameEquals(FrameData framedata)
		{
			PointFrameData pointFrameData = framedata as PointFrameData;
			bool flag = pointFrameData != null;
			return flag && base.FrameEquals(framedata) && FrameData.IsFloatEqual(this.X, pointFrameData.X, 0.0001f) && FrameData.IsFloatEqual(this.Y, pointFrameData.Y, 0.0001f);
		}
	}
}

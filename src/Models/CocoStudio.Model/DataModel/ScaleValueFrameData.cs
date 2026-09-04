using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200003B RID: 59
	[DataModelExtension(typeof(ScaleValueFrame))]
	[DataItem(Name = "ScaleFrame")]
	public class ScaleValueFrameData : FrameData
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000070C4 File Offset: 0x000052C4
		// (set) Token: 0x0600024F RID: 591 RVA: 0x000070DB File Offset: 0x000052DB
		[JsonProperty]
		[ItemProperty]
		public float X { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000070E4 File Offset: 0x000052E4
		// (set) Token: 0x06000251 RID: 593 RVA: 0x000070FB File Offset: 0x000052FB
		[JsonProperty]
		[ItemProperty]
		public float Y { get; set; }

		// Token: 0x06000253 RID: 595 RVA: 0x00007110 File Offset: 0x00005310
		public override bool FrameEquals(FrameData framedata)
		{
			ScaleValueFrameData scaleValueFrameData = framedata as ScaleValueFrameData;
			bool flag = scaleValueFrameData != null;
			return flag && base.FrameEquals(framedata) && FrameData.IsFloatEqual(this.X, scaleValueFrameData.X, 0.0001f) && FrameData.IsFloatEqual(this.Y, scaleValueFrameData.Y, 0.0001f);
		}
	}
}

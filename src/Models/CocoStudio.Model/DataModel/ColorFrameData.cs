using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000033 RID: 51
	[DataItem(Name = "ColorFrame")]
	[DataModelExtension(typeof(ColorFrame))]
	public class ColorFrameData : FrameData
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00006BDC File Offset: 0x00004DDC
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00006BF3 File Offset: 0x00004DF3
		[ItemProperty]
		[JsonProperty]
		public int Alpha { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00006BFC File Offset: 0x00004DFC
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00006C13 File Offset: 0x00004E13
		[JsonProperty]
		[ItemProperty]
		public ColorData Color { get; set; }

		// Token: 0x0600022F RID: 559 RVA: 0x00006C1C File Offset: 0x00004E1C
		public ColorFrameData()
		{
			this.Alpha = 255;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00006C34 File Offset: 0x00004E34
		public override bool FrameEquals(FrameData framedata)
		{
			ColorFrameData colorFrameData = framedata as ColorFrameData;
			bool flag = colorFrameData != null;
			return flag && base.FrameEquals(framedata) && this.Alpha == colorFrameData.Alpha && this.Color.Equals(colorFrameData.Color);
		}
	}
}

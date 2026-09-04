using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000009 RID: 9
	[DataItem(Name = "Frame")]
	[DataModelExtension(typeof(Frame))]
	public class FrameData : BaseObjectData, IFrameDataLuaExtend
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002518 File Offset: 0x00000718
		// (set) Token: 0x06000031 RID: 49 RVA: 0x0000252F File Offset: 0x0000072F
		[ItemProperty]
		[JsonProperty]
		public int FrameIndex { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002538 File Offset: 0x00000738
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000254F File Offset: 0x0000074F
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		[DefaultValue(true)]
		public bool Tween { get; set; }

		// Token: 0x06000034 RID: 52 RVA: 0x00002558 File Offset: 0x00000758
		public FrameData()
		{
			this.Tween = true;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002574 File Offset: 0x00000774
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000025AC File Offset: 0x000007AC
		[ItemProperty]
		[JsonProperty]
		public EasingValue EasingData
		{
			get
			{
				EasingValue result;
				if (!this.Tween || null == this.easingValue)
				{
					result = null;
				}
				else
				{
					result = this.easingValue;
				}
				return result;
			}
			set
			{
				this.easingValue = value;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000025B8 File Offset: 0x000007B8
		public virtual bool FrameEquals(FrameData framedata)
		{
			return this.Tween == framedata.Tween && ((this.EasingData == null && framedata.EasingData == null) || ((this.EasingData != null || framedata.EasingData == null) && (this.EasingData == null || framedata.EasingData != null) && this.EasingData.Equals(framedata.EasingData)));
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002654 File Offset: 0x00000854
		public static bool IsFloatEqual(float a, float b, float digit = 0.0001f)
		{
			return Math.Abs(a - b) < digit;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002674 File Offset: 0x00000874
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0000268B File Offset: 0x0000088B
		string IFrameDataLuaExtend.Property { get; set; }

		// Token: 0x0400000E RID: 14
		private EasingValue easingValue = null;
	}
}

using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000025 RID: 37
	[DataInclude(typeof(ListViewVertical))]
	[DataModelExtension(typeof(ListViewObject))]
	[DataInclude(typeof(ListViewDirectionType))]
	[DataInclude(typeof(ListViewHorizontal))]
	public class ListViewObjectData : ScrollViewObjectData
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000059F4 File Offset: 0x00003BF4
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00005A0B File Offset: 0x00003C0B
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		public int ItemMargin { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00005A14 File Offset: 0x00003C14
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00005A2B File Offset: 0x00003C2B
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = ListViewDirectionType.Horizontal)]
		[DefaultValue(ListViewDirectionType.Horizontal)]
		public ListViewDirectionType DirectionType { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00005A34 File Offset: 0x00003C34
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00005A4B File Offset: 0x00003C4B
		[DefaultValue(ListViewHorizontal.Align_Left)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = ListViewHorizontal.Align_Left)]
		public ListViewHorizontal HorizontalType { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00005A54 File Offset: 0x00003C54
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00005A6C File Offset: 0x00003C6C
		[DefaultValue(ListViewVertical.Align_Top)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = ListViewVertical.Align_Top)]
		public ListViewVertical VerticalType
		{
			get
			{
				return this._vertical;
			}
			set
			{
				if (value < ListViewVertical.Align_Top)
				{
					this._vertical = ListViewVertical.Align_Top;
				}
				else
				{
					this._vertical = value;
				}
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00005A95 File Offset: 0x00003C95
		public ListViewObjectData()
		{
			this.DirectionType = ListViewDirectionType.Horizontal;
		}

		// Token: 0x040000B3 RID: 179
		private ListViewVertical _vertical = ListViewVertical.Align_Top;
	}
}

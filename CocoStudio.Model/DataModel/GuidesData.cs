using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200011C RID: 284
	[Extension(Type = typeof(IUserData))]
	public class GuidesData : IUserData
	{
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x0002B450 File Offset: 0x00029650
		// (set) Token: 0x06000ADF RID: 2783 RVA: 0x0002B467 File Offset: 0x00029667
		[ItemProperty]
		public List<GuidesObject> HorizontalList { get; set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0002B470 File Offset: 0x00029670
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x0002B487 File Offset: 0x00029687
		[ItemProperty]
		public List<GuidesObject> VerticalList { get; set; }

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002B490 File Offset: 0x00029690
		public GuidesData()
		{
			this.HorizontalList = new List<GuidesObject>();
			this.VerticalList = new List<GuidesObject>();
		}

		// Token: 0x0400047D RID: 1149
		public const string GuidesListKey = "GuidesList";
	}
}

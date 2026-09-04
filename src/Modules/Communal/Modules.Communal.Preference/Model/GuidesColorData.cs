using System;
using System.Collections.Generic;
using System.Drawing;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.Preference.Model
{
	// Token: 0x02000008 RID: 8
	public class GuidesColorData
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002230 File Offset: 0x00000430
		public static GuidesColorData Instance
		{
			get
			{
				if (GuidesColorData.instance == null)
				{
					GuidesColorData.instance = new GuidesColorData();
				}
				return GuidesColorData.instance;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002248 File Offset: 0x00000448
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002250 File Offset: 0x00000450
		public List<GuidesColorInfo> GuidesColorList { get; private set; }

		// Token: 0x06000019 RID: 25 RVA: 0x0000225C File Offset: 0x0000045C
		private GuidesColorData()
		{
			this.GuidesColorList = new List<GuidesColorInfo>();
			GuidesColorInfo guidesColorInfo = new GuidesColorInfo();
			guidesColorInfo.Name = LanguageInfo.Preference_GuidesColor1;
			guidesColorInfo.RenderColor = Color.FromArgb(74, 255, 255);
			GuidesColorInfo guidesColorInfo2 = new GuidesColorInfo();
			guidesColorInfo2.Name = LanguageInfo.Preference_GuidesColor2;
			guidesColorInfo2.RenderColor = Color.FromArgb(74, 132, 255);
			GuidesColorInfo guidesColorInfo3 = new GuidesColorInfo();
			guidesColorInfo3.Name = LanguageInfo.Preference_GuidesColor3;
			guidesColorInfo3.RenderColor = Color.FromArgb(255, 74, 74);
			GuidesColorInfo guidesColorInfo4 = new GuidesColorInfo();
			guidesColorInfo4.Name = LanguageInfo.Preference_GuidesColor4;
			guidesColorInfo4.RenderColor = Color.FromArgb(74, 255, 74);
			GuidesColorInfo guidesColorInfo5 = new GuidesColorInfo();
			guidesColorInfo5.Name = LanguageInfo.Preference_GuidesColor5;
			guidesColorInfo5.RenderColor = Color.FromArgb(74, 74, 255);
			GuidesColorInfo guidesColorInfo6 = new GuidesColorInfo();
			guidesColorInfo6.Name = LanguageInfo.Preference_GuidesColor6;
			guidesColorInfo6.RenderColor = Color.FromArgb(255, 255, 74);
			GuidesColorInfo guidesColorInfo7 = new GuidesColorInfo();
			guidesColorInfo7.Name = LanguageInfo.Preference_GuidesColor7;
			guidesColorInfo7.RenderColor = Color.FromArgb(255, 74, 255);
			GuidesColorInfo guidesColorInfo8 = new GuidesColorInfo();
			guidesColorInfo8.Name = LanguageInfo.Preference_GuidesColor8;
			guidesColorInfo8.RenderColor = Color.FromArgb(230, 230, 230);
			this.GuidesColorList.Add(guidesColorInfo);
			this.GuidesColorList.Add(guidesColorInfo2);
			this.GuidesColorList.Add(guidesColorInfo3);
			this.GuidesColorList.Add(guidesColorInfo4);
			this.GuidesColorList.Add(guidesColorInfo5);
			this.GuidesColorList.Add(guidesColorInfo6);
			this.GuidesColorList.Add(guidesColorInfo7);
			this.GuidesColorList.Add(guidesColorInfo8);
		}

		// Token: 0x0400000F RID: 15
		private static GuidesColorData instance;
	}
}

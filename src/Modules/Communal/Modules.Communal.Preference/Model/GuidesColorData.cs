using System;
using System.Collections.Generic;
using System.Drawing;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.Preference.Model
{
	public class GuidesColorData
	{
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

		public List<GuidesColorInfo> GuidesColorList { get; private set; }

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

		private static GuidesColorData instance;
	}
}

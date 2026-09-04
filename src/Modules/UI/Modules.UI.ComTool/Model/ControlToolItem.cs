using System;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.UI.ComTool.Model
{
	// Token: 0x02000007 RID: 7
	public class ControlToolItem
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002104 File Offset: 0x00000304
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000211B File Offset: 0x0000031B
		public ControlGroupAttribute Group { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002124 File Offset: 0x00000324
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000213B File Offset: 0x0000033B
		public string DisplayName { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002144 File Offset: 0x00000344
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000215B File Offset: 0x0000035B
		public Image BitImage { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002164 File Offset: 0x00000364
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000217B File Offset: 0x0000037B
		public ModelMetaData ModelMedaData { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002184 File Offset: 0x00000384
		public EnumModelType ModelType
		{
			get
			{
				return this.ModelMedaData.ModelType;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021A1 File Offset: 0x000003A1
		public ControlToolItem(ModelMetaData uiMedaData, Image bitImage)
		{
			this.DisplayName = LanguageOption.GetValueBykey(uiMedaData.DisplayName);
			this.ModelMedaData = uiMedaData;
			this.BitImage = bitImage;
			this.Group = new ControlGroupAttribute("Control_Other", 0);
		}
	}
}

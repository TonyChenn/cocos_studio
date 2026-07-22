using System;
using System.Drawing;

namespace Modules.Communal.Preference.Model
{
	// Token: 0x02000009 RID: 9
	public class GuidesColorInfo
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002424 File Offset: 0x00000624
		// (set) Token: 0x0600001B RID: 27 RVA: 0x0000242C File Offset: 0x0000062C
		public string Name { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002435 File Offset: 0x00000635
		// (set) Token: 0x0600001D RID: 29 RVA: 0x0000243D File Offset: 0x0000063D
		public Color RenderColor { get; set; }

		// Token: 0x0600001E RID: 30 RVA: 0x00002446 File Offset: 0x00000646
		public GuidesColorInfo()
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000244E File Offset: 0x0000064E
		public GuidesColorInfo(string name, Color renderColor)
		{
			this.Name = name;
			this.RenderColor = renderColor;
		}
	}
}

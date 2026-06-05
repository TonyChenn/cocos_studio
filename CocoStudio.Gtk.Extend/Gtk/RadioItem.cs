using System;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	// Token: 0x0200000D RID: 13
	public class RadioItem
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000035D0 File Offset: 0x000017D0
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000035E7 File Offset: 0x000017E7
		public string ID { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000035F0 File Offset: 0x000017F0
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003607 File Offset: 0x00001807
		public string DisplayName { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003610 File Offset: 0x00001810
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003627 File Offset: 0x00001827
		public string Tooltip { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003630 File Offset: 0x00001830
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003647 File Offset: 0x00001847
		public XwtImage Icon { get; private set; }

		// Token: 0x0600006B RID: 107 RVA: 0x00003650 File Offset: 0x00001850
		public RadioItem(string id, string displayName, XwtImage icon, string tooltip = null)
		{
			this.ID = id;
			this.DisplayName = displayName;
			this.Icon = icon;
			this.Tooltip = (tooltip ?? string.Empty);
		}
	}
}

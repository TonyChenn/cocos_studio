using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200000F RID: 15
	public class GameFileLoadResult
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002C90 File Offset: 0x00000E90
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002CA7 File Offset: 0x00000EA7
		public AbstractNodeObject RootObject { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002CB0 File Offset: 0x00000EB0
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002CC7 File Offset: 0x00000EC7
		public TimelineAction TimelineAction { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002CD0 File Offset: 0x00000ED0
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002CE7 File Offset: 0x00000EE7
		public HashSet<string> Names { get; set; }

		// Token: 0x0600005B RID: 91 RVA: 0x00002CF0 File Offset: 0x00000EF0
		public GameFileLoadResult()
		{
			this.Names = new HashSet<string>();
		}
	}
}

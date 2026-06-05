using System;
using System.Collections.Generic;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000017 RID: 23
	public class MaterialItem
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00003147 File Offset: 0x00001347
		public MaterialItem(string container, List<string> resourceFileList)
		{
			this.ContainerFile = container;
			this.ResourceFileList = resourceFileList;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000315D File Offset: 0x0000135D
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003165 File Offset: 0x00001365
		public string ContainerFile { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000316E File Offset: 0x0000136E
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00003176 File Offset: 0x00001376
		public List<string> ResourceFileList { get; private set; }
	}
}

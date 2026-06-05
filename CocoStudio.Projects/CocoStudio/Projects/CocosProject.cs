using System;
using System.Collections.Generic;
using MonoDevelop.Projects;

namespace CocoStudio.Projects
{
	// Token: 0x02000002 RID: 2
	public class CocosProject : Project
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public static CocosProject Instance { get; private set; } = new CocosProject();

		// Token: 0x06000004 RID: 4 RVA: 0x0000206B File Offset: 0x0000026B
		private CocosProject()
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002074 File Offset: 0x00000274
		public override IEnumerable<string> GetProjectTypes()
		{
			return new string[]
			{
				".ccs"
			};
		}
	}
}

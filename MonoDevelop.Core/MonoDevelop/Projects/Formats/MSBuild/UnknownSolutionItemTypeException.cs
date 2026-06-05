using System;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001BA RID: 442
	internal class UnknownSolutionItemTypeException : InvalidOperationException
	{
		// Token: 0x060010D7 RID: 4311 RVA: 0x00041E51 File Offset: 0x00040051
		public UnknownSolutionItemTypeException() : base("Unknown solution item type")
		{
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00041E5E File Offset: 0x0004005E
		public UnknownSolutionItemTypeException(string name) : base("Unknown solution item type: " + name)
		{
			this.TypeName = name;
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x00041E78 File Offset: 0x00040078
		// (set) Token: 0x060010DA RID: 4314 RVA: 0x00041E80 File Offset: 0x00040080
		public string TypeName { get; private set; }
	}
}

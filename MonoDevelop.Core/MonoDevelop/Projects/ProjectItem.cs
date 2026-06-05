using System;
using System.Collections;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200012D RID: 301
	public class ProjectItem : IExtendedDataItem
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0002A9C7 File Offset: 0x00028BC7
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.extendedProperties == null)
				{
					this.extendedProperties = new Hashtable();
				}
				return this.extendedProperties;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0002A9E2 File Offset: 0x00028BE2
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x0002A9EA File Offset: 0x00028BEA
		internal string Condition { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x0002A9F3 File Offset: 0x00028BF3
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x0002A9FB File Offset: 0x00028BFB
		public ProjectItemFlags Flags { get; set; }

		// Token: 0x04000358 RID: 856
		private Hashtable extendedProperties;
	}
}

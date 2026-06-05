using System;
using System.Collections;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000125 RID: 293
	public class UnknownCompilationParameters : ConfigurationParameters, IExtendedDataItem
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x00028D00 File Offset: 0x00026F00
		public IDictionary ExtendedProperties
		{
			get
			{
				return this.table;
			}
		}

		// Token: 0x04000345 RID: 837
		private readonly Hashtable table = new Hashtable();
	}
}

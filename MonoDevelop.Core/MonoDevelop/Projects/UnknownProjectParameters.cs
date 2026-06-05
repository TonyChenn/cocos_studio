using System;
using System.Collections;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000126 RID: 294
	public class UnknownProjectParameters : ProjectParameters, IExtendedDataItem
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00028D1B File Offset: 0x00026F1B
		public IDictionary ExtendedProperties
		{
			get
			{
				return this.table;
			}
		}

		// Token: 0x04000346 RID: 838
		private readonly Hashtable table = new Hashtable();
	}
}

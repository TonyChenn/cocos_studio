using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000079 RID: 121
	public interface IDataItemMetadata
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003DB RID: 987
		IDictionary<string, string> ExtendedProperties { get; }
	}
}

using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200006C RID: 108
	public interface IDataItemAttribute
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600038A RID: 906
		string Name { get; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600038B RID: 907
		Type FallbackType { get; }
	}
}

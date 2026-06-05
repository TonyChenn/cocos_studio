using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000061 RID: 97
	public interface ITypeSerializer
	{
		// Token: 0x0600033B RID: 827
		DataCollection Serialize(object instance);

		// Token: 0x0600033C RID: 828
		void Deserialize(object instance, DataCollection data);

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600033D RID: 829
		SerializationContext SerializationContext { get; }
	}
}

using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200007A RID: 122
	public interface ICustomDataItem
	{
		// Token: 0x060003DC RID: 988
		DataCollection Serialize(ITypeSerializer handler);

		// Token: 0x060003DD RID: 989
		void Deserialize(ITypeSerializer handler, DataCollection data);
	}
}

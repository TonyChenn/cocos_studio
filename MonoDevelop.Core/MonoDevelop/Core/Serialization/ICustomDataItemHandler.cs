using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000077 RID: 119
	public interface ICustomDataItemHandler
	{
		// Token: 0x060003D8 RID: 984
		DataCollection Serialize(object obj, ITypeSerializer handler);

		// Token: 0x060003D9 RID: 985
		void Deserialize(object obj, ITypeSerializer handler, DataCollection data);
	}
}

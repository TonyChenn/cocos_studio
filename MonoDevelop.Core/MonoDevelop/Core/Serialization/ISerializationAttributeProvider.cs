using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200007D RID: 125
	public interface ISerializationAttributeProvider
	{
		// Token: 0x060003E1 RID: 993
		object GetCustomAttribute(object ob, Type type, bool inherit);

		// Token: 0x060003E2 RID: 994
		object[] GetCustomAttributes(object ob, Type type, bool inherit);

		// Token: 0x060003E3 RID: 995
		bool IsDefined(object ob, Type type, bool inherit);

		// Token: 0x060003E4 RID: 996
		ICustomDataItem GetCustomDataItem(object ob);

		// Token: 0x060003E5 RID: 997
		ItemMember[] GetItemMembers(Type type);
	}
}

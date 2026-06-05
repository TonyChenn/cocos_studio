using System;
using System.Linq;
using System.Reflection;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000087 RID: 135
	internal class TypeAttributeProvider : ISerializationAttributeProvider
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x0000F2EC File Offset: 0x0000D4EC
		public object GetCustomAttribute(object ob, Type type, bool inherit)
		{
			if (!(ob is MemberInfo))
			{
				return null;
			}
			if (type != null && !typeof(Attribute).IsAssignableFrom(type))
			{
				return Attribute.GetCustomAttributes((MemberInfo)ob, inherit).FirstOrDefault((Attribute a) => type.IsInstanceOfType(a));
			}
			return Attribute.GetCustomAttribute((MemberInfo)ob, type, inherit);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000F384 File Offset: 0x0000D584
		public object[] GetCustomAttributes(object ob, Type type, bool inherit)
		{
			if (!(ob is MemberInfo))
			{
				return null;
			}
			if (type != null && !typeof(Attribute).IsAssignableFrom(type))
			{
				return (from a in Attribute.GetCustomAttributes((MemberInfo)ob, inherit)
				where type.IsInstanceOfType(a)
				select a).ToArray<Attribute>();
			}
			return Attribute.GetCustomAttributes((MemberInfo)ob, type, inherit);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000F420 File Offset: 0x0000D620
		public bool IsDefined(object ob, Type type, bool inherit)
		{
			MemberInfo memberInfo = ob as MemberInfo;
			if (!(memberInfo != null))
			{
				return false;
			}
			if (type != null && !typeof(Attribute).IsAssignableFrom(type))
			{
				return Attribute.GetCustomAttributes(memberInfo, inherit).Any((Attribute a) => type.IsInstanceOfType(a));
			}
			return Attribute.IsDefined(memberInfo, type, inherit);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000F49E File Offset: 0x0000D69E
		public ICustomDataItem GetCustomDataItem(object ob)
		{
			return ob as ICustomDataItem;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000F4A6 File Offset: 0x0000D6A6
		public ItemMember[] GetItemMembers(Type type)
		{
			return new ItemMember[0];
		}

		// Token: 0x0400017A RID: 378
		public static ISerializationAttributeProvider Instance = new TypeAttributeProvider();
	}
}

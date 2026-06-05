using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.StringParsing
{
	// Token: 0x02000112 RID: 274
	public abstract class StringTagProvider<T> : IStringTagProvider
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x00028119 File Offset: 0x00026319
		IEnumerable<StringTagDescription> IStringTagProvider.GetTags(Type type)
		{
			if (typeof(T).IsAssignableFrom(type))
			{
				return this.GetTags();
			}
			return new StringTagDescription[0];
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0002813A File Offset: 0x0002633A
		object IStringTagProvider.GetTagValue(object instance, string tag)
		{
			return this.GetTagValue((T)((object)instance), tag);
		}

		// Token: 0x06000A67 RID: 2663
		public abstract object GetTagValue(T instance, string tag);

		// Token: 0x06000A68 RID: 2664
		public abstract IEnumerable<StringTagDescription> GetTags();
	}
}

using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200007C RID: 124
	public interface IPropertyFilter
	{
		// Token: 0x060003E0 RID: 992
		bool IncludeProperty(Type type, ItemProperty prop, object instance);
	}
}

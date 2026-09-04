using System;
using System.Collections.Generic;
using Mono.Addins;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000015 RID: 21
	[TypeExtensionPoint]
	public interface IPropertyFilter
	{
		// Token: 0x0600008A RID: 138
		bool CanHandle();

		// Token: 0x0600008B RID: 139
		bool CanShow(IReadOnlyList<object> selectedObjs, string propertyName);
	}
}

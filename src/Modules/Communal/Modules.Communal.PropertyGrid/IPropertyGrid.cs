using System;
using System.Collections.Generic;
using CocoStudio.Core;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200000C RID: 12
	public interface IPropertyGrid : IService
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000055 RID: 85
		// (set) Token: 0x06000056 RID: 86
		IReadOnlyList<object> SelectedObjects { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000057 RID: 87
		// (set) Token: 0x06000058 RID: 88
		bool IsShowTitle { get; set; }

		// Token: 0x06000059 RID: 89
		List<IPropertyEditor> GetEditors();

		// Token: 0x0600005A RID: 90
		IPropertyEditor GetEditor(string propertyName);

		// Token: 0x0600005B RID: 91
		void ForceRefresh();
	}
}

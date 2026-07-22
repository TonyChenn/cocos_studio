using System;
using System.Collections.Generic;
using Mono.Addins;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200000D RID: 13
	[TypeExtensionPoint]
	public interface IEditorController
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005C RID: 92
		IEnumerable<string> CorrespondProperties { get; }

		// Token: 0x0600005D RID: 93
		bool CanHandle();

		// Token: 0x0600005E RID: 94
		void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName);
	}
}

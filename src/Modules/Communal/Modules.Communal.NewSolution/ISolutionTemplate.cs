using System;
using Gtk;
using Modules.Communal.CocosAdapter;
using Mono.Addins;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000003 RID: 3
	[TypeExtensionPoint]
	public interface ISolutionTemplate
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		EnumTemplateGroup Group { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2
		bool Enable { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3
		SolutionTypeInfo Info { get; }

		// Token: 0x06000004 RID: 4
		void CreateNewSolution(CreateParams prms, CocosMonitor monitor, out string defaultScenePath);
	}
}

using System;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000002 RID: 2
	[TypeExtensionPoint]
	internal interface ICocosSupplyment
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		int Order { get; }

		// Token: 0x06000002 RID: 2
		bool RunSupplyment(string frameworkVersion, EnumProgramLanguage language, CocosMonitor monitor);

		// Token: 0x06000003 RID: 3
		bool CanSupplyment(string frameworkVersion);
	}
}

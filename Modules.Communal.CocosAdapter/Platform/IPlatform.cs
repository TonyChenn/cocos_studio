using System;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter.Platform
{
	// Token: 0x0200000F RID: 15
	[TypeExtensionPoint]
	public interface IPlatform
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005F RID: 95
		EnumPlatform PlatformType { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000060 RID: 96
		int Order { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000061 RID: 97
		bool IsShowConsoleWhenRun { get; }

		// Token: 0x06000062 RID: 98
		string GetDisplayName(EnumOperationType opType);

		// Token: 0x06000063 RID: 99
		bool CanShow(EnumOperationType opType);

		// Token: 0x06000064 RID: 100
		bool CanExecute(EnumOperationType opType, PackageParams prms);

		// Token: 0x06000065 RID: 101
		bool Execute(EnumOperationType opType, PackageParams prms, CocosMonitor monitor);
	}
}

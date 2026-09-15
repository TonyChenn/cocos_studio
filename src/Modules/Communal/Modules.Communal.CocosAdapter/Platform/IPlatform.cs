using System;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter.Platform
{
	[TypeExtensionPoint]
	public interface IPlatform
	{
		EnumPlatform PlatformType { get; }

		int Order { get; }

		bool IsShowConsoleWhenRun { get; }

		string GetDisplayName(EnumOperationType opType);

		bool CanShow(EnumOperationType opType);

		bool CanExecute(EnumOperationType opType, PackageParams prms);

		bool Execute(EnumOperationType opType, PackageParams prms, CocosMonitor monitor);
	}
}

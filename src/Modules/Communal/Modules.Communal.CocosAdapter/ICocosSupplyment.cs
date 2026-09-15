using System;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	[TypeExtensionPoint]
	internal interface ICocosSupplyment
	{
		int Order { get; }

		bool RunSupplyment(string frameworkVersion, EnumProgramLanguage language, CocosMonitor monitor);

		bool CanSupplyment(string frameworkVersion);
	}
}

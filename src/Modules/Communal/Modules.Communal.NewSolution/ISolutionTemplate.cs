using System;
using Gtk;
using Modules.Communal.CocosAdapter;
using Mono.Addins;

namespace Modules.Communal.NewSolution
{
	[TypeExtensionPoint]
	public interface ISolutionTemplate
	{
		EnumTemplateGroup Group { get; }

		bool Enable { get; }

		SolutionTypeInfo Info { get; }

		void CreateNewSolution(CreateParams prms, CocosMonitor monitor, out string defaultScenePath);
	}
}

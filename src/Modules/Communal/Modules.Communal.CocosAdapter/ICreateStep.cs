using System;
using Gtk;

namespace Modules.Communal.CocosAdapter
{
	internal interface ICreateStep
	{
		bool Run(CreateParams prms, CocosMonitor monitor);

		bool CanCreate(CreateParams prms);
	}
}

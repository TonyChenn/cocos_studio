using System;
using Modules.Communal.CocosAdapter;

namespace Modules.Communal.NewSolution
{
	public class CreateParamsSetArgs : EventArgs
	{
		public CreateParams Params { get; private set; }

		public CreateParamsSetArgs(CreateParams prms)
		{
			this.Params = prms;
		}
	}
}

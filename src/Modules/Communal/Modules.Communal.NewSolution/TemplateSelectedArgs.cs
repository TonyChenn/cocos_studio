using System;

namespace Modules.Communal.NewSolution
{
	public class TemplateSelectedArgs : EventArgs
	{
		public ISolutionTemplate SolutionTemplate { get; private set; }

		public TemplateSelectedArgs(ISolutionTemplate slnTemplate)
		{
			this.SolutionTemplate = slnTemplate;
		}
	}
}

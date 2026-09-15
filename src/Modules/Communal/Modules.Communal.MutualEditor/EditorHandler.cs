using System;
using CocoStudio.Core;

namespace Modules.Communal.MutualEditor
{
	internal class EditorHandler : BaseHandler
	{
		protected override int GetStartPort()
		{
			return 9000;
		}

		protected override void OnHandleMessageRecived(object sender, MessageArgs args)
		{
			if (args.Message != null && Services.ProjectsService != null && Services.ProjectsService.CurrentSolution != null)
			{
				if (args.Message.Data.Equals(Services.ProjectsService.CurrentSolution.FileName))
				{
					MutualCore.ShowCurrApplication();
				}
			}
		}

		private const int portNumber = 9000;
	}
}

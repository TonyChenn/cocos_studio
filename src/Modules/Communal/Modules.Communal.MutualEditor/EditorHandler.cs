using System;
using CocoStudio.Core;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x02000004 RID: 4
	internal class EditorHandler : BaseHandler
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000022B0 File Offset: 0x000004B0
		protected override int GetStartPort()
		{
			return 9000;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022C8 File Offset: 0x000004C8
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

		// Token: 0x04000004 RID: 4
		private const int portNumber = 9000;
	}
}

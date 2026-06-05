using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000197 RID: 407
	internal class LoadOperation
	{
		// Token: 0x06000FA7 RID: 4007 RVA: 0x0003A708 File Offset: 0x00038908
		public void Add(object ob)
		{
			ILoadController loadController = ob as ILoadController;
			if (loadController != null)
			{
				this.objects.Add(loadController);
				loadController.BeginLoad();
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0003A734 File Offset: 0x00038934
		public void End()
		{
			foreach (ILoadController loadController in this.objects)
			{
				loadController.EndLoad();
			}
		}

		// Token: 0x04000486 RID: 1158
		private List<ILoadController> objects = new List<ILoadController>();

		// Token: 0x04000487 RID: 1159
		public int LoadingCount;
	}
}

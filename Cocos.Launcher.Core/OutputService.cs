using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000039 RID: 57
	public class OutputService : IOutputService
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060001F9 RID: 505 RVA: 0x00009098 File Offset: 0x00007298
		// (remove) Token: 0x060001FA RID: 506 RVA: 0x000090D0 File Offset: 0x000072D0
		public event Action<string> Output = delegate(string param0)
		{
		};

		// Token: 0x060001FB RID: 507 RVA: 0x00009105 File Offset: 0x00007305
		public void Info(string info)
		{
			this.Output(info);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00009113 File Offset: 0x00007313
		public static OutputService Instance
		{
			get
			{
				if (OutputService.instance == null)
				{
					OutputService.instance = new OutputService();
				}
				return OutputService.instance;
			}
		}

		// Token: 0x040000C9 RID: 201
		private static OutputService instance;
	}
}

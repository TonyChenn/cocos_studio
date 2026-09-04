using System;
using CocoStudio.Basic;
using Gtk;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x0200001E RID: 30
	internal abstract class CreateStep : ICreateStep
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00005938 File Offset: 0x00003B38
		public bool Run(CreateParams prms, CocosMonitor monitor)
		{
			this.Monitor = monitor;
			bool result = false;
			try
			{
				result = this.OnRun(prms);
			}
			catch (Exception exception)
			{
				this.SendOutputInfo("Failed to create");
				LogConfig.Logger.Error("新建项目时出错", exception);
				result = false;
			}
			return result;
		}

		// Token: 0x060000F3 RID: 243
		protected abstract bool OnRun(CreateParams prms);

		// Token: 0x060000F4 RID: 244 RVA: 0x0000598C File Offset: 0x00003B8C
		public bool CanCreate(CreateParams prms)
		{
			return this.OnCanCreate(prms);
		}

		// Token: 0x060000F5 RID: 245
		protected abstract bool OnCanCreate(CreateParams prms);

		// Token: 0x060000F6 RID: 246 RVA: 0x00005995 File Offset: 0x00003B95
		protected void SendOutputInfo(string info)
		{
			if (this.Monitor != null && !string.IsNullOrEmpty(info))
			{
				this.Monitor.SendInfo(info);
			}
		}

		// Token: 0x04000049 RID: 73
		protected CocosMonitor Monitor;
	}
}

using System;

namespace CocoStudio.Core.ExtensionModel
{
	// Token: 0x02000007 RID: 7
	public abstract class MenuHandler
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00003181 File Offset: 0x00001381
		internal void InternalRun()
		{
			this.Run();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000318B File Offset: 0x0000138B
		internal void InternalRun(object dataItem)
		{
			this.Run(dataItem);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003196 File Offset: 0x00001396
		internal void InternalUpdate(MenuInfo info)
		{
			this.Update(info);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000031A1 File Offset: 0x000013A1
		internal void InternalUpdate(MenuArrayInfo info)
		{
			this.Update(info);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000031AC File Offset: 0x000013AC
		protected virtual void Run()
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000031AF File Offset: 0x000013AF
		protected virtual void Run(object dataItem)
		{
			this.Run();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000031B9 File Offset: 0x000013B9
		protected virtual void Update(MenuInfo info)
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000031BC File Offset: 0x000013BC
		protected virtual void Update(MenuArrayInfo info)
		{
		}
	}
}

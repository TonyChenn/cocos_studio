using System;
using CocoStudio.Basic;

namespace CocoStudio.Projects.ExtensionModel
{
	// Token: 0x0200000A RID: 10
	internal abstract class SolutionUpgrader : ISolutionUpgrader, IUpgrader
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001E RID: 30
		public abstract Version Version { get; }

		// Token: 0x0600001F RID: 31 RVA: 0x00002614 File Offset: 0x00000814
		public bool Upgrade(Solution solution)
		{
			bool result;
			try
			{
				result = this.OnUpgrade(solution);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("使用项目升级器{0}对项目进行升级时出错", this.ToString()), exception);
				result = false;
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000265C File Offset: 0x0000085C
		public bool Upgrade(string filePath)
		{
			bool result;
			try
			{
				result = this.OnUpgrade(filePath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("使用项目升级器{0}对项目进行升级时出错", this.ToString()), exception);
				result = false;
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026A4 File Offset: 0x000008A4
		protected virtual bool OnUpgrade(string filePath)
		{
			return false;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026A7 File Offset: 0x000008A7
		protected virtual bool OnUpgrade(Solution sln)
		{
			return false;
		}
	}
}

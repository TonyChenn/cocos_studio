using System;
using CocoStudio.Basic;

namespace CocoStudio.Projects.ExtensionModel
{
	internal abstract class SolutionUpgrader : ISolutionUpgrader, IUpgrader
	{
		public abstract Version Version { get; }

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

		protected virtual bool OnUpgrade(string filePath)
		{
			return false;
		}

		protected virtual bool OnUpgrade(Solution sln)
		{
			return false;
		}
	}
}

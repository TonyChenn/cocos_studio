using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.UserStatistics;

namespace CocoStudio.Core
{
	// Token: 0x02000037 RID: 55
	public class SolutionLockHandler
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00009A34 File Offset: 0x00007C34
		public string LockFolderDir
		{
			get
			{
				return SolutionLockHandler.lockFolderDir;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00009A4C File Offset: 0x00007C4C
		public static SolutionLockHandler Instance
		{
			get
			{
				if (SolutionLockHandler.solutionlockhander == null)
				{
					SolutionLockHandler.solutionlockhander = new SolutionLockHandler();
				}
				return SolutionLockHandler.solutionlockhander;
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009A7C File Offset: 0x00007C7C
		protected SolutionLockHandler()
		{
			SolutionLockHandler.lockFolderDir = Path.Combine(Option.UserCustomerConfigFolder, "LockTemp");
			if (!Directory.Exists(SolutionLockHandler.lockFolderDir))
			{
				Directory.CreateDirectory(SolutionLockHandler.lockFolderDir);
			}
			if (this.filelockhandler == null)
			{
				this.filelockhandler = FileLockHandler.Create();
			}
			UserStatisticsFactory.UnHandledExceptionEvent += this.UserStatisticsFactory_UnHandledExceptionEvent;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00009AF3 File Offset: 0x00007CF3
		private void UserStatisticsFactory_UnHandledExceptionEvent(EventArgs obj)
		{
			this.ReleaseLock();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009B00 File Offset: 0x00007D00
		public bool TryLockSolution(string slnFilePath)
		{
			bool result = false;
			string slnLockFilePath = SolutionLockHandler.getSlnLockFilePath(slnFilePath);
			try
			{
				result = this.filelockhandler.LockFile(slnLockFilePath);
			}
			catch (Exception)
			{
				LogConfig.Logger.Error(string.Format("can not lock solution {0}", slnFilePath));
			}
			return result;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009B5C File Offset: 0x00007D5C
		public bool IsSolutionLocked(string slnFilePath)
		{
			bool result = false;
			string slnLockFilePath = SolutionLockHandler.getSlnLockFilePath(slnFilePath);
			try
			{
				result = this.filelockhandler.IsFileLocked(slnLockFilePath);
			}
			catch (Exception)
			{
				LogConfig.Logger.Info(string.Format("solution {0} didn't locked ", slnFilePath), true);
			}
			return result;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009BB8 File Offset: 0x00007DB8
		public bool TryLockFile(string filePath)
		{
			bool result = false;
			try
			{
				result = this.filelockhandler.LockFile(filePath);
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(string.Format("can not lock file: {0}\r\n{1}", filePath, ex.ToString()));
			}
			return result;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00009C10 File Offset: 0x00007E10
		public bool IsFileLocked(string filePath)
		{
			return this.filelockhandler.IsFileLocked(filePath);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009C30 File Offset: 0x00007E30
		public bool ReleaseLock()
		{
			bool result = true;
			try
			{
				this.filelockhandler.ReleaseLock();
			}
			catch (Exception)
			{
				LogConfig.Logger.Error(" Current solution  release lock failed!");
				result = false;
			}
			return result;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00009C80 File Offset: 0x00007E80
		private static string getSlnLockFilePath(string slnFilePath)
		{
			if (!File.Exists(slnFilePath))
			{
				throw new FileNotFoundException(string.Format("solution {0} not exists: ", slnFilePath));
			}
			slnFilePath = Path.Combine(new string[]
			{
				slnFilePath
			});
			string md5String = FileLockHandler.GetMD5String(slnFilePath);
			return Path.Combine(SolutionLockHandler.lockFolderDir, md5String);
		}

		// Token: 0x04000116 RID: 278
		private FileLockHandler filelockhandler = null;

		// Token: 0x04000117 RID: 279
		private static string lockFolderDir = string.Empty;

		// Token: 0x04000118 RID: 280
		private static SolutionLockHandler solutionlockhander = null;
	}
}

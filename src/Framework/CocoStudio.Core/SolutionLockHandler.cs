using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.UserStatistics;

namespace CocoStudio.Core
{
	public class SolutionLockHandler
	{
		public string LockFolderDir
		{
			get
			{
				return SolutionLockHandler.lockFolderDir;
			}
		}

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

		private void UserStatisticsFactory_UnHandledExceptionEvent(EventArgs obj)
		{
			this.ReleaseLock();
		}

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

		public bool IsFileLocked(string filePath)
		{
			return this.filelockhandler.IsFileLocked(filePath);
		}

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

		private FileLockHandler filelockhandler = null;

		private static string lockFolderDir = string.Empty;

		private static SolutionLockHandler solutionlockhander = null;
	}
}

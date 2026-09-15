using System;
using System.IO;

namespace CocoStudio.Core
{
	internal class WinFileLockHandler : FileLockHandler
	{
		protected override bool OnLockFile(string filePath)
		{
			try
			{
				this.lockwritehandle = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
			}
			catch (Exception innerException)
			{
				throw new Exception("can not lock file: ", innerException);
			}
			return true;
		}

		protected override bool OnIsFileLocked(string filePath)
		{
			bool result = false;
			try
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
				{
					result = false;
					fileStream.Close();
				}
			}
			catch (IOException)
			{
				result = true;
			}
			catch (Exception innerException)
			{
				throw new Exception("can not be a lock file:", innerException);
			}
			return result;
		}

		protected override void OnReleaseLock()
		{
			using (this.lockwritehandle)
			{
				if (this.lockwritehandle == null)
				{
					throw new FileNotFoundException("no lock to release");
				}
				this.lockwritehandle.Close();
				this.lockwritehandle.Dispose();
			}
		}

		private FileStream lockwritehandle = null;
	}
}

using System;
using System.IO;

namespace CocoStudio.Core
{
	// Token: 0x02000027 RID: 39
	internal class WinFileLockHandler : FileLockHandler
	{
		// Token: 0x0600016D RID: 365 RVA: 0x00006638 File Offset: 0x00004838
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

		// Token: 0x0600016E RID: 366 RVA: 0x00006680 File Offset: 0x00004880
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

		// Token: 0x0600016F RID: 367 RVA: 0x00006704 File Offset: 0x00004904
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

		// Token: 0x040000D9 RID: 217
		private FileStream lockwritehandle = null;
	}
}

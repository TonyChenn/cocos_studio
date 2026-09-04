using System;
using Mono.Unix.Native;

namespace CocoStudio.Core
{
	// Token: 0x02000026 RID: 38
	internal class UnixFileLockHandler : FileLockHandler
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00006480 File Offset: 0x00004680
		public UnixFileLockHandler()
		{
			this.flockhandle.l_len = 0L;
			this.flockhandle.l_pid = Syscall.getpid();
			this.flockhandle.l_start = 0L;
			this.flockhandle.l_type = LockType.F_WRLCK;
			this.flockhandle.l_whence = SeekFlags.SEEK_SET;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000064E0 File Offset: 0x000046E0
		protected override bool OnLockFile(string filepath)
		{
			this.flockhandle.l_type = LockType.F_WRLCK;
			this.flockfd = Syscall.open(filepath, OpenFlags.O_CREAT | OpenFlags.O_RDWR, FilePermissions.DEFFILEMODE);
			int num = Syscall.fcntl(this.flockfd, FcntlCommand.F_SETLK, ref this.flockhandle);
			if (num != -1)
			{
				return true;
			}
			throw new InvalidOperationException(filepath + "has already been locked!");
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006548 File Offset: 0x00004748
		protected override bool OnIsFileLocked(string filePath)
		{
			bool flag = true;
			bool result;
			if (filePath == base.CurrLockFilePath)
			{
				result = flag;
			}
			else
			{
				this.flockhandle.l_type = LockType.F_WRLCK;
				int num = Syscall.open(filePath, OpenFlags.O_RDWR, FilePermissions.DEFFILEMODE);
				int num2 = Syscall.fcntl(num, FcntlCommand.F_SETLK, ref this.flockhandle);
				if (num2 != -1)
				{
					flag = false;
				}
				this.flockhandle.l_type = LockType.F_UNLCK;
				Syscall.fcntl(num, FcntlCommand.F_SETLK, ref this.flockhandle);
				Syscall.close(num);
				result = flag;
			}
			return result;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000065C8 File Offset: 0x000047C8
		protected override void OnReleaseLock()
		{
			this.flockhandle.l_type = LockType.F_UNLCK;
			int num = Syscall.fcntl(this.flockfd, FcntlCommand.F_SETLK, ref this.flockhandle);
			if (num == -1)
			{
				throw new InvalidOperationException("Release Lock File failed: " + base.CurrLockFilePath);
			}
			Syscall.close(this.flockfd);
		}

		// Token: 0x040000D7 RID: 215
		private Flock flockhandle;

		// Token: 0x040000D8 RID: 216
		private int flockfd = 0;
	}
}

using System;
using Mono.Unix.Native;

namespace CocoStudio.Core
{
	internal class UnixFileLockHandler : FileLockHandler
	{
		public UnixFileLockHandler()
		{
			this.flockhandle.l_len = 0L;
			this.flockhandle.l_pid = Syscall.getpid();
			this.flockhandle.l_start = 0L;
			this.flockhandle.l_type = LockType.F_WRLCK;
			this.flockhandle.l_whence = SeekFlags.SEEK_SET;
		}

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

		private Flock flockhandle;

		private int flockfd = 0;
	}
}

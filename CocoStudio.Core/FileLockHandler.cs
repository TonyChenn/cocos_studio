using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MonoDevelop.Core;

namespace CocoStudio.Core
{
	// Token: 0x02000025 RID: 37
	internal class FileLockHandler : IDisposable
	{
		// Token: 0x0600015D RID: 349 RVA: 0x000062AD File Offset: 0x000044AD
		protected FileLockHandler()
		{
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000062C4 File Offset: 0x000044C4
		public string CurrLockFilePath
		{
			get
			{
				return this.lockfilepath;
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000062DC File Offset: 0x000044DC
		public static FileLockHandler Create()
		{
			FileLockHandler result;
			if (Platform.IsWindows)
			{
				result = new WinFileLockHandler();
			}
			else
			{
				result = new UnixFileLockHandler();
			}
			return result;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006308 File Offset: 0x00004508
		public bool LockFile(string filepath)
		{
			bool result = this.OnLockFile(filepath);
			this.lockfilepath = filepath;
			return result;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000632C File Offset: 0x0000452C
		protected virtual bool OnLockFile(string filepath)
		{
			return false;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006340 File Offset: 0x00004540
		public bool IsFileLocked(string filePath)
		{
			bool result;
			if (!File.Exists(filePath))
			{
				result = false;
			}
			else
			{
				bool flag = this.OnIsFileLocked(filePath);
				result = flag;
			}
			return result;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006370 File Offset: 0x00004570
		protected virtual bool OnIsFileLocked(string filePath)
		{
			return false;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006384 File Offset: 0x00004584
		public void ReleaseLock()
		{
			if (!File.Exists(this.lockfilepath))
			{
				throw new FileNotFoundException("Locked File in Handler Not Found: ", this.lockfilepath);
			}
			this.OnReleaseLock();
			File.Delete(this.lockfilepath);
			this.lockfilepath = string.Empty;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000063D0 File Offset: 0x000045D0
		protected virtual void OnReleaseLock()
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000063D4 File Offset: 0x000045D4
		public static string GetMD5String(string filepath)
		{
			filepath = Path.Combine(new string[]
			{
				filepath
			});
			MD5 md = MD5.Create();
			byte[] array = md.ComputeHash(Encoding.Default.GetBytes(filepath));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006450 File Offset: 0x00004650
		public void Dispose()
		{
			try
			{
				this.ReleaseLock();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x040000D6 RID: 214
		private string lockfilepath = string.Empty;
	}
}

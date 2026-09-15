using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MonoDevelop.Core;

namespace CocoStudio.Core
{
	internal class FileLockHandler : IDisposable
	{
		protected FileLockHandler()
		{
		}

		public string CurrLockFilePath
		{
			get
			{
				return this.lockfilepath;
			}
		}

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

		public bool LockFile(string filepath)
		{
			bool result = this.OnLockFile(filepath);
			this.lockfilepath = filepath;
			return result;
		}

		protected virtual bool OnLockFile(string filepath)
		{
			return false;
		}

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

		protected virtual bool OnIsFileLocked(string filePath)
		{
			return false;
		}

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

		protected virtual void OnReleaseLock()
		{
		}

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

		private string lockfilepath = string.Empty;
	}
}

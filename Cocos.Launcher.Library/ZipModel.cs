using System;
using System.IO;
using System.Threading.Tasks;
using CocoStudio.Basic;
using GLib;
using ICSharpCode.SharpZipLib.Zip;

namespace Cocos.Launcher.Library
{
	// Token: 0x0200000A RID: 10
	public class ZipModel
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003216 File Offset: 0x00001416
		// (set) Token: 0x0600003E RID: 62 RVA: 0x0000321E File Offset: 0x0000141E
		public string Error { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003227 File Offset: 0x00001427
		// (set) Token: 0x06000040 RID: 64 RVA: 0x0000322F File Offset: 0x0000142F
		public bool IsSucceeded { get; private set; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000041 RID: 65 RVA: 0x00003238 File Offset: 0x00001438
		// (remove) Token: 0x06000042 RID: 66 RVA: 0x00003270 File Offset: 0x00001470
		public event EventHandler<UnZipEndInfoEventArgs> UnZipEndInfoEvent;

		// Token: 0x06000043 RID: 67 RVA: 0x000032A8 File Offset: 0x000014A8
		public static string GetRootFolderName(string zipFilePath)
		{
			if (!File.Exists(zipFilePath))
			{
				return string.Empty;
			}
			string result;
			try
			{
				using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
				{
					ZipEntry nextEntry = zipInputStream.GetNextEntry();
					if (nextEntry == null || !nextEntry.Name.EndsWith("/"))
					{
						result = string.Empty;
					}
					else
					{
						result = nextEntry.Name.Substring(0, nextEntry.Name.Length - 1);
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("Failed to get the root folder name in \"{0}\".", zipFilePath), exception);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003358 File Offset: 0x00001558
		public void UnZip(string zipFilePath, string unZipDir = "")
		{
			this.UnpackZipFile(zipFilePath, ref unZipDir);
			if (this.UnZipEndInfoEvent != null)
			{
				this.UnZipEndInfoEvent(this, new UnZipEndInfoEventArgs(this.IsSucceeded, this.Error, zipFilePath));
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003414 File Offset: 0x00001614
		public void UnZipAsync(string zipFilePath, string unZipDir = "")
		{
			Task task = new Task(delegate()
			{
				this.UnpackZipFile(zipFilePath, ref unZipDir);
				Timeout.Add(0U, delegate
				{
					if (this.UnZipEndInfoEvent != null)
					{
						this.UnZipEndInfoEvent(this, new UnZipEndInfoEventArgs(this.IsSucceeded, this.Error, zipFilePath));
					}
					return false;
				});
			});
			task.Start();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003454 File Offset: 0x00001654
		private void UnpackZipFile(string zipFilePath, ref string unZipDir)
		{
			try
			{
				if (!File.Exists(zipFilePath))
				{
					this.Error = "The file is not exist.";
					this.IsSucceeded = false;
				}
				else
				{
					this.zipFilePath = zipFilePath;
					if (unZipDir == string.Empty)
					{
						unZipDir = Path.GetDirectoryName(zipFilePath);
					}
					if (!Directory.Exists(unZipDir))
					{
						Directory.CreateDirectory(unZipDir);
					}
					using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
					{
						ZipEntry nextEntry;
						while ((nextEntry = zipInputStream.GetNextEntry()) != null)
						{
							string path2 = ZipModel.GetSafeExtractionPath(unZipDir, nextEntry.Name);
							string directoryName = Path.GetDirectoryName(path2);
							string fileName = Path.GetFileName(nextEntry.Name);
							if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
							{
								Directory.CreateDirectory(directoryName);
							}
							if (fileName != string.Empty)
							{
								using (FileStream fileStream = File.Create(path2))
								{
									if (nextEntry.Size != 0L)
									{
										byte[] array = new byte[4096];
										for (int i = zipInputStream.Read(array, 0, array.Length); i > 0; i = zipInputStream.Read(array, 0, array.Length))
										{
											fileStream.Write(array, 0, i);
										}
									}
								}
							}
						}
						this.IsSucceeded = true;
					}
				}
			}
			catch (Exception ex)
			{
				this.IsSucceeded = false;
				this.Error = "Failed to unpack:  " + ex.Message;
				LogConfig.Logger.Error(string.Format("Failed to unpack \"{0}\".", zipFilePath), ex);
			}
		}

		private static string GetSafeExtractionPath(string unZipDir, string entryName)
		{
			string fullPath = Path.GetFullPath(unZipDir);
			string text = fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
			string fullPath2 = Path.GetFullPath(Path.Combine(text, entryName));
			if (!fullPath2.StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidDataException("The zip entry is outside the destination directory: " + entryName);
			}
			return fullPath2;
		}

		// Token: 0x0400001D RID: 29
		private const int bufferSize = 4096;

		// Token: 0x0400001E RID: 30
		private string zipFilePath;
	}
}

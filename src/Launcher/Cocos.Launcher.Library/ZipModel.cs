using System;
using System.IO;
using System.Threading.Tasks;
using CocoStudio.Basic;
using GLib;
using ICSharpCode.SharpZipLib.Zip;

namespace Cocos.Launcher.Library
{
	public class ZipModel
	{
		public string Error { get; private set; }

		public bool IsSucceeded { get; private set; }

		public event EventHandler<UnZipEndInfoEventArgs> UnZipEndInfoEvent;

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

		public void UnZip(string zipFilePath, string unZipDir = "")
		{
			this.UnpackZipFile(zipFilePath, ref unZipDir);
			if (this.UnZipEndInfoEvent != null)
			{
				this.UnZipEndInfoEvent(this, new UnZipEndInfoEventArgs(this.IsSucceeded, this.Error, zipFilePath));
			}
		}

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

		private const int bufferSize = 4096;

		private string zipFilePath;
	}
}

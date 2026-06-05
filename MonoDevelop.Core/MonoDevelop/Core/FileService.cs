using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Mono.Addins;
using Mono.Unix.Native;
using MonoDevelop.Core.FileSystem;

namespace MonoDevelop.Core
{
	// Token: 0x02000035 RID: 53
	public static class FileService
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00007021 File Offset: 0x00005221
		public static string ApplicationRootPath
		{
			get
			{
				return FileService.applicationRootPath;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000703C File Offset: 0x0000523C
		static FileService()
		{
			AddinManager.ExtensionChanged += delegate(object sender, ExtensionEventArgs args)
			{
				if (args.PathChanged("/MonoDevelop/Core/FileSystemExtensions"))
				{
					FileService.UpdateExtensions();
				}
			};
			FileService.UpdateExtensions();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000070D4 File Offset: 0x000052D4
		private static void UpdateExtensions()
		{
			if (!Runtime.Initialized)
			{
				FileService.fileSystemChain = FileService.defaultExtension;
				return;
			}
			FileSystemExtension[] array = AddinManager.GetExtensionObjects("/MonoDevelop/Core/FileSystemExtensions", typeof(FileSystemExtension)).Cast<FileSystemExtension>().ToArray<FileSystemExtension>();
			for (int i = 0; i < array.Length - 1; i++)
			{
				array[i].Next = array[i + 1];
			}
			if (array.Length > 0)
			{
				array[array.Length - 1].Next = FileService.defaultExtension;
				FileService.fileSystemChain = array[0];
				return;
			}
			FileService.fileSystemChain = FileService.defaultExtension;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000715C File Offset: 0x0000535C
		public static FilePath ResolveFullPath(FilePath path)
		{
			FilePath result;
			try
			{
				result = FileService.GetFileSystemForPath(path, false).ResolveFullPath(path);
			}
			catch (Exception ex)
			{
				if (!FileService.HandleError(GettextCatalog.GetString("Can't resolve full path {0}", path), ex))
				{
					throw;
				}
				result = FilePath.Empty;
			}
			return result;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000071B4 File Offset: 0x000053B4
		public static void DeleteFile(string fileName)
		{
			try
			{
				FileService.GetFileSystemForPath(fileName, false).DeleteFile(fileName);
			}
			catch (Exception ex)
			{
				if (!FileService.HandleError(GettextCatalog.GetString("Can't remove file {0}", fileName), ex))
				{
					throw;
				}
				return;
			}
			FileService.OnFileRemoved(new FileEventArgs(fileName, false));
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007210 File Offset: 0x00005410
		public static void DeleteDirectory(string path)
		{
			try
			{
				FileService.GetFileSystemForPath(path, true).DeleteDirectory(path);
			}
			catch (Exception ex)
			{
				if (!FileService.HandleError(GettextCatalog.GetString("Can't remove directory {0}", path), ex))
				{
					throw;
				}
				return;
			}
			FileService.OnFileRemoved(new FileEventArgs(path, true));
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000726C File Offset: 0x0000546C
		public static void RenameFile(string oldName, string newName)
		{
			if (Path.GetFileName(oldName) != newName)
			{
				FilePath targetFile = oldName.ParentDirectory.Combine(new string[]
				{
					newName
				});
				FileService.InternalRenameFile(oldName, newName);
				FileService.OnFileRenamed(new FileCopyEventArgs(oldName, targetFile, false));
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000072C4 File Offset: 0x000054C4
		public static void RenameDirectory(string oldName, string newName)
		{
			if (Path.GetFileName(oldName) != newName)
			{
				string text = Path.Combine(Path.GetDirectoryName(oldName), newName);
				FileService.InternalMoveDirectory(oldName, text);
				FileService.OnFileRenamed(new FileCopyEventArgs(oldName, text, true));
				FileService.OnFileCreated(new FileEventArgs(text, false));
				FileService.OnFileRemoved(new FileEventArgs(oldName, false));
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000732C File Offset: 0x0000552C
		public static void CopyFile(string srcFile, string dstFile)
		{
			FileService.GetFileSystemForPath(dstFile, false).CopyFile(srcFile, dstFile, true);
			FileService.OnFileCopied(new FileCopyEventArgs(srcFile, dstFile, false));
			FileService.OnFileCreated(new FileEventArgs(dstFile, false));
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000737C File Offset: 0x0000557C
		public static void MoveFile(string srcFile, string dstFile)
		{
			FileService.InternalMoveFile(srcFile, dstFile);
			FileService.OnFileMoved(new FileCopyEventArgs(srcFile, dstFile, false));
			FileService.OnFileCreated(new FileEventArgs(dstFile, false));
			FileService.OnFileRemoved(new FileEventArgs(srcFile, false));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000073CC File Offset: 0x000055CC
		private static void InternalMoveFile(string srcFile, string dstFile)
		{
			FileSystemExtension fileSystemForPath = FileService.GetFileSystemForPath(srcFile, false);
			FileSystemExtension fileSystemForPath2 = FileService.GetFileSystemForPath(dstFile, false);
			if (fileSystemForPath == fileSystemForPath2)
			{
				fileSystemForPath.MoveFile(srcFile, dstFile);
				return;
			}
			fileSystemForPath2.CopyFile(srcFile, dstFile, true);
			fileSystemForPath.DeleteFile(srcFile);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007420 File Offset: 0x00005620
		private static void InternalRenameFile(string srcFile, string newName)
		{
			FileSystemExtension fileSystemForPath = FileService.GetFileSystemForPath(srcFile, false);
			fileSystemForPath.RenameFile(srcFile, newName);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007442 File Offset: 0x00005642
		public static void CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				FileService.GetFileSystemForPath(path, true).CreateDirectory(path);
				FileService.OnFileCreated(new FileEventArgs(path, true));
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000746F File Offset: 0x0000566F
		public static void CopyDirectory(string srcPath, string dstPath)
		{
			FileService.GetFileSystemForPath(dstPath, true).CopyDirectory(srcPath, dstPath);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000748C File Offset: 0x0000568C
		public static void MoveDirectory(string srcPath, string dstPath)
		{
			FileService.InternalMoveDirectory(srcPath, dstPath);
			FileService.OnFileMoved(new FileCopyEventArgs(srcPath, dstPath, true));
			FileService.OnFileCreated(new FileEventArgs(dstPath, true));
			FileService.OnFileRemoved(new FileEventArgs(srcPath, true));
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000074DC File Offset: 0x000056DC
		private static void InternalMoveDirectory(string srcPath, string dstPath)
		{
			FileSystemExtension fileSystemForPath = FileService.GetFileSystemForPath(srcPath, true);
			FileSystemExtension fileSystemForPath2 = FileService.GetFileSystemForPath(dstPath, true);
			if (fileSystemForPath == fileSystemForPath2)
			{
				fileSystemForPath.MoveDirectory(srcPath, dstPath);
				return;
			}
			fileSystemForPath2.CopyDirectory(srcPath, dstPath);
			fileSystemForPath.DeleteDirectory(srcPath);
		}

		/// <summary>
		/// Requests permission for modifying a file
		/// </summary>
		/// <param name="fileName">The file to be modified</param>
		/// <param name="throwIfFails">If set to false, it will catch the exception that would've been thrown.</param>
		/// <remarks>This method must be called before trying to write any file. It throws an exception if permission is not granted.</remarks>
		// Token: 0x060001AB RID: 427 RVA: 0x00007530 File Offset: 0x00005730
		public static bool RequestFileEdit(FilePath fileName, bool throwIfFails = true)
		{
			return FileService.RequestFileEdit(new FilePath[]
			{
				fileName
			}, throwIfFails);
		}

		/// <summary>
		/// Requests permission for modifying a set of files
		/// </summary>
		/// <param name="fileNames">Files</param>
		/// <remarks>This method must be called before trying to write any file. It throws an exception if permission is not granted.</remarks>
		// Token: 0x060001AC RID: 428 RVA: 0x00007568 File Offset: 0x00005768
		public static bool RequestFileEdit(IEnumerable<FilePath> fileNames, bool throwIfFails = true)
		{
			bool result;
			try
			{
				foreach (IGrouping<FileSystemExtension, FilePath> grouping in from f in fileNames
				group f by FileService.GetFileSystemForPath(f, false))
				{
					grouping.Key.RequestFileEdit(grouping);
				}
				result = true;
			}
			catch (Exception ex)
			{
				if (throwIfFails)
				{
					throw;
				}
				LoggingService.LogError("File can't be written", ex);
				result = false;
			}
			return result;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007600 File Offset: 0x00005800
		public static void NotifyFileChanged(FilePath fileName)
		{
			FileService.NotifyFileChanged(fileName, false);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000760C File Offset: 0x0000580C
		public static void NotifyFileChanged(FilePath fileName, bool autoReload)
		{
			FileService.NotifyFilesChanged(new FilePath[]
			{
				fileName
			}, autoReload);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007634 File Offset: 0x00005834
		public static void NotifyFilesChanged(IEnumerable<FilePath> files)
		{
			FileService.NotifyFilesChanged(files, false);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000764C File Offset: 0x0000584C
		public static void NotifyFilesChanged(IEnumerable<FilePath> files, bool autoReload)
		{
			try
			{
				foreach (IGrouping<FileSystemExtension, FilePath> grouping in from f in files
				group f by FileService.GetFileSystemForPath(f, false))
				{
					grouping.Key.NotifyFilesChanged(grouping);
				}
				FileService.OnFileChanged(new FileEventArgs(files, false, autoReload));
			}
			catch (Exception ex)
			{
				LoggingService.LogError("File change notification failed", ex);
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000076E4 File Offset: 0x000058E4
		public static void NotifyFileRemoved(string fileName)
		{
			FileService.NotifyFilesRemoved(new FilePath[]
			{
				fileName
			});
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007710 File Offset: 0x00005910
		public static void NotifyFilesRemoved(IEnumerable<FilePath> files)
		{
			try
			{
				FileService.OnFileRemoved(new FileEventArgs(files, false));
			}
			catch (Exception ex)
			{
				LoggingService.LogError("File remove notification failed", ex);
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000774C File Offset: 0x0000594C
		internal static FileSystemExtension GetFileSystemForPath(string path, bool isDirectory)
		{
			FileSystemExtension next = FileService.fileSystemChain;
			while (next != null && !next.CanHandlePath(path, isDirectory))
			{
				next = next.Next;
			}
			return next;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000777B File Offset: 0x0000597B
		private static bool IsSeparator(char ch)
		{
			return ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar || ch == Path.VolumeSeparatorChar;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007798 File Offset: 0x00005998
		public unsafe static string AbsoluteToRelativePath(string baseDirectoryPath, string absPath)
		{
			if (!Path.IsPathRooted(absPath) || string.IsNullOrEmpty(baseDirectoryPath))
			{
				return absPath;
			}
			absPath = FileService.GetFullPath(absPath);
			baseDirectoryPath = FileService.GetFullPath(baseDirectoryPath).TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar
			});
			fixed (char* ptr = baseDirectoryPath)
			{
				fixed (char* ptr2 = absPath)
				{
					char* ptr3 = ptr + baseDirectoryPath.Length;
					char* ptr4 = ptr2 + absPath.Length;
					char* ptr5 = ptr4;
					char* ptr6 = ptr3;
					int num = 0;
					char* ptr7 = ptr2;
					char* ptr8 = ptr;
					while (ptr7 < ptr4 && *ptr7 == *ptr8)
					{
						if (FileService.IsSeparator(*ptr7))
						{
							num++;
							ptr5 = ptr7 + 1;
							ptr6 = ptr8;
						}
						ptr7++;
						ptr8++;
						if (ptr8 >= ptr3)
						{
							if (ptr7 >= ptr4 || FileService.IsSeparator(*ptr7))
							{
								num++;
								ptr5 = ptr7 + 1;
								ptr6 = ptr8;
								break;
							}
							break;
						}
					}
					string result;
					if (num == 0)
					{
						result = absPath;
					}
					else if (ptr5 >= ptr4)
					{
						result = ".";
					}
					else
					{
						if (ptr7 >= ptr4 && FileService.IsSeparator(*ptr8))
						{
							ptr5 = ptr7 + 1;
							ptr6 = ptr8;
						}
						int num2 = 0;
						while (ptr6 < ptr3)
						{
							if (FileService.IsSeparator(*ptr6))
							{
								num2++;
							}
							ptr6++;
						}
						long num3 = (long)((num2 * 2 + num2) * 2 + ptr4 / 2 - ptr5);
						char[] array = new char[num3];
						fixed (char* ptr9 = array)
						{
							char* ptr10 = ptr9;
							for (int i = 0; i < num2; i++)
							{
								*(ptr10++) = '.';
								*(ptr10++) = '.';
								*(ptr10++) = Path.DirectorySeparatorChar;
							}
							while (ptr5 < ptr4)
							{
								*(ptr10++) = *(ptr5++);
							}
						}
						result = new string(array);
					}
					return result;
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000797C File Offset: 0x00005B7C
		public static string RelativeToAbsolutePath(string baseDirectoryPath, string relPath)
		{
			return Path.GetFullPath(Path.Combine(baseDirectoryPath, relPath));
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000798A File Offset: 0x00005B8A
		public static bool IsValidPath(string fileName)
		{
			return !string.IsNullOrEmpty(fileName) && !(fileName.Trim() == string.Empty) && fileName.IndexOfAny(Path.GetInvalidPathChars()) < 0;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000079B9 File Offset: 0x00005BB9
		public static bool IsValidFileName(string fileName)
		{
			return !string.IsNullOrEmpty(fileName) && !(fileName.Trim() == string.Empty) && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000079E8 File Offset: 0x00005BE8
		public static bool IsDirectory(string filename)
		{
			return Directory.Exists(filename);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000079F0 File Offset: 0x00005BF0
		public static string GetFullPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!Platform.IsWindows || path.IndexOf('*') == -1)
			{
				return Path.GetFullPath(path);
			}
			path = path.Replace("*", FileService.wildcardMarker);
			path = Path.GetFullPath(path);
			return path.Replace(FileService.wildcardMarker, "*");
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00007A50 File Offset: 0x00005C50
		public static string CreateTempDirectory()
		{
			Random random = new Random();
			string text;
			do
			{
				text = Path.Combine(Path.GetTempPath(), "mdTmpDir" + random.Next());
			}
			while (Directory.Exists(text));
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007A94 File Offset: 0x00005C94
		public static string NormalizeRelativePath(string path)
		{
			string text = path.Trim(new char[]
			{
				Path.DirectorySeparatorChar,
				' '
			});
			while (text.StartsWith("." + Path.DirectorySeparatorChar))
			{
				text = text.Substring(2);
				text = text.Trim(new char[]
				{
					Path.DirectorySeparatorChar
				});
			}
			if (!(text == "."))
			{
				return text;
			}
			return "";
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007B10 File Offset: 0x00005D10
		public static void SystemRename(string sourceFile, string destFile)
		{
			if (string.IsNullOrEmpty(sourceFile))
			{
				throw new ArgumentException("sourceFile");
			}
			if (string.IsNullOrEmpty(destFile))
			{
				throw new ArgumentException("destFile");
			}
			if (Platform.IsWindows)
			{
				string text = null;
				if (File.Exists(destFile))
				{
					do
					{
						text = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
					}
					while (File.Exists(text));
					File.Move(destFile, text);
				}
				try
				{
					try
					{
						File.Move(sourceFile, destFile);
					}
					catch
					{
						try
						{
							if (text != null)
							{
								File.Move(text, destFile);
							}
						}
						catch
						{
							text = null;
						}
						throw;
					}
					return;
				}
				finally
				{
					if (text != null)
					{
						try
						{
							File.Delete(text);
						}
						catch
						{
						}
					}
				}
			}
			if (Stdlib.rename(sourceFile, destFile) != 0)
			{
				Errno lastError = Stdlib.GetLastError();
				if (lastError <= 13)
				{
					switch (lastError)
					{
					case 1:
						break;
					case 2:
						throw new FileNotFoundException();
					default:
						if (lastError != 13)
						{
							goto IL_F7;
						}
						break;
					}
					throw new UnauthorizedAccessException();
				}
				switch (lastError)
				{
				case 20:
					throw new DirectoryNotFoundException();
				case 21:
					break;
				case 22:
					throw new InvalidOperationException();
				default:
					if (lastError == 36)
					{
						throw new PathTooLongException();
					}
					break;
				}
				IL_F7:
				throw new IOException();
			}
		}

		/// <summary>
		/// Renames a directory
		/// </summary>
		/// <param name="sourceDir">Source directory</param>
		/// <param name="destDir">Destination directory</param>
		/// <remarks>
		/// It works like Directory.Move, but it supports changing the case of a directory name in case-insensitive file systems
		/// </remarks>
		// Token: 0x060001BE RID: 446 RVA: 0x00007C50 File Offset: 0x00005E50
		public static void SystemDirectoryRename(string sourceDir, string destDir)
		{
			if (Directory.Exists(destDir) && string.Equals(Path.GetFullPath(sourceDir), Path.GetFullPath(destDir), StringComparison.CurrentCultureIgnoreCase))
			{
				string text = Directory.GetDirectories(Path.GetDirectoryName(destDir), Path.GetFileName(destDir)).FirstOrDefault<string>();
				if (text == null || Path.GetFileName(text) == Path.GetFileName(sourceDir))
				{
					string text2 = destDir + ".renaming";
					int num = 0;
					while (Directory.Exists(text2) || File.Exists(text2))
					{
						text2 = destDir + ".renaming_" + num++;
					}
					Directory.Move(sourceDir, text2);
					try
					{
						Directory.Move(text2, destDir);
					}
					catch
					{
						Directory.Move(text2, sourceDir);
					}
					return;
				}
			}
			Directory.Move(sourceDir, destDir);
		}

		/// <summary>
		/// Removes the directory if it's empty.
		/// </summary>
		// Token: 0x060001BF RID: 447 RVA: 0x00007D14 File Offset: 0x00005F14
		public static void RemoveDirectoryIfEmpty(string directory)
		{
			if (Directory.Exists(directory) && !Directory.GetFiles(directory).Any<string>())
			{
				Directory.Delete(directory);
			}
		}

		/// <summary>
		/// Makes the path separators native.
		/// </summary>
		// Token: 0x060001C0 RID: 448 RVA: 0x00007D34 File Offset: 0x00005F34
		public static string MakePathSeparatorsNative(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			char oldChar = (Path.DirectorySeparatorChar == '\\') ? '/' : '\\';
			return path.Replace(oldChar, Path.DirectorySeparatorChar);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007D67 File Offset: 0x00005F67
		private static bool HandleError(string message, Exception ex)
		{
			return FileService.errorHandler != null && FileService.errorHandler(message, ex);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00007D7E File Offset: 0x00005F7E
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00007D85 File Offset: 0x00005F85
		public static FileServiceErrorHandler ErrorHandler
		{
			get
			{
				return FileService.errorHandler;
			}
			set
			{
				FileService.errorHandler = value;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007D8D File Offset: 0x00005F8D
		public static void FreezeEvents()
		{
			FileService.eventQueue.Freeze();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00007D99 File Offset: 0x00005F99
		public static void ThawEvents()
		{
			FileService.eventQueue.Thaw();
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060001C6 RID: 454 RVA: 0x00007DA8 File Offset: 0x00005FA8
		// (remove) Token: 0x060001C7 RID: 455 RVA: 0x00007DDC File Offset: 0x00005FDC
		public static event EventHandler<FileEventArgs> FileCreated;

		// Token: 0x060001C8 RID: 456 RVA: 0x00007E18 File Offset: 0x00006018
		private static void OnFileCreated(FileEventArgs args)
		{
			foreach (FileEventInfo fileEventInfo in args)
			{
				if (fileEventInfo.IsDirectory)
				{
					Counters.DirectoriesCreated = ++Counters.DirectoriesCreated;
				}
				else
				{
					Counters.FilesCreated = ++Counters.FilesCreated;
				}
			}
			FileService.eventQueue.RaiseEvent(() => FileService.FileCreated, args);
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060001C9 RID: 457 RVA: 0x00007EAC File Offset: 0x000060AC
		// (remove) Token: 0x060001CA RID: 458 RVA: 0x00007EE0 File Offset: 0x000060E0
		public static event EventHandler<FileCopyEventArgs> FileCopied;

		// Token: 0x060001CB RID: 459 RVA: 0x00007F1A File Offset: 0x0000611A
		private static void OnFileCopied(FileCopyEventArgs args)
		{
			FileService.eventQueue.RaiseEvent(() => FileService.FileCopied, args);
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060001CC RID: 460 RVA: 0x00007F44 File Offset: 0x00006144
		// (remove) Token: 0x060001CD RID: 461 RVA: 0x00007F78 File Offset: 0x00006178
		public static event EventHandler<FileCopyEventArgs> FileMoved;

		// Token: 0x060001CE RID: 462 RVA: 0x00007FB2 File Offset: 0x000061B2
		private static void OnFileMoved(FileCopyEventArgs args)
		{
			FileService.eventQueue.RaiseEvent(() => FileService.FileMoved, args);
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060001CF RID: 463 RVA: 0x00007FDC File Offset: 0x000061DC
		// (remove) Token: 0x060001D0 RID: 464 RVA: 0x00008010 File Offset: 0x00006210
		public static event EventHandler<FileCopyEventArgs> FileRenamed;

		// Token: 0x060001D1 RID: 465 RVA: 0x0000804C File Offset: 0x0000624C
		private static void OnFileRenamed(FileCopyEventArgs args)
		{
			foreach (FileCopyEventInfo fileCopyEventInfo in args)
			{
				if (fileCopyEventInfo.IsDirectory)
				{
					Counters.DirectoriesRenamed = ++Counters.DirectoriesRenamed;
				}
				else
				{
					Counters.FilesRenamed = ++Counters.FilesRenamed;
				}
			}
			FileService.eventQueue.RaiseEvent(() => FileService.FileRenamed, args);
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060001D2 RID: 466 RVA: 0x000080E0 File Offset: 0x000062E0
		// (remove) Token: 0x060001D3 RID: 467 RVA: 0x00008114 File Offset: 0x00006314
		public static event EventHandler<FileEventArgs> FileRemoved;

		// Token: 0x060001D4 RID: 468 RVA: 0x00008150 File Offset: 0x00006350
		private static void OnFileRemoved(FileEventArgs args)
		{
			foreach (FileEventInfo fileEventInfo in args)
			{
				if (fileEventInfo.IsDirectory)
				{
					Counters.DirectoriesRemoved = ++Counters.DirectoriesRemoved;
				}
				else
				{
					Counters.FilesRemoved = ++Counters.FilesRemoved;
				}
			}
			FileService.eventQueue.RaiseEvent(() => FileService.FileRemoved, args);
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060001D5 RID: 469 RVA: 0x000081E4 File Offset: 0x000063E4
		// (remove) Token: 0x060001D6 RID: 470 RVA: 0x00008218 File Offset: 0x00006418
		public static event EventHandler<FileEventArgs> FileChanged;

		// Token: 0x060001D7 RID: 471 RVA: 0x00008252 File Offset: 0x00006452
		private static void OnFileChanged(FileEventArgs args)
		{
			Counters.FileChangeNotifications = ++Counters.FileChangeNotifications;
			FileService.eventQueue.RaiseEvent(() => FileService.FileChanged, null, args);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00008498 File Offset: 0x00006698
		public static Task<bool> UpdateDownloadedCacheFile(string url, string cacheFile, Func<Stream, bool> validateDownload = null, CancellationToken ct = default(CancellationToken))
		{
			return WebRequestHelper.GetResponseAsync(() => (HttpWebRequest)WebRequest.Create(url), delegate(HttpWebRequest r)
			{
				FileInfo fileInfo = new FileInfo(cacheFile);
				if (fileInfo.Exists)
				{
					r.IfModifiedSince = fileInfo.LastWriteTime;
				}
			}, ct).ContinueWith<bool>(delegate(Task<HttpWebResponse> t)
			{
				bool flag = true;
				string text = cacheFile + ".temp";
				bool result2;
				try
				{
					ct.ThrowIfCancellationRequested();
					if (t.IsFaulted)
					{
						WebException ex = t.Exception.Flatten().InnerException as WebException;
						if (ex != null)
						{
							HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
							if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.NotModified)
							{
								return false;
							}
						}
					}
					HttpWebResponse result = t.Result;
					if (result.StatusCode == HttpStatusCode.OK)
					{
						using (FileStream fileStream = File.Create(text))
						{
							result.GetResponseStream().CopyTo(fileStream, 2048);
						}
					}
					if (validateDownload != null)
					{
						ct.ThrowIfCancellationRequested();
						using (FileStream fileStream2 = File.OpenRead(text))
						{
							bool flag2;
							try
							{
								flag2 = validateDownload(fileStream2);
							}
							catch (Exception innerException)
							{
								throw new Exception("Failed to validate downloaded file", innerException);
							}
							if (!flag2)
							{
								throw new Exception("Failed to validate downloaded file");
							}
						}
					}
					ct.ThrowIfCancellationRequested();
					FileService.SystemRename(text, cacheFile);
					flag = false;
					result2 = true;
				}
				finally
				{
					if (flag)
					{
						try
						{
							File.Delete(text);
						}
						catch (Exception ex2)
						{
							LoggingService.LogError("Failed to delete temp download file", ex2);
						}
					}
				}
				return result2;
			}, ct);
		}

		// Token: 0x0400009F RID: 159
		private const string addinFileSystemExtensionPath = "/MonoDevelop/Core/FileSystemExtensions";

		// Token: 0x040000A0 RID: 160
		private static FileServiceErrorHandler errorHandler;

		// Token: 0x040000A1 RID: 161
		private static FileSystemExtension fileSystemChain;

		// Token: 0x040000A2 RID: 162
		private static readonly FileSystemExtension defaultExtension = Platform.IsWindows ? new DefaultFileSystemExtension() : new UnixFileSystemExtension();

		// Token: 0x040000A3 RID: 163
		private static readonly EventQueue eventQueue = new EventQueue();

		// Token: 0x040000A4 RID: 164
		private static readonly string applicationRootPath = Path.Combine(PropertyService.EntryAssemblyPath, "..");

		// Token: 0x040000A5 RID: 165
		private static readonly string wildcardMarker = "_" + Guid.NewGuid().ToString() + "_";
	}
}

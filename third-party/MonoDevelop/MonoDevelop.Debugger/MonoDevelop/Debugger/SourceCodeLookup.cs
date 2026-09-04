using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	internal static class SourceCodeLookup
	{
		private static readonly List<Tuple<FilePath, FilePath>> possiblePaths = new List<Tuple<FilePath, FilePath>>();

		private static readonly Dictionary<FilePath, FilePath> directMapping = new Dictionary<FilePath, FilePath>();

		public static FilePath FindSourceFile(FilePath originalFile, byte[] hash)
		{
			if (directMapping.ContainsKey(originalFile))
			{
				return directMapping[originalFile];
			}
			foreach (Tuple<FilePath, FilePath> possiblePath in possiblePaths)
			{
				FilePath filePath = originalFile.ToRelative(possiblePath.Item1);
				FilePath filePath2 = possiblePath.Item2.Combine(filePath);
				if (CheckFileMd5(filePath2, hash))
				{
					directMapping.Add(originalFile, filePath2);
					return filePath2;
				}
			}
			foreach (Document item in IdeApp.Workbench.Documents.Where((Document d) => d.FileName.FileName == originalFile.FileName))
			{
				if (!directMapping.ContainsKey(originalFile) && CheckFileMd5(item.FileName, hash))
				{
					AddLoadedFile(item.FileName, originalFile);
					return item.FileName;
				}
			}
			foreach (Breakpoint item2 in from bp in DebuggingService.Breakpoints.GetBreakpoints()
				where Path.GetFileName(bp.FileName) == originalFile.FileName
				select bp)
			{
				if (!directMapping.ContainsKey(originalFile) && CheckFileMd5(item2.FileName, hash))
				{
					AddLoadedFile(item2.FileName, originalFile);
					return item2.FileName;
				}
			}
			return FilePath.Null;
		}

		public static bool CheckFileMd5(FilePath file, byte[] hash)
		{
			if (File.Exists(file))
			{
				using (FileStream inputStream = File.OpenRead(file))
				{
					using (MD5 mD = MD5.Create())
					{
						if (mD.ComputeHash(inputStream).SequenceEqual(hash))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public static void AddLoadedFile(FilePath file, FilePath originalFile)
		{
			if (directMapping.ContainsKey(originalFile))
			{
				return;
			}
			directMapping.Add(originalFile, file);
			FilePath parentDirectory = file.ParentDirectory;
			FilePath parentDirectory2 = originalFile.ParentDirectory;
			if (parentDirectory == parentDirectory2)
			{
				possiblePaths.Add(new Tuple<FilePath, FilePath>(parentDirectory2, parentDirectory));
				return;
			}
			while (parentDirectory.FileName == parentDirectory2.FileName)
			{
				parentDirectory = parentDirectory.ParentDirectory;
				parentDirectory2 = parentDirectory2.ParentDirectory;
			}
			possiblePaths.Add(new Tuple<FilePath, FilePath>(parentDirectory2, parentDirectory));
		}
	}
}

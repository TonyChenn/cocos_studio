using System;
using System.Collections.Generic;
using System.IO;

namespace MonoDevelop.Core.FileSystem
{
	// Token: 0x02000041 RID: 65
	internal class DefaultFileSystemExtension : FileSystemExtension
	{
		// Token: 0x06000221 RID: 545 RVA: 0x00008C5F File Offset: 0x00006E5F
		public override bool CanHandlePath(FilePath path, bool isDirectory)
		{
			return true;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00008C62 File Offset: 0x00006E62
		public override void CopyFile(FilePath source, FilePath dest, bool overwrite)
		{
			File.Copy(source, dest, overwrite);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00008C76 File Offset: 0x00006E76
		public override void RenameFile(FilePath file, string newName)
		{
			File.Move(file, Path.Combine(file.ParentDirectory, newName));
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008C95 File Offset: 0x00006E95
		public override void MoveFile(FilePath source, FilePath dest)
		{
			File.Move(source, dest);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008CA8 File Offset: 0x00006EA8
		public override void DeleteFile(FilePath file)
		{
			File.Delete(file);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008CB5 File Offset: 0x00006EB5
		public override void CreateDirectory(FilePath path)
		{
			Directory.CreateDirectory(path);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00008CC3 File Offset: 0x00006EC3
		public override void CopyDirectory(FilePath sourcePath, FilePath destPath)
		{
			this.CopyDirectory(sourcePath, destPath, "");
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008CD8 File Offset: 0x00006ED8
		private void CopyDirectory(FilePath src, FilePath dest, FilePath subdir)
		{
			string text = Path.Combine(dest, subdir);
			if (!Directory.Exists(text))
			{
				FileService.CreateDirectory(text);
			}
			foreach (string text2 in Directory.GetFiles(src))
			{
				FileService.CopyFile(text2, Path.Combine(text, Path.GetFileName(text2)));
			}
			foreach (string text3 in Directory.GetDirectories(src))
			{
				this.CopyDirectory(text3, dest, Path.Combine(subdir, Path.GetFileName(text3)));
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00008D83 File Offset: 0x00006F83
		public override void RenameDirectory(FilePath path, string newName)
		{
			Directory.Move(path, newName);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00008D91 File Offset: 0x00006F91
		public override FilePath ResolveFullPath(FilePath path)
		{
			return Path.GetFullPath(path);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00008DA3 File Offset: 0x00006FA3
		public override void MoveDirectory(FilePath source, FilePath dest)
		{
			FileService.SystemDirectoryRename(source, dest);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00008DB6 File Offset: 0x00006FB6
		public override void DeleteDirectory(FilePath path)
		{
			Directory.Delete(path, true);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00008DC4 File Offset: 0x00006FC4
		public override void RequestFileEdit(IEnumerable<FilePath> files)
		{
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00008DC6 File Offset: 0x00006FC6
		public override void NotifyFilesChanged(IEnumerable<FilePath> file)
		{
		}
	}
}

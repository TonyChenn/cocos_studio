using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDevelop.Core.FileSystem
{
	// Token: 0x0200003F RID: 63
	public abstract class FileSystemExtension
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00008A35 File Offset: 0x00006C35
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00008A3D File Offset: 0x00006C3D
		internal FileSystemExtension Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x0600020E RID: 526
		public abstract bool CanHandlePath(FilePath path, bool isDirectory);

		// Token: 0x0600020F RID: 527 RVA: 0x00008A48 File Offset: 0x00006C48
		private FileSystemExtension GetNextForPath(FilePath file, bool isDirectory)
		{
			FileSystemExtension fileSystemExtension = this.next;
			while (fileSystemExtension != null && !fileSystemExtension.CanHandlePath(file, isDirectory))
			{
				fileSystemExtension = fileSystemExtension.next;
			}
			return fileSystemExtension;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008A73 File Offset: 0x00006C73
		public virtual void CopyFile(FilePath source, FilePath dest, bool overwrite)
		{
			this.GetNextForPath(dest, false).CopyFile(source, dest, overwrite);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00008A85 File Offset: 0x00006C85
		public virtual void RenameFile(FilePath file, string newName)
		{
			this.GetNextForPath(file, false).RenameFile(file, newName);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00008A96 File Offset: 0x00006C96
		public virtual FilePath ResolveFullPath(FilePath path)
		{
			return this.GetNextForPath(path, true).ResolveFullPath(path);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00008AA8 File Offset: 0x00006CA8
		public virtual void MoveFile(FilePath source, FilePath dest)
		{
			FileSystemExtension nextForPath = this.GetNextForPath(source, false);
			FileSystemExtension nextForPath2 = this.GetNextForPath(dest, false);
			if (nextForPath == nextForPath2)
			{
				nextForPath.MoveFile(source, dest);
				return;
			}
			nextForPath2.CopyFile(source, dest, true);
			nextForPath.DeleteFile(source);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008AE4 File Offset: 0x00006CE4
		public virtual void DeleteFile(FilePath file)
		{
			this.GetNextForPath(file, false).DeleteFile(file);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00008AF4 File Offset: 0x00006CF4
		public virtual void CreateDirectory(FilePath path)
		{
			this.GetNextForPath(path, true).CreateDirectory(path);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00008B04 File Offset: 0x00006D04
		public virtual void CopyDirectory(FilePath sourcePath, FilePath destPath)
		{
			this.GetNextForPath(destPath, true).CopyDirectory(sourcePath, destPath);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008B15 File Offset: 0x00006D15
		public virtual void RenameDirectory(FilePath path, string newName)
		{
			this.GetNextForPath(path, true).RenameDirectory(path, newName);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00008B28 File Offset: 0x00006D28
		public virtual void MoveDirectory(FilePath sourcePath, FilePath destPath)
		{
			FileSystemExtension nextForPath = this.GetNextForPath(sourcePath, true);
			FileSystemExtension nextForPath2 = this.GetNextForPath(destPath, true);
			if (nextForPath == nextForPath2)
			{
				nextForPath.MoveDirectory(sourcePath, destPath);
				return;
			}
			nextForPath2.CopyDirectory(sourcePath, destPath);
			nextForPath.DeleteDirectory(sourcePath);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00008B63 File Offset: 0x00006D63
		public virtual void DeleteDirectory(FilePath path)
		{
			this.GetNextForPath(path, true).DeleteDirectory(path);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00008B80 File Offset: 0x00006D80
		public virtual void RequestFileEdit(IEnumerable<FilePath> files)
		{
			foreach (IGrouping<FileSystemExtension, FilePath> grouping in from f in files
			group f by this.GetNextForPath(f, false))
			{
				grouping.Key.RequestFileEdit(grouping);
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00008BEC File Offset: 0x00006DEC
		public virtual void NotifyFilesChanged(IEnumerable<FilePath> files)
		{
			foreach (IGrouping<FileSystemExtension, FilePath> grouping in from f in files
			group f by this.GetNextForPath(f, false))
			{
				grouping.Key.NotifyFilesChanged(files);
			}
		}

		// Token: 0x040000C2 RID: 194
		private FileSystemExtension next;
	}
}

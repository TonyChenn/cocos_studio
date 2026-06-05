using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace MonoDevelop.Core
{
	// Token: 0x0200021B RID: 539
	[Serializable]
	public struct FilePath : IComparable<FilePath>, IComparable, IEquatable<FilePath>
	{
		// Token: 0x0600143B RID: 5179 RVA: 0x00053B8F File Offset: 0x00051D8F
		public FilePath(string name)
		{
			this.fileName = name;
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00053B98 File Offset: 0x00051D98
		public bool IsNull
		{
			get
			{
				return this.fileName == null;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00053BA3 File Offset: 0x00051DA3
		public bool IsNullOrEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.fileName);
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00053BB0 File Offset: 0x00051DB0
		public bool IsNotNull
		{
			get
			{
				return this.fileName != null;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x00053BBE File Offset: 0x00051DBE
		public bool IsEmpty
		{
			get
			{
				return this.fileName != null && this.fileName.Length == 0;
			}
		}

		// Token: 0x06001440 RID: 5184
		[DllImport("libc")]
		private static extern IntPtr realpath(string path, IntPtr buffer);

		// Token: 0x06001441 RID: 5185 RVA: 0x00053BD8 File Offset: 0x00051DD8
		public FilePath ResolveFullPath()
		{
			if (Platform.IsWindows)
			{
				return Path.GetFullPath(this);
			}
			IntPtr intPtr = IntPtr.Zero;
			FilePath result;
			try
			{
				intPtr = Marshal.AllocHGlobal(4097);
				IntPtr value = FilePath.realpath(this, intPtr);
				result = ((value == IntPtr.Zero) ? "" : Marshal.PtrToStringAuto(intPtr));
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return result;
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x00053C6C File Offset: 0x00051E6C
		public FilePath FullPath
		{
			get
			{
				return new FilePath((!string.IsNullOrEmpty(this.fileName)) ? Path.GetFullPath(this.fileName) : "");
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x00053C92 File Offset: 0x00051E92
		public bool IsDirectory
		{
			get
			{
				return Directory.Exists(this.FullPath);
			}
		}

		/// <summary>
		/// Returns a path in standard form, which can be used to be compared
		/// for equality with other canonical paths. It is similar to FullPath,
		/// but unlike FullPath, the directory "/a/b" is considered equal to "/a/b/"
		/// </summary>
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x00053CA4 File Offset: 0x00051EA4
		public FilePath CanonicalPath
		{
			get
			{
				if (string.IsNullOrEmpty(this.fileName))
				{
					return FilePath.Empty;
				}
				string fullPath = Path.GetFullPath(this.fileName);
				if (fullPath.Length > 0 && fullPath[fullPath.Length - 1] == Path.DirectorySeparatorChar)
				{
					return fullPath.TrimEnd(new char[]
					{
						Path.DirectorySeparatorChar
					});
				}
				if (fullPath.Length > 0 && fullPath[fullPath.Length - 1] == Path.AltDirectorySeparatorChar)
				{
					return fullPath.TrimEnd(new char[]
					{
						Path.AltDirectorySeparatorChar
					});
				}
				return fullPath;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x00053D4A File Offset: 0x00051F4A
		public string FileName
		{
			get
			{
				return Path.GetFileName(this.fileName);
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x00053D57 File Offset: 0x00051F57
		public string Extension
		{
			get
			{
				return Path.GetExtension(this.fileName);
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00053D64 File Offset: 0x00051F64
		public bool HasExtension(string extension)
		{
			return this.fileName.Length > extension.Length && this.fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase) && this.fileName[this.fileName.Length - extension.Length - 1] != Path.PathSeparator;
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x00053DBE File Offset: 0x00051FBE
		public string FileNameWithoutExtension
		{
			get
			{
				return Path.GetFileNameWithoutExtension(this.fileName);
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x00053DCB File Offset: 0x00051FCB
		public FilePath ParentDirectory
		{
			get
			{
				return new FilePath(Path.GetDirectoryName(this.fileName));
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x00053DDD File Offset: 0x00051FDD
		public bool IsAbsolute
		{
			get
			{
				return Path.IsPathRooted(this.fileName);
			}
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00053DEC File Offset: 0x00051FEC
		public bool IsChildPathOf(FilePath basePath)
		{
			if (basePath.fileName[basePath.fileName.Length - 1] != Path.DirectorySeparatorChar)
			{
				return this.fileName.StartsWith(basePath.fileName + Path.DirectorySeparatorChar, FilePath.PathComparison);
			}
			return this.fileName.StartsWith(basePath.fileName, FilePath.PathComparison);
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00053E58 File Offset: 0x00052058
		public FilePath ChangeExtension(string ext)
		{
			return Path.ChangeExtension(this.fileName, ext);
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00053E6C File Offset: 0x0005206C
		public FilePath Combine(params FilePath[] paths)
		{
			string text = this.fileName;
			foreach (FilePath filePath in paths)
			{
				text = Path.Combine(text, filePath.fileName);
			}
			return new FilePath(text);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00053EB1 File Offset: 0x000520B1
		public FilePath Combine(params string[] paths)
		{
			return new FilePath(Path.Combine(this.fileName, Path.Combine(paths)));
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00053ECC File Offset: 0x000520CC
		public void Delete()
		{
			this.MakeWritable(true);
			this.ParentDirectory.MakeWritable(false);
			if (Directory.Exists(this))
			{
				Directory.Delete(this, true);
				return;
			}
			if (File.Exists(this))
			{
				File.Delete(this);
			}
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x00053F35 File Offset: 0x00052135
		public void MakeWritable()
		{
			this.MakeWritable(false);
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x00053F40 File Offset: 0x00052140
		public void MakeWritable(bool recurse)
		{
			if (Directory.Exists(this))
			{
				try
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(this);
					directoryInfo.Attributes &= ~FileAttributes.ReadOnly;
				}
				catch
				{
				}
				if (recurse)
				{
					foreach (string name in Directory.GetFileSystemEntries(this))
					{
						name.MakeWritable(recurse);
					}
					return;
				}
			}
			else if (File.Exists(this))
			{
				try
				{
					FileInfo fileInfo = new FileInfo(this);
					fileInfo.Attributes &= ~FileAttributes.ReadOnly;
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Builds a path by combining all provided path sections
		/// </summary>
		// Token: 0x06001452 RID: 5202 RVA: 0x00054014 File Offset: 0x00052214
		public static FilePath Build(params string[] paths)
		{
			return FilePath.Empty.Combine(paths);
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x00054030 File Offset: 0x00052230
		public static FilePath GetCommonRootPath(IEnumerable<FilePath> paths)
		{
			FilePath filePath = FilePath.Null;
			foreach (FilePath filePath2 in paths)
			{
				if (filePath.IsNull)
				{
					filePath = filePath2;
				}
				else if (!(filePath == filePath2))
				{
					if (filePath.IsChildPathOf(filePath2))
					{
						filePath = filePath2;
					}
					else
					{
						while (!filePath.IsNullOrEmpty && !filePath2.IsChildPathOf(filePath))
						{
							filePath = filePath.ParentDirectory;
						}
					}
				}
			}
			return filePath;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x000540B8 File Offset: 0x000522B8
		public FilePath ToAbsolute(FilePath basePath)
		{
			if (this.IsAbsolute)
			{
				return this.FullPath;
			}
			return this.Combine(new FilePath[]
			{
				basePath,
				this
			}).FullPath;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x00054109 File Offset: 0x00052309
		public FilePath ToRelative(FilePath basePath)
		{
			return FileService.AbsoluteToRelativePath(basePath, this.fileName);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00054121 File Offset: 0x00052321
		public static implicit operator FilePath(string name)
		{
			return new FilePath(name);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x00054129 File Offset: 0x00052329
		public static implicit operator string(FilePath filePath)
		{
			return filePath.fileName;
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00054132 File Offset: 0x00052332
		public static bool operator ==(FilePath name1, FilePath name2)
		{
			return FilePath.PathComparer.Equals(name1.fileName, name2.fileName);
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0005414C File Offset: 0x0005234C
		public static bool operator !=(FilePath name1, FilePath name2)
		{
			return !(name1 == name2);
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00054158 File Offset: 0x00052358
		public override bool Equals(object obj)
		{
			if (!(obj is FilePath))
			{
				return false;
			}
			FilePath name = (FilePath)obj;
			return this == name;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00054182 File Offset: 0x00052382
		public override int GetHashCode()
		{
			if (this.fileName == null)
			{
				return 0;
			}
			return FilePath.PathComparer.GetHashCode(this.fileName);
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0005419E File Offset: 0x0005239E
		public override string ToString()
		{
			return this.fileName;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x000541A6 File Offset: 0x000523A6
		public int CompareTo(FilePath filePath)
		{
			return FilePath.PathComparer.Compare(this.fileName, filePath.fileName);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x000541BF File Offset: 0x000523BF
		int IComparable.CompareTo(object obj)
		{
			if (!(obj is FilePath))
			{
				return -1;
			}
			return this.CompareTo((FilePath)obj);
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x000541D7 File Offset: 0x000523D7
		bool IEquatable<FilePath>.Equals(FilePath other)
		{
			return this == other;
		}

		// Token: 0x04000614 RID: 1556
		private const int PATHMAX = 4097;

		// Token: 0x04000615 RID: 1557
		private static readonly StringComparer PathComparer = (Platform.IsWindows || Platform.IsMac) ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

		// Token: 0x04000616 RID: 1558
		private static readonly StringComparison PathComparison = (Platform.IsWindows || Platform.IsMac) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

		// Token: 0x04000617 RID: 1559
		private readonly string fileName;

		// Token: 0x04000618 RID: 1560
		public static readonly FilePath Null = new FilePath(null);

		// Token: 0x04000619 RID: 1561
		public static readonly FilePath Empty = new FilePath(string.Empty);
	}
}

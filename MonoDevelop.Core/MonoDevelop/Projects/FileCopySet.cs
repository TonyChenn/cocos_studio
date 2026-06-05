using System;
using System.Collections;
using System.Collections.Generic;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	/// <summary>
	/// A list of files to be deployed together with the project output binary
	/// </summary>
	// Token: 0x0200017A RID: 378
	public class FileCopySet : IEnumerable<FileCopySet.Item>, IEnumerable
	{
		/// <summary>
		/// Add a file
		/// </summary>
		/// <param name="sourcePath">
		/// File path.
		/// </param>
		// Token: 0x06000F10 RID: 3856 RVA: 0x00038CDD File Offset: 0x00036EDD
		public void Add(FilePath sourcePath)
		{
			this.Add(sourcePath, false);
		}

		/// <summary>
		/// Add a file
		/// </summary>
		/// <param name="sourcePath">
		/// File path.
		/// </param>
		/// <param name="copyOnlyIfNewer">
		/// Copy to otuput dir only if the file has changed.
		/// </param>
		// Token: 0x06000F11 RID: 3857 RVA: 0x00038CE7 File Offset: 0x00036EE7
		public void Add(FilePath sourcePath, bool copyOnlyIfNewer)
		{
			this.Add(sourcePath, copyOnlyIfNewer, sourcePath.FileName);
		}

		/// <summary>
		/// Add a file
		/// </summary>
		/// <param name="sourcePath">
		/// File path.
		/// </param>
		/// <param name="copyOnlyIfNewer">
		/// Copy to otuput dir only if the file has changed.
		/// </param>
		/// <param name="targetRelativePath">
		/// Directory (relative to the output directory) where the file has to be copied.
		/// </param>
		// Token: 0x06000F12 RID: 3858 RVA: 0x00038CFE File Offset: 0x00036EFE
		public bool Add(FilePath sourcePath, bool copyOnlyIfNewer, FilePath targetRelativePath)
		{
			if (this.files.ContainsKey(targetRelativePath))
			{
				return false;
			}
			this.files.Add(targetRelativePath, new FileCopySet.Item(sourcePath, copyOnlyIfNewer, targetRelativePath));
			return true;
		}

		/// <summary>
		/// Remove a file
		/// </summary>
		/// <param name="fileName">
		/// File name.
		/// </param>
		// Token: 0x06000F13 RID: 3859 RVA: 0x00038D28 File Offset: 0x00036F28
		public FileCopySet.Item Remove(FilePath fileName)
		{
			string fileName2 = fileName.FileName;
			FileCopySet.Item result;
			if (this.files.TryGetValue(fileName2, out result))
			{
				this.files.Remove(fileName2);
				return result;
			}
			return null;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00038D67 File Offset: 0x00036F67
		IEnumerator<FileCopySet.Item> IEnumerable<FileCopySet.Item>.GetEnumerator()
		{
			return this.files.Values.GetEnumerator();
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00038D7E File Offset: 0x00036F7E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.files.Values.GetEnumerator();
		}

		// Token: 0x0400045B RID: 1115
		private Dictionary<FilePath, FileCopySet.Item> files = new Dictionary<FilePath, FileCopySet.Item>();

		/// <summary>
		/// A file
		/// </summary>
		// Token: 0x0200017B RID: 379
		public class Item
		{
			// Token: 0x06000F16 RID: 3862 RVA: 0x00038D95 File Offset: 0x00036F95
			public Item(FilePath src, bool copyOnlyIfNewer, FilePath target)
			{
				this.Src = src;
				this.Target = target;
				this.CopyOnlyIfNewer = copyOnlyIfNewer;
			}

			/// <summary>
			/// Gets or sets a value indicating whether the file has to be copied only if it has changed.
			/// </summary>
			// Token: 0x17000324 RID: 804
			// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00038DB2 File Offset: 0x00036FB2
			// (set) Token: 0x06000F18 RID: 3864 RVA: 0x00038DBA File Offset: 0x00036FBA
			public bool CopyOnlyIfNewer { get; private set; }

			/// <summary>
			/// Gets or sets the target directory (must be a relative path)
			/// </summary>
			// Token: 0x17000325 RID: 805
			// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00038DC3 File Offset: 0x00036FC3
			// (set) Token: 0x06000F1A RID: 3866 RVA: 0x00038DCB File Offset: 0x00036FCB
			public FilePath Target { get; private set; }

			/// <summary>
			/// Gets or sets the source path
			/// </summary>
			// Token: 0x17000326 RID: 806
			// (get) Token: 0x06000F1B RID: 3867 RVA: 0x00038DD4 File Offset: 0x00036FD4
			// (set) Token: 0x06000F1C RID: 3868 RVA: 0x00038DDC File Offset: 0x00036FDC
			public FilePath Src { get; private set; }
		}
	}
}

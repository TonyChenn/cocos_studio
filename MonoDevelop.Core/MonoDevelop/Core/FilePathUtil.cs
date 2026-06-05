using System;
using System.Collections.Generic;

namespace MonoDevelop.Core
{
	// Token: 0x0200021C RID: 540
	public static class FilePathUtil
	{
		// Token: 0x06001461 RID: 5217 RVA: 0x00054248 File Offset: 0x00052448
		public static string[] ToStringArray(this FilePath[] paths)
		{
			string[] array = new string[paths.Length];
			for (int i = 0; i < paths.Length; i++)
			{
				array[i] = paths[i].ToString();
			}
			return array;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00054284 File Offset: 0x00052484
		public static FilePath[] ToFilePathArray(this string[] paths)
		{
			FilePath[] array = new FilePath[paths.Length];
			for (int i = 0; i < paths.Length; i++)
			{
				array[i] = paths[i];
			}
			return array;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00054454 File Offset: 0x00052654
		public static IEnumerable<string> ToPathStrings(this IEnumerable<FilePath> paths)
		{
			foreach (FilePath p in paths)
			{
				FilePath filePath = p;
				yield return filePath.ToString();
			}
			yield break;
		}
	}
}

using System;
using System.IO;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200001D RID: 29
	[Extension(typeof(IFileFormat))]
	internal class TTFFileFormat : FileFormat
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00003B24 File Offset: 0x00001D24
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is TTFFile;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003B30 File Offset: 0x00001D30
		protected override bool OnCanReadFile(FilePath filePath, Type expectedObjectType)
		{
			return expectedObjectType.Equals(typeof(ResourceItem)) && FileFormat.CheckFileSuffix(filePath, new string[]
			{
				".ttf"
			});
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003B6C File Offset: 0x00001D6C
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new TTFFile(file);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003B74 File Offset: 0x00001D74
		public static bool CheckDataIsValid(FilePath fileName)
		{
			bool result;
			try
			{
				using (FileStream fileStream = File.Open(fileName, FileMode.Open))
				{
					int num = 100;
					byte[] array = new byte[num];
					if (fileStream.Read(array, 0, num) < num || array[0] != 0 || array[1] != 1 || array[2] != 0 || array[3] != 0)
					{
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("判断TTF资源是否合法时出错", exception);
				result = false;
			}
			return result;
		}

		// Token: 0x0400002D RID: 45
		public const string Suffix = ".ttf";
	}
}

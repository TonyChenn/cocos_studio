using System;
using System.IO;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200001B RID: 27
	[Extension(typeof(IFileFormat))]
	internal class TTCFileFormat : FileFormat
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00003811 File Offset: 0x00001A11
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is TTFFile;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000381C File Offset: 0x00001A1C
		protected override bool OnCanReadFile(FilePath filePath, Type expectedObjectType)
		{
			return expectedObjectType.Equals(typeof(ResourceItem)) && FileFormat.CheckFileSuffix(filePath, new string[]
			{
				".ttc"
			});
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003858 File Offset: 0x00001A58
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new TTFFile(file);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003860 File Offset: 0x00001A60
		public static bool CheckDataIsValid(FilePath fileName)
		{
			bool result;
			try
			{
				using (FileStream fileStream = File.Open(fileName, FileMode.Open))
				{
					int num = 100;
					byte[] array = new byte[num];
					if (fileStream.Read(array, 0, num) < num || array[0] != 116 || array[1] != 116 || array[2] != 99 || array[3] != 102)
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
				LogConfig.Logger.Error("判断TTC资源是否合法时出错", exception);
				result = false;
			}
			return result;
		}

		// Token: 0x0400002C RID: 44
		public const string Suffix = ".ttc";
	}
}

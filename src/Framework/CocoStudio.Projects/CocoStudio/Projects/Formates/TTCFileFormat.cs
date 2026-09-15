using System;
using System.IO;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	internal class TTCFileFormat : FileFormat
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is TTFFile;
		}

		protected override bool OnCanReadFile(FilePath filePath, Type expectedObjectType)
		{
			return expectedObjectType.Equals(typeof(ResourceItem)) && FileFormat.CheckFileSuffix(filePath, new string[]
			{
				".ttc"
			});
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new TTFFile(file);
		}

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

		public const string Suffix = ".ttc";
	}
}

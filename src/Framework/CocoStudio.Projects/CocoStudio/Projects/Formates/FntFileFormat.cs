using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IPublishProcesser))]
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	internal class FntFileFormat : CompositeFormat, IPublishProcesser
	{
		private static Regex PageCountMatchReg
		{
			get
			{
				if (FntFileFormat.pageCountMatchReg == null)
				{
					FntFileFormat.pageCountMatchReg = new Regex("((?<=\\bpages=)\\d*)", RegexOptions.ExplicitCapture);
				}
				return FntFileFormat.pageCountMatchReg;
			}
		}

		protected override bool OnCanWriteFile(object obj)
		{
			return obj is FntFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (!FntFileFormat.CheckFileSuffix(file))
				{
					return false;
				}
				if (!expectedObjectType.Equals(typeof(ResourceItem)))
				{
					return false;
				}
				return true;
			}
			catch
			{
			}
			return false;
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new FntFile(file);
		}

		private static bool CheckFileSuffix(FilePath filePath)
		{
			return FileFormat.CheckFileSuffix(filePath, new string[]
			{
				".fnt"
			});
		}

		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			return resourceData.Type != EnumResourceType.PlistSubImage && FntFileFormat.CheckFileSuffix(resourceData.Path);
		}

		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			if (EnumResourceType.Default == resourceData.Type)
			{
				string filePath = ProjectsService.Instance.GetFullPath(resourceData);
				return CompositeResourceHelp.GetResourcesIncludeImage(resourceData, ((ICompositeResourceProcesser)this).GetFiles(filePath));
			}
			ResourceGroup currentResourceGroup = ProjectsService.Instance.CurrentResourceGroup;
			CompositeResourceFile compositeResourceFile = currentResourceGroup.FindResourceItem(resourceData) as CompositeResourceFile;
			return CompositeResourceHelp.GetResourcesIncludeImage(resourceData, compositeResourceFile.ImageFiles.ToList<string>());
		}

		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		public override List<string> GetFiles(string filePath)
		{
			string text;
			List<string> result = FntFileFormat.ParseAndCheckFntFile(filePath, out text);
			if (text != null)
			{
				return null;
			}
			return result;
		}

		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".fnt"
			};
		}

		public static List<string> ParseAndCheckFntFile(string fullPath, out string errorInfo)
		{
			List<string> list = null;
			errorInfo = null;
			try
			{
				FileStream fileStream;
				FileStream fs = fileStream = File.OpenRead(fullPath);
				try
				{
					switch (FntFileFormat.CheckFntType(fs))
					{
					case FntFileFormat.FntType.Text:
						list = FntFileFormat.ParseTextFormatForPictureFile(fs, Path.GetDirectoryName(fullPath));
						break;
					case FntFileFormat.FntType.XML:
						errorInfo = LanguageInfo.DataError16_FntXmlNotSupport;
						break;
					case FntFileFormat.FntType.Binary:
						list = FntFileFormat.ParseBinFormatForPictureFile(fs, Path.GetDirectoryName(fullPath));
						break;
					}
				}
				finally
				{
					if (fileStream != null)
					{
						((IDisposable)fileStream).Dispose();
					}
				}
			}
			catch (Exception)
			{
			}
			if (list == null)
			{
				if (string.IsNullOrEmpty(errorInfo))
				{
					errorInfo = LanguageInfo.DataError1_PngNotExist;
				}
			}
			else if (list.Count > 1)
			{
				errorInfo = LanguageInfo.DataError15_FntFormatNotSupport;
			}
			return list;
		}

		private static FntFileFormat.FntType CheckFntType(FileStream fs)
		{
			FntFileFormat.FntType result = FntFileFormat.FntType.Unknown;
			try
			{
				fs.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[3];
				byte[] array2 = array;
				fs.Read(array2, 0, 3);
				string text = Encoding.Default.GetString(array2).Trim();
				string a;
				if ((a = text) != null)
				{
					if (!(a == "BMF"))
					{
						if (!(a == "inf"))
						{
							if (a == "<?x")
							{
								result = FntFileFormat.FntType.XML;
							}
						}
						else
						{
							result = FntFileFormat.FntType.Text;
						}
					}
					else
					{
						result = FntFileFormat.FntType.Binary;
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		private static List<string> ParseTextFormatForPictureFile(FileStream fs, string filePath)
		{
			List<string> list = new List<string>();
			try
			{
				fs.Seek(0L, SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(fs);
				string input = streamReader.ReadToEnd();
				Regex regex = FntFileFormat.PageCountMatchReg;
				MatchCollection matchCollection = regex.Matches(input);
				if (matchCollection.Count == 1)
				{
					int num = 0;
					if (int.TryParse(matchCollection[0].Value.Trim(), out num))
					{
						for (int i = 0; i < num; i++)
						{
							string pattern = string.Format("((?<=\\bpage id={0} file=\\\")[^\\\"]*)", i);
							Regex regex2 = new Regex(pattern, RegexOptions.ExplicitCapture);
							matchCollection = regex2.Matches(input);
							if (matchCollection.Count > 0)
							{
								string text = matchCollection[0].Value.Trim();
								text = Path.Combine(filePath, text);
								list.Add(text);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			if (list.Count <= 0)
			{
				list = null;
			}
			return list;
		}

		private static List<string> ParseBinFormatForPictureFile(FileStream fs, string filePath)
		{
			List<string> list = new List<string>();
			try
			{
				byte[] array = new byte[5];
				int num = 4;
				fs.Seek((long)num, SeekOrigin.Begin);
				while ((long)num < fs.Length)
				{
					int num2 = fs.Read(array, 0, 5);
					num += num2;
					if (num2 != 5)
					{
						break;
					}
					byte b = array[0];
					int num3 = (int)array[1] | (int)array[2] << 8 | (int)array[3] << 16 | (int)array[4] << 24;
					if (b == 3)
					{
						byte[] array2 = new byte[num3];
						num2 = fs.Read(array2, 0, num3);
						if (num2 > 0)
						{
							string text = Encoding.UTF8.GetString(array2).Trim();
							string text2 = text;
							char[] separator = new char[1];
							string[] array3 = text2.Split(separator);
							foreach (string text3 in array3)
							{
								if (!string.IsNullOrEmpty(text3.Trim()))
								{
									list.Add(Path.Combine(filePath, text3.Trim()));
								}
							}
							break;
						}
						break;
					}
					else
					{
						num += num3;
						fs.Seek((long)num, SeekOrigin.Begin);
					}
				}
			}
			catch (Exception)
			{
			}
			if (list.Count <= 0)
			{
				list = null;
			}
			return list;
		}

		private const string RegPagesCountMatch = "((?<=\\bpages=)\\d*)";

		private const string RegPageMatchStr = "((?<=\\bpage id={0} file=\\\")[^\\\"]*)";

		private static Regex pageCountMatchReg;

		public enum FntType
		{
			Text,
			XML,
			Binary,
			Unknown
		}
	}
}

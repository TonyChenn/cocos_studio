using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(IPublishProcesser))]
	internal class PuFileFormat : CompositeFormat, IPublishProcesser
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is PuFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (!this.CheckFileSuffix(file, PuFileFormat.Particle3DSuffixPu))
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

		private bool CheckFileSuffix(string path, string FileSuffix)
		{
			return Path.GetExtension(path).Equals(FileSuffix, StringComparison.OrdinalIgnoreCase);
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new PuFile(file);
		}

		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			string path = resourceData.Path;
			return this.CheckFileSuffix(path, PuFileFormat.Particle3DSuffixPu);
		}

		private List<string> GetMeshRelatedFiles(string filePath)
		{
			CSVectorString meshResourceArray = CSCocosHelp.GetMeshResourceArray(filePath);
			string directoryName = Path.GetDirectoryName(filePath);
			List<string> list = new List<string>();
			if (meshResourceArray == null)
			{
				return null;
			}
			foreach (string name in meshResourceArray)
			{
				string item = ((FilePath)name).ToAbsolute(directoryName);
				list.Add(item);
			}
			return list;
		}

		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			hashSet.Add(resourceData);
			string fullpath = ProjectsService.Instance.GetFullPath(resourceData);
			string text = ProjectsService.Instance.CurrentSolution.ItemDirectory;
			if (!text.EndsWith(string.Concat(Path.DirectorySeparatorChar)))
			{
				text += Path.DirectorySeparatorChar;
			}
			string text2 = Path.Combine(Option.AssemblyDir, "EditorDefaultRes");
			if (!text2.EndsWith(string.Concat(Path.DirectorySeparatorChar)))
			{
				text2 += Path.DirectorySeparatorChar;
			}
			PuParser puParser = new PuParser();
			if (!puParser.LoadPuFileAndGetAllResource(fullpath))
			{
				return null;
			}
			List<string> list = puParser.GetMaterialFiles();
			if (list != null && list.Count > 0)
			{
				foreach (string text3 in list)
				{
					string path = text3.Replace((resourceData.Type == EnumResourceType.Normal) ? text : text2, "");
					ResourceData item = new ResourceData(resourceData.Type, path);
					hashSet.Add(item);
				}
			}
			list = puParser.GetTextureFiles();
			if (list != null && list.Count > 0)
			{
				foreach (string text4 in list)
				{
					string path2 = text4.Replace((resourceData.Type == EnumResourceType.Normal) ? text : text2, "");
					ResourceData item2 = new ResourceData(resourceData.Type, path2);
					hashSet.Add(item2);
				}
			}
			list = puParser.GetAllMeshFiles();
			if (list != null && list.Count > 0)
			{
				foreach (string text5 in list)
				{
					string path3 = text5.Replace((resourceData.Type == EnumResourceType.Normal) ? text : text2, "");
					ResourceData item3 = new ResourceData(resourceData.Type, path3);
					hashSet.Add(item3);
					List<string> meshRelatedFiles = this.GetMeshRelatedFiles(text5);
					if (meshRelatedFiles != null && meshRelatedFiles.Count > 0)
					{
						foreach (string text6 in meshRelatedFiles)
						{
							string path4 = text6.Replace((resourceData.Type == EnumResourceType.Normal) ? text : text2, "");
							ResourceData item4 = new ResourceData(resourceData.Type, path4);
							hashSet.Add(item4);
						}
					}
				}
			}
			return hashSet;
		}

		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		public override List<string> GetFiles(string filePath)
		{
			List<string> list = new List<string>();
			try
			{
				string text = Path.GetDirectoryName(Path.GetDirectoryName(filePath));
				text = Path.Combine(text, "materials");
				if (Directory.Exists(text))
				{
					list.Add(text);
					string[] files = Directory.GetFiles(text, "*.material");
					list.AddRange(files);
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				PuFileFormat.Particle3DSuffixPu
			};
		}

		private const string DefaultResFolder = "EditorDefaultRes";

		private static string Particle3DSuffixPu = ".pu";
	}
}

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
	// Token: 0x0200002F RID: 47
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(IPublishProcesser))]
	internal class PuFileFormat : CompositeFormat, IPublishProcesser
	{
		// Token: 0x0600010B RID: 267 RVA: 0x000050CB File Offset: 0x000032CB
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is PuFile;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000050D8 File Offset: 0x000032D8
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

		// Token: 0x0600010D RID: 269 RVA: 0x0000512C File Offset: 0x0000332C
		private bool CheckFileSuffix(string path, string FileSuffix)
		{
			return Path.GetExtension(path).Equals(FileSuffix, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000513B File Offset: 0x0000333B
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new PuFile(file);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005144 File Offset: 0x00003344
		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			string path = resourceData.Path;
			return this.CheckFileSuffix(path, PuFileFormat.Particle3DSuffixPu);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000516C File Offset: 0x0000336C
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

		// Token: 0x06000111 RID: 273 RVA: 0x000051F0 File Offset: 0x000033F0
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

		// Token: 0x06000112 RID: 274 RVA: 0x000054C0 File Offset: 0x000036C0
		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000054D8 File Offset: 0x000036D8
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

		// Token: 0x06000114 RID: 276 RVA: 0x0000553C File Offset: 0x0000373C
		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				PuFileFormat.Particle3DSuffixPu
			};
		}

		// Token: 0x04000047 RID: 71
		private const string DefaultResFolder = "EditorDefaultRes";

		// Token: 0x04000048 RID: 72
		private static string Particle3DSuffixPu = ".pu";
	}
}

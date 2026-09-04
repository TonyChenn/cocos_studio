using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200002A RID: 42
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IPublishProcesser))]
	internal class MeshFileFormat : CompositeFormat, IPublishProcesser
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00004A72 File Offset: 0x00002C72
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is MeshFile;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004A80 File Offset: 0x00002C80
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (!FileFormat.CheckFileSuffix(file, new string[]
				{
					".c3b",
					".c3t",
					".obj"
				}))
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

		// Token: 0x060000EB RID: 235 RVA: 0x00004AEC File Offset: 0x00002CEC
		private bool CheckFileSuffix(string path, string FileSuffix)
		{
			return Path.GetExtension(path).Equals(FileSuffix, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004AFB File Offset: 0x00002CFB
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new MeshFile(file);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004B04 File Offset: 0x00002D04
		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			string path = resourceData.Path;
			return FileFormat.CheckFileSuffix(path, new string[]
			{
				".c3b",
				".c3t",
				".obj"
			});
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004B48 File Offset: 0x00002D48
		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			hashSet.Add(resourceData);
			string filePath = ProjectsService.Instance.GetFullPath(resourceData);
			CSVectorString meshResourceArray = CSCocosHelp.GetMeshResourceArray(filePath);
			if (meshResourceArray == null)
			{
				return hashSet;
			}
			string directoryName = Path.GetDirectoryName(resourceData.Path);
			foreach (string path in meshResourceArray)
			{
				string fileName = Path.GetFileName(path);
				string path2 = Path.Combine(directoryName, fileName);
				ResourceData item = new ResourceData(resourceData.Type, path2);
				hashSet.Add(item);
			}
			return hashSet;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004BF4 File Offset: 0x00002DF4
		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004C0C File Offset: 0x00002E0C
		public override List<string> GetFiles(string filePath)
		{
			return new List<string>();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004C20 File Offset: 0x00002E20
		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".c3b",
				".c3t",
				".obj"
			};
		}

		// Token: 0x04000044 RID: 68
		private const string MeshFileSuffixC3B = ".c3b";

		// Token: 0x04000045 RID: 69
		private const string MeshFileSuffixC3T = ".c3t";

		// Token: 0x04000046 RID: 70
		private const string MeshFileSuffixOBJ = ".obj";
	}
}

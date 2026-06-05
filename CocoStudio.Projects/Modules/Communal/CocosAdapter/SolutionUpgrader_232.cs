using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;
using CocoStudio.Projects.Formates;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x0200000B RID: 11
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_232 : SolutionUpgrader
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000026B2 File Offset: 0x000008B2
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_232.version;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000026BC File Offset: 0x000008BC
		protected override bool OnUpgrade(Solution sln)
		{
			bool result = false;
			ResourceGroup resourceGroup = sln.RootFolder.Items[0] as ResourceGroup;
			if (resourceGroup != null)
			{
				ResourceFolder rootFolder = resourceGroup.RootFolder;
				if (rootFolder != null)
				{
					result = this.CheckUpgradeItem(rootFolder);
				}
			}
			return result;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000026F8 File Offset: 0x000008F8
		private bool CheckUpgradeItem(ResourceFolder root)
		{
			bool flag = false;
			if (root != null)
			{
				for (int i = root.Items.Count - 1; i >= 0; i--)
				{
					ResourceItem resourceItem = root.Items[i];
					if (resourceItem is ResourceFolder)
					{
						string fullPath = resourceItem.FullPath;
						List<FileFormat> fileFormats = ProjectsService.Instance.FormatManager.GetFileFormats(fullPath, typeof(ResourceItem));
						if (fileFormats.Count > 0 && this.IsContainType(fileFormats, typeof(MaterialFolderFormat)))
						{
							MaterialFolder materialFolder = new MaterialFolder(fullPath);
							materialFolder.Items.Clear();
							foreach (ResourceItem resourceItem2 in (resourceItem as ResourceFolder).Items)
							{
								if (!resourceItem2.FullPath.Equals(Path.Combine(fullPath, "materials")))
								{
									materialFolder.Items.Add(resourceItem2);
								}
							}
							root.Items.RemoveAt(i);
							(resourceItem as ResourceFolder).Items.Clear();
							root.Items.Insert(i, materialFolder);
							flag = true;
						}
						bool flag2 = this.CheckUpgradeItem(resourceItem as ResourceFolder);
						if (!flag)
						{
							flag = flag2;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002850 File Offset: 0x00000A50
		private bool IsContainType(List<FileFormat> list, Type type)
		{
			bool result = false;
			foreach (FileFormat fileFormat in list)
			{
				if (fileFormat.GetType() == type)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0400000B RID: 11
		private static readonly Version version = new Version("2.3.1.0");
	}
}

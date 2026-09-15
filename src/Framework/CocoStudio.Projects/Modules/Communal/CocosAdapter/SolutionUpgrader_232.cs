using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;
using CocoStudio.Projects.Formates;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_232 : SolutionUpgrader
	{
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_232.version;
			}
		}

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

		private static readonly Version version = new Version("2.3.1.0");
	}
}

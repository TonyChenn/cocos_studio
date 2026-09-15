using System;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel.Upgrade
{
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_210 : SolutionUpgrader
	{
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_210.version;
			}
		}

		protected override bool OnUpgrade(Solution sln)
		{
			this.UpdateContentType(sln);
			return true;
		}

		private void UpdateContentType(Solution sln)
		{
			ResourceGroup resourceGroup = sln.RootFolder.Items[0] as ResourceGroup;
			this.RecursiveUpdataProject(resourceGroup.RootFolder);
		}

		private void RecursiveUpdataProject(ResourceFolder root)
		{
			foreach (ResourceItem resourceItem in root.Items)
			{
				if (resourceItem is ResourceFolder)
				{
					this.RecursiveUpdataProject((ResourceFolder)resourceItem);
				}
				CocosItem cocosItem = resourceItem as CocosItem;
				if (cocosItem != null)
				{
					cocosItem.ContentType = cocosItem.CocosFile.Type;
					cocosItem.Save(ProjectsService.Instance.DefaultMonitor);
				}
			}
		}

		private static readonly Version version = new Version("2.1.0.0");
	}
}

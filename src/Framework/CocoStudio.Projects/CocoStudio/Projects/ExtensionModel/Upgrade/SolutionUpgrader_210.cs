using System;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel.Upgrade
{
	// Token: 0x0200000C RID: 12
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_210 : SolutionUpgrader
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000028C5 File Offset: 0x00000AC5
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_210.version;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000028CC File Offset: 0x00000ACC
		protected override bool OnUpgrade(Solution sln)
		{
			this.UpdateContentType(sln);
			return true;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028D8 File Offset: 0x00000AD8
		private void UpdateContentType(Solution sln)
		{
			ResourceGroup resourceGroup = sln.RootFolder.Items[0] as ResourceGroup;
			this.RecursiveUpdataProject(resourceGroup.RootFolder);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002908 File Offset: 0x00000B08
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

		// Token: 0x0400000C RID: 12
		private static readonly Version version = new Version("2.1.0.0");
	}
}

using System;
using MonoDevelop.Core;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C5 RID: 453
	internal class GenericItemTypeNode : ItemTypeNode
	{
		// Token: 0x0600114B RID: 4427 RVA: 0x00046413 File Offset: 0x00044613
		public GenericItemTypeNode() : base("{9344BDBB-3E7F-41FC-A0DD-8665D75EE146}", "mdproj", null)
		{
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00046426 File Offset: 0x00044626
		public override bool CanHandleItem(SolutionEntityItem item)
		{
			return true;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0004642C File Offset: 0x0004462C
		public override SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName, MSBuildFileFormat expectedFormat, string itemGuid)
		{
			MSBuildProjectHandler msbuildProjectHandler = new MSBuildProjectHandler(base.Guid, base.Import, itemGuid);
			return msbuildProjectHandler.Load(monitor, fileName, expectedFormat, null, null);
		}
	}
}

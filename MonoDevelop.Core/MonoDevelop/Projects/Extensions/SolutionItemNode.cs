using System;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000192 RID: 402
	public class SolutionItemNode : ItemTypeNode
	{
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0003A407 File Offset: 0x00038607
		public Type ItemType
		{
			get
			{
				return base.Addin.GetType(this.type, true);
			}
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0003A41B File Offset: 0x0003861B
		public override bool CanHandleItem(SolutionEntityItem item)
		{
			return this.ItemType != null && this.ItemType.IsAssignableFrom(item.GetType());
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0003A440 File Offset: 0x00038640
		public override SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName, MSBuildFileFormat expectedFormat, string itemGuid)
		{
			MSBuildProjectHandler msbuildProjectHandler = base.CreateHandler<MSBuildProjectHandler>(fileName, itemGuid);
			return msbuildProjectHandler.Load(monitor, fileName, expectedFormat, null, this.ItemType);
		}

		// Token: 0x04000480 RID: 1152
		[NodeAttribute(Required = true)]
		private string type;
	}
}

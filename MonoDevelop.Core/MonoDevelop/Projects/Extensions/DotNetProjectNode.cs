using System;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000191 RID: 401
	public class DotNetProjectNode : ItemTypeNode
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x0003A335 File Offset: 0x00038535
		public override bool CanHandleItem(SolutionEntityItem item)
		{
			return item is DotNetProject && ((DotNetProject)item).LanguageName == this.language;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0003A358 File Offset: 0x00038558
		public override bool CanHandleFile(string fileName, string typeGuid)
		{
			if (base.CanHandleFile(fileName, typeGuid))
			{
				return true;
			}
			if (!string.IsNullOrEmpty(typeGuid) && typeGuid.Contains(base.Guid))
			{
				DotNetProjectSubtypeNode dotNetProjectSubtype = MSBuildProjectService.GetDotNetProjectSubtype(typeGuid);
				if (dotNetProjectSubtype != null && dotNetProjectSubtype.CanHandleFile(fileName, typeGuid))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0003A3A0 File Offset: 0x000385A0
		public override SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName, MSBuildFileFormat expectedFormat, string itemGuid)
		{
			MSBuildProjectHandler msbuildProjectHandler = base.CreateHandler<MSBuildProjectHandler>(fileName, itemGuid);
			msbuildProjectHandler.SetCustomResourceHandler(this.GetResourceHandler());
			return msbuildProjectHandler.Load(monitor, fileName, expectedFormat, this.language, null);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0003A3D3 File Offset: 0x000385D3
		public IResourceHandler GetResourceHandler()
		{
			if (!string.IsNullOrEmpty(this.resourceHandler))
			{
				return (IResourceHandler)base.Addin.CreateInstance(this.resourceHandler, true);
			}
			return new MSBuildResourceHandler();
		}

		// Token: 0x0400047E RID: 1150
		[NodeAttribute(Required = true)]
		private string language;

		// Token: 0x0400047F RID: 1151
		[NodeAttribute]
		private string resourceHandler;
	}
}

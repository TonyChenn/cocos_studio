using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001EF RID: 495
	[PolicyType("Namespace and resource naming")]
	[DataItem("DotNetNamingPolicy")]
	public class DotNetNamingPolicy : IEquatable<DotNetNamingPolicy>
	{
		// Token: 0x060012FB RID: 4859 RVA: 0x0004EC67 File Offset: 0x0004CE67
		public DotNetNamingPolicy()
		{
			this.ResourceNamePolicy = ResourceNamePolicy.FileFormatDefault;
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0004EC76 File Offset: 0x0004CE76
		public DotNetNamingPolicy(DirectoryNamespaceAssociation association, ResourceNamePolicy resourceNamePolicy)
		{
			this.DirectoryNamespaceAssociation = association;
			this.ResourceNamePolicy = resourceNamePolicy;
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0004EC8C File Offset: 0x0004CE8C
		// (set) Token: 0x060012FE RID: 4862 RVA: 0x0004EC94 File Offset: 0x0004CE94
		[ItemProperty]
		public DirectoryNamespaceAssociation DirectoryNamespaceAssociation { get; private set; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0004EC9D File Offset: 0x0004CE9D
		// (set) Token: 0x06001300 RID: 4864 RVA: 0x0004ECA5 File Offset: 0x0004CEA5
		[ItemProperty]
		public ResourceNamePolicy ResourceNamePolicy { get; private set; }

		// Token: 0x06001301 RID: 4865 RVA: 0x0004ECAE File Offset: 0x0004CEAE
		public bool Equals(DotNetNamingPolicy other)
		{
			return other != null && other.DirectoryNamespaceAssociation == this.DirectoryNamespaceAssociation && other.ResourceNamePolicy == this.ResourceNamePolicy;
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0004ECD4 File Offset: 0x0004CED4
		internal static ResourceNamePolicy GetDefaultResourceNamePolicy(object ob)
		{
			FileFormat fileFormat = null;
			if (ob is SolutionEntityItem)
			{
				fileFormat = ((SolutionEntityItem)ob).FileFormat;
			}
			else if (ob is SolutionItem)
			{
				fileFormat = ((SolutionItem)ob).ParentSolution.FileFormat;
			}
			else if (ob is Solution)
			{
				fileFormat = ((Solution)ob).FileFormat;
			}
			if (fileFormat != null && fileFormat.Name.StartsWith("MSBuild"))
			{
				return ResourceNamePolicy.MSBuild;
			}
			return ResourceNamePolicy.FileName;
		}
	}
}

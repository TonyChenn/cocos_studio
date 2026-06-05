using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001DA RID: 474
	internal class CompiledAssemblyProjectMSBuildHandler : MSBuildProjectHandler
	{
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x000497DC File Offset: 0x000479DC
		public override bool HasSlnData
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x000497E0 File Offset: 0x000479E0
		public override DataItem WriteSlnData()
		{
			return (DataItem)new DataSerializer(MSBuildProjectService.DataContext)
			{
				SerializationContext = 
				{
					BaseFile = base.EntityItem.FileName,
					DirectorySeparatorChar = '\\'
				}
			}.Serialize(base.EntityItem, typeof(CompiledAssemblyProject));
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00049840 File Offset: 0x00047A40
		public override void ReadSlnData(DataItem item)
		{
			CompiledAssemblyProject compiledAssemblyProject = (CompiledAssemblyProject)base.EntityItem;
			compiledAssemblyProject.Configurations.Clear();
			new DataSerializer(MSBuildProjectService.DataContext)
			{
				SerializationContext = 
				{
					BaseFile = base.EntityItem.FileName,
					DirectorySeparatorChar = '\\'
				}
			}.Deserialize(compiledAssemblyProject, item);
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0004989F File Offset: 0x00047A9F
		protected override void SaveItem(IProgressMonitor monitor)
		{
		}
	}
}

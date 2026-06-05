using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects.SharedAssetsProjects
{
	// Token: 0x02000251 RID: 593
	internal class SharedAssetsProjectMSBuildExtension : MSBuildExtension
	{
		// Token: 0x060015E1 RID: 5601 RVA: 0x00058848 File Offset: 0x00056A48
		public override void LoadProject(IProgressMonitor monitor, SolutionEntityItem item, MSBuildProject msproject)
		{
			base.LoadProject(monitor, item, msproject);
			DotNetProject dotNetProject = item as DotNetProject;
			if (dotNetProject == null)
			{
				return;
			}
			foreach (MSBuildImport msbuildImport in from im in msproject.Imports
			where im.Label == "Shared" && im.Project.EndsWith(".projitems")
			select im)
			{
				string text = msbuildImport.Project;
				if (!string.IsNullOrEmpty(text))
				{
					text = MSBuildProjectService.FromMSBuildPath(item.ItemDirectory, text);
					text = Path.Combine(Path.GetDirectoryName(msproject.FileName), text);
					if (File.Exists(text))
					{
						MSBuildSerializer msbuildSerializer = base.Handler.CreateSerializer();
						msbuildSerializer.SerializationContext.BaseFile = text;
						msbuildSerializer.SerializationContext.ProgressMonitor = monitor;
						MSBuildProject msbuildProject = new MSBuildProject();
						msbuildProject.Load(text);
						base.Handler.LoadProjectItems(msbuildProject, msbuildSerializer, ProjectItemFlags.Hidden | ProjectItemFlags.DontPersist);
						ProjectReference projectReference = new ProjectReference(ReferenceType.Project, Path.GetFileNameWithoutExtension(text));
						projectReference.Flags = ProjectItemFlags.DontPersist;
						projectReference.SetItemsProjectPath(text);
						dotNetProject.References.Add(projectReference);
					}
				}
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x000589B4 File Offset: 0x00056BB4
		public override void SaveProject(IProgressMonitor monitor, SolutionEntityItem item, MSBuildProject project)
		{
			base.SaveProject(monitor, item, project);
			DotNetProject dotNetProject = item as DotNetProject;
			if (dotNetProject == null)
			{
				return;
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ProjectReference r in from rp in dotNetProject.References
			where rp.ReferenceType == ReferenceType.Project
			select rp)
			{
				string ip = r.GetItemsProjectPath();
				if (!string.IsNullOrEmpty(ip))
				{
					ip = MSBuildProjectService.ToMSBuildPath(item.ItemDirectory, ip);
					hashSet.Add(ip);
					if (!project.Imports.Any((MSBuildImport im) => im.Project == ip))
					{
						MSBuildImport msbuildImport = project.AddNewImport(ip, project.Imports.FirstOrDefault((MSBuildImport i) => i.Label != "Shared"));
						msbuildImport.Label = "Shared";
						msbuildImport.Condition = "Exists('" + ip + "')";
					}
				}
			}
			foreach (MSBuildImport msbuildImport2 in project.Imports)
			{
				if (msbuildImport2.Label == "Shared" && msbuildImport2.Project.EndsWith(".projitems") && !hashSet.Contains(msbuildImport2.Project))
				{
					project.RemoveImport(msbuildImport2.Project);
				}
			}
		}
	}
}

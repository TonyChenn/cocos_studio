using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.SharedAssetsProjects
{
	// Token: 0x0200024F RID: 591
	public class SharedAssetsProject : Project, IDotNetFileContainer
	{
		// Token: 0x060015B9 RID: 5561 RVA: 0x00057DFB File Offset: 0x00055FFB
		public SharedAssetsProject()
		{
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x00057E03 File Offset: 0x00056003
		public SharedAssetsProject(string language)
		{
			this.languageName = language;
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x00057E12 File Offset: 0x00056012
		public SharedAssetsProject(ProjectCreateInformation projectCreateInfo, XmlElement projectOptions)
		{
			this.languageName = projectOptions.GetAttribute("language");
			this.DefaultNamespace = projectCreateInfo.ProjectName;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x00057E38 File Offset: 0x00056038
		protected internal override List<FilePath> OnGetItemFiles(bool includeReferencedFiles)
		{
			List<FilePath> list = base.OnGetItemFiles(includeReferencedFiles);
			if (!string.IsNullOrEmpty(this.FileName))
			{
				list.Add(this.ProjItemsPath);
			}
			return list;
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x00057E6C File Offset: 0x0005606C
		// (set) Token: 0x060015BE RID: 5566 RVA: 0x00057E74 File Offset: 0x00056074
		public string LanguageName
		{
			get
			{
				return this.languageName;
			}
			set
			{
				this.languageName = value;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x00057E7D File Offset: 0x0005607D
		// (set) Token: 0x060015C0 RID: 5568 RVA: 0x00057E85 File Offset: 0x00056085
		public string DefaultNamespace { get; set; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x00057E90 File Offset: 0x00056090
		// (set) Token: 0x060015C2 RID: 5570 RVA: 0x00057EC4 File Offset: 0x000560C4
		public FilePath ProjItemsPath
		{
			get
			{
				if (this.projItemsPath.IsNull)
				{
					return this.FileName.ChangeExtension(".projitems");
				}
				return this.projItemsPath;
			}
			set
			{
				this.projItemsPath = value;
			}
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00057FB8 File Offset: 0x000561B8
		public override IEnumerable<string> GetProjectTypes()
		{
			yield return "SharedAssets";
			yield return "DotNet";
			yield break;
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00057FD8 File Offset: 0x000561D8
		public override string[] SupportedLanguages
		{
			get
			{
				return new string[]
				{
					"",
					this.languageName
				};
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x00057FFE File Offset: 0x000561FE
		public IDotNetLanguageBinding LanguageBinding
		{
			get
			{
				if (this.languageBinding == null)
				{
					this.languageBinding = (LanguageBindingService.GetBindingPerLanguageName(this.languageName) as IDotNetLanguageBinding);
				}
				return this.languageBinding;
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x00058024 File Offset: 0x00056224
		public override bool IsCompileable(string fileName)
		{
			return this.LanguageBinding.IsSourceCodeFile(fileName);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00058037 File Offset: 0x00056237
		protected override BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return new BuildResult();
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0005803E File Offset: 0x0005623E
		protected internal override bool OnGetSupportsTarget(string target)
		{
			return false;
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00058041 File Offset: 0x00056241
		protected internal override bool OnGetSupportsExecute()
		{
			return false;
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00058044 File Offset: 0x00056244
		public override bool FastCheckNeedsBuild(ConfigurationSelector configuration)
		{
			return false;
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x00058047 File Offset: 0x00056247
		protected override IEnumerable<string> GetStandardBuildActions()
		{
			return BuildAction.DotNetActions;
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0005804E File Offset: 0x0005624E
		protected override IList<string> GetCommonBuildActions()
		{
			return BuildAction.DotNetCommonActions;
		}

		/// <summary>
		/// Gets the default namespace for the file, according to the naming policy.
		/// </summary>
		/// <remarks>Always returns a valid namespace, even if the fileName is null.</remarks>
		// Token: 0x060015CD RID: 5581 RVA: 0x00058055 File Offset: 0x00056255
		public string GetDefaultNamespace(string fileName)
		{
			return DotNetProject.GetDefaultNamespace(this, this.DefaultNamespace, fileName);
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00058064 File Offset: 0x00056264
		protected override void OnBoundToSolution()
		{
			if (this.currentSolution != null)
			{
				this.DisconnectFromSolution();
			}
			base.OnBoundToSolution();
			base.ParentSolution.ReferenceAddedToProject += this.HandleReferenceAddedToProject;
			base.ParentSolution.ReferenceRemovedFromProject += this.HandleReferenceRemovedFromProject;
			base.ParentSolution.SolutionItemAdded += this.HandleSolutionItemAdded;
			this.currentSolution = base.ParentSolution;
			foreach (DotNetProject p in base.ParentSolution.GetAllSolutionItems<DotNetProject>())
			{
				this.ProcessProject(p);
			}
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0005811C File Offset: 0x0005631C
		private void HandleSolutionItemAdded(object sender, SolutionItemChangeEventArgs e)
		{
			DotNetProject dotNetProject = e.SolutionItem as DotNetProject;
			if (dotNetProject != null)
			{
				this.ProcessProject(dotNetProject);
			}
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0005813F File Offset: 0x0005633F
		public override void Dispose()
		{
			base.Dispose();
			this.DisconnectFromSolution();
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x00058150 File Offset: 0x00056350
		private void DisconnectFromSolution()
		{
			if (this.currentSolution != null)
			{
				this.currentSolution.ReferenceAddedToProject -= this.HandleReferenceAddedToProject;
				this.currentSolution.ReferenceRemovedFromProject -= this.HandleReferenceRemovedFromProject;
				this.currentSolution.SolutionItemAdded -= this.HandleSolutionItemAdded;
				this.currentSolution = null;
			}
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x000581B4 File Offset: 0x000563B4
		private void HandleReferenceRemovedFromProject(object sender, ProjectReferenceEventArgs e)
		{
			if (e.ProjectReference.ReferenceType == ReferenceType.Project && e.ProjectReference.Reference == this.Name)
			{
				foreach (ProjectFile projectFile in base.Files)
				{
					ProjectFile projectFile2 = e.Project.GetProjectFile(projectFile.FilePath);
					if ((projectFile2.Flags & ProjectItemFlags.DontPersist) != ProjectItemFlags.None)
					{
						e.Project.Files.Remove(projectFile2.FilePath);
					}
				}
			}
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0005825C File Offset: 0x0005645C
		private void HandleReferenceAddedToProject(object sender, ProjectReferenceEventArgs e)
		{
			if (e.ProjectReference.ReferenceType == ReferenceType.Project && e.ProjectReference.Reference == this.Name)
			{
				this.ProcessNewReference(e.ProjectReference);
			}
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x000582DC File Offset: 0x000564DC
		private void ProcessProject(DotNetProject p)
		{
			List<ProjectReference> list = (from r in p.References
			where r.GetItemsProjectPath() == this.ProjItemsPath && r.Reference != this.Name
			select r).ToList<ProjectReference>();
			foreach (ProjectReference item in list)
			{
				p.References.Remove(item);
				p.References.Add(new ProjectReference(this));
			}
			foreach (ProjectReference pref in from r in p.References
			where r.ReferenceType == ReferenceType.Project && r.Reference == this.Name
			select r)
			{
				this.ProcessNewReference(pref);
			}
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x000583B0 File Offset: 0x000565B0
		private void ProcessNewReference(ProjectReference pref)
		{
			pref.Flags = ProjectItemFlags.DontPersist;
			pref.SetItemsProjectPath(this.ProjItemsPath);
			foreach (ProjectFile projectFile in base.Files)
			{
				if (pref.OwnerProject.Files.GetFile(projectFile.FilePath) == null)
				{
					ProjectFile projectFile2 = (ProjectFile)projectFile.Clone();
					projectFile2.Flags |= (ProjectItemFlags.Hidden | ProjectItemFlags.DontPersist);
					pref.OwnerProject.Files.Add(projectFile2);
				}
			}
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00058454 File Offset: 0x00056654
		protected override void OnFilePropertyChangedInProject(ProjectFileEventArgs e)
		{
			base.OnFilePropertyChangedInProject(e);
			foreach (DotNetProject dotNetProject in this.GetReferencingProjects())
			{
				foreach (ProjectFileEventInfo projectFileEventInfo in e)
				{
					if (projectFileEventInfo.ProjectFile.Subtype != Subtype.Directory)
					{
						ProjectFile projectFile = (ProjectFile)projectFileEventInfo.ProjectFile.Clone();
						projectFile.Flags |= (ProjectItemFlags.Hidden | ProjectItemFlags.DontPersist);
						dotNetProject.Files.Remove(projectFile.FilePath);
						dotNetProject.Files.Add(projectFile);
					}
				}
			}
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00058528 File Offset: 0x00056728
		protected override void OnFileAddedToProject(ProjectFileEventArgs e)
		{
			base.OnFileAddedToProject(e);
			foreach (DotNetProject dotNetProject in this.GetReferencingProjects())
			{
				foreach (ProjectFileEventInfo projectFileEventInfo in e)
				{
					if (projectFileEventInfo.ProjectFile.Subtype != Subtype.Directory && dotNetProject.Files.GetFile(projectFileEventInfo.ProjectFile.FilePath) == null)
					{
						ProjectFile projectFile = (ProjectFile)projectFileEventInfo.ProjectFile.Clone();
						projectFile.Flags |= (ProjectItemFlags.Hidden | ProjectItemFlags.DontPersist);
						dotNetProject.Files.Add(projectFile);
					}
				}
			}
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00058604 File Offset: 0x00056804
		protected override void OnFileRemovedFromProject(ProjectFileEventArgs e)
		{
			base.OnFileRemovedFromProject(e);
			foreach (DotNetProject dotNetProject in this.GetReferencingProjects())
			{
				foreach (ProjectFileEventInfo projectFileEventInfo in e)
				{
					if (projectFileEventInfo.ProjectFile.Subtype != Subtype.Directory)
					{
						dotNetProject.Files.Remove(projectFileEventInfo.ProjectFile.FilePath);
					}
				}
			}
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x000586AC File Offset: 0x000568AC
		protected override void OnFileRenamedInProject(ProjectFileRenamedEventArgs e)
		{
			base.OnFileRenamedInProject(e);
			foreach (DotNetProject dotNetProject in this.GetReferencingProjects())
			{
				foreach (ProjectFileRenamedEventInfo projectFileRenamedEventInfo in e)
				{
					if (projectFileRenamedEventInfo.ProjectFile.Subtype != Subtype.Directory)
					{
						ProjectFile projectFile = (ProjectFile)projectFileRenamedEventInfo.ProjectFile.Clone();
						dotNetProject.Files.Remove(projectFileRenamedEventInfo.OldName);
						projectFile.Flags |= (ProjectItemFlags.Hidden | ProjectItemFlags.DontPersist);
						dotNetProject.Files.Add(projectFile);
					}
				}
			}
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x000587B8 File Offset: 0x000569B8
		private IEnumerable<DotNetProject> GetReferencingProjects()
		{
			if (base.ParentSolution == null)
			{
				return new DotNetProject[0];
			}
			return from p in base.ParentSolution.GetAllSolutionItems<DotNetProject>()
			where p.References.Any((ProjectReference r) => r.GetItemsProjectPath() != null)
			select p;
		}

		// Token: 0x0400068C RID: 1676
		private Solution currentSolution;

		// Token: 0x0400068D RID: 1677
		private IDotNetLanguageBinding languageBinding;

		// Token: 0x0400068E RID: 1678
		private string languageName;

		// Token: 0x0400068F RID: 1679
		private FilePath projItemsPath;
	}
}

using System;
using System.IO;
using System.Xml;

namespace MonoDevelop.Projects
{
	// Token: 0x02000120 RID: 288
	public class DotNetProjectBinding : IProjectBinding
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x00028765 File Offset: 0x00026965
		public virtual string Name
		{
			get
			{
				return "DotNet";
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002876C File Offset: 0x0002696C
		public Project CreateProject(ProjectCreateInformation info, XmlElement projectOptions)
		{
			string attribute = projectOptions.GetAttribute("language");
			return this.CreateProject(attribute, info, projectOptions);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0002878E File Offset: 0x0002698E
		protected virtual DotNetProject CreateProject(string languageName, ProjectCreateInformation info, XmlElement projectOptions)
		{
			return new DotNetAssemblyProject(languageName, info, projectOptions);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00028798 File Offset: 0x00026998
		public Project CreateSingleFileProject(string file)
		{
			IDotNetLanguageBinding dotNetLanguageBinding = LanguageBindingService.GetBindingPerFileName(file) as IDotNetLanguageBinding;
			if (dotNetLanguageBinding != null)
			{
				ProjectCreateInformation projectCreateInformation = new ProjectCreateInformation();
				projectCreateInformation.ProjectName = Path.GetFileNameWithoutExtension(file);
				projectCreateInformation.SolutionPath = Path.GetDirectoryName(file);
				projectCreateInformation.ProjectBasePath = Path.GetDirectoryName(file);
				Project project = this.CreateProject(dotNetLanguageBinding.Language, projectCreateInformation, null);
				project.Files.Add(new ProjectFile(file));
				return project;
			}
			return null;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002880C File Offset: 0x00026A0C
		public bool CanCreateSingleFileProject(string file)
		{
			IDotNetLanguageBinding dotNetLanguageBinding = LanguageBindingService.GetBindingPerFileName(file) as IDotNetLanguageBinding;
			return dotNetLanguageBinding != null;
		}
	}
}

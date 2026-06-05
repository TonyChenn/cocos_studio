using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000142 RID: 322
	public class ProjectCreateInformation
	{
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x0002D217 File Offset: 0x0002B417
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x0002D21F File Offset: 0x0002B41F
		public string ProjectName
		{
			get
			{
				return this.projectName;
			}
			set
			{
				this.projectName = value;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x0002D228 File Offset: 0x0002B428
		// (set) Token: 0x06000C15 RID: 3093 RVA: 0x0002D230 File Offset: 0x0002B430
		public string SolutionName
		{
			get
			{
				return this.solutionName;
			}
			set
			{
				this.solutionName = value;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0002D23C File Offset: 0x0002B43C
		public FilePath BinPath
		{
			get
			{
				return this.projectBasePath.Combine(new string[]
				{
					"bin"
				});
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0002D264 File Offset: 0x0002B464
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x0002D26C File Offset: 0x0002B46C
		public FilePath SolutionPath
		{
			get
			{
				return this.solutionPath;
			}
			set
			{
				this.solutionPath = value;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x0002D275 File Offset: 0x0002B475
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x0002D27D File Offset: 0x0002B47D
		public FilePath ProjectBasePath
		{
			get
			{
				return this.projectBasePath;
			}
			set
			{
				this.projectBasePath = value;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0002D286 File Offset: 0x0002B486
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x0002D28E File Offset: 0x0002B48E
		public SolutionFolder ParentFolder { get; set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x0002D297 File Offset: 0x0002B497
		// (set) Token: 0x06000C1E RID: 3102 RVA: 0x0002D29F File Offset: 0x0002B49F
		public ConfigurationSelector ActiveConfiguration { get; set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x0002D2A8 File Offset: 0x0002B4A8
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x0002D2B0 File Offset: 0x0002B4B0
		public ProjectCreateParameters Parameters { get; set; }

		// Token: 0x06000C21 RID: 3105 RVA: 0x0002D2B9 File Offset: 0x0002B4B9
		public ProjectCreateInformation()
		{
			this.Parameters = new ProjectCreateParameters();
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0002D2CC File Offset: 0x0002B4CC
		public ProjectCreateInformation(ProjectCreateInformation projectCreateInformation)
		{
			this.projectName = projectCreateInformation.ProjectName;
			this.solutionName = projectCreateInformation.SolutionName;
			this.solutionPath = projectCreateInformation.SolutionPath;
			this.projectBasePath = projectCreateInformation.ProjectBasePath;
			this.ParentFolder = projectCreateInformation.ParentFolder;
			this.ActiveConfiguration = projectCreateInformation.ActiveConfiguration;
			this.Parameters = projectCreateInformation.Parameters;
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0002D334 File Offset: 0x0002B534
		public bool ShouldCreate(string createCondition)
		{
			if (string.IsNullOrWhiteSpace(createCondition))
			{
				return true;
			}
			createCondition = createCondition.Trim();
			string notConditionParameterName = ProjectCreateInformation.GetNotConditionParameterName(createCondition);
			if (notConditionParameterName != null)
			{
				return !this.Parameters.GetBoolean(notConditionParameterName, false);
			}
			return this.Parameters.GetBoolean(createCondition, false);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0002D37B File Offset: 0x0002B57B
		private static string GetNotConditionParameterName(string createCondition)
		{
			if (createCondition.StartsWith("!"))
			{
				return createCondition.Substring(1).TrimStart(new char[0]);
			}
			return null;
		}

		// Token: 0x0400039C RID: 924
		private string projectName;

		// Token: 0x0400039D RID: 925
		private string solutionName;

		// Token: 0x0400039E RID: 926
		private FilePath solutionPath;

		// Token: 0x0400039F RID: 927
		private FilePath projectBasePath;
	}
}

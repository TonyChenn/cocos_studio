using System;
using System.CodeDom.Compiler;
using System.Xml;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000144 RID: 324
	public interface IDotNetLanguageBinding : ILanguageBinding
	{
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000C2B RID: 3115
		string ProjectStockIcon { get; }

		// Token: 0x06000C2C RID: 3116
		ConfigurationParameters CreateCompilationParameters(XmlElement projectOptions);

		// Token: 0x06000C2D RID: 3117
		ProjectParameters CreateProjectParameters(XmlElement projectOptions);

		// Token: 0x06000C2E RID: 3118
		BuildResult Compile(ProjectItemCollection items, DotNetProjectConfiguration configuration, ConfigurationSelector configSelector, IProgressMonitor monitor);

		// Token: 0x06000C2F RID: 3119
		ClrVersion[] GetSupportedClrVersions();

		// Token: 0x06000C30 RID: 3120
		CodeDomProvider GetCodeDomProvider();
	}
}

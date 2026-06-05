using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Projects
{
	// Token: 0x02000127 RID: 295
	[Extension]
	internal class ProjectTagProvider : StringTagProvider<DotNetProjectConfiguration>, IStringTagProvider
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x00028F54 File Offset: 0x00027154
		public override IEnumerable<StringTagDescription> GetTags()
		{
			yield return new StringTagDescription("ProjectConfig", GettextCatalog.GetString("Project Configuration"));
			yield return new StringTagDescription("ProjectConfigName", GettextCatalog.GetString("Project Configuration Name"));
			yield return new StringTagDescription("ProjectConfigPlat", GettextCatalog.GetString("Project Configuration Platform"));
			yield return new StringTagDescription("TargetFile", GettextCatalog.GetString("Target File"));
			yield return new StringTagDescription("TargetPath", GettextCatalog.GetString("Target Path"));
			yield return new StringTagDescription("TargetName", GettextCatalog.GetString("Target Name"));
			yield return new StringTagDescription("TargetDir", GettextCatalog.GetString("Target Directory"));
			yield return new StringTagDescription("TargetExt", GettextCatalog.GetString("Target Extension"));
			yield break;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00028F74 File Offset: 0x00027174
		public override object GetTagValue(DotNetProjectConfiguration conf, string tag)
		{
			switch (tag)
			{
			case "TARGETPATH":
			case "TARGETFILE":
				return conf.CompiledOutputName;
			case "TARGETNAME":
				return conf.CompiledOutputName.FileName;
			case "TARGETDIR":
				return conf.CompiledOutputName.ParentDirectory;
			case "TARGETEXT":
				return conf.CompiledOutputName.Extension;
			case "PROJECTCONFIG":
				return conf.Name + "." + conf.Platform;
			case "PROJECTCONFIGNAME":
				return conf.Name;
			case "PROJECTCONFIGPLAT":
				return conf.Platform;
			}
			throw new NotSupportedException();
		}
	}
}

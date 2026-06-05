using System;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Addins;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x0200020F RID: 527
	public abstract class DocGenerator
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x0005292B File Offset: 0x00050B2B
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x00052932 File Offset: 0x00050B32
		public static DocGenerator Instance { get; private set; }

		// Token: 0x060013E0 RID: 5088
		public abstract string GenerateDocumentation(IMember member, string linePrefix);

		// Token: 0x060013E1 RID: 5089 RVA: 0x00052976 File Offset: 0x00050B76
		static DocGenerator()
		{
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/ProjectModel/DocumentationGenerator", delegate(object sender, ExtensionNodeEventArgs args)
			{
				ExtensionChange change = args.Change;
				if (change != ExtensionChange.Add)
				{
					return;
				}
				if (DocGenerator.Instance != null)
				{
					LoggingService.LogWarning("Duplicate doc generator defined.");
				}
				DocGenerator.Instance = (DocGenerator)args.ExtensionObject;
			});
		}
	}
}

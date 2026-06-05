using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Projects
{
	// Token: 0x02000164 RID: 356
	[Extension]
	internal class SolutionTagProvider : StringTagProvider<Solution>, IStringTagProvider
	{
		// Token: 0x06000DD2 RID: 3538 RVA: 0x00032C08 File Offset: 0x00030E08
		public override IEnumerable<StringTagDescription> GetTags()
		{
			yield return new StringTagDescription("SolutionFile", GettextCatalog.GetString("Solution File"));
			yield return new StringTagDescription("SolutionName", GettextCatalog.GetString("Solution Name"));
			yield return new StringTagDescription("SolutionDir", GettextCatalog.GetString("Solution Directory"));
			yield break;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00032C28 File Offset: 0x00030E28
		public override object GetTagValue(Solution sol, string tag)
		{
			if (tag != null)
			{
				if (tag == "SOLUTIONNAME")
				{
					return sol.Name;
				}
				if (tag == "COMBINEFILENAME" || tag == "SOLUTIONFILE")
				{
					return sol.FileName;
				}
				if (tag == "SOLUTIONDIR")
				{
					return sol.BaseDirectory;
				}
			}
			throw new NotSupportedException();
		}
	}
}

using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Projects
{
	// Token: 0x02000113 RID: 275
	[Extension]
	internal class SolutionEntityItemTagProvider : StringTagProvider<SolutionEntityItem>, IStringTagProvider
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x00028224 File Offset: 0x00026424
		public override IEnumerable<StringTagDescription> GetTags()
		{
			yield return new StringTagDescription("ProjectFile", "Project File");
			yield break;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00028244 File Offset: 0x00026444
		public override object GetTagValue(SolutionEntityItem item, string tag)
		{
			if (tag != null && (tag == "ITEMFILE" || tag == "PROJECTFILE" || tag == "PROJECTFILENAME"))
			{
				return item.FileName;
			}
			throw new NotSupportedException();
		}
	}
}

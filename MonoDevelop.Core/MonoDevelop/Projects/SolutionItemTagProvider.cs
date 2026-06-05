using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Projects
{
	// Token: 0x0200016F RID: 367
	[Extension]
	internal class SolutionItemTagProvider : StringTagProvider<SolutionItem>, IStringTagProvider
	{
		// Token: 0x06000E66 RID: 3686 RVA: 0x000355F4 File Offset: 0x000337F4
		public override IEnumerable<StringTagDescription> GetTags()
		{
			yield return new StringTagDescription("ProjectName", "Project Name");
			yield return new StringTagDescription("ProjectDir", "Project Directory");
			yield return new StringTagDescription("AuthorName", "Project Author Name");
			yield return new StringTagDescription("AuthorEmail", "Project Author Email");
			yield return new StringTagDescription("AuthorCopyright", "Project Author Copyright");
			yield return new StringTagDescription("AuthorCompany", "Project Author Company");
			yield return new StringTagDescription("AuthorTrademark", "Project Trademark");
			yield break;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00035614 File Offset: 0x00033814
		public override object GetTagValue(SolutionItem item, string tag)
		{
			switch (tag)
			{
			case "ITEMNAME":
			case "PROJECTNAME":
				return item.Name;
			case "AUTHORCOPYRIGHT":
			{
				AuthorInformation authorInformation = item.AuthorInformation ?? AuthorInformation.Default;
				return authorInformation.Copyright;
			}
			case "AUTHORCOMPANY":
			{
				AuthorInformation authorInformation = item.AuthorInformation ?? AuthorInformation.Default;
				return authorInformation.Company;
			}
			case "AUTHORTRADEMARK":
			{
				AuthorInformation authorInformation = item.AuthorInformation ?? AuthorInformation.Default;
				return authorInformation.Trademark;
			}
			case "AUTHOREMAIL":
			{
				AuthorInformation authorInformation = item.AuthorInformation ?? AuthorInformation.Default;
				return authorInformation.Email;
			}
			case "AUTHORNAME":
			{
				AuthorInformation authorInformation = item.AuthorInformation ?? AuthorInformation.Default;
				return authorInformation.Name;
			}
			case "ITEMDIR":
			case "PROJECTDIR":
				return item.BaseDirectory;
			}
			throw new NotSupportedException();
		}
	}
}

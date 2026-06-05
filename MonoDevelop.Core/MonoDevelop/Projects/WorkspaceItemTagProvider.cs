using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Projects
{
	// Token: 0x02000160 RID: 352
	[Extension]
	internal class WorkspaceItemTagProvider : IStringTagProvider
	{
		// Token: 0x06000D55 RID: 3413 RVA: 0x00030D0C File Offset: 0x0002EF0C
		public IEnumerable<StringTagDescription> GetTags(Type type)
		{
			if (typeof(WorkspaceItem).IsAssignableFrom(type) && !typeof(Solution).IsAssignableFrom(type))
			{
				yield return new StringTagDescription("WorkspaceFile", GettextCatalog.GetString("Workspace File"));
				yield return new StringTagDescription("WorkspaceName", GettextCatalog.GetString("Workspace Name"));
				yield return new StringTagDescription("WorkspaceDir", GettextCatalog.GetString("Workspace Directory"));
			}
			yield break;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00030D30 File Offset: 0x0002EF30
		public object GetTagValue(object obj, string tag)
		{
			WorkspaceItem workspaceItem = (WorkspaceItem)obj;
			if (tag != null)
			{
				if (tag == "WORKSPACENAME")
				{
					return workspaceItem.Name;
				}
				if (tag == "WORKSPACEFILE")
				{
					return workspaceItem.FileName;
				}
				if (tag == "WORKSPACEDIR")
				{
					return workspaceItem.BaseDirectory;
				}
			}
			throw new NotSupportedException();
		}
	}
}

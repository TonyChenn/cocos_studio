using System;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel.Upgrade
{
	// Token: 0x0200000D RID: 13
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_212 : SolutionUpgrader
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000029A9 File Offset: 0x00000BA9
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_212.version;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000029B0 File Offset: 0x00000BB0
		protected override bool OnUpgrade(Solution sln)
		{
			if (sln.UserData.Properties.ContainsKey("TabsParamsKey"))
			{
				return false;
			}
			if (sln.UserData.OpenedDocuments != null && sln.UserData.ActiveDocument != null)
			{
				TabsInfo value = new TabsInfo
				{
					OpenedDocuments = sln.UserData.OpenedDocuments,
					ActiveDocument = sln.UserData.ActiveDocument
				};
				sln.UserData.Properties.Add("TabsParamsKey", value);
				sln.UserData.OpenedDocuments = null;
				sln.UserData.ActiveDocument = null;
			}
			return true;
		}

		// Token: 0x0400000D RID: 13
		private static readonly Version version = new Version("2.1.2.0");
	}
}

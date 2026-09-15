using System;
using Mono.Addins;

namespace CocoStudio.Projects.ExtensionModel.Upgrade
{
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_212 : SolutionUpgrader
	{
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_212.version;
			}
		}

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

		private static readonly Version version = new Version("2.1.2.0");
	}
}

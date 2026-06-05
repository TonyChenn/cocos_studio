using System;
using Mono.Addins;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x020000E3 RID: 227
	internal class PlatformCondition : ConditionType
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x000207E0 File Offset: 0x0001E9E0
		public override bool Evaluate(NodeElement conditionNode)
		{
			string text = conditionNode.GetAttribute("value");
			bool flag = false;
			if (text.StartsWith("!", StringComparison.Ordinal))
			{
				text = text.Substring(1);
				flag = true;
			}
			string key;
			bool flag2;
			switch (key = text.ToLower())
			{
			case "windows":
			case "win32":
				flag2 = Platform.IsWindows;
				goto IL_FC;
			case "mac":
			case "macos":
			case "macosx":
				flag2 = Platform.IsMac;
				goto IL_FC;
			case "unix":
			case "linux":
				flag2 = (!Platform.IsMac && !Platform.IsWindows);
				goto IL_FC;
			}
			flag2 = false;
			IL_FC:
			Version v;
			if (Version.TryParse(conditionNode.GetAttribute("minVersion"), out v))
			{
				flag2 &= ((Platform.IsMac ? MacSystemInformation.OsVersion : Environment.OSVersion.Version) >= v);
			}
			if (Version.TryParse(conditionNode.GetAttribute("maxVersion"), out v))
			{
				flag2 &= ((Platform.IsMac ? MacSystemInformation.OsVersion : Environment.OSVersion.Version) <= v);
			}
			if (!flag)
			{
				return flag2;
			}
			return !flag2;
		}
	}
}

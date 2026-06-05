using System;
using System.Collections.Generic;
using System.IO;
using Mono.Addins;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x0200023E RID: 574
	public class TargetsAvailableCondition : ConditionType
	{
		// Token: 0x0600153B RID: 5435 RVA: 0x00056D00 File Offset: 0x00054F00
		public override bool Evaluate(NodeElement conditionNode)
		{
			string attribute = conditionNode.GetAttribute("target");
			if (string.IsNullOrEmpty(attribute))
			{
				return false;
			}
			string msbuildExtensionsPath = Runtime.SystemAssemblyService.CurrentRuntime.GetMSBuildExtensionsPath();
			Dictionary<string, string> customTags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				{
					"MSBuildExtensionsPath64",
					msbuildExtensionsPath
				},
				{
					"MSBuildExtensionsPath32",
					msbuildExtensionsPath
				},
				{
					"MSBuildExtensionsPath",
					msbuildExtensionsPath
				}
			};
			string text = StringParserService.Parse<string>(attribute, customTags);
			if (Path.DirectorySeparatorChar != '\\')
			{
				text = text.Replace('\\', Path.DirectorySeparatorChar);
			}
			return File.Exists(text);
		}
	}
}

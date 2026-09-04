using System;
using System.IO;
using CocoStudio.Basic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CocoStudio.Projects.ExtensionModel.Upgrade
{
	// Token: 0x02000008 RID: 8
	public static class SolutionUpgraderHelper
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002520 File Offset: 0x00000720
		public static void UpdateConfigJson(string path, bool isLandscape)
		{
			try
			{
				string path2 = Path.Combine(path, "config.json");
				JObject jobject;
				if (!File.Exists(path2))
				{
					jobject = JObject.Parse("{ 'init_cfg':{ 'isLandscape': false, 'width': 640, 'height': 960 } }");
				}
				else
				{
					jobject = JObject.Parse(File.ReadAllText(path2));
				}
				if (jobject != null)
				{
					JObject jobject2 = jobject["init_cfg"] as JObject;
					jobject2["isLandscape"] = isLandscape;
					int num = (int)jobject2["width"];
					int num2 = (int)jobject2["height"];
					if ((isLandscape && num < num2) || (!isLandscape && num > num2))
					{
						jobject2["width"] = num2;
						jobject2["height"] = num;
					}
					string contents = JsonConvert.SerializeObject(jobject, Formatting.Indented);
					File.WriteAllText(path2, contents);
				}
			}
			catch (Exception ex)
			{
				LogConfig.OutputWithoutTip.Error(ex.Data);
			}
		}

		// Token: 0x04000009 RID: 9
		private const string ConfigFilename = "config.json";

		// Token: 0x0400000A RID: 10
		private const string DefaultConfig = "{ 'init_cfg':{ 'isLandscape': false, 'width': 640, 'height': 960 } }";
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x0200007F RID: 127
	[Extension(typeof(IGameFileSerializer))]
	[SerializerExtension(true)]
	internal class JsonSerializer : BaseCocosFileSerializer
	{
		// Token: 0x06000476 RID: 1142 RVA: 0x000137E0 File Offset: 0x000119E0
		protected override string OnGetID()
		{
			return "Serializer_Json";
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000137F8 File Offset: 0x000119F8
		protected override string OnGetLabel()
		{
			return LanguageInfo.ProjSetting_jsonFile;
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00013810 File Offset: 0x00011A10
		public override string Description
		{
			get
			{
				return LanguageInfo.ProjSetting_jsonInfo;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00013828 File Offset: 0x00011A28
		protected override int DisplayIndex
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001383B File Offset: 0x00011A3B
		public JsonSerializer()
		{
			this.InitSerializeSetting();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00013850 File Offset: 0x00011A50
		private void InitSerializeSetting()
		{
			this.setting = new JsonSerializerSettings();
			this.setting.DefaultValueHandling = DefaultValueHandling.Include;
			this.setting.NullValueHandling = NullValueHandling.Ignore;
			this.setting.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
			this.setting.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
			this.setting.TypeNameHandling = TypeNameHandling.None;
			this.setting.Formatting = Formatting.Indented;
			JsonConverter item = new StringEnumConverter();
			JsonConverter item2 = new FloatJsonConvert();
			this.setting.Converters.Add(item);
			this.setting.Converters.Add(item2);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000138E8 File Offset: 0x00011AE8
		protected override string OnSerialize(PublishInfo info, GameFile gameFile)
		{
			string result;
			try
			{
				GameFileData gameFileData = this.SetUsedResources(gameFile);
				string path = Path.ChangeExtension(info.DestinationFilePath, ".json");
				string contents = JsonConvert.SerializeObject(gameFile, this.setting);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				File.WriteAllText(path, contents);
				if (gameFileData != null)
				{
					gameFileData.UsedResources = null;
				}
				result = string.Empty;
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("发布Json失败：\r\n" + ex.ToString());
				result = string.Format("Failed to publish to json, {0}", ex.Message);
			}
			return result;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000139AC File Offset: 0x00011BAC
		private GameFileData SetUsedResources(GameFile gameFile)
		{
			GameFileData result;
			try
			{
				IProgressMonitor defaultMonitor = ProjectsService.Instance.DefaultMonitor;
				HashSet<ResourceData> usedResources = gameFile.GetUsedResources(defaultMonitor);
				HashSet<ResourceData> hashSet = ProjectsService.Instance.ProcessResourceDatas(defaultMonitor, usedResources);
				if (!defaultMonitor.AsyncOperation.Success)
				{
					result = null;
				}
				else
				{
					string baseDir = gameFile.FileName;
					List<string> list = new List<string>();
					foreach (ResourceData resourceData in hashSet)
					{
						string text = this.GetRelativePath(baseDir, resourceData.Path);
						if (Path.GetExtension(text).Equals(".csd"))
						{
							text = Path.ChangeExtension(text, ".json");
						}
						list.Add(text);
					}
					foreach (ResourceData resourceData in usedResources)
					{
						if (resourceData != null && resourceData.Type == EnumResourceType.MarkedSubImage)
						{
							string relativePath = this.GetRelativePath(baseDir, resourceData.Plist);
							if (!list.Contains(relativePath))
							{
								string item = Path.ChangeExtension(relativePath, ".png");
								list.Add(relativePath);
								list.Add(item);
							}
						}
					}
					list.Sort();
					GameFileContent gameFileContent = gameFile.Content as GameFileContent;
					GameFileData content = gameFileContent.Content;
					content.UsedResources = list;
					result = content;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("创建项目文件使用的资源列表时出错", exception);
				result = null;
			}
			return result;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00013BC0 File Offset: 0x00011DC0
		private string GetRelativePath(string baseDir, string resourcePath)
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			string uriString = Path.Combine(currentSolution.BaseDirectory, "CocosStudio".ToLower(), resourcePath);
			Uri uri = new Uri(baseDir);
			Uri uri2 = new Uri(uriString);
			Uri uri3 = uri.MakeRelativeUri(uri2);
			string filePath = Uri.UnescapeDataString(uri3.ToString());
			return Option.ConvertToMacPath(filePath);
		}

		// Token: 0x04000223 RID: 547
		public const string JsonFileExtension = ".json";

		// Token: 0x04000224 RID: 548
		private JsonSerializerSettings setting;
	}
}

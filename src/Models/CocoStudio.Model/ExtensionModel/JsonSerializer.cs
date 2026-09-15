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
	[Extension(typeof(IGameFileSerializer))]
	[SerializerExtension(true)]
	internal class JsonSerializer : BaseCocosFileSerializer
	{
		protected override string OnGetID()
		{
			return "Serializer_Json";
		}

		protected override string OnGetLabel()
		{
			return LanguageInfo.ProjSetting_jsonFile;
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.ProjSetting_jsonInfo;
			}
		}

		protected override int DisplayIndex
		{
			get
			{
				return 1;
			}
		}

		public JsonSerializer()
		{
			this.InitSerializeSetting();
		}

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

		public const string JsonFileExtension = ".json";

		private JsonSerializerSettings setting;
	}
}

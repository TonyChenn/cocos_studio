using System;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using EditorCommon.JsonModel.Component.GUI;
using EditorCommon.JsonModel.JsonManager;
using Gdk;
using GLib;
using Modules.Communal.ProjectsConvertor.Model;
using MonoDevelop.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EditorCommon.JsonModel
{
	// Token: 0x02000030 RID: 48
	public class JsonFileHelp
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000354 RID: 852 RVA: 0x000086E9 File Offset: 0x000068E9
		// (set) Token: 0x06000355 RID: 853 RVA: 0x000086F0 File Offset: 0x000068F0
		internal static IProgressMonitor Monitor { get; set; }

		// Token: 0x06000356 RID: 854 RVA: 0x000086F8 File Offset: 0x000068F8
		private static string FormateJson(string unFormateJsonString)
		{
			object value = JsonConvert.DeserializeObject(unFormateJsonString);
			return JsonConvert.SerializeObject(value, Formatting.Indented);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00008714 File Offset: 0x00006914
		public static string UnFormateJson(string formateJsonString)
		{
			object value = JsonConvert.DeserializeObject(formateJsonString);
			return JsonConvert.SerializeObject(value, Formatting.None);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00008730 File Offset: 0x00006930
		private static Stream ReadFormateJson(string filePath)
		{
			string formateJsonString = File.ReadAllText(filePath);
			string s = JsonFileHelp.UnFormateJson(formateJsonString);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			return new MemoryStream(bytes);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00008760 File Offset: 0x00006960
		private static JsonSerializerSettings GetJsonSerializerSettings(bool isFormating = false, bool isTypeNameNone = false)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
			jsonSerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
			jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
			if (isTypeNameNone)
			{
				jsonSerializerSettings.TypeNameHandling = TypeNameHandling.None;
			}
			if (isFormating)
			{
				jsonSerializerSettings.Formatting = Formatting.Indented;
			}
			return jsonSerializerSettings;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000879D File Offset: 0x0000699D
		public static void SetOldToNewRelativeResDir(string oldjsonabspath, string oldresDir = "")
		{
			if (!string.IsNullOrEmpty(oldresDir))
			{
				JsonFileHelp.oldresDirs = oldresDir;
			}
			JsonFileHelp.uiAniRelasResDir = FileService.AbsoluteToRelativePath(JsonFileHelp.oldresDirs, oldjsonabspath);
			JsonFileHelp.uiAniRelasResDir = Option.ConvertToMacPath(JsonFileHelp.uiAniRelasResDir);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000087CC File Offset: 0x000069CC
		public static void SetOldResDir(string oldresDir)
		{
			JsonFileHelp.oldresDirs = oldresDir;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000087D4 File Offset: 0x000069D4
		public static string GetResRelativePath(string absPath)
		{
			string filePath = FileService.AbsoluteToRelativePath(Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath, absPath);
			return Option.ConvertToMacPath(filePath);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00008804 File Offset: 0x00006A04
		public static string GetResAbsPath(string relativePath)
		{
			string directoryName = Path.GetDirectoryName(JsonFileHelp.uiAndAniJson);
			return Path.Combine(directoryName, relativePath);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00008828 File Offset: 0x00006A28
		public static void ReportWarning(string message)
		{
			if (JsonFileHelp.Monitor != null)
			{
				Timeout.Add(0U, () => false);
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00008858 File Offset: 0x00006A58
		public static void ImportCanvas(string filePath, GameFileData gameFileData)
		{
			if (!File.Exists(filePath))
			{
				return;
			}
			CanvasGameObjectSurrogate canvasGameObjectSurrogate = null;
			string jsonData = File.ReadAllText(filePath);
			string value = JsonFileHelp.SceneVersionConvertToNewJsonData(jsonData);
			try
			{
				canvasGameObjectSurrogate = JsonConvert.DeserializeObject<CanvasGameObjectSurrogate>(value, JsonFileHelp.GetJsonSerializerSettings(false, false));
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(ex.ToString());
			}
			JsonFileHelp.setCanvasSize(canvasGameObjectSurrogate, filePath);
			canvasGameObjectSurrogate.InitGameFileData(gameFileData);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000088C0 File Offset: 0x00006AC0
		private static void setCanvasSize(CanvasGameObjectSurrogate canvas, string filePath)
		{
			string json = File.ReadAllText(filePath);
			JObject jobject = null;
			try
			{
				jobject = JObject.Parse(json);
			}
			catch (Exception ex)
			{
				LogConfig.Output.Error(ex.ToString());
			}
			JToken jtoken = jobject["CanvasSize"];
			if (jtoken != null)
			{
				int width = (int)jtoken["_width"];
				int height = (int)jtoken["_height"];
				canvas.CanvasSize = new Size(width, height);
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00008944 File Offset: 0x00006B44
		private static string SceneVersionConvertToNewJsonData(string jsonData)
		{
			string text = jsonData.Replace("__type", "$type");
			text = text.Replace("ComGameObjectSurrogate:#EditorCommon.JsonModel", "EditorCommon.JsonModel.ComGameObjectSurrogate, Modules.Communal.ProjectsConvertor");
			text = text.Replace("ComArmatureAdapterSurrogate:#EditorCommon.JsonModel.Component", "EditorCommon.JsonModel.Component.ComArmatureAdapterSurrogate, Modules.Communal.ProjectsConvertor");
			text = text.Replace("ComGameMapSurrogate:#EditorCommon.JsonModel.Component", "EditorCommon.JsonModel.Component.ComGameMapSurrogate, Modules.Communal.ProjectsConvertor");
			text = text.Replace("ComGUIAdapterSurrogate:#EditorCommon.JsonModel.Component", "EditorCommon.JsonModel.Component.ComGUIAdapterSurrogate, Modules.Communal.ProjectsConvertor");
			text = text.Replace("ComParticleSystemSurrogate:#EditorCommon.JsonModel.Component", "EditorCommon.JsonModel.Component.ComParticleSystemSurrogate, Modules.Communal.ProjectsConvertor");
			text = text.Replace("ComPropertySurrogate:#EditorCommon.JsonModel.Component", "EditorCommon.JsonModel.Component.ComPropertySurrogate, Modules.Communal.ProjectsConvertor");
			text = text.Replace("ComSceneSurrogate:#EditorCommon.JsonModel.Component", "EditorCommon.JsonModel.Component.ComSceneSurrogate, Modules.Communal.ProjectsConvertor");
			text = text.Replace("ComSimpleAudioSurrogate:#EditorCommon.JsonModel.Component", "EditorCommon.JsonModel.Component.ComSimpleAudioSurrogate, Modules.Communal.ProjectsConvertor");
			return text.Replace("ComSpriteSurrogate:#EditorCommon.JsonModel.Component", "EditorCommon.JsonModel.Component.ComSpriteSurrogate, Modules.Communal.ProjectsConvertor");
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00008A00 File Offset: 0x00006C00
		public static bool ImportUIFromFile(string filePath, GameFileData gameFileData)
		{
			if (!File.Exists(filePath))
			{
				return false;
			}
			JsonFileHelp.plistfilehelper.Clear();
			string json = File.ReadAllText(filePath);
			JObject jobject = null;
			try
			{
				jobject = JObject.Parse(json);
			}
			catch (Exception ex)
			{
				LogConfig.Output.Error(ex.ToString());
			}
			JArray jarray = jobject["textures"] as JArray;
			int num = (int)jobject["designWidth"];
			int num2 = (int)jobject["designHeight"];
			if (jarray != null)
			{
				string text = Path.GetDirectoryName(filePath);
				for (int i = 0; i < jarray.Count; i++)
				{
					text = Path.GetDirectoryName(filePath);
					text = Path.Combine(text, (string)jarray[i]);
					JsonFileHelp.plistfilehelper.AddPlistConfigFile(text);
				}
			}
			string jsonData = File.ReadAllText(filePath);
			string value = UIVersionHelp.ConvertToNewJsonData(jsonData);
			ComGUIRootSurrogate comGUIRootSurrogate = null;
			try
			{
				comGUIRootSurrogate = JsonConvert.DeserializeObject<ComGUIRootSurrogate>(value, JsonFileHelp.GetJsonSerializerSettings(false, false));
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.ToString());
			}
			gameFileData.ObjectData.Size = new SizeF((float)num, (float)num2);
			comGUIRootSurrogate.InitGameProjectData(gameFileData);
			return true;
		}

		// Token: 0x04000186 RID: 390
		public static PlistConfigFileHelper plistfilehelper = new PlistConfigFileHelper();

		// Token: 0x04000187 RID: 391
		public static string uiAndAniJson = string.Empty;

		// Token: 0x04000188 RID: 392
		public static bool isBasedProject = true;

		// Token: 0x04000189 RID: 393
		private static string uiAniRelasResDir = string.Empty;

		// Token: 0x0400018A RID: 394
		private static string oldresDirs = string.Empty;

		// Token: 0x0400018B RID: 395
		private static string output = string.Empty;
	}
}

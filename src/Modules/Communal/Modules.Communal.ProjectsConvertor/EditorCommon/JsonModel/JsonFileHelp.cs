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
	public class JsonFileHelp
	{
		internal static IProgressMonitor Monitor { get; set; }

		private static string FormateJson(string unFormateJsonString)
		{
			object value = JsonConvert.DeserializeObject(unFormateJsonString);
			return JsonConvert.SerializeObject(value, Formatting.Indented);
		}

		public static string UnFormateJson(string formateJsonString)
		{
			object value = JsonConvert.DeserializeObject(formateJsonString);
			return JsonConvert.SerializeObject(value, Formatting.None);
		}

		private static Stream ReadFormateJson(string filePath)
		{
			string formateJsonString = File.ReadAllText(filePath);
			string s = JsonFileHelp.UnFormateJson(formateJsonString);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			return new MemoryStream(bytes);
		}

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

		public static void SetOldToNewRelativeResDir(string oldjsonabspath, string oldresDir = "")
		{
			if (!string.IsNullOrEmpty(oldresDir))
			{
				JsonFileHelp.oldresDirs = oldresDir;
			}
			JsonFileHelp.uiAniRelasResDir = FileService.AbsoluteToRelativePath(JsonFileHelp.oldresDirs, oldjsonabspath);
			JsonFileHelp.uiAniRelasResDir = Option.ConvertToMacPath(JsonFileHelp.uiAniRelasResDir);
		}

		public static void SetOldResDir(string oldresDir)
		{
			JsonFileHelp.oldresDirs = oldresDir;
		}

		public static string GetResRelativePath(string absPath)
		{
			string filePath = FileService.AbsoluteToRelativePath(Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath, absPath);
			return Option.ConvertToMacPath(filePath);
		}

		public static string GetResAbsPath(string relativePath)
		{
			string directoryName = Path.GetDirectoryName(JsonFileHelp.uiAndAniJson);
			return Path.Combine(directoryName, relativePath);
		}

		public static void ReportWarning(string message)
		{
			if (JsonFileHelp.Monitor != null)
			{
				Timeout.Add(0U, () => false);
			}
		}

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

		public static PlistConfigFileHelper plistfilehelper = new PlistConfigFileHelper();

		public static string uiAndAniJson = string.Empty;

		public static bool isBasedProject = true;

		private static string uiAniRelasResDir = string.Empty;

		private static string oldresDirs = string.Empty;

		private static string output = string.Empty;
	}
}

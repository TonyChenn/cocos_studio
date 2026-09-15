using System;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Lua.Templates;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Model.Lua
{
	[SerializerExtension(true)]
	[Extension(typeof(IGameFileSerializer))]
	public class LuaSerializer : BaseCocosFileSerializer
	{
		protected override string OnGetID()
		{
			return "Serializer_Lua";
		}

		protected override string OnGetLabel()
		{
			return LanguageInfo.ProjSetting_luaFile;
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.ProjSetting_luaInfo;
			}
		}

		protected override int DisplayIndex
		{
			get
			{
				return 2;
			}
		}

		protected override string OnSerialize(PublishInfo info, GameFile projFile)
		{
			GameFileContent gameFileContent = projFile.Content as GameFileContent;
			GameFileData content = gameFileContent.Content;
			this.CheckProjectNameStandardized(gameFileContent);
			string result;
			try
			{
				FilePath filePath = Path.ChangeExtension(info.DestinationFilePath, ".lua");
				LuaObjectSerializer.Prepare(new StringBuilder(), content, filePath.FileName, gameFileContent.ObjectsCount);
				LuaGameFileData luaGameFileData = new LuaGameFileData();
				string contents = luaGameFileData.TransformText();
				File.WriteAllText(filePath, contents);
				LuaObjectSerializer.Dispose();
				result = null;
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Publish lua file failed.", ex);
				result = ex.ToString();
			}
			return result;
		}

		public override void ContextInitialize(PublishInfo publishInfo)
		{
			string luaHeader = LuaGameFileData.LuaHeader;
			string path = Path.Combine(publishInfo.PublishDirectory, "LuaExtend.lua");
			File.WriteAllText(path, luaHeader);
			this.CopyLuaScriptFolder(publishInfo);
		}

		private void CheckProjectNameStandardized(GameFileContent gameFileContent)
		{
			foreach (string text in gameFileContent.Names)
			{
				if (!RegexModel.IsValidObjectName(text))
				{
					MessageBox.Show(string.Format(LanguageInfo.MessageBox259_InvalidLuaVariableName, text) + "\n" + LanguageInfo.MessageBox247_LuaNameStandard, MessageBoxImage.Warning, null, null);
					break;
				}
			}
		}

		private void CopyLuaScriptFolder(PublishInfo publishInfo)
		{
			string directoryName = Path.GetDirectoryName(publishInfo.PublishDirectory.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar
			}));
			string text = Path.Combine(directoryName, "LuaScript");
			if (Directory.Exists(text))
			{
				Directory.Delete(text, true);
			}
			if (!Directory.Exists(Option.LuaScriptFolder))
			{
				return;
			}
			Directory.CreateDirectory(text);
			FileService.CopyDirectory(Option.LuaScriptFolder, text);
		}

		protected void InitializeCallback(NodeObjectData objectData)
		{
			if (string.IsNullOrEmpty(objectData.CallBackName))
			{
				return;
			}
			switch (objectData.CallBackType)
			{
			default:
				return;
			}
		}

		public const string LuaFileSuffix = ".lua";
	}
}

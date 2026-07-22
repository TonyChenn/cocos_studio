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
	// Token: 0x0200001B RID: 27
	[SerializerExtension(true)]
	[Extension(typeof(IGameFileSerializer))]
	public class LuaSerializer : BaseCocosFileSerializer
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x0000572C File Offset: 0x0000392C
		protected override string OnGetID()
		{
			return "Serializer_Lua";
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005733 File Offset: 0x00003933
		protected override string OnGetLabel()
		{
			return LanguageInfo.ProjSetting_luaFile;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000573A File Offset: 0x0000393A
		public override string Description
		{
			get
			{
				return LanguageInfo.ProjSetting_luaInfo;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00005741 File Offset: 0x00003941
		protected override int DisplayIndex
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005744 File Offset: 0x00003944
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

		// Token: 0x060000AE RID: 174 RVA: 0x000057F8 File Offset: 0x000039F8
		public override void ContextInitialize(PublishInfo publishInfo)
		{
			string luaHeader = LuaGameFileData.LuaHeader;
			string path = Path.Combine(publishInfo.PublishDirectory, "LuaExtend.lua");
			File.WriteAllText(path, luaHeader);
			this.CopyLuaScriptFolder(publishInfo);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000582C File Offset: 0x00003A2C
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

		// Token: 0x060000B0 RID: 176 RVA: 0x000058A4 File Offset: 0x00003AA4
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

		// Token: 0x060000B1 RID: 177 RVA: 0x0000590C File Offset: 0x00003B0C
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

		// Token: 0x0400001A RID: 26
		public const string LuaFileSuffix = ".lua";
	}
}

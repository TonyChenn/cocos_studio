using System;
using System.CodeDom.Compiler;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000007 RID: 7
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaBaseObject : LuaObjectSerializer
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002748 File Offset: 0x00000948
		protected string GetNameDeclaration(string nameString)
		{
			this.ValidateLuaVariableName(nameString);
			if (LuaObjectSerializer.ObjectCount > 190)
			{
				return "result['" + nameString + "']";
			}
			return "local " + nameString;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002779 File Offset: 0x00000979
		public bool CanExport<T>(T value, T defaultValue)
		{
			return !value.Equals(defaultValue) || !LuaBaseObject.hideDefaultValue;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000279A File Offset: 0x0000099A
		private void ValidateLuaVariableName(string nameString)
		{
			if (!RegexModel.IsValidObjectName(nameString))
			{
				LogConfig.Output.Info(string.Format(LanguageInfo.MessageBox259_InvalidLuaVariableName, nameString), true);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000027BA File Offset: 0x000009BA
		protected string GetNameString(string nameString)
		{
			if (LuaObjectSerializer.ObjectCount > 190)
			{
				return "result['" + nameString + "']";
			}
			return nameString;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000027DA File Offset: 0x000009DA
		protected string LuaPathFormat(string pathString)
		{
			return Option.ConvertToMacPath(pathString);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000027E2 File Offset: 0x000009E2
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000027FA File Offset: 0x000009FA
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(BaseObjectData) == objectData.GetType();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002811 File Offset: 0x00000A11
		public override void CreateObject(BaseObjectData objectData)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002818 File Offset: 0x00000A18
		public override void AddChild(BaseObjectData parent, BaseObjectData child)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002820 File Offset: 0x00000A20
		public override void InitializeObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(this.GetNameString(objectData.Name)));
			base.Write(":setName(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(objectData.Name));
			base.Write("\")\r\n");
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000287C File Offset: 0x00000A7C
		protected void PreloadPlist(ResourceData data)
		{
			if (data == null)
			{
				return;
			}
			if (data.Type == EnumResourceType.PlistSubImage || data.Type == EnumResourceType.MarkedSubImage)
			{
				base.Write("cc.SpriteFrameCache:getInstance():addSpriteFrames(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(data.Plist));
				base.Write("\")\r\n");
			}
		}

		// Token: 0x0400000F RID: 15
		private const int MaxObjectCount = 190;

		// Token: 0x04000010 RID: 16
		public const string SceneBrushSuffix = "_SceneBrush";

		// Token: 0x04000011 RID: 17
		public const string LightSuffix = "_light";

		// Token: 0x04000012 RID: 18
		public const string BrushSuffix = "_Brush";

		// Token: 0x04000013 RID: 19
		private static bool hideDefaultValue = true;
	}
}

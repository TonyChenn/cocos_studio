using System;
using System.CodeDom.Compiler;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaBaseObject : LuaObjectSerializer
	{
		protected string GetNameDeclaration(string nameString)
		{
			this.ValidateLuaVariableName(nameString);
			if (LuaObjectSerializer.ObjectCount > 190)
			{
				return "result['" + nameString + "']";
			}
			return "local " + nameString;
		}

		public bool CanExport<T>(T value, T defaultValue)
		{
			return !value.Equals(defaultValue) || !LuaBaseObject.hideDefaultValue;
		}

		private void ValidateLuaVariableName(string nameString)
		{
			if (!RegexModel.IsValidObjectName(nameString))
			{
				LogConfig.Output.Info(string.Format(LanguageInfo.MessageBox259_InvalidLuaVariableName, nameString), true);
			}
		}

		protected string GetNameString(string nameString)
		{
			if (LuaObjectSerializer.ObjectCount > 190)
			{
				return "result['" + nameString + "']";
			}
			return nameString;
		}

		protected string LuaPathFormat(string pathString)
		{
			return Option.ConvertToMacPath(pathString);
		}

		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(BaseObjectData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			throw new InvalidOperationException();
		}

		public override void AddChild(BaseObjectData parent, BaseObjectData child)
		{
			throw new InvalidOperationException();
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(this.GetNameString(objectData.Name)));
			base.Write(":setName(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(objectData.Name));
			base.Write("\")\r\n");
		}

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

		private const int MaxObjectCount = 190;

		public const string SceneBrushSuffix = "_SceneBrush";

		public const string LightSuffix = "_light";

		public const string BrushSuffix = "_Brush";

		private static bool hideDefaultValue = true;
	}
}

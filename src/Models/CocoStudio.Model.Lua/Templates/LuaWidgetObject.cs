using System;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaWidgetObject : LuaNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		protected override void InitializeLayout(BaseObjectData objectData)
		{
			WidgetObjectData widgetObjectData = objectData as WidgetObjectData;
			base.InitializeLayout(widgetObjectData);
			if (base.CanExport<bool>(widgetObjectData.StretchWidthEnable, false))
			{
				base.Write("layout:setStretchWidthEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(widgetObjectData.StretchWidthEnable));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(widgetObjectData.StretchHeightEnable, false))
			{
				base.Write("layout:setStretchHeightEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(widgetObjectData.StretchHeightEnable));
				base.Write(")\r\n");
			}
		}

		private bool getTouchEnableDefaultValueByType(WidgetObjectData data)
		{
			return data is ButtonObjectData || data is CheckBoxObjectData || data is TextFieldObjectData || data is SliderObjectData || data is ListViewObjectData || data is ScrollViewObjectData || data is PageViewObjectData;
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			WidgetObjectData widgetObjectData = objectData as WidgetObjectData;
			if (widgetObjectData == null)
			{
				return;
			}
			if (base.CanExport<bool>(widgetObjectData.TouchEnable, this.getTouchEnableDefaultValueByType(widgetObjectData)))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(widgetObjectData.Name)));
				base.Write(":setTouchEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(widgetObjectData.TouchEnable));
				base.Write(");\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(widgetObjectData.Name)));
			base.Write(":setLayoutComponentEnabled(true)\r\n");
			base.InitializeObject(widgetObjectData);
		}

		protected static string GetLabelTextString(string labelText)
		{
			if (!LuaWidgetObject.luaBracketRegex.IsMatch(labelText))
			{
				return "[[" + labelText + "]]";
			}
			return LuaWidgetObject.luaLeftBrackets + labelText + LuaWidgetObject.luaRightBrackets;
		}

		private static readonly Regex luaBracketRegex = new Regex("^[[]|]]|]$");

		private static readonly string luaLeftBrackets = "[==========[";

		private static readonly string luaRightBrackets = "]==========]";
	}
}

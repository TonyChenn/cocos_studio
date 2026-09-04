using System;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000014 RID: 20
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaWidgetObject : LuaNodeObject
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00004650 File Offset: 0x00002850
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004668 File Offset: 0x00002868
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

		// Token: 0x06000085 RID: 133 RVA: 0x00004705 File Offset: 0x00002905
		private bool getTouchEnableDefaultValueByType(WidgetObjectData data)
		{
			return data is ButtonObjectData || data is CheckBoxObjectData || data is TextFieldObjectData || data is SliderObjectData || data is ListViewObjectData || data is ScrollViewObjectData || data is PageViewObjectData;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004744 File Offset: 0x00002944
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

		// Token: 0x06000087 RID: 135 RVA: 0x000047EF File Offset: 0x000029EF
		protected static string GetLabelTextString(string labelText)
		{
			if (!LuaWidgetObject.luaBracketRegex.IsMatch(labelText))
			{
				return "[[" + labelText + "]]";
			}
			return LuaWidgetObject.luaLeftBrackets + labelText + LuaWidgetObject.luaRightBrackets;
		}

		// Token: 0x04000016 RID: 22
		private static readonly Regex luaBracketRegex = new Regex("^[[]|]]|]$");

		// Token: 0x04000017 RID: 23
		private static readonly string luaLeftBrackets = "[==========[";

		// Token: 0x04000018 RID: 24
		private static readonly string luaRightBrackets = "]==========]";
	}
}

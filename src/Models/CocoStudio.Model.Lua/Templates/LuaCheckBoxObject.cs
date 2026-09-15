using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaCheckBoxObject : LuaWidgetObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(CheckBoxObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.CheckBox:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			CheckBoxObjectData checkBoxObjectData = objectData as CheckBoxObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
			base.Write(":ignoreContentAdaptWithSize(false)\r\n");
			if (checkBoxObjectData.NormalBackFileData != null)
			{
				base.PreloadPlist(checkBoxObjectData.NormalBackFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
				base.Write(":loadTextureBackGround(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(checkBoxObjectData.NormalBackFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(checkBoxObjectData.NormalBackFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (checkBoxObjectData.PressedBackFileData != null)
			{
				base.PreloadPlist(checkBoxObjectData.PressedBackFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
				base.Write(":loadTextureBackGroundSelected(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(checkBoxObjectData.PressedBackFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(checkBoxObjectData.PressedBackFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (checkBoxObjectData.DisableBackFileData != null)
			{
				base.PreloadPlist(checkBoxObjectData.DisableBackFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
				base.Write(":loadTextureBackGroundDisabled(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(checkBoxObjectData.DisableBackFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(checkBoxObjectData.DisableBackFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (checkBoxObjectData.NodeNormalFileData != null)
			{
				base.PreloadPlist(checkBoxObjectData.NodeNormalFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
				base.Write(":loadTextureFrontCross(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(checkBoxObjectData.NodeNormalFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(checkBoxObjectData.NodeNormalFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (checkBoxObjectData.NodeDisableFileData != null)
			{
				base.PreloadPlist(checkBoxObjectData.NodeDisableFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
				base.Write(":loadTextureFrontCrossDisabled(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(checkBoxObjectData.NodeDisableFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(checkBoxObjectData.NodeDisableFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(checkBoxObjectData.CheckedState, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
				base.Write(":setSelected(");
				base.Write(base.ToStringHelper.ToStringWithCulture(checkBoxObjectData.CheckedState));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(checkBoxObjectData.DisplayState, true))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
				base.Write(":setBright(");
				base.Write(base.ToStringHelper.ToStringWithCulture(checkBoxObjectData.DisplayState));
				base.Write(")\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(checkBoxObjectData.Name)));
				base.Write(":setEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(checkBoxObjectData.DisplayState));
				base.Write(")\r\n");
			}
			base.InitializeObject(checkBoxObjectData);
		}
	}
}

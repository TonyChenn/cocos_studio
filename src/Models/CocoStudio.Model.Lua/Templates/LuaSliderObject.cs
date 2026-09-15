using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSliderObject : LuaWidgetObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(SliderObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.Slider:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			SliderObjectData sliderObjectData = objectData as SliderObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
			base.Write(":ignoreContentAdaptWithSize(false)\r\n");
			if (sliderObjectData.BackGroundData != null)
			{
				base.PreloadPlist(sliderObjectData.BackGroundData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
				base.Write(":loadBarTexture(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(sliderObjectData.BackGroundData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(sliderObjectData.BackGroundData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (sliderObjectData.ProgressBarData != null)
			{
				base.PreloadPlist(sliderObjectData.ProgressBarData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
				base.Write(":loadProgressBarTexture(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(sliderObjectData.ProgressBarData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(sliderObjectData.ProgressBarData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (sliderObjectData.BallNormalData != null)
			{
				base.PreloadPlist(sliderObjectData.BallNormalData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
				base.Write(":loadSlidBallTextureNormal(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(sliderObjectData.BallNormalData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(sliderObjectData.BallNormalData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (sliderObjectData.BallPressedData != null)
			{
				base.PreloadPlist(sliderObjectData.BallPressedData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
				base.Write(":loadSlidBallTexturePressed(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(sliderObjectData.BallPressedData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(sliderObjectData.BallPressedData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (sliderObjectData.BallDisabledData != null)
			{
				base.PreloadPlist(sliderObjectData.BallDisabledData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
				base.Write(":loadSlidBallTextureDisabled(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(sliderObjectData.BallDisabledData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(sliderObjectData.BallDisabledData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (base.CanExport<int>(sliderObjectData.PercentInfo, 0))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
				base.Write(":setPercent(");
				base.Write(base.ToStringHelper.ToStringWithCulture(sliderObjectData.PercentInfo));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(sliderObjectData.DisplayState, true))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
				base.Write(":setBright(");
				base.Write(base.ToStringHelper.ToStringWithCulture(sliderObjectData.DisplayState));
				base.Write(")\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sliderObjectData.Name)));
				base.Write(":setEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(sliderObjectData.DisplayState));
				base.Write(")\r\n");
			}
			base.InitializeObject(sliderObjectData);
		}
	}
}

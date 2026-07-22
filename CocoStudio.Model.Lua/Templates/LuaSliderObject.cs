using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000027 RID: 39
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSliderObject : LuaWidgetObject
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00006E1F File Offset: 0x0000501F
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006E38 File Offset: 0x00005038
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(SliderObjectData) == objectData.GetType();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006E5C File Offset: 0x0000505C
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.Slider:create()\r\n");
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006E88 File Offset: 0x00005088
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

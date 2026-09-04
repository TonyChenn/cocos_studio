using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200001E RID: 30
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaPanelObject : LuaWidgetObject
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00005D94 File Offset: 0x00003F94
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005DAC File Offset: 0x00003FAC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(PanelObjectData) == objectData.GetType();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005DD0 File Offset: 0x00003FD0
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.Layout:create()\r\n");
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005DFC File Offset: 0x00003FFC
		public override void InitializeObject(BaseObjectData objectData)
		{
			PanelObjectData panelObjectData = objectData as PanelObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
			base.Write(":ignoreContentAdaptWithSize(false)\r\n");
			if (panelObjectData.FileData != null)
			{
				base.PreloadPlist(panelObjectData.FileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setBackGroundImage(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(panelObjectData.FileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.FileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(panelObjectData.ClipAble, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setClippingEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.ClipAble));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(panelObjectData.Scale9Enable, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setBackGroundImageCapInsets(cc.rect(");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.Scale9OriginX));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.Scale9OriginY));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.Scale9Width));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.Scale9Height));
				base.Write("))\r\n");
			}
			if (base.CanExport<int>(panelObjectData.ComboBoxIndex, 0))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setBackGroundColorType(");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.ComboBoxIndex));
				base.Write(")\r\n");
			}
			if (panelObjectData.ComboBoxIndex == 1)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setBackGroundColor(");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.SingleColor));
				base.Write(")\r\n");
			}
			if (panelObjectData.ComboBoxIndex == 2)
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setBackGroundColor(");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.FirstColor));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.EndColor));
				base.Write(")\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setBackGroundColorVector(cc.p(");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.ColorVector.ScaleX));
				base.Write(",");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.ColorVector.ScaleY));
				base.Write("))\r\n");
			}
			if (base.CanExport<int>(panelObjectData.BackColorAlpha, 255))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setBackGroundColorOpacity(");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.BackColorAlpha));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(panelObjectData.Scale9Enable, false))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(panelObjectData.Name)));
				base.Write(":setBackGroundImageScale9Enabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(panelObjectData.Scale9Enable));
				base.Write(")\r\n");
			}
			base.InitializeObject(panelObjectData);
		}
	}
}

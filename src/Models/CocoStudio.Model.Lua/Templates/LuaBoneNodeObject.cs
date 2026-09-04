using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200000C RID: 12
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaBoneNodeObject : LuaNodeObject
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00003AD3 File Offset: 0x00001CD3
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003AEC File Offset: 0x00001CEC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(BoneNodeObjectData) == objectData.GetType();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003B10 File Offset: 0x00001D10
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccs.BoneNode:create()\r\n");
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003B3C File Offset: 0x00001D3C
		public override void InitializeObject(BaseObjectData objectData)
		{
			BoneNodeObjectData boneNodeObjectData = objectData as BoneNodeObjectData;
			base.InitializeObject(boneNodeObjectData);
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
			base.Write(":setTag(");
			base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.Tag));
			base.Write(")\r\n");
			if (base.CanExport<bool>(boneNodeObjectData.VisibleForFrame, true))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setVisible(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.VisibleForFrame));
				base.Write(")\r\n");
			}
			if (base.CanExport<PointF>(boneNodeObjectData.Position, PointF.Empty))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setPosition(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.Position));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(boneNodeObjectData.Scale.ScaleX, 1f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setScaleX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.Scale.ScaleX));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(boneNodeObjectData.Scale.ScaleY, 1f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setScaleY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.Scale.ScaleY));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(boneNodeObjectData.RotationSkewX, 0f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setRotationSkewX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.RotationSkewX));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(boneNodeObjectData.RotationSkewY, 0f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setRotationSkewY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.RotationSkewY));
				base.Write(")\r\n");
			}
			if (base.CanExport<int>(boneNodeObjectData.Alpha, 255))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setOpacity(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.Alpha));
				base.Write(")\r\n");
			}
			if (base.CanExport<ColorData>(boneNodeObjectData.CColor, ColorData.White))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setColor(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.CColor));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(boneNodeObjectData.Length, 50f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(boneNodeObjectData.Name)));
				base.Write(":setDebugDrawLength(");
				base.Write(base.ToStringHelper.ToStringWithCulture(boneNodeObjectData.Length));
				base.Write(")\r\n");
			}
		}
	}
}

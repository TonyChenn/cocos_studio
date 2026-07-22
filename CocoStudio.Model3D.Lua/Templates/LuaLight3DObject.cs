using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model3D.Lua.Templates
{
	// Token: 0x02000003 RID: 3
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaLight3DObject : LuaNode3DObject
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002446 File Offset: 0x00000646
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002460 File Offset: 0x00000660
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(Light3DObjectData) == objectData.GetType();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002484 File Offset: 0x00000684
		public override void InitializeObject(BaseObjectData objectData)
		{
			Light3DObjectData light3DObjectData = objectData as Light3DObjectData;
			base.InitializeObject(light3DObjectData);
			string nameString = light3DObjectData.Name + "_light";
			switch (light3DObjectData.Type)
			{
			case LightType.DIRECTIONAL:
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(nameString)));
				base.Write("=cc.DirectionLight:create(cc.vec3(0,0,1), ");
				base.Write(base.ToStringHelper.ToStringWithCulture(light3DObjectData.CColor));
				base.Write(")\r\n");
				break;
			case LightType.POINT:
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(nameString)));
				base.Write("=cc.PointLight:create(cc.vec3(0,0,0), ");
				base.Write(base.ToStringHelper.ToStringWithCulture(light3DObjectData.CColor));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(light3DObjectData.Range));
				base.Write(")\r\n");
				break;
			case LightType.SPOT:
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(nameString)));
				base.Write("=cc.SpotLight:create(cc.vec3(0,0,1), cc.vec3(0,0,0), ");
				base.Write(base.ToStringHelper.ToStringWithCulture(light3DObjectData.CColor));
				base.Write(", 0,  ");
				base.Write(base.ToStringHelper.ToStringWithCulture((double)(light3DObjectData.OuterAngle * 0.017453292f) * 0.5));
				base.Write(", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(light3DObjectData.Range));
				base.Write(")\r\n");
				break;
			case LightType.AMBIENT:
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(nameString)));
				base.Write("=cc.AmbientLight:create(");
				base.Write(base.ToStringHelper.ToStringWithCulture(light3DObjectData.CColor));
				base.Write(")\r\n");
				break;
			default:
				return;
			}
			if (base.CanExport<float>(light3DObjectData.Intensity, 1f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nameString)));
				base.Write(":setIntensity(");
				base.Write(base.ToStringHelper.ToStringWithCulture(light3DObjectData.Intensity));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(light3DObjectData.Enable, true))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nameString)));
				base.Write(":setEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(light3DObjectData.Enable));
				base.Write(")\r\n");
			}
			if (base.CanExport<LightFlag>(light3DObjectData.Flag, LightFlag.LIGHT0))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nameString)));
				base.Write(":setLightFlag(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)light3DObjectData.Flag));
				base.Write(")\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(light3DObjectData.Name)));
			base.Write(":addChild(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nameString)));
			base.Write(")\r\n");
		}
	}
}

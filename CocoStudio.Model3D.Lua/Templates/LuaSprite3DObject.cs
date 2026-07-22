using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model3D.Lua.Templates
{
	// Token: 0x02000005 RID: 5
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSprite3DObject : LuaNode3DObject
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000028CF File Offset: 0x00000ACF
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000028E8 File Offset: 0x00000AE8
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(Sprite3DObjectData) == objectData.GetType();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000290C File Offset: 0x00000B0C
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			Sprite3DObjectData sprite3DObjectData = objectData as Sprite3DObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.Sprite3D:create(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(sprite3DObjectData.FileData.Path));
			base.Write("\")\r\n");
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002970 File Offset: 0x00000B70
		public override void InitializeObject(BaseObjectData objectData)
		{
			Sprite3DObjectData sprite3DObjectData = objectData as Sprite3DObjectData;
			base.InitializeObject(sprite3DObjectData);
			if (sprite3DObjectData == null)
			{
				return;
			}
			if (base.CanExport<LightFlag>(sprite3DObjectData.LightFlag, LightFlag.LIGHT0))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(sprite3DObjectData.Name)));
				base.Write(":setLightMask(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)sprite3DObjectData.LightFlag));
				base.Write(")\r\n");
			}
		}
	}
}

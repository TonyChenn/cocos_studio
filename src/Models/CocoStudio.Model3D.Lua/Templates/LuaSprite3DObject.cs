using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model3D.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaSprite3DObject : LuaNode3DObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(Sprite3DObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			Sprite3DObjectData sprite3DObjectData = objectData as Sprite3DObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = cc.Sprite3D:create(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(sprite3DObjectData.FileData.Path));
			base.Write("\")\r\n");
		}

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

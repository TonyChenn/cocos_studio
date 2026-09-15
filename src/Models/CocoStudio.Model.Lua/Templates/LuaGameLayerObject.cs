using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaGameLayerObject : LuaNodeObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(GameLayerObjectData) == objectData.GetType() || typeof(LayerObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.OnCreateObject(objectData);
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			GameLayerObjectData gameLayerObjectData = objectData as GameLayerObjectData;
			base.InitializeObject(gameLayerObjectData);
			base.Write("layout = ccui.LayoutComponent:bindLayoutComponent(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(gameLayerObjectData.Name)));
			base.Write(")\r\nlayout:setSize(cc.size(");
			base.Write(base.ToStringHelper.ToStringWithCulture(gameLayerObjectData.Size));
			base.Write("))\r\n");
		}
	}
}

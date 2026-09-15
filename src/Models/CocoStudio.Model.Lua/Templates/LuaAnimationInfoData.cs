using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaAnimationInfoData : LuaBaseObject
	{
		public override string TransformText()
		{
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(AnimationInfoData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			AnimationInfoData animationInfoData = objectData as AnimationInfoData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = {name=\"");
			base.Write(base.ToStringHelper.ToStringWithCulture(animationInfoData.Name));
			base.Write("\", startIndex=");
			base.Write(base.ToStringHelper.ToStringWithCulture(animationInfoData.StartIndex));
			base.Write(", endIndex=");
			base.Write(base.ToStringHelper.ToStringWithCulture(animationInfoData.EndIndex));
			base.Write("}\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
		}
	}
}

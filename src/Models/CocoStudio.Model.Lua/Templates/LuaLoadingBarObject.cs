using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaLoadingBarObject : LuaWidgetObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(LoadingBarObjectData) == objectData.GetType();
		}

		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.LoadingBar:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			LoadingBarObjectData loadingBarObjectData = objectData as LoadingBarObjectData;
			if (loadingBarObjectData.ImageFileData != null)
			{
				base.PreloadPlist(loadingBarObjectData.ImageFileData);
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(loadingBarObjectData.Name)));
				base.Write(":loadTexture(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(loadingBarObjectData.ImageFileData.Path)));
				base.Write("\",");
				base.Write(base.ToStringHelper.ToStringWithCulture(loadingBarObjectData.ImageFileData.Type.ToLuaType()));
				base.Write(")\r\n");
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(loadingBarObjectData.Name)));
			base.Write(":ignoreContentAdaptWithSize(false)\r\n");
			if (base.CanExport<LoadingBarDirectionType>(loadingBarObjectData.ProgressType, LoadingBarDirectionType.Left_To_Right))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(loadingBarObjectData.Name)));
				base.Write(":setDirection(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)loadingBarObjectData.ProgressType));
				base.Write(")\r\n");
			}
			if (base.CanExport<int>(loadingBarObjectData.ProgressInfo, 100))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(loadingBarObjectData.Name)));
				base.Write(":setPercent(");
				base.Write(base.ToStringHelper.ToStringWithCulture(loadingBarObjectData.ProgressInfo));
				base.Write(")\r\n");
			}
			base.InitializeObject(loadingBarObjectData);
		}
	}
}

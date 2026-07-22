using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000020 RID: 32
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaLoadingBarObject : LuaWidgetObject
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x000064D2 File Offset: 0x000046D2
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000064EC File Offset: 0x000046EC
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(LoadingBarObjectData) == objectData.GetType();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006510 File Offset: 0x00004710
		protected override void OnCreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccui.LoadingBar:create()\r\n");
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000653C File Offset: 0x0000473C
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

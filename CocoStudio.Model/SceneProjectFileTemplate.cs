using System;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace CocoStudio.Model
{
	// Token: 0x020000C7 RID: 199
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class SceneProjectFileTemplate : BaseProjectFileTemplate
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00019B04 File Offset: 0x00017D04
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Scene;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00019B1C File Offset: 0x00017D1C
		public override NodeType FileType
		{
			get
			{
				return NodeType.Scene;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00019B30 File Offset: 0x00017D30
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00019B44 File Offset: 0x00017D44
		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_SceneDes;
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00019B5C File Offset: 0x00017D5C
		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.sence.png";
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00019B73 File Offset: 0x00017D73
		protected override void OnChangeView(CanvasObject canvas, CocosItem project)
		{
			canvas.Size = canvas.GetSceneSize();
			canvas.SetCenterLineVisible(false);
		}
	}
}

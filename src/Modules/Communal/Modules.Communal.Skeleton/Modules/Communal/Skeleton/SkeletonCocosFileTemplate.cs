using System;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000027 RID: 39
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class SkeletonCocosFileTemplate : BaseProjectFileTemplate
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000921C File Offset: 0x0000741C
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Skeleton;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00009223 File Offset: 0x00007423
		public override NodeType FileType
		{
			get
			{
				return NodeType.Skeleton;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00009226 File Offset: 0x00007426
		public override int Order
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00009229 File Offset: 0x00007429
		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_SkeletonDes;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009230 File Offset: 0x00007430
		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.SkeletonFile.png";
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00009237 File Offset: 0x00007437
		public override int MaxSize
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000923A File Offset: 0x0000743A
		public override bool CanEditSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000923D File Offset: 0x0000743D
		public override bool IsShowTrackPoint
		{
			get
			{
				return Services.RemindService.IsShowSkeletonTrackPoint;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009249 File Offset: 0x00007449
		protected override void OnChangeView(CanvasObject canvas, CocosItem cocosItem)
		{
			canvas.Size = new SizeF(0f, 0f);
			canvas.SetCenterLineVisible(true);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009267 File Offset: 0x00007467
		protected override void OnInitGameProejctData(GameFileData gameFileData)
		{
			gameFileData.ObjectData = new SkeletonNodeObjectData();
			if (Services.RemindService.IsShowSkeletonTrackPoint)
			{
				Services.RemindService.IsShowSkeletonTrackPoint = false;
				Services.RemindService.Save();
			}
		}
	}
}

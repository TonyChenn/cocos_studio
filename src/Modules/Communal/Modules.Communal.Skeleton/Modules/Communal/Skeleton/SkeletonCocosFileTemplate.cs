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
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class SkeletonCocosFileTemplate : BaseProjectFileTemplate
	{
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Skeleton;
			}
		}

		public override NodeType FileType
		{
			get
			{
				return NodeType.Skeleton;
			}
		}

		public override int Order
		{
			get
			{
				return 5;
			}
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_SkeletonDes;
			}
		}

		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.SkeletonFile.png";
		}

		public override int MaxSize
		{
			get
			{
				return 0;
			}
		}

		public override bool CanEditSize
		{
			get
			{
				return false;
			}
		}

		public override bool IsShowTrackPoint
		{
			get
			{
				return Services.RemindService.IsShowSkeletonTrackPoint;
			}
		}

		protected override void OnChangeView(CanvasObject canvas, CocosItem cocosItem)
		{
			canvas.Size = new SizeF(0f, 0f);
			canvas.SetCenterLineVisible(true);
		}

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

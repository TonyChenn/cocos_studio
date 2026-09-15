using System;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace CocoStudio.Model
{
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class SceneProjectFileTemplate : BaseProjectFileTemplate
	{
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Scene;
			}
		}

		public override NodeType FileType
		{
			get
			{
				return NodeType.Scene;
			}
		}

		public override int Order
		{
			get
			{
				return 0;
			}
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_SceneDes;
			}
		}

		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.sence.png";
		}

		protected override void OnChangeView(CanvasObject canvas, CocosItem project)
		{
			canvas.Size = canvas.GetSceneSize();
			canvas.SetCenterLineVisible(false);
		}
	}
}

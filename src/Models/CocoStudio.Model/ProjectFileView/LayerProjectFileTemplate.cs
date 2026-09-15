using System;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace CocoStudio.Model.ProjectFileView
{
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class LayerProjectFileTemplate : BaseProjectFileTemplate
	{
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Layer;
			}
		}

		public override NodeType FileType
		{
			get
			{
				return NodeType.Layer;
			}
		}

		public override int Order
		{
			get
			{
				return 1;
			}
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_LayerDes;
			}
		}

		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.layer.png";
		}

		public override int MaxSize
		{
			get
			{
				return 4096;
			}
		}

		public override bool CanEditSize
		{
			get
			{
				return true;
			}
		}

		protected override void OnInitGameProejctData(GameFileData gameProjectData)
		{
			gameProjectData.ObjectData = new GameLayerObjectData();
		}

		protected override void OnChangeView(CanvasObject canvas, CocosItem project)
		{
			if (canvas != null && project != null)
			{
				try
				{
					AbstractNodeObject rootNode = project.GetRootNode();
					if (rootNode != null)
					{
						canvas.Size = rootNode.Size;
						canvas.SetCenterLineVisible(false);
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("设置画布中心线可见性时出错", exception);
				}
			}
		}
	}
}

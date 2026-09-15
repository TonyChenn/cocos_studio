using System;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace CocoStudio.Model.ProjectFileView
{
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class NodeProjectFileTemplate : BaseProjectFileTemplate
	{
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_AnimationNode;
			}
		}

		public override NodeType FileType
		{
			get
			{
				return NodeType.Node;
			}
		}

		public override int Order
		{
			get
			{
				return 2;
			}
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_NodeDes;
			}
		}

		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.node.png";
		}

		public override int MaxSize
		{
			get
			{
				return 0;
			}
		}

		protected override void OnChangeView(CanvasObject canvas, CocosItem project)
		{
			canvas.Size = SizeF.Empty;
			canvas.SetCenterLineVisible(true);
		}
	}
}

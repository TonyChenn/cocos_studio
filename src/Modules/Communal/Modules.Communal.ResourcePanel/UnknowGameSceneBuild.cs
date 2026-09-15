using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(UnknowGameSceneBuild))]
	internal class UnknowGameSceneBuild : CocosFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(UnknowGameScene);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = UnknowGameSceneBuild.expandIcon
			};
		}

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.BuildNode(treeBuilder, dataObject, nodeInfo);
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("Other.png"));
	}
}

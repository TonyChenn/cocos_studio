using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(GameFileBuild))]
	internal class GameFileBuild : CocosFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(GameFile);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = GameFileBuild.expandIcon
			};
		}

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("Images.commonFile.png"));
	}
}

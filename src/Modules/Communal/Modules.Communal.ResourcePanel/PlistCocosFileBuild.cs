using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(PlistCocosFileBuild))]
	internal class PlistCocosFileBuild : CocosFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(CocosFile);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = PlistCocosFileBuild.expandIcon
			};
		}

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("csi.png"));
	}
}

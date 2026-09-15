using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(PlistParticleFileBuild))]
	public class PlistParticleFileBuild : ResourceFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(PlistParticleFile);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = PlistParticleFileBuild.expandIcon
			};
		}

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		public override bool CanRename()
		{
			return false;
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("Particle.png"));
	}
}

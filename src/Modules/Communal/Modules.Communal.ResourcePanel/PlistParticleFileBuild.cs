using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000014 RID: 20
	[ResourcePanelExtension(typeof(PlistParticleFileBuild))]
	public class PlistParticleFileBuild : ResourceFileBuild
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000382F File Offset: 0x00001A2F
		public override Type NodeDataType
		{
			get
			{
				return typeof(PlistParticleFile);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000383C File Offset: 0x00001A3C
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = PlistParticleFileBuild.expandIcon
			};
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000385B File Offset: 0x00001A5B
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003866 File Offset: 0x00001A66
		public override bool CanRename()
		{
			return false;
		}

		// Token: 0x04000031 RID: 49
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("Particle.png"));
	}
}

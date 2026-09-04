using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000013 RID: 19
	[ResourcePanelExtension(typeof(FntFileBuild))]
	public class FntFileBuild : ResourceFileBuild
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000037D5 File Offset: 0x000019D5
		public override Type NodeDataType
		{
			get
			{
				return typeof(FntFile);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000037E4 File Offset: 0x000019E4
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = FntFileBuild.expandIcon
			};
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003803 File Offset: 0x00001A03
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000380E File Offset: 0x00001A0E
		public override bool CanRename()
		{
			return false;
		}

		// Token: 0x04000030 RID: 48
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("Particle.png"));
	}
}

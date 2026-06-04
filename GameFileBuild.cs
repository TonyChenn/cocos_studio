using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000018 RID: 24
	[ResourcePanelExtension(typeof(GameFileBuild))]
	internal class GameFileBuild : CocosFileBuild
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003AD8 File Offset: 0x00001CD8
		public override Type NodeDataType
		{
			get
			{
				return typeof(GameFile);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003AE4 File Offset: 0x00001CE4
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = GameFileBuild.expandIcon
			};
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003B03 File Offset: 0x00001D03
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x04000035 RID: 53
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("Images.commonFile.png"));
	}
}

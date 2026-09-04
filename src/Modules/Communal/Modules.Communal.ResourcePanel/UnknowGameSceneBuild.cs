using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000022 RID: 34
	[ResourcePanelExtension(typeof(UnknowGameSceneBuild))]
	internal class UnknowGameSceneBuild : CocosFileBuild
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004254 File Offset: 0x00002454
		public override Type NodeDataType
		{
			get
			{
				return typeof(UnknowGameScene);
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004260 File Offset: 0x00002460
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = UnknowGameSceneBuild.expandIcon
			};
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000427F File Offset: 0x0000247F
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.BuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x04000040 RID: 64
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("Other.png"));
	}
}

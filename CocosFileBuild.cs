using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000017 RID: 23
	[ResourcePanelExtension(typeof(CocosFileBuild))]
	internal class CocosFileBuild : ResourceFileBuild
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003A83 File Offset: 0x00001C83
		public override Type NodeDataType
		{
			get
			{
				return typeof(CocosFile);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003A90 File Offset: 0x00001C90
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = CocosFileBuild.expandIcon
			};
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003AAF File Offset: 0x00001CAF
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x04000034 RID: 52
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("file.png"));
	}
}

using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200001E RID: 30
	[ResourcePanelExtension(typeof(PlistCocosFileBuild))]
	internal class PlistCocosFileBuild : CocosFileBuild
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00003FB0 File Offset: 0x000021B0
		public override Type NodeDataType
		{
			get
			{
				return typeof(CocosFile);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003FBC File Offset: 0x000021BC
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = PlistCocosFileBuild.expandIcon
			};
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003FDB File Offset: 0x000021DB
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x0400003C RID: 60
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("csi.png"));
	}
}

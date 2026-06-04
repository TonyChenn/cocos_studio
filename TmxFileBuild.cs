using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000016 RID: 22
	[ResourcePanelExtension(typeof(TmxFileBuild))]
	public class TmxFileBuild : ResourceFileBuild
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003A2A File Offset: 0x00001C2A
		public override Type NodeDataType
		{
			get
			{
				return typeof(TmxFile);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003A38 File Offset: 0x00001C38
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = TmxFileBuild.expandIcon
			};
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003A57 File Offset: 0x00001C57
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003A62 File Offset: 0x00001C62
		public override bool CanRename()
		{
			return false;
		}

		// Token: 0x04000033 RID: 51
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("file.png"));
	}
}

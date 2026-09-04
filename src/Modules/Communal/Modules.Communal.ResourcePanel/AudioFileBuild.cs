using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000011 RID: 17
	[ResourcePanelExtension(typeof(AudioFileBuild))]
	internal class AudioFileBuild : ResourceFileBuild
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003703 File Offset: 0x00001903
		public override Type NodeDataType
		{
			get
			{
				return typeof(AudioFile);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003710 File Offset: 0x00001910
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = AudioFileBuild.expandIcon
			};
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000372F File Offset: 0x0000192F
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x0400002F RID: 47
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("Audio.png"));
	}
}

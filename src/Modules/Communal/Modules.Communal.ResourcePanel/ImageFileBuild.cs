using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000019 RID: 25
	[ResourcePanelExtension(typeof(ImageFileBuild))]
	internal class ImageFileBuild : ResourceFileBuild
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003B2C File Offset: 0x00001D2C
		public override Type NodeDataType
		{
			get
			{
				return typeof(ImageFile);
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003B40 File Offset: 0x00001D40
		protected override IconInfo GetIcon(object dataObject)
		{
			IconInfo iconInfo = new IconInfo();
			ImageFile imageFile = dataObject as ImageFile;
			if (imageFile != null && imageFile.IsPacked())
			{
				iconInfo.ExpandIcon = ImageFileBuild.packedresourceIcon;
			}
			else
			{
				iconInfo.ExpandIcon = ImageFileBuild.expandIcon;
			}
			return iconInfo;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003B7E File Offset: 0x00001D7E
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003B8C File Offset: 0x00001D8C
		public override void OnNodeAdded(object dateObject)
		{
			ImageFile imageFile = dateObject as ImageFile;
			if (imageFile != null)
			{
				imageFile.PackedChanged += this.image_PackedChanged;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003BB8 File Offset: 0x00001DB8
		private void image_PackedChanged(object sender, EventArgs e)
		{
			ITreeBuild treeBuilder = base.Context.GetTreeBuilder();
			if (treeBuilder != null)
			{
				treeBuilder.Update(sender);
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003BDC File Offset: 0x00001DDC
		public override void OnNodeRemoved(object dateObject)
		{
			ImageFile imageFile = dateObject as ImageFile;
			if (imageFile != null)
			{
				imageFile.PackedChanged -= this.image_PackedChanged;
			}
		}

		// Token: 0x04000036 RID: 54
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("image.png"));

		// Token: 0x04000037 RID: 55
		private static readonly Xwt.Drawing.Image packedresourceIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("graph.png"));
	}
}

using System;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(ImageFileBuild))]
	internal class ImageFileBuild : ResourceFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(ImageFile);
			}
		}

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

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		public override void OnNodeAdded(object dateObject)
		{
			ImageFile imageFile = dateObject as ImageFile;
			if (imageFile != null)
			{
				imageFile.PackedChanged += this.image_PackedChanged;
			}
		}

		private void image_PackedChanged(object sender, EventArgs e)
		{
			ITreeBuild treeBuilder = base.Context.GetTreeBuilder();
			if (treeBuilder != null)
			{
				treeBuilder.Update(sender);
			}
		}

		public override void OnNodeRemoved(object dateObject)
		{
			ImageFile imageFile = dateObject as ImageFile;
			if (imageFile != null)
			{
				imageFile.PackedChanged -= this.image_PackedChanged;
			}
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("image.png"));

		private static readonly Xwt.Drawing.Image packedresourceIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("graph.png"));
	}
}

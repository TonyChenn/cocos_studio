using System;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(PlistImageFileBuild))]
	internal class PlistImageFileBuild : ResourceFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(PlistImageFile);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = PlistImageFileBuild.expandIcon
			};
		}

		public override bool CanRename()
		{
			return false;
		}

		internal override bool CanDelete()
		{
			return false;
		}

		public override string CanMove(object moveSource, object moveTarget, TreeViewDropPosition pos)
		{
			return LanguageInfo.FileMove_Plist;
		}

		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			PlistImageFile plistImageFile = dataObject as PlistImageFile;
			return plistImageFile.Parent.Parent as ResourceFolder;
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("graph.png"));
	}
}

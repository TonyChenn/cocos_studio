using System;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200001B RID: 27
	[ResourcePanelExtension(typeof(PlistImageFileBuild))]
	internal class PlistImageFileBuild : ResourceFileBuild
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003C2F File Offset: 0x00001E2F
		public override Type NodeDataType
		{
			get
			{
				return typeof(PlistImageFile);
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003C3C File Offset: 0x00001E3C
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = PlistImageFileBuild.expandIcon
			};
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003C5B File Offset: 0x00001E5B
		public override bool CanRename()
		{
			return false;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003C5E File Offset: 0x00001E5E
		internal override bool CanDelete()
		{
			return false;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003C61 File Offset: 0x00001E61
		public override string CanMove(object moveSource, object moveTarget, TreeViewDropPosition pos)
		{
			return LanguageInfo.FileMove_Plist;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003C68 File Offset: 0x00001E68
		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			PlistImageFile plistImageFile = dataObject as PlistImageFile;
			return plistImageFile.Parent.Parent as ResourceFolder;
		}

		// Token: 0x04000038 RID: 56
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("graph.png"));
	}
}

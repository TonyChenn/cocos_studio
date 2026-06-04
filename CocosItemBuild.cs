using System;
using System.Collections.Generic;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200001F RID: 31
	[ResourcePanelExtension(typeof(CocosItemBuild))]
	public class CocosItemBuild : ResourceFileBuild
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004004 File Offset: 0x00002204
		public override Type NodeDataType
		{
			get
			{
				return typeof(CocosItem);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004010 File Offset: 0x00002210
		protected override IconInfo GetIcon(object dataObject)
		{
			IconInfo iconInfo = new IconInfo();
			CocosItem cocosItem = dataObject as CocosItem;
			if (cocosItem == null)
			{
				return iconInfo;
			}
			string text = string.IsNullOrWhiteSpace(cocosItem.ContentType) ? "Scene" : cocosItem.ContentType;
			string resourceID = StaticVariable.GetResourceID(text + ".png");
			if (!CocosItemBuild.iconDir.ContainsKey(text))
			{
				Xwt.Drawing.Image icon = ImageIcon.GetIcon(resourceID);
				CocosItemBuild.iconDir.Add(text, icon);
			}
			iconInfo.ExpandIcon = CocosItemBuild.iconDir[text];
			return iconInfo;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004090 File Offset: 0x00002290
		public override void BuildChildNodes(ITreeBuild treeBuilder, object dataObject)
		{
			CocosItem cocosItem = dataObject as CocosItem;
			treeBuilder.AddChildren(cocosItem.SourceFiles);
		}

		// Token: 0x0400003D RID: 61
		private static Dictionary<string, Xwt.Drawing.Image> iconDir = new Dictionary<string, Xwt.Drawing.Image>();
	}
}

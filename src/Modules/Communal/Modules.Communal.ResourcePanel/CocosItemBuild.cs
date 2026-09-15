using System;
using System.Collections.Generic;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(CocosItemBuild))]
	public class CocosItemBuild : ResourceFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(CocosItem);
			}
		}

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

		public override void BuildChildNodes(ITreeBuild treeBuilder, object dataObject)
		{
			CocosItem cocosItem = dataObject as CocosItem;
			treeBuilder.AddChildren(cocosItem.SourceFiles);
		}

		private static Dictionary<string, Xwt.Drawing.Image> iconDir = new Dictionary<string, Xwt.Drawing.Image>();
	}
}

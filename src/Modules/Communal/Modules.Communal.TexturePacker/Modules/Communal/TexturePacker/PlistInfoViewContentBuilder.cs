using System;
using CocoStudio.Core;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.TexturePacker
{
	[Extension(Path = "CocoStudio/Ide/DisplayBuilder")]
	internal class PlistInfoViewContentBuilder : IViewDisplayBuilder, IDisplayBuilder
	{
		public IViewContentExtend CreateContent(FilePath fileName, string mimeType, CocosItem ownerCocosItem)
		{
			PlistInfoViewContent plistInfoViewContent = new PlistInfoViewContent();
			plistInfoViewContent.Initialize(ownerCocosItem);
			return plistInfoViewContent;
		}

		public string Name
		{
			get
			{
				return "PackerWidgetViewBuilder";
			}
		}

		public bool CanHandle(FilePath fileName, string mimeType, CocosItem ownerCocosItem)
		{
			return ownerCocosItem != null && ownerCocosItem.CocosFile is PlistInfoCocosFile;
		}

		public bool CanUseAsDefault
		{
			get
			{
				return true;
			}
		}
	}
}

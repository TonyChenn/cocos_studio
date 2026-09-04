using System;
using CocoStudio.Core;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.TexturePacker
{
	// Token: 0x02000013 RID: 19
	[Extension(Path = "CocoStudio/Ide/DisplayBuilder")]
	internal class PlistInfoViewContentBuilder : IViewDisplayBuilder, IDisplayBuilder
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00004F98 File Offset: 0x00003198
		public IViewContentExtend CreateContent(FilePath fileName, string mimeType, CocosItem ownerCocosItem)
		{
			PlistInfoViewContent plistInfoViewContent = new PlistInfoViewContent();
			plistInfoViewContent.Initialize(ownerCocosItem);
			return plistInfoViewContent;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004FB3 File Offset: 0x000031B3
		public string Name
		{
			get
			{
				return "PackerWidgetViewBuilder";
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004FBA File Offset: 0x000031BA
		public bool CanHandle(FilePath fileName, string mimeType, CocosItem ownerCocosItem)
		{
			return ownerCocosItem != null && ownerCocosItem.CocosFile is PlistInfoCocosFile;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004FD1 File Offset: 0x000031D1
		public bool CanUseAsDefault
		{
			get
			{
				return true;
			}
		}
	}
}

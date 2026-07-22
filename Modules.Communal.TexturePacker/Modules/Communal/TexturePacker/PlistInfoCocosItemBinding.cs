using System;
using CocoStudio.Projects;
using Mono.Addins;

namespace Modules.Communal.TexturePacker
{
	// Token: 0x0200000D RID: 13
	[Extension(typeof(ICocosItemBinding))]
	internal class PlistInfoCocosItemBinding : CocosItemBinding
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00004A98 File Offset: 0x00002C98
		protected override CocosItem OnCreateItem(CocosItemCreateInfo info)
		{
			PlistInfoCocosFile plistInfoCocosFile = new PlistInfoCocosFile(info);
			plistInfoCocosFile.Content = this.CreateCocosFileContent(info);
			return new PlistInfoCocosItem(info.FileName, plistInfoCocosFile);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004AC7 File Offset: 0x00002CC7
		private ICocosFileContent CreateCocosFileContent(CocosItemCreateInfo info)
		{
			return new PlistInfoData();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004ACE File Offset: 0x00002CCE
		protected override bool OnCanCreateItem(string cocosItemType)
		{
			return "Plist" == cocosItemType;
		}

		// Token: 0x04000035 RID: 53
		private const string cocosItemTypeName = "Plist";
	}
}

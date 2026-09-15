using System;
using CocoStudio.Projects;
using Mono.Addins;

namespace Modules.Communal.TexturePacker
{
	[Extension(typeof(ICocosItemBinding))]
	internal class PlistInfoCocosItemBinding : CocosItemBinding
	{
		protected override CocosItem OnCreateItem(CocosItemCreateInfo info)
		{
			PlistInfoCocosFile plistInfoCocosFile = new PlistInfoCocosFile(info);
			plistInfoCocosFile.Content = this.CreateCocosFileContent(info);
			return new PlistInfoCocosItem(info.FileName, plistInfoCocosFile);
		}

		private ICocosFileContent CreateCocosFileContent(CocosItemCreateInfo info)
		{
			return new PlistInfoData();
		}

		protected override bool OnCanCreateItem(string cocosItemType)
		{
			return "Plist" == cocosItemType;
		}

		private const string cocosItemTypeName = "Plist";
	}
}

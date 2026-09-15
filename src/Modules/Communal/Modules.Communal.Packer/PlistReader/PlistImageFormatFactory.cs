using System;
using CocoStudio.Basic;
using Modules.Communal.Packer.PlistReader.Formates;
using Modules.Communal.PList;

namespace Modules.Communal.Packer.PlistReader
{
	public static class PlistImageFormatFactory
	{
		public static PlistImageFormat CreatePlistFormat(PListDict rootPlistDict)
		{
			PlistImageFormat result;
			if (!rootPlistDict.ContainsKey("metadata"))
			{
				result = null;
			}
			else
			{
				PListDict plistDict = rootPlistDict["metadata"] as PListDict;
				switch ((int)((PListInteger)plistDict["format"]).Value)
				{
				case 0:
					result = new PlistFormatCocos2d_Original();
					break;
				case 1:
					result = new PlistFormatCocos2d_0994();
					break;
				case 2:
					result = new PlistFormatCocos2d();
					break;
				case 3:
					result = new PlistFormatZwoptex();
					break;
				default:
					result = null;
					break;
				}
			}
			return result;
		}

		public static PlistImageFormat CreatePlistFormat(string plistFilePath)
		{
			PlistImageFormat result;
			try
			{
				PListRoot plistRoot = PListRoot.Load(plistFilePath);
				result = PlistImageFormatFactory.CreatePlistFormat(plistRoot.Root as PListDict);
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
				result = null;
			}
			return result;
		}

		public static PlistImageFormat CreatePlistFormat(PlistFormats formate)
		{
			PlistImageFormat result;
			switch (formate)
			{
			case PlistFormats.Cocos2d:
				result = new PlistFormatCocos2d();
				break;
			case PlistFormats.Cocos2d_0994:
				result = new PlistFormatCocos2d_0994();
				break;
			case PlistFormats.Cocos2d_Original:
				result = new PlistFormatCocos2d_Original();
				break;
			case PlistFormats.Zwoptex:
				result = new PlistFormatZwoptex();
				break;
			default:
				result = new PlistFormatCocos2d();
				break;
			}
			return result;
		}

		public const string MetaDataName = "metadata";
	}
}

using System;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.PList;

namespace Modules.Communal.Packer
{
	public class PlistParticleReader
	{
		public static bool CheckIsParticle(string filePath)
		{
			bool result;
			try
			{
				if (!Path.GetExtension(filePath).Equals(".plist", StringComparison.OrdinalIgnoreCase))
				{
					result = false;
				}
				else
				{
					result = PlistParticleReader.CheckPlist(filePath);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Read plist failed.", exception);
				result = false;
			}
			return result;
		}

		public static bool IsValid(string filePath)
		{
			bool result;
			if (!File.Exists(filePath))
			{
				result = false;
			}
			else
			{
				PListDict plistDict = PlistParticleReader.ReadPlist(filePath);
				string matchImage = PlistParticleReader.GetMatchImage(plistDict);
				if (!string.IsNullOrEmpty(matchImage) && !Path.IsPathRooted(matchImage))
				{
					string directoryName = Path.GetDirectoryName(filePath);
					string path = Path.Combine(directoryName, matchImage);
					if (File.Exists(path))
					{
						return true;
					}
				}
				result = (plistDict.ContainsKey("textureImageData") && !string.IsNullOrEmpty(((PListString)plistDict["textureImageData"]).Value));
			}
			return result;
		}

		private static bool CheckPlist(string filePath)
		{
			bool result;
			if (!File.Exists(filePath))
			{
				result = false;
			}
			else
			{
				PListDict plistDict = PlistParticleReader.ReadPlist(filePath);
				bool flag = plistDict.ContainsKey("particleLifespan");
				result = flag;
			}
			return result;
		}

		private static PListDict ReadPlist(string filePath)
		{
			PListRoot plistRoot = null;
			using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				plistRoot = PListRoot.Load(fileStream);
			}
			return (PListDict)plistRoot.Root;
		}

		private static string GetMatchImage(PListDict plistDic)
		{
			string result;
			if (plistDic.ContainsKey("textureImageData") && !string.IsNullOrEmpty(((PListString)plistDic["textureImageData"]).Value))
			{
				result = null;
			}
			else if (plistDic.ContainsKey("textureFileName"))
			{
				result = ((PListString)plistDic["textureFileName"]).Value;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static string GetMatchImage(string filePath)
		{
			PListDict plistDic = PlistParticleReader.ReadPlist(filePath);
			return PlistParticleReader.GetMatchImage(plistDic);
		}

		private const string ParticleLifespanKey = "particleLifespan";
	}
}

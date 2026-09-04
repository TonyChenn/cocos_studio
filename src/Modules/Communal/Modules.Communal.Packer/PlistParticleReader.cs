using System;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.PList;

namespace Modules.Communal.Packer
{
	// Token: 0x02000019 RID: 25
	public class PlistParticleReader
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00005FE4 File Offset: 0x000041E4
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

		// Token: 0x06000096 RID: 150 RVA: 0x00006040 File Offset: 0x00004240
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

		// Token: 0x06000097 RID: 151 RVA: 0x000060EC File Offset: 0x000042EC
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

		// Token: 0x06000098 RID: 152 RVA: 0x00006120 File Offset: 0x00004320
		private static PListDict ReadPlist(string filePath)
		{
			PListRoot plistRoot = null;
			using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				plistRoot = PListRoot.Load(fileStream);
			}
			return (PListDict)plistRoot.Root;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006178 File Offset: 0x00004378
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

		// Token: 0x0600009A RID: 154 RVA: 0x000061E8 File Offset: 0x000043E8
		public static string GetMatchImage(string filePath)
		{
			PListDict plistDic = PlistParticleReader.ReadPlist(filePath);
			return PlistParticleReader.GetMatchImage(plistDic);
		}

		// Token: 0x0400003F RID: 63
		private const string ParticleLifespanKey = "particleLifespan";
	}
}

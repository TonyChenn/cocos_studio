using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.PList;

namespace Modules.Communal.Packer.PlistReader
{
	// Token: 0x02000010 RID: 16
	public abstract class PlistImageFormat
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00004790 File Offset: 0x00002990
		public PlistImageFormat()
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000479C File Offset: 0x0000299C
		public List<ImageInfo> ToImageList(PListDict plistDict)
		{
			List<ImageInfo> result;
			try
			{
				result = this.OnToImageList(plistDict);
			}
			catch (Exception arg)
			{
				LogConfig.Logger.Error("解析Plist文件出错:" + arg);
				result = null;
			}
			return result;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000047E4 File Offset: 0x000029E4
		public PListRoot ToPlist(List<ImageInfo> imageList, Size size, string imageKey)
		{
			PListRoot result;
			try
			{
				result = this.OnToPlist(imageList, size, imageKey);
			}
			catch (Exception arg)
			{
				LogConfig.Logger.Error("转换为Plist文件出错:" + arg);
				result = null;
			}
			return result;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004830 File Offset: 0x00002A30
		public string GetImageFilePath(string plistFilePath)
		{
			string result;
			try
			{
				result = this.OnGetImageFilePath(plistFilePath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("GetImageFilePath failed", exception);
				result = null;
			}
			return result;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004874 File Offset: 0x00002A74
		public string GetImageFileName(PListDict plistRoot)
		{
			return this.GetTextureFileName(plistRoot);
		}

		// Token: 0x06000068 RID: 104
		protected abstract PListRoot OnToPlist(List<ImageInfo> imageList, Size size, string imageKey);

		// Token: 0x06000069 RID: 105
		protected abstract List<ImageInfo> OnToImageList(PListDict plistDict);

		// Token: 0x0600006A RID: 106 RVA: 0x00004890 File Offset: 0x00002A90
		protected virtual string OnGetImageFilePath(string plistFilePath)
		{
			string textureFilePath = this.GetTextureFilePath(plistFilePath);
			string result;
			if (textureFilePath != null)
			{
				result = textureFilePath;
			}
			else
			{
				result = this.GetSameNameImageFile(plistFilePath);
			}
			return result;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000048C0 File Offset: 0x00002AC0
		private string GetTextureFilePath(string plistFilePath)
		{
			string result;
			try
			{
				PListRoot plistRoot = PListRoot.Load(plistFilePath);
				string textureFileName = this.GetTextureFileName(plistRoot.Root as PListDict);
				result = Path.Combine(Path.GetDirectoryName(plistFilePath), textureFileName);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004910 File Offset: 0x00002B10
		private string GetTextureFileName(PListDict rootElement)
		{
			string result;
			try
			{
				PListDict plistDict = rootElement["metadata"] as PListDict;
				string value = ((PListString)plistDict["textureFileName"]).Value;
				result = value;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004964 File Offset: 0x00002B64
		private string GetSameNameImageFile(string plistFilePath)
		{
			string text = Path.ChangeExtension(plistFilePath, ".png");
			string result;
			if (File.Exists(text))
			{
				result = text;
			}
			else
			{
				string text2 = Path.ChangeExtension(plistFilePath, ".jpg");
				if (File.Exists(text2))
				{
					result = text2;
				}
				else
				{
					result = text;
				}
			}
			return result;
		}

		// Token: 0x04000035 RID: 53
		protected const string framesName = "frames";
	}
}

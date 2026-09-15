using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.PList;

namespace Modules.Communal.Packer.PlistReader
{
	public abstract class PlistImageFormat
	{
		public PlistImageFormat()
		{
		}

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

		public string GetImageFileName(PListDict plistRoot)
		{
			return this.GetTextureFileName(plistRoot);
		}

		protected abstract PListRoot OnToPlist(List<ImageInfo> imageList, Size size, string imageKey);

		protected abstract List<ImageInfo> OnToImageList(PListDict plistDict);

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

		protected const string framesName = "frames";
	}
}

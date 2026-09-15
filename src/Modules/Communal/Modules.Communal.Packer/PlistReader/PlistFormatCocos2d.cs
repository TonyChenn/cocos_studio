using System;
using System.Collections.Generic;
using System.Drawing;
using Modules.Communal.PList;

namespace Modules.Communal.Packer.PlistReader
{
	internal class PlistFormatCocos2d : PlistImageFormat
	{
		protected override List<ImageInfo> OnToImageList(PListDict plistDict)
		{
			List<ImageInfo> list = new List<ImageInfo>();
			PListDict plistDict2 = plistDict["frames"] as PListDict;
			foreach (KeyValuePair<string, IPListElement> keyValuePair in plistDict2)
			{
				string key = keyValuePair.Key;
				PListDict plistDict3 = keyValuePair.Value as PListDict;
				string value = ((PListString)plistDict3["frame"]).Value;
				Rectangle bounding = PlistFormatHelp.ConvertToRect(value);
				string value2 = ((PListString)plistDict3["offset"]).Value;
				PointF pointF = PlistFormatHelp.ConvertToPointF(value2);
				bool value3 = ((PListBool)plistDict3["rotated"]).Value;
				string value4 = ((PListString)plistDict3["sourceSize"]).Value;
				Size sourceSize = PlistFormatHelp.ConvertToSize(value4);
				int x = (int)((double)(sourceSize.Width - bounding.Width) / 2.0 + (double)pointF.X);
				int y = (int)((double)(sourceSize.Height - bounding.Height) / 2.0 - (double)pointF.Y);
				ImageInfo item = new ImageInfo(key, bounding, sourceSize, new Point(x, y), value3);
				list.Add(item);
			}
			return list;
		}

		protected override PListRoot OnToPlist(List<ImageInfo> imageList, Size size, string imageKey)
		{
			PListRoot plistRoot = new PListRoot();
			PListDict plistDict = new PListDict();
			plistRoot.Root = plistDict;
			PListDict plistDict2 = new PListDict();
			plistDict.Add("frames", plistDict2);
			foreach (ImageInfo imageInfo in imageList)
			{
				PListDict plistDict3 = new PListDict();
				plistDict2.Add(imageInfo.Name, plistDict3);
				string value = PlistFormatHelp.ConvertToString(imageInfo.Bounding);
				plistDict3.Add("frame", new PListString(value));
				int x = imageInfo.SourceLocation.X + imageInfo.Bounding.Width / 2 - imageInfo.SourceSize.Width / 2;
				int y = imageInfo.SourceSize.Height / 2 - (imageInfo.SourceLocation.Y + imageInfo.Bounding.Height / 2);
				Point point = new Point(x, y);
				string value2 = PlistFormatHelp.ConvertToString(point);
				plistDict3.Add("offset", new PListString(value2));
				plistDict3.Add("rotated", new PListBool(imageInfo.IsRotation));
				string value3 = PlistFormatHelp.ConvertToString(imageInfo.SourceSize);
				plistDict3.Add("sourceSize", new PListString(value3));
			}
			PListDict plistDict4 = new PListDict();
			plistDict.Add("metadata", plistDict4);
			plistDict4.Add("format", new PListInteger(2L));
			plistDict4.Add("textureFileName", new PListString(imageKey));
			plistDict4.Add("realTextureFileName", new PListString(imageKey));
			string value4 = string.Concat(new object[]
			{
				"{",
				size.Width,
				",",
				size.Height,
				"}"
			});
			plistDict4.Add("size", new PListString(value4));
			PListDict plistDict5 = new PListDict();
			plistDict.Add("texture", plistDict5);
			plistDict5.Add("width", new PListInteger((long)size.Width));
			plistDict5.Add("height", new PListInteger((long)size.Height));
			return plistRoot;
		}
	}
}

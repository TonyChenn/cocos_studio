using System;
using System.Collections.Generic;
using System.Drawing;
using Modules.Communal.PList;

namespace Modules.Communal.Packer.PlistReader
{
	internal class PlistFormatCocos2d_0994 : PlistImageFormat
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
				string value3 = ((PListString)plistDict3["sourceSize"]).Value;
				Size sourceSize = PlistFormatHelp.ConvertToSize(value3);
				int x = (int)((double)(sourceSize.Width - bounding.Width) / 2.0 + (double)pointF.X);
				int y = (int)((double)(sourceSize.Height - bounding.Height) / 2.0 - (double)pointF.Y);
				ImageInfo item = new ImageInfo(key, bounding, sourceSize, new Point(x, y));
				list.Add(item);
			}
			return list;
		}

		protected override PListRoot OnToPlist(List<ImageInfo> imageList, Size size, string imageKey)
		{
			throw new NotImplementedException();
		}
	}
}

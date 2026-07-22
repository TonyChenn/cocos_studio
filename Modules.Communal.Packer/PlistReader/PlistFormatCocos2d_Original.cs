using System;
using System.Collections.Generic;
using System.Drawing;
using Modules.Communal.PList;

namespace Modules.Communal.Packer.PlistReader
{
	// Token: 0x02000013 RID: 19
	internal class PlistFormatCocos2d_Original : PlistImageFormat
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00004F5C File Offset: 0x0000315C
		protected override List<ImageInfo> OnToImageList(PListDict plistDict)
		{
			List<ImageInfo> list = new List<ImageInfo>();
			PListDict plistDict2 = plistDict["frames"] as PListDict;
			foreach (KeyValuePair<string, IPListElement> keyValuePair in plistDict2)
			{
				string key = keyValuePair.Key;
				PListDict plistDict3 = keyValuePair.Value as PListDict;
				int width = (int)((PListInteger)plistDict3["width"]).Value;
				int height = (int)((PListInteger)plistDict3["height"]).Value;
				int x = (int)((PListInteger)plistDict3["x"]).Value;
				int y = (int)((PListInteger)plistDict3["y"]).Value;
				Rectangle bounding = new Rectangle(x, y, width, height);
				int width2 = (int)((PListInteger)plistDict3["originalWidth"]).Value;
				int height2 = (int)((PListInteger)plistDict3["originalHeight"]).Value;
				Size sourceSize = new Size(width2, height2);
				int num = (int)((PListReal)plistDict3["offsetX"]).Value;
				int num2 = (int)((PListReal)plistDict3["offsetY"]).Value;
				int x2 = (sourceSize.Width - bounding.Width) / 2 + num;
				int y2 = (sourceSize.Height - bounding.Height) / 2 - num2;
				Point sourceLocation = new Point(x2, y2);
				ImageInfo item = new ImageInfo(key, bounding, sourceSize, sourceLocation);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005128 File Offset: 0x00003328
		protected override PListRoot OnToPlist(List<ImageInfo> imageList, Size size, string imageKey)
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using Modules.Communal.PList;

namespace Modules.Communal.Packer.PlistReader
{
	// Token: 0x02000016 RID: 22
	internal class PlistFormatZwoptex : PlistImageFormat
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00005644 File Offset: 0x00003844
		protected override List<ImageInfo> OnToImageList(PListDict plistDict)
		{
			List<ImageInfo> list = new List<ImageInfo>();
			PListDict plistDict2 = plistDict["frames"] as PListDict;
			foreach (KeyValuePair<string, IPListElement> keyValuePair in plistDict2)
			{
				string key = keyValuePair.Key;
				PListDict plistDict3 = keyValuePair.Value as PListDict;
				string value = ((PListString)plistDict3["spriteColorRect"]).Value;
				Rectangle rectangle = PlistFormatHelp.ConvertToRect(value);
				string value2 = ((PListString)plistDict3["textureRect"]).Value;
				Rectangle bounding = PlistFormatHelp.ConvertToRect(value2);
				string value3 = ((PListString)plistDict3["spriteSourceSize"]).Value;
				Size sourceSize = PlistFormatHelp.ConvertToSize(value3);
				bool value4 = ((PListBool)plistDict3["textureRotated"]).Value;
				if (value4)
				{
					int width = bounding.Width;
					bounding.Width = bounding.Height;
					bounding.Height = width;
				}
				ImageInfo item = new ImageInfo(key, bounding, sourceSize, rectangle.Location, value4);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000057A8 File Offset: 0x000039A8
		protected override PListRoot OnToPlist(List<ImageInfo> imageList, Size size, string imageKey)
		{
			throw new NotImplementedException();
		}
	}
}

using System;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000037 RID: 55
	public class FrameDataEqualHelper
	{
		// Token: 0x0600023A RID: 570 RVA: 0x00006D74 File Offset: 0x00004F74
		public static bool FrameDataEquals(FrameData f1, FrameData f2)
		{
			BoolFrameData boolFrameData = f1 as BoolFrameData;
			BoolFrameData boolFrameData2 = f2 as BoolFrameData;
			bool result;
			if (boolFrameData != null && boolFrameData2 != null)
			{
				result = (boolFrameData.Value == boolFrameData2.Value);
			}
			else
			{
				IntFrameData intFrameData = f1 as IntFrameData;
				IntFrameData intFrameData2 = f2 as IntFrameData;
				if (intFrameData != null && intFrameData2 != null)
				{
					result = (intFrameData.Value == intFrameData2.Value);
				}
				else
				{
					StringFrameData stringFrameData = f1 as StringFrameData;
					StringFrameData stringFrameData2 = f2 as StringFrameData;
					if (stringFrameData != null && stringFrameData2 != null)
					{
						result = (stringFrameData.Value == stringFrameData2.Value);
					}
					else
					{
						PointFrameData pointFrameData = f1 as PointFrameData;
						PointFrameData pointFrameData2 = f2 as PointFrameData;
						if (pointFrameData != null && pointFrameData2 != null)
						{
							result = (FrameData.IsFloatEqual(pointFrameData.X, pointFrameData2.X, 0.0001f) && FrameData.IsFloatEqual(pointFrameData.Y, pointFrameData2.Y, 0.0001f));
						}
						else
						{
							ColorFrameData colorFrameData = f1 as ColorFrameData;
							ColorFrameData colorFrameData2 = f2 as ColorFrameData;
							result = (colorFrameData != null && colorFrameData2 != null && colorFrameData.Color.Equals(colorFrameData2.Color));
						}
					}
				}
			}
			return result;
		}
	}
}

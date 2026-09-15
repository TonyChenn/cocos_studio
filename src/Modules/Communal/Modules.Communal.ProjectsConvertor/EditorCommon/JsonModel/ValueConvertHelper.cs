using System;
using CocoStudio.Model;

namespace EditorCommon.JsonModel
{
	internal static class ValueConvertHelper
	{
		public static bool ConvertToBool(this byte a)
		{
			return a != 0;
		}

		public static bool ConvertToBool(this int a)
		{
			return a != 0;
		}

		public static byte ConvertToByte(this bool a)
		{
			if (!a)
			{
				return 0;
			}
			return 1;
		}

		public static float PointToAngle(float x, float y)
		{
			if (x != 0f)
			{
				double num = Math.Atan((double)(y / x));
				num = num * 180.0 / 3.141592653589793;
				if (x < 0f)
				{
					num += 180.0;
				}
				if (num < 0.0)
				{
					num += 360.0;
				}
				return (float)num;
			}
			if (y > 0f)
			{
				return 90f;
			}
			if (y < 0f)
			{
				return 270f;
			}
			return 0f;
		}

		public static ScaleValue AngleToVector(float angle)
		{
			float num = (float)(3.141592653589793 * (double)(90f - angle) / 180.0);
			float scaleX = (float)Math.Sin((double)num);
			float scaleY = (float)Math.Cos((double)num);
			return new ScaleValue(scaleX, scaleY, 0.1, -99999999.0, 99999999.0);
		}
	}
}

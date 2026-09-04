using System;
using CocoStudio.Model;

namespace EditorCommon.JsonModel
{
	// Token: 0x0200003F RID: 63
	internal static class ValueConvertHelper
	{
		// Token: 0x060003F0 RID: 1008 RVA: 0x0000A347 File Offset: 0x00008547
		public static bool ConvertToBool(this byte a)
		{
			return a != 0;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000A34F File Offset: 0x0000854F
		public static bool ConvertToBool(this int a)
		{
			return a != 0;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000A357 File Offset: 0x00008557
		public static byte ConvertToByte(this bool a)
		{
			if (!a)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000A360 File Offset: 0x00008560
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

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000A3E8 File Offset: 0x000085E8
		public static ScaleValue AngleToVector(float angle)
		{
			float num = (float)(3.141592653589793 * (double)(90f - angle) / 180.0);
			float scaleX = (float)Math.Sin((double)num);
			float scaleY = (float)Math.Cos((double)num);
			return new ScaleValue(scaleX, scaleY, 0.1, -99999999.0, 99999999.0);
		}
	}
}

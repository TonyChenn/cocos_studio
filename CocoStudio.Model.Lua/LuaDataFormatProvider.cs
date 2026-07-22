using System;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua
{
	// Token: 0x02000003 RID: 3
	public class LuaDataFormatProvider : IFormatProvider, ICustomFormatter
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002050 File Offset: 0x00000250
		public object GetFormat(Type formatType)
		{
			if (formatType == typeof(ICustomFormatter))
			{
				return this;
			}
			return null;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002068 File Offset: 0x00000268
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if (formatProvider != this)
			{
				return string.Empty;
			}
			if (arg is float)
			{
				return ((float)arg).ToString("F4");
			}
			if (arg is double)
			{
				return ((double)arg).ToString("F4");
			}
			if (arg is bool)
			{
				return ((bool)arg).ToString().ToLower();
			}
			SizeF sizeF = arg as SizeF;
			if (sizeF != null)
			{
				string arg2 = this.Format(null, sizeF.Width, this);
				string arg3 = this.Format(null, sizeF.Height, this);
				return string.Format("{0}, {1}", arg2, arg3);
			}
			PointF pointF = arg as PointF;
			if (pointF != null)
			{
				string arg4 = this.Format(null, pointF.X, this);
				string arg5 = this.Format(null, pointF.Y, this);
				return string.Format("{0}, {1}", arg4, arg5);
			}
			ScaleValue scaleValue = arg as ScaleValue;
			if (scaleValue != null)
			{
				string arg6 = this.Format(null, scaleValue.ScaleX, this);
				string arg7 = this.Format(null, scaleValue.ScaleY, this);
				return string.Format("{0}, {1}", arg6, arg7);
			}
			ColorData colorData = arg as ColorData;
			if (colorData != null)
			{
				string arg8 = this.Format(null, colorData.R, this);
				string arg9 = this.Format(null, colorData.G, this);
				string arg10 = this.Format(null, colorData.B, this);
				return string.Format("cc.c3b({0}, {1}, {2})", arg8, arg9, arg10);
			}
			return arg.ToString();
		}
	}
}

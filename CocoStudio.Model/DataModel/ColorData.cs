using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000005 RID: 5
	[DataModelExtension(typeof(Color))]
	public sealed class ColorData : IDataConvert
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002140 File Offset: 0x00000340
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002157 File Offset: 0x00000357
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty]
		[DefaultValue(255)]
		public int A { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002160 File Offset: 0x00000360
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002177 File Offset: 0x00000377
		[DefaultValue(255)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty]
		public int R { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002180 File Offset: 0x00000380
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002197 File Offset: 0x00000397
		[DefaultValue(255)]
		[ItemProperty]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int G { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000021A0 File Offset: 0x000003A0
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000021B7 File Offset: 0x000003B7
		[ItemProperty]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(255)]
		public int B { get; private set; }

		// Token: 0x06000014 RID: 20 RVA: 0x000021C0 File Offset: 0x000003C0
		public ColorData()
		{
			this.A = (this.R = (this.G = (this.B = 255)));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002200 File Offset: 0x00000400
		public ColorData(Color color) : this(color.A, color.R, color.G, color.B)
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002227 File Offset: 0x00000427
		public ColorData(byte a, byte r, byte g, byte b)
		{
			this.A = (int)a;
			this.R = (int)r;
			this.G = (int)g;
			this.B = (int)b;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002254 File Offset: 0x00000454
		public static implicit operator ColorData(Color color)
		{
			return new ColorData(color);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000226C File Offset: 0x0000046C
		public static implicit operator Color(ColorData color)
		{
			return Color.FromArgb(255, color.R, color.G, color.B);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000229C File Offset: 0x0000049C
		public object CreateViewModel()
		{
			Color color = this;
			return color;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000022BC File Offset: 0x000004BC
		public void SetData(object viewObject)
		{
			if (!(viewObject is Color))
			{
				throw new ArgumentException("Can only receive System.Drawing.Color object to apply value.");
			}
			Color color = (Color)viewObject;
			this.A = (int)color.A;
			this.R = (int)color.R;
			this.G = (int)color.G;
			this.B = (int)color.B;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002324 File Offset: 0x00000524
		public override bool Equals(object obj)
		{
			ColorData colorData = obj as ColorData;
			return colorData != null && this.R == colorData.R && this.G == colorData.G && this.B == colorData.B && this.A == colorData.A;
		}

		// Token: 0x04000002 RID: 2
		public static readonly ColorData White = new ColorData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}

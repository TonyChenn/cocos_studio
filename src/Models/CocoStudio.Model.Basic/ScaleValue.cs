using System;
using System.ComponentModel;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	// Token: 0x02000016 RID: 22
	[JsonObject(MemberSerialization.OptIn)]
	public sealed class ScaleValue
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003180 File Offset: 0x00001380
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00003197 File Offset: 0x00001397
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0f)]
		[ItemProperty(DefaultValue = 0f)]
		public float ScaleX { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000031A0 File Offset: 0x000013A0
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000031B7 File Offset: 0x000013B7
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		public float ScaleY { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000031C0 File Offset: 0x000013C0
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000031DB File Offset: 0x000013DB
		public bool CanSetX
		{
			get
			{
				return !this.canSetX;
			}
			set
			{
				this.canSetX = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000031E8 File Offset: 0x000013E8
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003203 File Offset: 0x00001403
		public bool CanSetY
		{
			get
			{
				return !this.canSetY;
			}
			set
			{
				this.canSetY = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003210 File Offset: 0x00001410
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00003228 File Offset: 0x00001428
		public double Increment
		{
			get
			{
				return this.increment;
			}
			set
			{
				if (this.increment != value)
				{
					this.increment = value;
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003250 File Offset: 0x00001450
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00003268 File Offset: 0x00001468
		public double MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				if (this.minValue != value)
				{
					this.minValue = value;
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003290 File Offset: 0x00001490
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000032A8 File Offset: 0x000014A8
		public double MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				if (this.maxValue != value)
				{
					this.maxValue = value;
				}
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000032CF File Offset: 0x000014CF
		public ScaleValue()
		{
			this.ScaleX = 0f;
			this.ScaleY = 0f;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000032F2 File Offset: 0x000014F2
		public ScaleValue(float scaleX, float scaleY, double increment = 0.1, double minValue = -99999999.0, double maxValue = 99999999.0)
		{
			this.ScaleX = scaleX;
			this.ScaleY = scaleY;
			this.Increment = increment;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003328 File Offset: 0x00001528
		public ScaleValue(float zoom)
		{
			this.ScaleY = zoom;
			this.ScaleX = zoom;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003350 File Offset: 0x00001550
		public override string ToString()
		{
			return this.ScaleX + "," + this.ScaleY;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003384 File Offset: 0x00001584
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is ScaleValue)
			{
				ScaleValue scaleValue = (ScaleValue)obj;
				result = (obj != null && this.ScaleX == scaleValue.ScaleX && this.ScaleY == scaleValue.ScaleY);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000033D8 File Offset: 0x000015D8
		public override int GetHashCode()
		{
			return this.ScaleX.GetHashCode() ^ this.ScaleY.GetHashCode();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003408 File Offset: 0x00001608
		public static explicit operator double(ScaleValue scaleValue)
		{
			double num = Math.Pow((double)scaleValue.ScaleX, 2.0);
			double num2 = Math.Pow((double)scaleValue.ScaleY, 2.0);
			return Math.Sqrt(num + num2);
		}

		// Token: 0x0400004E RID: 78
		public static readonly ScaleValue Empty = new ScaleValue(0f, 0f, 0.1, -99999999.0, 99999999.0);

		// Token: 0x0400004F RID: 79
		public static readonly ScaleValue Half = new ScaleValue(0.5f, 0.5f, 0.1, -99999999.0, 99999999.0);

		// Token: 0x04000050 RID: 80
		private bool canSetX;

		// Token: 0x04000051 RID: 81
		private bool canSetY;

		// Token: 0x04000052 RID: 82
		private double increment;

		// Token: 0x04000053 RID: 83
		private double minValue;

		// Token: 0x04000054 RID: 84
		private double maxValue;
	}
}

using System;
using System.ComponentModel;
using Gdk;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	// Token: 0x02000012 RID: 18
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public sealed class PointF : ICloneable
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002BB0 File Offset: 0x00000DB0
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00002BC8 File Offset: 0x00000DC8
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		[JsonProperty]
		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002BD4 File Offset: 0x00000DD4
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002BEC File Offset: 0x00000DEC
		[JsonProperty]
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public bool IsEmpty
		{
			get
			{
				return this.x == 0f && this.y == 0f;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002C28 File Offset: 0x00000E28
		public PointF()
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002C33 File Offset: 0x00000E33
		public PointF(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002C4C File Offset: 0x00000E4C
		public PointF(Point p)
		{
			this.x = (float)p.X;
			this.y = (float)p.Y;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002C74 File Offset: 0x00000E74
		public static implicit operator PointF(Point p)
		{
			return new PointF(p);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002C8C File Offset: 0x00000E8C
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is PointF)
			{
				PointF pointF = (PointF)obj;
				result = (obj != null && this.X == pointF.X && this.Y == pointF.Y);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002D10 File Offset: 0x00000F10
		public static bool operator ==(PointF leftValue, PointF rightValue)
		{
			bool result;
			if (object.ReferenceEquals(leftValue, null))
			{
				result = object.ReferenceEquals(rightValue, null);
			}
			else
			{
				result = leftValue.Equals(rightValue);
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002D44 File Offset: 0x00000F44
		public static bool operator !=(PointF leftValue, PointF rightValue)
		{
			return !(leftValue == rightValue);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002D60 File Offset: 0x00000F60
		public object Clone()
		{
			return new PointF
			{
				X = this.X,
				Y = this.Y
			};
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002D94 File Offset: 0x00000F94
		public override string ToString()
		{
			return string.Format("X:{0} Y:{1}", this.X, this.Y);
		}

		// Token: 0x04000042 RID: 66
		private float x;

		// Token: 0x04000043 RID: 67
		private float y;

		// Token: 0x04000044 RID: 68
		public static readonly PointF Empty = new PointF(0f, 0f);
	}
}

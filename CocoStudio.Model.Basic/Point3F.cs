using System;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	// Token: 0x02000007 RID: 7
	public sealed class Point3F
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000021E0 File Offset: 0x000003E0
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000021F7 File Offset: 0x000003F7
		[JsonProperty]
		[ItemProperty(DefaultValue = 0f)]
		public float X { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002200 File Offset: 0x00000400
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002217 File Offset: 0x00000417
		[ItemProperty(DefaultValue = 0f)]
		[JsonProperty]
		public float Y { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002220 File Offset: 0x00000420
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002237 File Offset: 0x00000437
		[ItemProperty(DefaultValue = 0f)]
		[JsonProperty]
		public float Z { get; set; }

		// Token: 0x0600001A RID: 26 RVA: 0x00002240 File Offset: 0x00000440
		public Point3F()
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000224B File Offset: 0x0000044B
		public Point3F(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002270 File Offset: 0x00000470
		public override bool Equals(object obj)
		{
			Point3F point3F = obj as Point3F;
			return point3F != null && (this.X == point3F.X && this.Y == point3F.Y) && this.Z == point3F.Z;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022C8 File Offset: 0x000004C8
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000022E0 File Offset: 0x000004E0
		public void Subtract(Point3F pz)
		{
			this.X -= pz.X;
			this.Y -= pz.Y;
			this.Z -= pz.Z;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002320 File Offset: 0x00000520
		public static Point3F Subtract(Point3F pt, Point3F pz)
		{
			return new Point3F(pt.X - pz.X, pt.Y - pz.Y, pt.X - pz.Y);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000235E File Offset: 0x0000055E
		public void Add(Point3F point3F)
		{
			this.X += point3F.X;
			this.Y += point3F.Y;
			this.Z += point3F.Z;
		}

		// Token: 0x04000022 RID: 34
		public static readonly Point3F Unit = new Point3F(1f, 1f, 1f);

		// Token: 0x04000023 RID: 35
		public static readonly Point3F UnitX = new Point3F(1f, 0f, 0f);

		// Token: 0x04000024 RID: 36
		public static readonly Point3F UnitY = new Point3F(0f, 1f, 0f);

		// Token: 0x04000025 RID: 37
		public static readonly Point3F UnitZ = new Point3F(0f, 0f, 1f);

		// Token: 0x04000026 RID: 38
		public static readonly Point3F Empty = new Point3F(0f, 0f, 0f);
	}
}

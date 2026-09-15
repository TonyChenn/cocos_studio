using System;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	public sealed class Point3F
	{
		[JsonProperty]
		[ItemProperty(DefaultValue = 0f)]
		public float X { get; set; }

		[ItemProperty(DefaultValue = 0f)]
		[JsonProperty]
		public float Y { get; set; }

		[ItemProperty(DefaultValue = 0f)]
		[JsonProperty]
		public float Z { get; set; }

		public Point3F()
		{
		}

		public Point3F(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public override bool Equals(object obj)
		{
			Point3F point3F = obj as Point3F;
			return point3F != null && (this.X == point3F.X && this.Y == point3F.Y) && this.Z == point3F.Z;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public void Subtract(Point3F pz)
		{
			this.X -= pz.X;
			this.Y -= pz.Y;
			this.Z -= pz.Z;
		}

		public static Point3F Subtract(Point3F pt, Point3F pz)
		{
			return new Point3F(pt.X - pz.X, pt.Y - pz.Y, pt.X - pz.Y);
		}

		public void Add(Point3F point3F)
		{
			this.X += point3F.X;
			this.Y += point3F.Y;
			this.Z += point3F.Z;
		}

		public static readonly Point3F Unit = new Point3F(1f, 1f, 1f);

		public static readonly Point3F UnitX = new Point3F(1f, 0f, 0f);

		public static readonly Point3F UnitY = new Point3F(0f, 1f, 0f);

		public static readonly Point3F UnitZ = new Point3F(0f, 0f, 1f);

		public static readonly Point3F Empty = new Point3F(0f, 0f, 0f);
	}
}

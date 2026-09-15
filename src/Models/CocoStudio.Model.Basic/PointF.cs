using System;
using System.ComponentModel;
using Gdk;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public sealed class PointF : ICloneable
	{
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

		public bool IsEmpty
		{
			get
			{
				return this.x == 0f && this.y == 0f;
			}
		}

		public PointF()
		{
		}

		public PointF(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public PointF(Point p)
		{
			this.x = (float)p.X;
			this.y = (float)p.Y;
		}

		public static implicit operator PointF(Point p)
		{
			return new PointF(p);
		}

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

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

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

		public static bool operator !=(PointF leftValue, PointF rightValue)
		{
			return !(leftValue == rightValue);
		}

		public object Clone()
		{
			return new PointF
			{
				X = this.X,
				Y = this.Y
			};
		}

		public override string ToString()
		{
			return string.Format("X:{0} Y:{1}", this.X, this.Y);
		}

		private float x;

		private float y;

		public static readonly PointF Empty = new PointF(0f, 0f);
	}
}

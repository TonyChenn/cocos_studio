using System;
using System.ComponentModel;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	public sealed class ScaleValue
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0f)]
		[ItemProperty(DefaultValue = 0f)]
		public float ScaleX { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		public float ScaleY { get; set; }

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

		public ScaleValue()
		{
			this.ScaleX = 0f;
			this.ScaleY = 0f;
		}

		public ScaleValue(float scaleX, float scaleY, double increment = 0.1, double minValue = -99999999.0, double maxValue = 99999999.0)
		{
			this.ScaleX = scaleX;
			this.ScaleY = scaleY;
			this.Increment = increment;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		public ScaleValue(float zoom)
		{
			this.ScaleY = zoom;
			this.ScaleX = zoom;
		}

		public override string ToString()
		{
			return this.ScaleX + "," + this.ScaleY;
		}

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

		public override int GetHashCode()
		{
			return this.ScaleX.GetHashCode() ^ this.ScaleY.GetHashCode();
		}

		public static explicit operator double(ScaleValue scaleValue)
		{
			double num = Math.Pow((double)scaleValue.ScaleX, 2.0);
			double num2 = Math.Pow((double)scaleValue.ScaleY, 2.0);
			return Math.Sqrt(num + num2);
		}

		public static readonly ScaleValue Empty = new ScaleValue(0f, 0f, 0.1, -99999999.0, 99999999.0);

		public static readonly ScaleValue Half = new ScaleValue(0.5f, 0.5f, 0.1, -99999999.0, 99999999.0);

		private bool canSetX;

		private bool canSetY;

		private double increment;

		private double minValue;

		private double maxValue;
	}
}

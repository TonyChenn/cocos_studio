using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(Color))]
	public sealed class ColorData : IDataConvert
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty]
		[DefaultValue(255)]
		public int A { get; private set; }

		[DefaultValue(255)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty]
		public int R { get; private set; }

		[DefaultValue(255)]
		[ItemProperty]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int G { get; private set; }

		[ItemProperty]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(255)]
		public int B { get; private set; }

		public ColorData()
		{
			this.A = (this.R = (this.G = (this.B = 255)));
		}

		public ColorData(Color color) : this(color.A, color.R, color.G, color.B)
		{
		}

		public ColorData(byte a, byte r, byte g, byte b)
		{
			this.A = (int)a;
			this.R = (int)r;
			this.G = (int)g;
			this.B = (int)b;
		}

		public static implicit operator ColorData(Color color)
		{
			return new ColorData(color);
		}

		public static implicit operator Color(ColorData color)
		{
			return Color.FromArgb(255, color.R, color.G, color.B);
		}

		public object CreateViewModel()
		{
			Color color = this;
			return color;
		}

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

		public override bool Equals(object obj)
		{
            return obj is ColorData colorData && this.R == colorData.R && this.G == colorData.G && this.B == colorData.B && this.A == colorData.A;
        }

		public static readonly ColorData White = new ColorData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}

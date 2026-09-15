using System;
using System.ComponentModel;
using System.Runtime;
using Gdk;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	public sealed class SizeF
	{
		public SizeF()
		{
		}

		public SizeF(SizeF size)
		{
			this.width = size.width;
			this.height = size.height;
		}

		public SizeF(PointF pt)
		{
			this.width = pt.X;
			this.height = pt.Y;
		}

		public SizeF(float width, float height)
		{
			this.width = width;
			this.height = height;
		}

		public static SizeF operator +(SizeF sz1, SizeF sz2)
		{
			return SizeF.Add(sz1, sz2);
		}

		public static SizeF operator -(SizeF sz1, SizeF sz2)
		{
			return SizeF.Subtract(sz1, sz2);
		}

		public static explicit operator PointF(SizeF size)
		{
			return new PointF(size.Width, size.Height);
		}

		[Browsable(false)]
		[JsonIgnore]
		public bool IsEmpty
		{
			[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
			get
			{
				return this.width == 0f && this.height == 0f;
			}
		}

		[JsonProperty(PropertyName = "X")]
		[ItemProperty(Name = "X")]
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		[ItemProperty(Name = "Y")]
		[JsonProperty(PropertyName = "Y")]
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		public static SizeF Add(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
		}

		public static SizeF Subtract(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (!(obj is SizeF))
			{
				result = false;
			}
			else
			{
				SizeF sizeF = (SizeF)obj;
				result = (sizeF.Width == this.Width && sizeF.Height == this.Height && sizeF.GetType().Equals(base.GetType()));
			}
			return result;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public PointF ToPointF()
		{
			return (PointF)this;
		}

		public Size ToSize()
		{
			return new Size((int)this.width, (int)this.height);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"{Width=",
				this.width,
				", Height=",
				this.height,
				"}"
			});
		}

		public static readonly SizeF Empty = new SizeF();

		private float width;

		private float height;

		public static readonly SizeF Zero = new SizeF(0f, 0f);
	}
}

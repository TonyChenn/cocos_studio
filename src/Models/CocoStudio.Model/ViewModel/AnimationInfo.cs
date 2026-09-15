using System;
using System.Drawing;

namespace CocoStudio.Model.ViewModel
{
	public class AnimationInfo : ICloneable
	{
		public AnimationInfo()
		{
		}

		public AnimationInfo(string name, int start, int end)
		{
			this.name = name;
			this.startIndex = start;
			this.endIndex = end;
		}

		public AnimationInfo(AnimationInfo other)
		{
			this.name = other.Name;
			this.startIndex = other.StartIndex;
			this.endIndex = other.EndIndex;
			this.rendercolor = other.RenderColor;
		}

		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
			set
			{
				if (this.startIndex != value)
				{
					this.startIndex = value;
					this.NotifyChanged();
				}
			}
		}

		public int EndIndex
		{
			get
			{
				return this.endIndex;
			}
			set
			{
				if (this.endIndex != value)
				{
					this.endIndex = value;
					this.NotifyChanged();
				}
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					this.NotifyChanged();
				}
			}
		}

		public Color RenderColor
		{
			get
			{
				if (this.rendercolor.IsEmpty)
				{
					this.rendercolor = AnimationInfo.GetRandomColor();
				}
				return this.rendercolor;
			}
			set
			{
				if (!this.rendercolor.Equals(value))
				{
					this.rendercolor = value;
					this.NotifyChanged();
				}
			}
		}

		public static Color GetRandomColor()
		{
			int color = AnimationInfo.rand.Next(28, 167);
			Color baseColor = Color.FromKnownColor((KnownColor)color);
			return Color.FromArgb(150, baseColor);
		}

		public event AnimationInfoChangedHandler AnimationInfoChanged;

		private void NotifyChanged()
		{
			if (this.AnimationInfoChanged != null)
			{
				this.AnimationInfoChanged();
			}
		}

		public object Clone()
		{
			return new AnimationInfo
			{
				name = this.Name,
				startIndex = this.StartIndex,
				endIndex = this.endIndex
			};
		}

		public const string InfoAllName = "-- ALL --";

		private int startIndex;

		private int endIndex;

		private string name;

		private Color rendercolor = Color.Empty;

		private static readonly Random rand = new Random((int)DateTime.Now.Ticks);
	}
}

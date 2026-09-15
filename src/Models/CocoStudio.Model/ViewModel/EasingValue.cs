using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.ViewModel
{
	[JsonObject(MemberSerialization.OptIn)]
	public class EasingValue : ICloneable
	{
		public TweenType TweenType
		{
			get
			{
				return this.tweenType;
			}
			set
			{
				this.tweenType = value;
			}
		}

		[JsonProperty]
		[ItemProperty]
		private List<PointF> Points
		{
			get
			{
				List<PointF> result;
				if (this.TweenType == TweenType.Custom)
				{
					result = this.proportionPoints;
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				this.proportionPoints = value;
			}
		}

		public IEnumerable<PointF> PropPoints
		{
			get
			{
				return this.proportionPoints;
			}
			private set
			{
				this.proportionPoints = value.ToList<PointF>();
			}
		}

		[JsonProperty]
		[ItemProperty]
		private int Type
		{
			get
			{
				return (int)this.TweenType;
			}
			set
			{
				this.TweenType = (TweenType)value;
			}
		}

		public EasingValue()
		{
			this.tweenType = TweenType.Linear;
		}

		public EasingValue(TweenType tweentype)
		{
			this.tweenType = tweentype;
		}

		public EasingValue(TweenType tweentype, List<PointF> points) : this(tweentype)
		{
			this.proportionPoints.AddRange(points);
		}

		public EasingValue(TweenType tweenType, List<float> easingparma) : this(tweenType)
		{
			if (easingparma != null)
			{
				if (tweenType == TweenType.Custom)
				{
					int count = easingparma.Count;
					for (int i = 0; i < count; i += 2)
					{
						this.proportionPoints.Add(new PointF(easingparma[i], easingparma[i + 1]));
					}
				}
				else
				{
					int count = easingparma.Count;
					for (int i = 0; i < count; i++)
					{
						this.proportionPoints.Add(new PointF(easingparma[i], 0f));
					}
				}
			}
		}

		public override int GetHashCode()
		{
			int num = -1;
			int count = this.proportionPoints.Count;
			if (this.PropPoints != null && count > 1)
			{
				num = this.proportionPoints[0].GetHashCode();
				for (int i = 1; i < count; i++)
				{
					if (i % 2 == 0)
					{
						num &= this.proportionPoints[i].GetHashCode();
					}
					else
					{
						num ^= this.proportionPoints[i].GetHashCode();
					}
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is EasingValue)
			{
				flag = true;
				EasingValue easingValue = obj as EasingValue;
				if (easingValue.TweenType != this.TweenType || easingValue.PropPoints == null || this.PropPoints == null || easingValue.PropPoints.Count<PointF>() != this.PropPoints.Count<PointF>())
				{
					flag = false;
				}
				if (flag)
				{
					int num = this.PropPoints.Count<PointF>();
					for (int i = 0; i < num; i++)
					{
						if (this.proportionPoints[i] != this.proportionPoints[i])
						{
							flag = false;
							break;
						}
					}
				}
			}
			return flag;
		}

		public object Clone()
		{
			return new EasingValue(this.TweenType, this.PropPoints.ToList<PointF>());
		}

		private TweenType tweenType = TweenType.Linear;

		private List<PointF> proportionPoints = new List<PointF>();
	}
}

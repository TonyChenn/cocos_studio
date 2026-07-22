using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000119 RID: 281
	[JsonObject(MemberSerialization.OptIn)]
	public class EasingValue : ICloneable
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0002A92C File Offset: 0x00028B2C
		// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x0002A944 File Offset: 0x00028B44
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

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0002A950 File Offset: 0x00028B50
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x0002A97C File Offset: 0x00028B7C
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

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0002A988 File Offset: 0x00028B88
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x0002A9A0 File Offset: 0x00028BA0
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

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0002A9B0 File Offset: 0x00028BB0
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x0002A9C8 File Offset: 0x00028BC8
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

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002A9D3 File Offset: 0x00028BD3
		public EasingValue()
		{
			this.tweenType = TweenType.Linear;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0002A9F7 File Offset: 0x00028BF7
		public EasingValue(TweenType tweentype)
		{
			this.tweenType = tweentype;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0002AA1B File Offset: 0x00028C1B
		public EasingValue(TweenType tweentype, List<PointF> points) : this(tweentype)
		{
			this.proportionPoints.AddRange(points);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0002AA34 File Offset: 0x00028C34
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

		// Token: 0x06000ABD RID: 2749 RVA: 0x0002AADC File Offset: 0x00028CDC
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

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002AB78 File Offset: 0x00028D78
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

		// Token: 0x06000ABF RID: 2751 RVA: 0x0002AC4C File Offset: 0x00028E4C
		public object Clone()
		{
			return new EasingValue(this.TweenType, this.PropPoints.ToList<PointF>());
		}

		// Token: 0x04000472 RID: 1138
		private TweenType tweenType = TweenType.Linear;

		// Token: 0x04000473 RID: 1139
		private List<PointF> proportionPoints = new List<PointF>();
	}
}

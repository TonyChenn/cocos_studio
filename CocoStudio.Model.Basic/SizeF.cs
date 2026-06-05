using System;
using System.ComponentModel;
using System.Runtime;
using Gdk;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	// Token: 0x02000019 RID: 25
	public sealed class SizeF
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x0000369A File Offset: 0x0000189A
		public SizeF()
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000036A5 File Offset: 0x000018A5
		public SizeF(SizeF size)
		{
			this.width = size.width;
			this.height = size.height;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000036C8 File Offset: 0x000018C8
		public SizeF(PointF pt)
		{
			this.width = pt.X;
			this.height = pt.Y;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000036EB File Offset: 0x000018EB
		public SizeF(float width, float height)
		{
			this.width = width;
			this.height = height;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003704 File Offset: 0x00001904
		public static SizeF operator +(SizeF sz1, SizeF sz2)
		{
			return SizeF.Add(sz1, sz2);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003720 File Offset: 0x00001920
		public static SizeF operator -(SizeF sz1, SizeF sz2)
		{
			return SizeF.Subtract(sz1, sz2);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000373C File Offset: 0x0000193C
		public static explicit operator PointF(SizeF size)
		{
			return new PointF(size.Width, size.Height);
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003760 File Offset: 0x00001960
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003790 File Offset: 0x00001990
		// (set) Token: 0x060000BE RID: 190 RVA: 0x000037A8 File Offset: 0x000019A8
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000037B4 File Offset: 0x000019B4
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x000037CC File Offset: 0x000019CC
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

		// Token: 0x060000C1 RID: 193 RVA: 0x000037D8 File Offset: 0x000019D8
		public static SizeF Add(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000380C File Offset: 0x00001A0C
		public static SizeF Subtract(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003840 File Offset: 0x00001A40
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

		// Token: 0x060000C4 RID: 196 RVA: 0x0000389C File Offset: 0x00001A9C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000038B4 File Offset: 0x00001AB4
		public PointF ToPointF()
		{
			return (PointF)this;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000038CC File Offset: 0x00001ACC
		public Size ToSize()
		{
			return new Size((int)this.width, (int)this.height);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000038F4 File Offset: 0x00001AF4
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

		// Token: 0x0400005E RID: 94
		public static readonly SizeF Empty = new SizeF();

		// Token: 0x0400005F RID: 95
		private float width;

		// Token: 0x04000060 RID: 96
		private float height;

		// Token: 0x04000061 RID: 97
		public static readonly SizeF Zero = new SizeF(0f, 0f);
	}
}

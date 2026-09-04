using System;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	// Token: 0x02000002 RID: 2
	[JsonObject(MemberSerialization.OptIn)]
	public class BlendFuncValue : ICloneable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public BlendFuncValue()
		{
			this.BlendSrc = BlendSrc.GL_ONE;
			this.BlendDst = BlendDst.GL_ONE_MINUS_SRC_ALPHA;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206F File Offset: 0x0000026F
		public BlendFuncValue(BlendSrc src, BlendDst dst)
		{
			this.BlendSrc = src;
			this.BlendDst = dst;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000208C File Offset: 0x0000028C
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020A3 File Offset: 0x000002A3
		public BlendSrc BlendSrc { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020AC File Offset: 0x000002AC
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020C3 File Offset: 0x000002C3
		public BlendDst BlendDst { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020CC File Offset: 0x000002CC
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000020E4 File Offset: 0x000002E4
		[ItemProperty]
		[JsonProperty]
		private int Src
		{
			get
			{
				return (int)this.BlendSrc;
			}
			set
			{
				this.BlendSrc = (BlendSrc)value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020F0 File Offset: 0x000002F0
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002108 File Offset: 0x00000308
		[JsonProperty]
		[ItemProperty]
		private int Dst
		{
			get
			{
				return (int)this.BlendDst;
			}
			set
			{
				this.BlendDst = (BlendDst)value;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002114 File Offset: 0x00000314
		public object Clone()
		{
			return new BlendFuncValue(this.BlendSrc, this.BlendDst);
		}

		// Token: 0x04000001 RID: 1
		public static readonly BlendFuncValue ADDITIVE = new BlendFuncValue(BlendSrc.GL_SRC_ALPHA, BlendDst.GL_ONE);

		// Token: 0x04000002 RID: 2
		public static readonly BlendFuncValue ALPHA_PREMULTIPLIED = new BlendFuncValue(BlendSrc.GL_ONE, BlendDst.GL_ONE_MINUS_SRC_ALPHA);

		// Token: 0x04000003 RID: 3
		public static readonly BlendFuncValue Default = new BlendFuncValue(BlendSrc.GL_ONE, BlendDst.GL_ONE_MINUS_SRC_ALPHA);
	}
}

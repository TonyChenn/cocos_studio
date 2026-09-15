using System;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	public class BlendFuncValue : ICloneable
	{
		public BlendFuncValue()
		{
			this.BlendSrc = BlendSrc.GL_ONE;
			this.BlendDst = BlendDst.GL_ONE_MINUS_SRC_ALPHA;
		}

		public BlendFuncValue(BlendSrc src, BlendDst dst)
		{
			this.BlendSrc = src;
			this.BlendDst = dst;
		}

		public BlendSrc BlendSrc { get; private set; }

		public BlendDst BlendDst { get; private set; }

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

		public object Clone()
		{
			return new BlendFuncValue(this.BlendSrc, this.BlendDst);
		}

		public static readonly BlendFuncValue ADDITIVE = new BlendFuncValue(BlendSrc.GL_SRC_ALPHA, BlendDst.GL_ONE);

		public static readonly BlendFuncValue ALPHA_PREMULTIPLIED = new BlendFuncValue(BlendSrc.GL_ONE, BlendDst.GL_ONE_MINUS_SRC_ALPHA);

		public static readonly BlendFuncValue Default = new BlendFuncValue(BlendSrc.GL_ONE, BlendDst.GL_ONE_MINUS_SRC_ALPHA);
	}
}

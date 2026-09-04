using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the result of resolving an expression.
	/// </summary>
	// Token: 0x02000033 RID: 51
	public class ResolveResult
	{
		// Token: 0x06000191 RID: 401 RVA: 0x000059DD File Offset: 0x000049DD
		public ResolveResult(IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.type = type;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000059FA File Offset: 0x000049FA
		public IType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00005A02 File Offset: 0x00004A02
		public virtual bool IsCompileTimeConstant
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00005A05 File Offset: 0x00004A05
		public virtual object ConstantValue
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00005A08 File Offset: 0x00004A08
		public virtual bool IsError
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00005A0C File Offset: 0x00004A0C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[",
				base.GetType().Name,
				" ",
				this.type,
				"]"
			});
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00005A55 File Offset: 0x00004A55
		public virtual IEnumerable<ResolveResult> GetChildResults()
		{
			return Enumerable.Empty<ResolveResult>();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00005A5C File Offset: 0x00004A5C
		public virtual DomRegion GetDefinitionRegion()
		{
			return DomRegion.Empty;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00005A63 File Offset: 0x00004A63
		public virtual ResolveResult ShallowClone()
		{
			return (ResolveResult)base.MemberwiseClone();
		}

		// Token: 0x0400005B RID: 91
		private readonly IType type;
	}
}

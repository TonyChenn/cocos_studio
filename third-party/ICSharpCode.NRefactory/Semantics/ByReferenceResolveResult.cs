using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the resolve result of an 'ref x' or 'out x' expression.
	/// </summary>
	// Token: 0x0200003A RID: 58
	public class ByReferenceResolveResult : ResolveResult
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00005D88 File Offset: 0x00004D88
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00005D90 File Offset: 0x00004D90
		public bool IsOut { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00005D99 File Offset: 0x00004D99
		public bool IsRef
		{
			get
			{
				return !this.IsOut;
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00005DA4 File Offset: 0x00004DA4
		public ByReferenceResolveResult(ResolveResult elementResult, bool isOut) : this(elementResult.Type, isOut)
		{
			this.ElementResult = elementResult;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00005DBA File Offset: 0x00004DBA
		public ByReferenceResolveResult(IType elementType, bool isOut) : base(new ByReferenceType(elementType))
		{
			this.IsOut = isOut;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00005DCF File Offset: 0x00004DCF
		public IType ElementType
		{
			get
			{
				return ((ByReferenceType)base.Type).ElementType;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005DE4 File Offset: 0x00004DE4
		public override IEnumerable<ResolveResult> GetChildResults()
		{
			if (this.ElementResult != null)
			{
				return new ResolveResult[]
				{
					this.ElementResult
				};
			}
			return Enumerable.Empty<ResolveResult>();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005E10 File Offset: 0x00004E10
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1} {2}]", new object[]
			{
				base.GetType().Name,
				this.IsOut ? "out" : "ref",
				this.ElementType
			});
		}

		// Token: 0x04000065 RID: 101
		public readonly ResolveResult ElementResult;
	}
}

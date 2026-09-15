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
	public class ByReferenceResolveResult : ResolveResult
	{
		public bool IsOut { get; private set; }

		public bool IsRef
		{
			get
			{
				return !this.IsOut;
			}
		}

		public ByReferenceResolveResult(ResolveResult elementResult, bool isOut) : this(elementResult.Type, isOut)
		{
			this.ElementResult = elementResult;
		}

		public ByReferenceResolveResult(IType elementType, bool isOut) : base(new ByReferenceType(elementType))
		{
			this.IsOut = isOut;
		}

		public IType ElementType
		{
			get
			{
				return ((ByReferenceType)base.Type).ElementType;
			}
		}

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

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1} {2}]", new object[]
			{
				base.GetType().Name,
				this.IsOut ? "out" : "ref",
				this.ElementType
			});
		}

		public readonly ResolveResult ElementResult;
	}
}

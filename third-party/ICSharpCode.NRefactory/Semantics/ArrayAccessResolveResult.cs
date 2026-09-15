using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Resolve result representing an array access.
	/// </summary>
	public class ArrayAccessResolveResult : ResolveResult
	{
		public ArrayAccessResolveResult(IType elementType, ResolveResult array, IList<ResolveResult> indexes) : base(elementType)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (indexes == null)
			{
				throw new ArgumentNullException("indexes");
			}
			this.Array = array;
			this.Indexes = indexes;
		}

		public override IEnumerable<ResolveResult> GetChildResults()
		{
			return new ResolveResult[]
			{
				this.Array
			}.Concat(this.Indexes);
		}

		public readonly ResolveResult Array;

		public readonly IList<ResolveResult> Indexes;
	}
}

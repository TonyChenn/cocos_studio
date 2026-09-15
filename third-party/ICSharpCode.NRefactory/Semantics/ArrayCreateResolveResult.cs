using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Resolve result representing an array creation.
	/// </summary>
	public class ArrayCreateResolveResult : ResolveResult
	{
		public ArrayCreateResolveResult(IType arrayType, IList<ResolveResult> sizeArguments, IList<ResolveResult> initializerElements) : base(arrayType)
		{
			if (sizeArguments == null)
			{
				throw new ArgumentNullException("sizeArguments");
			}
			this.SizeArguments = sizeArguments;
			this.InitializerElements = initializerElements;
		}

		public override IEnumerable<ResolveResult> GetChildResults()
		{
			if (this.InitializerElements != null)
			{
				return this.SizeArguments.Concat(this.InitializerElements);
			}
			return this.SizeArguments;
		}

		/// <summary>
		/// Gets the size arguments.
		/// </summary>
		public readonly IList<ResolveResult> SizeArguments;

		/// <summary>
		/// Gets the initializer elements.
		/// This field may be null if no initializer was specified.
		/// </summary>
		public readonly IList<ResolveResult> InitializerElements;
	}
}

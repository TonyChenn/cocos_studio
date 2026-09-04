using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Resolve result representing an array creation.
	/// </summary>
	// Token: 0x02000039 RID: 57
	public class ArrayCreateResolveResult : ResolveResult
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x00005D41 File Offset: 0x00004D41
		public ArrayCreateResolveResult(IType arrayType, IList<ResolveResult> sizeArguments, IList<ResolveResult> initializerElements) : base(arrayType)
		{
			if (sizeArguments == null)
			{
				throw new ArgumentNullException("sizeArguments");
			}
			this.SizeArguments = sizeArguments;
			this.InitializerElements = initializerElements;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00005D66 File Offset: 0x00004D66
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
		// Token: 0x04000063 RID: 99
		public readonly IList<ResolveResult> SizeArguments;

		/// <summary>
		/// Gets the initializer elements.
		/// This field may be null if no initializer was specified.
		/// </summary>
		// Token: 0x04000064 RID: 100
		public readonly IList<ResolveResult> InitializerElements;
	}
}

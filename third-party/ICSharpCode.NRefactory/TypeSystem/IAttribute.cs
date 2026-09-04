using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an attribute.
	/// </summary>
	// Token: 0x0200007C RID: 124
	public interface IAttribute
	{
		/// <summary>
		/// Gets the code region of this attribute.
		/// </summary>
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060003F1 RID: 1009
		DomRegion Region { get; }

		/// <summary>
		/// Gets the type of the attribute.
		/// </summary>
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060003F2 RID: 1010
		IType AttributeType { get; }

		/// <summary>
		/// Gets the constructor being used.
		/// This property may return null if no matching constructor was found.
		/// </summary>
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060003F3 RID: 1011
		IMethod Constructor { get; }

		/// <summary>
		/// Gets the positional arguments.
		/// </summary>
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060003F4 RID: 1012
		IList<ResolveResult> PositionalArguments { get; }

		/// <summary>
		/// Gets the named arguments passed to the attribute.
		/// </summary>
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060003F5 RID: 1013
		IList<KeyValuePair<IMember, ResolveResult>> NamedArguments { get; }
	}
}

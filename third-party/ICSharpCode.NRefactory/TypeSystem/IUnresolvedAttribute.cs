using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an unresolved attribute.
	/// </summary>
	// Token: 0x0200007E RID: 126
	public interface IUnresolvedAttribute
	{
		/// <summary>
		/// Gets the code region of this attribute.
		/// </summary>
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000401 RID: 1025
		DomRegion Region { get; }

		/// <summary>
		/// Resolves the attribute.
		/// </summary>
		// Token: 0x06000402 RID: 1026
		IAttribute CreateResolvedAttribute(ITypeResolveContext context);
	}
}

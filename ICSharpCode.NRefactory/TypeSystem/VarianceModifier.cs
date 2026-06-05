using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents the variance of a type parameter.
	/// </summary>
	// Token: 0x020000EE RID: 238
	public enum VarianceModifier : byte
	{
		/// <summary>
		/// The type parameter is not variant.
		/// </summary>
		// Token: 0x0400027E RID: 638
		Invariant,
		/// <summary>
		/// The type parameter is covariant (used in output position).
		/// </summary>
		// Token: 0x0400027F RID: 639
		Covariant,
		/// <summary>
		/// The type parameter is contravariant (used in input position).
		/// </summary>
		// Token: 0x04000280 RID: 640
		Contravariant
	}
}

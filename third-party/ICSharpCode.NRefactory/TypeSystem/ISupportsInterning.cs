using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Interface for TypeSystem objects that support interning.
	/// See <see cref="T:ICSharpCode.NRefactory.TypeSystem.InterningProvider" /> for more information.
	/// </summary>
	// Token: 0x0200006B RID: 107
	public interface ISupportsInterning
	{
		/// <summary>
		/// Gets a hash code for interning.
		/// </summary>
		// Token: 0x06000379 RID: 889
		int GetHashCodeForInterning();

		/// <summary>
		/// Equality test for interning.
		/// </summary>
		// Token: 0x0600037A RID: 890
		bool EqualsForInterning(ISupportsInterning other);
	}
}

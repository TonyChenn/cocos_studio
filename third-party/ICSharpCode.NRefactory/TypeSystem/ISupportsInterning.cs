using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Interface for TypeSystem objects that support interning.
	/// See <see cref="T:ICSharpCode.NRefactory.TypeSystem.InterningProvider" /> for more information.
	/// </summary>
	public interface ISupportsInterning
	{
		/// <summary>
		/// Gets a hash code for interning.
		/// </summary>
		int GetHashCodeForInterning();

		/// <summary>
		/// Equality test for interning.
		/// </summary>
		bool EqualsForInterning(ISupportsInterning other);
	}
}

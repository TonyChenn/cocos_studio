using System;

namespace ICSharpCode.NRefactory.Analysis
{
	/// <summary>
	/// Used to check the compatibility state of two compilations.
	/// </summary>
	public enum AbiCompatibility
	{
		/// <summary>
		/// The ABI is equal
		/// </summary>
		Equal,
		/// <summary>
		/// Some items got added, but the ABI remains to be compatible
		/// </summary>
		Bigger,
		/// <summary>
		/// The ABI has changed
		/// </summary>
		Incompatible
	}
}

using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Composite format string parser.
	/// </summary>
	/// <remarks>
	/// Implements a complete parser for valid strings as well as
	/// error reporting and best-effort parsing for invalid strings.
	/// </remarks>		
	public interface IFormatStringSegment
	{
		int StartLocation { get; set; }

		int EndLocation { get; set; }

		bool HasErrors { get; }

		IEnumerable<IFormatStringError> Errors { get; }
	}
}

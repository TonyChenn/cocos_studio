using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Composite format string parser.
	/// </summary>
	/// <remarks>
	/// Implements a complete parser for valid strings as well as
	/// error reporting and best-effort parsing for invalid strings.
	/// </remarks>		
	public abstract class FormatStringSegmentBase : IFormatStringSegment
	{
		public FormatStringSegmentBase()
		{
			this.Errors = new List<IFormatStringError>();
		}

		public int StartLocation { get; set; }

		public int EndLocation { get; set; }

		public bool HasErrors
		{
			get
			{
				return this.Errors.Any<IFormatStringError>();
			}
		}

		public IList<IFormatStringError> Errors { get; set; }

		IEnumerable<IFormatStringError> IFormatStringSegment.Errors
		{
			get
			{
				return this.Errors;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.Utils
{
	public class FormatStringParseResult
	{
		public FormatStringParseResult()
		{
			this.Segments = new List<IFormatStringSegment>();
		}

		public IList<IFormatStringSegment> Segments { get; private set; }

		public bool HasErrors
		{
			get
			{
				return this.Segments.SelectMany((IFormatStringSegment segment) => segment.Errors).Any<IFormatStringError>();
			}
		}
	}
}

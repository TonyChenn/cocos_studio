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
	// Token: 0x02000133 RID: 307
	public interface IFormatStringSegment
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000AAC RID: 2732
		// (set) Token: 0x06000AAD RID: 2733
		int StartLocation { get; set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000AAE RID: 2734
		// (set) Token: 0x06000AAF RID: 2735
		int EndLocation { get; set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000AB0 RID: 2736
		bool HasErrors { get; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000AB1 RID: 2737
		IEnumerable<IFormatStringError> Errors { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x02000132 RID: 306
	public class FormatStringParseResult
	{
		// Token: 0x06000AA7 RID: 2727 RVA: 0x00020263 File Offset: 0x0001F263
		public FormatStringParseResult()
		{
			this.Segments = new List<IFormatStringSegment>();
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00020276 File Offset: 0x0001F276
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x0002027E File Offset: 0x0001F27E
		public IList<IFormatStringSegment> Segments { get; private set; }

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0002028F File Offset: 0x0001F28F
		public bool HasErrors
		{
			get
			{
				return this.Segments.SelectMany((IFormatStringSegment segment) => segment.Errors).Any<IFormatStringError>();
			}
		}
	}
}

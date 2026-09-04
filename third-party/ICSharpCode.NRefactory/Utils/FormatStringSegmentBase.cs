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
	// Token: 0x02000134 RID: 308
	public abstract class FormatStringSegmentBase : IFormatStringSegment
	{
		// Token: 0x06000AB2 RID: 2738 RVA: 0x000202BE File Offset: 0x0001F2BE
		public FormatStringSegmentBase()
		{
			this.Errors = new List<IFormatStringError>();
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x000202D1 File Offset: 0x0001F2D1
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x000202D9 File Offset: 0x0001F2D9
		public int StartLocation { get; set; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x000202E2 File Offset: 0x0001F2E2
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x000202EA File Offset: 0x0001F2EA
		public int EndLocation { get; set; }

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x000202F3 File Offset: 0x0001F2F3
		public bool HasErrors
		{
			get
			{
				return this.Errors.Any<IFormatStringError>();
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00020300 File Offset: 0x0001F300
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x00020308 File Offset: 0x0001F308
		public IList<IFormatStringError> Errors { get; set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00020311 File Offset: 0x0001F311
		IEnumerable<IFormatStringError> IFormatStringSegment.Errors
		{
			get
			{
				return this.Errors;
			}
		}
	}
}

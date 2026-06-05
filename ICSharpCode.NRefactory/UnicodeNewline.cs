using System;

namespace ICSharpCode.NRefactory
{
	// Token: 0x0200013F RID: 319
	public enum UnicodeNewline
	{
		// Token: 0x040003C0 RID: 960
		Unknown,
		/// <summary>
		/// Line Feed, U+000A
		/// </summary>
		// Token: 0x040003C1 RID: 961
		LF = 10,
		// Token: 0x040003C2 RID: 962
		CRLF = 3338,
		/// <summary>
		/// Carriage Return, U+000D
		/// </summary>
		// Token: 0x040003C3 RID: 963
		CR = 13,
		/// <summary>
		/// Next Line, U+0085
		/// </summary>
		// Token: 0x040003C4 RID: 964
		NEL = 133,
		/// <summary>
		/// Vertical Tab, U+000B
		/// </summary>
		// Token: 0x040003C5 RID: 965
		VT = 11,
		/// <summary>
		/// Form Feed, U+000C
		/// </summary>
		// Token: 0x040003C6 RID: 966
		FF,
		/// <summary>
		/// Line Separator, U+2028
		/// </summary>
		// Token: 0x040003C7 RID: 967
		LS = 8232,
		/// <summary>
		/// Paragraph Separator, U+2029
		/// </summary>
		// Token: 0x040003C8 RID: 968
		PS
	}
}

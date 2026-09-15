using System;

namespace ICSharpCode.NRefactory
{
	public enum UnicodeNewline
	{
		Unknown,
		/// <summary>
		/// Line Feed, U+000A
		/// </summary>
		LF = 10,
		CRLF = 3338,
		/// <summary>
		/// Carriage Return, U+000D
		/// </summary>
		CR = 13,
		/// <summary>
		/// Next Line, U+0085
		/// </summary>
		NEL = 133,
		/// <summary>
		/// Vertical Tab, U+000B
		/// </summary>
		VT = 11,
		/// <summary>
		/// Form Feed, U+000C
		/// </summary>
		FF,
		/// <summary>
		/// Line Separator, U+2028
		/// </summary>
		LS = 8232,
		/// <summary>
		/// Paragraph Separator, U+2029
		/// </summary>
		PS
	}
}

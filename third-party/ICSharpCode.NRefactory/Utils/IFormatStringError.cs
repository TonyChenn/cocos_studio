using System;

namespace ICSharpCode.NRefactory.Utils
{
	public interface IFormatStringError
	{
		int StartLocation { get; }

		int EndLocation { get; }

		string Message { get; }

		string OriginalText { get; }

		string SuggestedReplacementText { get; }
	}
}

using System;

namespace ICSharpCode.NRefactory.Completion
{
	[Flags]
	public enum DisplayFlags
	{
		None = 0,
		Hidden = 1,
		Obsolete = 2,
		DescriptionHasMarkup = 4,
		NamedArgument = 8,
		IsImportCompletion = 16,
		MarkedBold = 32
	}
}

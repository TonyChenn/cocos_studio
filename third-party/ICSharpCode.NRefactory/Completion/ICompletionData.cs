using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Completion
{
	public interface ICompletionData
	{
		CompletionCategory CompletionCategory { get; set; }

		string DisplayText { get; set; }

		string Description { get; set; }

		string CompletionText { get; set; }

		DisplayFlags DisplayFlags { get; set; }

		bool HasOverloads { get; }

		IEnumerable<ICompletionData> OverloadedData { get; }

		void AddOverload(ICompletionData data);
	}
}

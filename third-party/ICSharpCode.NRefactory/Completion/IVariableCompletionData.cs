using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Completion
{
	public interface IVariableCompletionData : ICompletionData
	{
		IVariable Variable { get; }
	}
}

using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Completion
{
	public interface IEntityCompletionData : ICompletionData
	{
		IEntity Entity { get; }
	}
}

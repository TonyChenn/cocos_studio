using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Completion
{
	// Token: 0x0200012E RID: 302
	public interface IVariableCompletionData : ICompletionData
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000A87 RID: 2695
		IVariable Variable { get; }
	}
}

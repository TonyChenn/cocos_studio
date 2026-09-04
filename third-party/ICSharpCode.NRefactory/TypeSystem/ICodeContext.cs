using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000076 RID: 118
	public interface ICodeContext : ITypeResolveContext, ICompilationProvider
	{
		/// <summary>
		/// Gets all currently visible local variables and lambda parameters.
		/// Does not include method parameters.
		/// </summary>
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060003C8 RID: 968
		IEnumerable<IVariable> LocalVariables { get; }

		/// <summary>
		/// Gets whether the context is within a lambda expression or anonymous method.
		/// </summary>
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060003C9 RID: 969
		bool IsWithinLambdaExpression { get; }
	}
}

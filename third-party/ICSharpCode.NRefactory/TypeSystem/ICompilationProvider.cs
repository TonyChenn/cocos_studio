using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200005B RID: 91
	public interface ICompilationProvider
	{
		/// <summary>
		/// Gets the parent compilation.
		/// This property never returns null.
		/// </summary>
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002B8 RID: 696
		ICompilation Compilation { get; }
	}
}

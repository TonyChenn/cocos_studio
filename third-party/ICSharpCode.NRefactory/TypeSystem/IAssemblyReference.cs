using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200008D RID: 141
	public interface IAssemblyReference
	{
		/// <summary>
		/// Resolves this assembly.
		/// </summary>
		// Token: 0x06000489 RID: 1161
		IAssembly Resolve(ITypeResolveContext context);
	}
}

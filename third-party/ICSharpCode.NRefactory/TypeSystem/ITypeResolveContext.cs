using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000075 RID: 117
	public interface ITypeResolveContext : ICompilationProvider
	{
		/// <summary>
		/// Gets the current assembly.
		/// This property may return null if this context does not specify any assembly.
		/// </summary>
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060003C3 RID: 963
		IAssembly CurrentAssembly { get; }

		/// <summary>
		/// Gets the current type definition.
		/// </summary>
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060003C4 RID: 964
		ITypeDefinition CurrentTypeDefinition { get; }

		/// <summary>
		/// Gets the current member.
		/// </summary>
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060003C5 RID: 965
		IMember CurrentMember { get; }

		// Token: 0x060003C6 RID: 966
		ITypeResolveContext WithCurrentTypeDefinition(ITypeDefinition typeDefinition);

		// Token: 0x060003C7 RID: 967
		ITypeResolveContext WithCurrentMember(IMember member);
	}
}

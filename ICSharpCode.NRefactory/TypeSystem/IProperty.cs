using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a property or indexer.
	/// </summary>
	// Token: 0x02000061 RID: 97
	public interface IProperty : IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000306 RID: 774
		bool CanGet { get; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000307 RID: 775
		bool CanSet { get; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000308 RID: 776
		IMethod Getter { get; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000309 RID: 777
		IMethod Setter { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600030A RID: 778
		bool IsIndexer { get; }
	}
}

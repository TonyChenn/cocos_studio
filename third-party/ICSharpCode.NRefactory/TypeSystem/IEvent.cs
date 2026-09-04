using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000096 RID: 150
	public interface IEvent : IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060004C1 RID: 1217
		bool CanAdd { get; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060004C2 RID: 1218
		bool CanRemove { get; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060004C3 RID: 1219
		bool CanInvoke { get; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060004C4 RID: 1220
		IMethod AddAccessor { get; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060004C5 RID: 1221
		IMethod RemoveAccessor { get; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060004C6 RID: 1222
		IMethod InvokeAccessor { get; }
	}
}

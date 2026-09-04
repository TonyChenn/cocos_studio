using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000095 RID: 149
	public interface IUnresolvedEvent : IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060004BA RID: 1210
		bool CanAdd { get; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060004BB RID: 1211
		bool CanRemove { get; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060004BC RID: 1212
		bool CanInvoke { get; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060004BD RID: 1213
		IUnresolvedMethod AddAccessor { get; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060004BE RID: 1214
		IUnresolvedMethod RemoveAccessor { get; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060004BF RID: 1215
		IUnresolvedMethod InvokeAccessor { get; }

		/// <summary>
		/// Resolves the member.
		/// </summary>
		/// <param name="context">
		/// Context for looking up the member. The context must specify the current assembly.
		/// A <see cref="T:ICSharpCode.NRefactory.TypeSystem.SimpleTypeResolveContext" /> that specifies the current assembly is sufficient.
		/// </param>
		/// <returns>
		/// Returns the resolved member, or <c>null</c> if the member could not be found.
		/// </returns>
		// Token: 0x060004C0 RID: 1216
		IEvent Resolve(ITypeResolveContext context);
	}
}

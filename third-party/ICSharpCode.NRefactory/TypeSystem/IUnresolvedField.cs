using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a field or constant.
	/// </summary>
	// Token: 0x02000097 RID: 151
	public interface IUnresolvedField : IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		/// <summary>
		/// Gets whether this field is readonly.
		/// </summary>
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060004C7 RID: 1223
		bool IsReadOnly { get; }

		/// <summary>
		/// Gets whether this field is volatile.
		/// </summary>
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060004C8 RID: 1224
		bool IsVolatile { get; }

		/// <summary>
		/// Gets whether this field is a constant (C#-like const).
		/// </summary>
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060004C9 RID: 1225
		bool IsConst { get; }

		/// <summary>
		/// Gets whether this field is a fixed size buffer (C#-like fixed).
		/// If this is true, then ConstantValue contains the size of the buffer.
		/// </summary>
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060004CA RID: 1226
		bool IsFixed { get; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060004CB RID: 1227
		IConstantValue ConstantValue { get; }

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
		// Token: 0x060004CC RID: 1228
		IField Resolve(ITypeResolveContext context);
	}
}

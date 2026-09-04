using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a field or constant.
	/// </summary>
	// Token: 0x02000098 RID: 152
	public interface IField : IMember, IEntity, ICompilationProvider, INamedElement, IHasAccessibility, IVariable, ISymbol
	{
		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060004CD RID: 1229
		string Name { get; }

		/// <summary>
		/// Gets the region where the field is declared.
		/// </summary>
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060004CE RID: 1230
		DomRegion Region { get; }

		/// <summary>
		/// Gets whether this field is readonly.
		/// </summary>
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060004CF RID: 1231
		bool IsReadOnly { get; }

		/// <summary>
		/// Gets whether this field is volatile.
		/// </summary>
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060004D0 RID: 1232
		bool IsVolatile { get; }

		/// <summary>
		/// Gets whether this field is a fixed size buffer (C#-like fixed).
		/// If this is true, then ConstantValue contains the size of the buffer.
		/// </summary>
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060004D1 RID: 1233
		bool IsFixed { get; }

		// Token: 0x060004D2 RID: 1234
		IMemberReference ToReference();
	}
}

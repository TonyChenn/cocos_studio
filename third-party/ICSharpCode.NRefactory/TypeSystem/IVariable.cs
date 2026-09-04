using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a variable (name/type pair).
	/// </summary>
	// Token: 0x02000078 RID: 120
	public interface IVariable : ISymbol
	{
		/// <summary>
		/// Gets the name of the variable.
		/// </summary>
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060003E0 RID: 992
		string Name { get; }

		/// <summary>
		/// Gets the declaration region of the variable.
		/// </summary>
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060003E1 RID: 993
		DomRegion Region { get; }

		/// <summary>
		/// Gets the type of the variable.
		/// </summary>
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060003E2 RID: 994
		IType Type { get; }

		/// <summary>
		/// Gets whether this variable is a constant (C#-like const).
		/// </summary>
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060003E3 RID: 995
		bool IsConst { get; }

		/// <summary>
		/// If this field is a constant, retrieves the value.
		/// For parameters, this is the default value.
		/// </summary>
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060003E4 RID: 996
		object ConstantValue { get; }
	}
}

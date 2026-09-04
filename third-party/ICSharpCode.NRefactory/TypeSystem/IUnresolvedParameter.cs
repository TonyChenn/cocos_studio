using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x020000C4 RID: 196
	public interface IUnresolvedParameter
	{
		/// <summary>
		/// Gets the name of the variable.
		/// </summary>
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060006F1 RID: 1777
		string Name { get; }

		/// <summary>
		/// Gets the declaration region of the variable.
		/// </summary>
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060006F2 RID: 1778
		DomRegion Region { get; }

		/// <summary>
		/// Gets the type of the variable.
		/// </summary>
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060006F3 RID: 1779
		ITypeReference Type { get; }

		/// <summary>
		/// Gets the list of attributes.
		/// </summary>
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060006F4 RID: 1780
		IList<IUnresolvedAttribute> Attributes { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'ref' parameter.
		/// </summary>
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060006F5 RID: 1781
		bool IsRef { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'out' parameter.
		/// </summary>
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060006F6 RID: 1782
		bool IsOut { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'params' parameter.
		/// </summary>
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060006F7 RID: 1783
		bool IsParams { get; }

		/// <summary>
		/// Gets whether this parameter is optional.
		/// </summary>
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060006F8 RID: 1784
		bool IsOptional { get; }

		// Token: 0x060006F9 RID: 1785
		IParameter CreateResolvedParameter(ITypeResolveContext context);
	}
}

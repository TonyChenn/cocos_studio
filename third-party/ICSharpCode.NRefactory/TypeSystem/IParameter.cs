using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x020000AC RID: 172
	public interface IParameter : IVariable, ISymbol
	{
		/// <summary>
		/// Gets the list of attributes.
		/// </summary>
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060005A2 RID: 1442
		IList<IAttribute> Attributes { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'ref' parameter.
		/// </summary>
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060005A3 RID: 1443
		bool IsRef { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'out' parameter.
		/// </summary>
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060005A4 RID: 1444
		bool IsOut { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'params' parameter.
		/// </summary>
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060005A5 RID: 1445
		bool IsParams { get; }

		/// <summary>
		/// Gets whether this parameter is optional.
		/// The default value is given by the <see cref="P:ICSharpCode.NRefactory.TypeSystem.IVariable.ConstantValue" /> property.
		/// </summary>
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060005A6 RID: 1446
		bool IsOptional { get; }

		/// <summary>
		/// Gets the owner of this parameter.
		/// May return null; for example when parameters belong to lambdas or anonymous methods.
		/// </summary>
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060005A7 RID: 1447
		IParameterizedMember Owner { get; }
	}
}

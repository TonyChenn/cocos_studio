using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an unknown method.
	/// </summary>
	// Token: 0x02000050 RID: 80
	public class UnknownMethodResolveResult : UnknownMemberResolveResult
	{
		// Token: 0x06000248 RID: 584 RVA: 0x00006AE8 File Offset: 0x00005AE8
		public UnknownMethodResolveResult(IType targetType, string methodName, IEnumerable<IType> typeArguments, IEnumerable<IParameter> parameters) : base(targetType, methodName, typeArguments)
		{
			this.parameters = new ReadOnlyCollection<IParameter>(parameters.ToArray<IParameter>());
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000249 RID: 585 RVA: 0x00006B05 File Offset: 0x00005B05
		public ReadOnlyCollection<IParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x040000AC RID: 172
		private readonly ReadOnlyCollection<IParameter> parameters;
	}
}

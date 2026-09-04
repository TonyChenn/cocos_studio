using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Resolve result for a C# 'is' expression.
	/// "Input is TargetType".
	/// </summary>
	// Token: 0x0200004D RID: 77
	public class TypeIsResolveResult : ResolveResult
	{
		// Token: 0x0600023F RID: 575 RVA: 0x000069F6 File Offset: 0x000059F6
		public TypeIsResolveResult(ResolveResult input, IType targetType, IType booleanType) : base(booleanType)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			this.Input = input;
			this.TargetType = targetType;
		}

		// Token: 0x040000A6 RID: 166
		public readonly ResolveResult Input;

		/// <summary>
		/// Type that is being compared with.
		/// </summary>
		// Token: 0x040000A7 RID: 167
		public readonly IType TargetType;
	}
}

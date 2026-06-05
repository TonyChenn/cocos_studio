using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents a unary/binary/ternary operator invocation.
	/// </summary>
	// Token: 0x0200004B RID: 75
	public class OperatorResolveResult : ResolveResult
	{
		// Token: 0x06000236 RID: 566 RVA: 0x0000695B File Offset: 0x0000595B
		public OperatorResolveResult(IType resultType, ExpressionType operatorType, params ResolveResult[] operands) : base(resultType)
		{
			if (operands == null)
			{
				throw new ArgumentNullException("operands");
			}
			this.operatorType = operatorType;
			this.operands = operands;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00006980 File Offset: 0x00005980
		public OperatorResolveResult(IType resultType, ExpressionType operatorType, IMethod userDefinedOperatorMethod, bool isLiftedOperator, IList<ResolveResult> operands) : base(resultType)
		{
			if (operands == null)
			{
				throw new ArgumentNullException("operands");
			}
			this.operatorType = operatorType;
			this.userDefinedOperatorMethod = userDefinedOperatorMethod;
			this.isLiftedOperator = isLiftedOperator;
			this.operands = operands;
		}

		/// <summary>
		/// Gets the operator type.
		/// </summary>
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000238 RID: 568 RVA: 0x000069B6 File Offset: 0x000059B6
		public ExpressionType OperatorType
		{
			get
			{
				return this.operatorType;
			}
		}

		/// <summary>
		/// Gets the operands.
		/// </summary>
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000239 RID: 569 RVA: 0x000069BE File Offset: 0x000059BE
		public IList<ResolveResult> Operands
		{
			get
			{
				return this.operands;
			}
		}

		/// <summary>
		/// Gets the user defined operator method.
		/// Returns null if this is a predefined operator.
		/// </summary>
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000069C6 File Offset: 0x000059C6
		public IMethod UserDefinedOperatorMethod
		{
			get
			{
				return this.userDefinedOperatorMethod;
			}
		}

		/// <summary>
		/// Gets whether this is a lifted operator.
		/// </summary>
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600023B RID: 571 RVA: 0x000069CE File Offset: 0x000059CE
		public bool IsLiftedOperator
		{
			get
			{
				return this.isLiftedOperator;
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000069D6 File Offset: 0x000059D6
		public override IEnumerable<ResolveResult> GetChildResults()
		{
			return this.operands;
		}

		// Token: 0x040000A1 RID: 161
		private readonly ExpressionType operatorType;

		// Token: 0x040000A2 RID: 162
		private readonly IMethod userDefinedOperatorMethod;

		// Token: 0x040000A3 RID: 163
		private readonly IList<ResolveResult> operands;

		// Token: 0x040000A4 RID: 164
		private readonly bool isLiftedOperator;
	}
}

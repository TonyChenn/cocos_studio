using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the result of a method, constructor or indexer invocation.
	/// </summary>
	// Token: 0x02000047 RID: 71
	public class InvocationResolveResult : MemberResolveResult
	{
		// Token: 0x06000223 RID: 547 RVA: 0x00006707 File Offset: 0x00005707
		public InvocationResolveResult(ResolveResult targetResult, IParameterizedMember member, IList<ResolveResult> arguments = null, IList<ResolveResult> initializerStatements = null, IType returnTypeOverride = null) : base(targetResult, member, returnTypeOverride)
		{
			this.Arguments = (arguments ?? EmptyList<ResolveResult>.Instance);
			this.InitializerStatements = (initializerStatements ?? EmptyList<ResolveResult>.Instance);
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00006734 File Offset: 0x00005734
		public new IParameterizedMember Member
		{
			get
			{
				return (IParameterizedMember)base.Member;
			}
		}

		/// <summary>
		/// Gets the arguments in the order they are being passed to the method.
		/// For parameter arrays (params), this will return an ArrayCreateResolveResult.
		/// </summary>
		// Token: 0x06000225 RID: 549 RVA: 0x00006741 File Offset: 0x00005741
		public virtual IList<ResolveResult> GetArgumentsForCall()
		{
			return this.Arguments;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00006749 File Offset: 0x00005749
		public override IEnumerable<ResolveResult> GetChildResults()
		{
			return base.GetChildResults().Concat(this.Arguments).Concat(this.InitializerStatements);
		}

		/// <summary>
		/// Gets the arguments that are being passed to the method, in the order the arguments are being evaluated.
		/// </summary>
		// Token: 0x04000099 RID: 153
		public readonly IList<ResolveResult> Arguments;

		/// <summary>
		/// Gets the list of initializer statements that are appplied to the result of this invocation.
		/// This is used to represent object and collection initializers.
		/// With the initializer statements, the <see cref="T:ICSharpCode.NRefactory.Semantics.InitializedObjectResolveResult" /> is used
		/// to refer to the result of this invocation.
		/// </summary>
		// Token: 0x0400009A RID: 154
		public readonly IList<ResolveResult> InitializerStatements;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the result of a method, constructor or indexer invocation.
	/// </summary>
	public class InvocationResolveResult : MemberResolveResult
	{
		public InvocationResolveResult(ResolveResult targetResult, IParameterizedMember member, IList<ResolveResult> arguments = null, IList<ResolveResult> initializerStatements = null, IType returnTypeOverride = null) : base(targetResult, member, returnTypeOverride)
		{
			this.Arguments = (arguments ?? EmptyList<ResolveResult>.Instance);
			this.InitializerStatements = (initializerStatements ?? EmptyList<ResolveResult>.Instance);
		}

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
		public virtual IList<ResolveResult> GetArgumentsForCall()
		{
			return this.Arguments;
		}

		public override IEnumerable<ResolveResult> GetChildResults()
		{
			return base.GetChildResults().Concat(this.Arguments).Concat(this.InitializerStatements);
		}

		/// <summary>
		/// Gets the arguments that are being passed to the method, in the order the arguments are being evaluated.
		/// </summary>
		public readonly IList<ResolveResult> Arguments;

		/// <summary>
		/// Gets the list of initializer statements that are appplied to the result of this invocation.
		/// This is used to represent object and collection initializers.
		/// With the initializer statements, the <see cref="T:ICSharpCode.NRefactory.Semantics.InitializedObjectResolveResult" /> is used
		/// to refer to the result of this invocation.
		/// </summary>
		public readonly IList<ResolveResult> InitializerStatements;
	}
}

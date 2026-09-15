using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an ambiguous field/property/event access.
	/// </summary>
	public class AmbiguousMemberResolveResult : MemberResolveResult
	{
		public AmbiguousMemberResolveResult(ResolveResult targetResult, IMember member) : base(targetResult, member, null)
		{
		}

		public override bool IsError
		{
			get
			{
				return true;
			}
		}
	}
}

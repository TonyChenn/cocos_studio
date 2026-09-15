using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an ambiguous type resolve result.
	/// </summary>
	public class AmbiguousTypeResolveResult : TypeResolveResult
	{
		public AmbiguousTypeResolveResult(IType type) : base(type)
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

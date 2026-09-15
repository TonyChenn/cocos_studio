using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the result of resolving an expression.
	/// </summary>
	public class ResolveResult
	{
		public ResolveResult(IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.type = type;
		}

		public IType Type
		{
			get
			{
				return this.type;
			}
		}

		public virtual bool IsCompileTimeConstant
		{
			get
			{
				return false;
			}
		}

		public virtual object ConstantValue
		{
			get
			{
				return null;
			}
		}

		public virtual bool IsError
		{
			get
			{
				return false;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[",
				base.GetType().Name,
				" ",
				this.type,
				"]"
			});
		}

		public virtual IEnumerable<ResolveResult> GetChildResults()
		{
			return Enumerable.Empty<ResolveResult>();
		}

		public virtual DomRegion GetDefinitionRegion()
		{
			return DomRegion.Empty;
		}

		public virtual ResolveResult ShallowClone()
		{
			return (ResolveResult)base.MemberwiseClone();
		}

		private readonly IType type;
	}
}

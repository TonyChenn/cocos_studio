using System;
using System.Globalization;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents that an expression resolved to a namespace.
	/// </summary>
	public class NamespaceResolveResult : ResolveResult
	{
		public NamespaceResolveResult(INamespace ns) : base(SpecialType.UnknownType)
		{
			this.ns = ns;
		}

		public INamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public string NamespaceName
		{
			get
			{
				return this.ns.FullName;
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1}]", new object[]
			{
				base.GetType().Name,
				this.ns
			});
		}

		private readonly INamespace ns;
	}
}

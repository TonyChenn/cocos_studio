using System;
using System.Globalization;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents that an expression resolved to a namespace.
	/// </summary>
	// Token: 0x0200004A RID: 74
	public class NamespaceResolveResult : ResolveResult
	{
		// Token: 0x06000232 RID: 562 RVA: 0x000068F6 File Offset: 0x000058F6
		public NamespaceResolveResult(INamespace ns) : base(SpecialType.UnknownType)
		{
			this.ns = ns;
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000690A File Offset: 0x0000590A
		public INamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00006912 File Offset: 0x00005912
		public string NamespaceName
		{
			get
			{
				return this.ns.FullName;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00006920 File Offset: 0x00005920
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1}]", new object[]
			{
				base.GetType().Name,
				this.ns
			});
		}

		// Token: 0x040000A0 RID: 160
		private readonly INamespace ns;
	}
}

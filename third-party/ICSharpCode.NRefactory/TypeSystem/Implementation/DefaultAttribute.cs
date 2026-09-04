using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// IAttribute implementation for already-resolved attributes.
	/// </summary>
	// Token: 0x020000AA RID: 170
	public class DefaultAttribute : IAttribute
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0000D3EC File Offset: 0x0000C3EC
		public DefaultAttribute(IType attributeType, IList<ResolveResult> positionalArguments = null, IList<KeyValuePair<IMember, ResolveResult>> namedArguments = null, DomRegion region = default(DomRegion))
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			this.attributeType = attributeType;
			this.positionalArguments = (positionalArguments ?? EmptyList<ResolveResult>.Instance);
			this.namedArguments = (namedArguments ?? EmptyList<KeyValuePair<IMember, ResolveResult>>.Instance);
			this.region = region;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0000D43C File Offset: 0x0000C43C
		public DefaultAttribute(IMethod constructor, IList<ResolveResult> positionalArguments = null, IList<KeyValuePair<IMember, ResolveResult>> namedArguments = null, DomRegion region = default(DomRegion))
		{
			if (constructor == null)
			{
				throw new ArgumentNullException("constructor");
			}
			this.constructor = constructor;
			this.attributeType = constructor.DeclaringType;
			this.positionalArguments = (positionalArguments ?? EmptyList<ResolveResult>.Instance);
			this.namedArguments = (namedArguments ?? EmptyList<KeyValuePair<IMember, ResolveResult>>.Instance);
			this.region = region;
			if (this.positionalArguments.Count != constructor.Parameters.Count)
			{
				throw new ArgumentException("Positional argument count must match the constructor's parameter count");
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0000D4BD File Offset: 0x0000C4BD
		public IType AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0000D4C5 File Offset: 0x0000C4C5
		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0000D4F8 File Offset: 0x0000C4F8
		public IMethod Constructor
		{
			get
			{
				IMethod method = this.constructor;
				if (method == null)
				{
					foreach (IMethod method2 in this.AttributeType.GetConstructors((IUnresolvedMethod m) => m.Parameters.Count == this.positionalArguments.Count, GetMemberOptions.IgnoreInheritedMembers))
					{
						if ((from p in method2.Parameters
						select p.Type).SequenceEqual(from a in this.PositionalArguments
						select a.Type))
						{
							method = method2;
							break;
						}
					}
					this.constructor = method;
				}
				return method;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0000D5CC File Offset: 0x0000C5CC
		public IList<ResolveResult> PositionalArguments
		{
			get
			{
				return this.positionalArguments;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0000D5D4 File Offset: 0x0000C5D4
		public IList<KeyValuePair<IMember, ResolveResult>> NamedArguments
		{
			get
			{
				return this.namedArguments;
			}
		}

		// Token: 0x04000185 RID: 389
		private readonly IType attributeType;

		// Token: 0x04000186 RID: 390
		private readonly IList<ResolveResult> positionalArguments;

		// Token: 0x04000187 RID: 391
		private readonly IList<KeyValuePair<IMember, ResolveResult>> namedArguments;

		// Token: 0x04000188 RID: 392
		private readonly DomRegion region;

		// Token: 0x04000189 RID: 393
		private volatile IMethod constructor;
	}
}

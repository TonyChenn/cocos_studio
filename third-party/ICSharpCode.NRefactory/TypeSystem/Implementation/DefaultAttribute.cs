using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// IAttribute implementation for already-resolved attributes.
	/// </summary>
	public class DefaultAttribute : IAttribute
	{
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

		public IType AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

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

		public IList<ResolveResult> PositionalArguments
		{
			get
			{
				return this.positionalArguments;
			}
		}

		public IList<KeyValuePair<IMember, ResolveResult>> NamedArguments
		{
			get
			{
				return this.namedArguments;
			}
		}

		private readonly IType attributeType;

		private readonly IList<ResolveResult> positionalArguments;

		private readonly IList<KeyValuePair<IMember, ResolveResult>> namedArguments;

		private readonly DomRegion region;

		private volatile IMethod constructor;
	}
}

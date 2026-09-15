using System;
using System.Collections.Generic;
using System.Globalization;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedTypeParameter" />.
	/// </summary>
	[Serializable]
	public class DefaultUnresolvedTypeParameter : IUnresolvedTypeParameter, INamedElement, IFreezable
	{
		public void Freeze()
		{
			if (!this.flags[1])
			{
				this.FreezeInternal();
				this.flags[1] = true;
			}
		}

		protected virtual void FreezeInternal()
		{
			this.attributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.attributes);
			this.constraints = FreezableHelper.FreezeList<ITypeReference>(this.constraints);
		}

		public DefaultUnresolvedTypeParameter(SymbolKind ownerType, int index, string name = null)
		{
			this.ownerType = ownerType;
			this.index = index;
			this.name = (name ?? (((ownerType == SymbolKind.Method) ? "!!" : "!") + index.ToString(CultureInfo.InvariantCulture)));
		}

		public SymbolKind OwnerType
		{
			get
			{
				return this.ownerType;
			}
		}

		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public bool IsFrozen
		{
			get
			{
				return this.flags[1];
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.name = value;
			}
		}

		string INamedElement.FullName
		{
			get
			{
				return this.name;
			}
		}

		string INamedElement.Namespace
		{
			get
			{
				return string.Empty;
			}
		}

		string INamedElement.ReflectionName
		{
			get
			{
				if (this.ownerType == SymbolKind.Method)
				{
					return "``" + this.index.ToString(CultureInfo.InvariantCulture);
				}
				return "`" + this.index.ToString(CultureInfo.InvariantCulture);
			}
		}

		public IList<IUnresolvedAttribute> Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new List<IUnresolvedAttribute>();
				}
				return this.attributes;
			}
		}

		public IList<ITypeReference> Constraints
		{
			get
			{
				if (this.constraints == null)
				{
					this.constraints = new List<ITypeReference>();
				}
				return this.constraints;
			}
		}

		public VarianceModifier Variance
		{
			get
			{
				return this.variance;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.variance = value;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.region;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.region = value;
			}
		}

		public bool HasDefaultConstructorConstraint
		{
			get
			{
				return this.flags[8];
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.flags[8] = value;
			}
		}

		public bool HasReferenceTypeConstraint
		{
			get
			{
				return this.flags[2];
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.flags[2] = value;
			}
		}

		public bool HasValueTypeConstraint
		{
			get
			{
				return this.flags[4];
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.flags[4] = value;
			}
		}

		/// <summary>
		/// Uses the specified interning provider to intern
		/// strings and lists in this entity.
		/// This method does not test arbitrary objects to see if they implement ISupportsInterning;
		/// instead we assume that those are interned immediately when they are created (before they are added to this entity).
		/// </summary>
		public virtual void ApplyInterningProvider(InterningProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			FreezableHelper.ThrowIfFrozen(this);
			this.name = provider.Intern(this.name);
			this.attributes = provider.InternList<IUnresolvedAttribute>(this.attributes);
			this.constraints = provider.InternList<ITypeReference>(this.constraints);
		}

		public virtual ITypeParameter CreateResolvedTypeParameter(ITypeResolveContext context)
		{
			IEntity entity = null;
			if (this.OwnerType == SymbolKind.Method)
			{
				entity = (context.CurrentMember as IMethod);
			}
			else if (this.OwnerType == SymbolKind.TypeDefinition)
			{
				entity = context.CurrentTypeDefinition;
			}
			if (entity == null)
			{
				throw new InvalidOperationException("Could not determine the type parameter's owner.");
			}
			return new DefaultTypeParameter(entity, this.index, this.name, this.variance, this.Attributes.CreateResolvedAttributes(context), this.Region, this.HasValueTypeConstraint, this.HasReferenceTypeConstraint, this.HasDefaultConstructorConstraint, this.Constraints.Resolve(context));
		}

		private const ushort FlagFrozen = 1;

		private const ushort FlagReferenceTypeConstraint = 2;

		private const ushort FlagValueTypeConstraint = 4;

		private const ushort FlagDefaultConstructorConstraint = 8;

		private readonly int index;

		private IList<IUnresolvedAttribute> attributes;

		private IList<ITypeReference> constraints;

		private string name;

		private DomRegion region;

		private SymbolKind ownerType;

		private VarianceModifier variance;

		private BitVector16 flags;
	}
}

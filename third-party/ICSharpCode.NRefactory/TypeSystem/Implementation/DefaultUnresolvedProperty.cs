using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedProperty" />.
	/// </summary>
	[Serializable]
	public class DefaultUnresolvedProperty : AbstractUnresolvedMember, IUnresolvedProperty, IUnresolvedParameterizedMember, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		protected override void FreezeInternal()
		{
			this.parameters = FreezableHelper.FreezeListAndElements<IUnresolvedParameter>(this.parameters);
			FreezableHelper.Freeze(this.getter);
			FreezableHelper.Freeze(this.setter);
			base.FreezeInternal();
		}

		public override object Clone()
		{
			DefaultUnresolvedProperty defaultUnresolvedProperty = (DefaultUnresolvedProperty)base.Clone();
			if (this.parameters != null)
			{
				defaultUnresolvedProperty.parameters = new List<IUnresolvedParameter>(this.parameters);
			}
			return defaultUnresolvedProperty;
		}

		public override void ApplyInterningProvider(InterningProvider provider)
		{
			base.ApplyInterningProvider(provider);
			this.parameters = provider.InternList<IUnresolvedParameter>(this.parameters);
		}

		public DefaultUnresolvedProperty()
		{
			base.SymbolKind = SymbolKind.Property;
		}

		public DefaultUnresolvedProperty(IUnresolvedTypeDefinition declaringType, string name)
		{
			base.SymbolKind = SymbolKind.Property;
			base.DeclaringTypeDefinition = declaringType;
			base.Name = name;
			if (declaringType != null)
			{
				base.UnresolvedFile = declaringType.UnresolvedFile;
			}
		}

		public bool IsIndexer
		{
			get
			{
				return base.SymbolKind == SymbolKind.Indexer;
			}
		}

		public IList<IUnresolvedParameter> Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					this.parameters = new List<IUnresolvedParameter>();
				}
				return this.parameters;
			}
		}

		public bool CanGet
		{
			get
			{
				return this.getter != null;
			}
		}

		public bool CanSet
		{
			get
			{
				return this.setter != null;
			}
		}

		public IUnresolvedMethod Getter
		{
			get
			{
				return this.getter;
			}
			set
			{
				base.ThrowIfFrozen();
				this.getter = value;
			}
		}

		public IUnresolvedMethod Setter
		{
			get
			{
				return this.setter;
			}
			set
			{
				base.ThrowIfFrozen();
				this.setter = value;
			}
		}

		public override IMember CreateResolved(ITypeResolveContext context)
		{
			return new DefaultResolvedProperty(this, context);
		}

		public override IMember Resolve(ITypeResolveContext context)
		{
			ITypeReference explicitInterfaceTypeReference = null;
			if (base.IsExplicitInterfaceImplementation && base.ExplicitInterfaceImplementations.Count == 1)
			{
				explicitInterfaceTypeReference = base.ExplicitInterfaceImplementations[0].DeclaringTypeReference;
			}
			return AbstractUnresolvedMember.Resolve(AbstractUnresolvedMember.ExtendContextForType(context, base.DeclaringTypeDefinition), base.SymbolKind, base.Name, explicitInterfaceTypeReference, null, (from p in this.Parameters
			select p.Type).ToList<ITypeReference>());
		}

		IProperty IUnresolvedProperty.Resolve(ITypeResolveContext context)
		{
			return (IProperty)this.Resolve(context);
		}

		private IUnresolvedMethod getter;

		private IUnresolvedMethod setter;

		private IList<IUnresolvedParameter> parameters;
	}
}

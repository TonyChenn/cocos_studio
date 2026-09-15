using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedField" />.
	/// </summary>
	[Serializable]
	public class DefaultUnresolvedField : AbstractUnresolvedMember, IUnresolvedField, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		protected override void FreezeInternal()
		{
			FreezableHelper.Freeze(this.constantValue);
			base.FreezeInternal();
		}

		public DefaultUnresolvedField()
		{
			base.SymbolKind = SymbolKind.Field;
		}

		public DefaultUnresolvedField(IUnresolvedTypeDefinition declaringType, string name)
		{
			base.SymbolKind = SymbolKind.Field;
			base.DeclaringTypeDefinition = declaringType;
			base.Name = name;
			if (declaringType != null)
			{
				base.UnresolvedFile = declaringType.UnresolvedFile;
			}
		}

		public bool IsConst
		{
			get
			{
				return this.constantValue != null && !this.IsFixed;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this.flags[4096];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[4096] = value;
			}
		}

		public bool IsVolatile
		{
			get
			{
				return this.flags[8192];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[8192] = value;
			}
		}

		public bool IsFixed
		{
			get
			{
				return this.flags[16384];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[16384] = value;
			}
		}

		public IConstantValue ConstantValue
		{
			get
			{
				return this.constantValue;
			}
			set
			{
				base.ThrowIfFrozen();
				this.constantValue = value;
			}
		}

		public override IMember CreateResolved(ITypeResolveContext context)
		{
			return new DefaultResolvedField(this, context);
		}

		IField IUnresolvedField.Resolve(ITypeResolveContext context)
		{
			return (IField)this.Resolve(context);
		}

		private IConstantValue constantValue;
	}
}

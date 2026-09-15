using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedEvent" />.
	/// </summary>
	[Serializable]
	public class DefaultUnresolvedEvent : AbstractUnresolvedMember, IUnresolvedEvent, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			FreezableHelper.Freeze(this.addAccessor);
			FreezableHelper.Freeze(this.removeAccessor);
			FreezableHelper.Freeze(this.invokeAccessor);
		}

		public DefaultUnresolvedEvent()
		{
			base.SymbolKind = SymbolKind.Event;
		}

		public DefaultUnresolvedEvent(IUnresolvedTypeDefinition declaringType, string name)
		{
			base.SymbolKind = SymbolKind.Event;
			base.DeclaringTypeDefinition = declaringType;
			base.Name = name;
			if (declaringType != null)
			{
				base.UnresolvedFile = declaringType.UnresolvedFile;
			}
		}

		public bool CanAdd
		{
			get
			{
				return this.addAccessor != null;
			}
		}

		public bool CanRemove
		{
			get
			{
				return this.removeAccessor != null;
			}
		}

		public bool CanInvoke
		{
			get
			{
				return this.invokeAccessor != null;
			}
		}

		public IUnresolvedMethod AddAccessor
		{
			get
			{
				return this.addAccessor;
			}
			set
			{
				base.ThrowIfFrozen();
				this.addAccessor = value;
			}
		}

		public IUnresolvedMethod RemoveAccessor
		{
			get
			{
				return this.removeAccessor;
			}
			set
			{
				base.ThrowIfFrozen();
				this.removeAccessor = value;
			}
		}

		public IUnresolvedMethod InvokeAccessor
		{
			get
			{
				return this.invokeAccessor;
			}
			set
			{
				base.ThrowIfFrozen();
				this.invokeAccessor = value;
			}
		}

		public override IMember CreateResolved(ITypeResolveContext context)
		{
			return new DefaultResolvedEvent(this, context);
		}

		IEvent IUnresolvedEvent.Resolve(ITypeResolveContext context)
		{
			return (IEvent)this.Resolve(context);
		}

		private IUnresolvedMethod addAccessor;

		private IUnresolvedMethod removeAccessor;

		private IUnresolvedMethod invokeAccessor;
	}
}

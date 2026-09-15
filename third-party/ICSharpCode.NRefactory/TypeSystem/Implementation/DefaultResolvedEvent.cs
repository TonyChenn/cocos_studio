using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public class DefaultResolvedEvent : AbstractResolvedMember, IEvent, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		public DefaultResolvedEvent(IUnresolvedEvent unresolved, ITypeResolveContext parentContext) : base(unresolved, parentContext)
		{
			this.unresolved = unresolved;
		}

		public bool CanAdd
		{
			get
			{
				return this.unresolved.CanAdd;
			}
		}

		public bool CanRemove
		{
			get
			{
				return this.unresolved.CanRemove;
			}
		}

		public bool CanInvoke
		{
			get
			{
				return this.unresolved.CanInvoke;
			}
		}

		public IMethod AddAccessor
		{
			get
			{
				return base.GetAccessor(ref this.addAccessor, this.unresolved.AddAccessor);
			}
		}

		public IMethod RemoveAccessor
		{
			get
			{
				return base.GetAccessor(ref this.removeAccessor, this.unresolved.RemoveAccessor);
			}
		}

		public IMethod InvokeAccessor
		{
			get
			{
				return base.GetAccessor(ref this.invokeAccessor, this.unresolved.InvokeAccessor);
			}
		}

		public override IMember Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedEvent(this, substitution);
		}

		protected new readonly IUnresolvedEvent unresolved;

		private IMethod addAccessor;

		private IMethod removeAccessor;

		private IMethod invokeAccessor;
	}
}

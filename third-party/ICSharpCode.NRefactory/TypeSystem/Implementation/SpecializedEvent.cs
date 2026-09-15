using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a specialized IEvent (event after type substitution).
	/// </summary>
	public class SpecializedEvent : SpecializedMember, IEvent, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		public SpecializedEvent(IEvent eventDefinition, TypeParameterSubstitution substitution) : base(eventDefinition)
		{
			this.eventDefinition = eventDefinition;
			base.AddSubstitution(substitution);
		}

		public bool CanAdd
		{
			get
			{
				return this.eventDefinition.CanAdd;
			}
		}

		public bool CanRemove
		{
			get
			{
				return this.eventDefinition.CanRemove;
			}
		}

		public bool CanInvoke
		{
			get
			{
				return this.eventDefinition.CanInvoke;
			}
		}

		public IMethod AddAccessor
		{
			get
			{
				return base.WrapAccessor(ref this.addAccessor, this.eventDefinition.AddAccessor);
			}
		}

		public IMethod RemoveAccessor
		{
			get
			{
				return base.WrapAccessor(ref this.removeAccessor, this.eventDefinition.RemoveAccessor);
			}
		}

		public IMethod InvokeAccessor
		{
			get
			{
				return base.WrapAccessor(ref this.invokeAccessor, this.eventDefinition.InvokeAccessor);
			}
		}

		private readonly IEvent eventDefinition;

		private IMethod addAccessor;

		private IMethod removeAccessor;

		private IMethod invokeAccessor;
	}
}

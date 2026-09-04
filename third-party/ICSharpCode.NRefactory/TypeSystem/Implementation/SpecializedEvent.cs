using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a specialized IEvent (event after type substitution).
	/// </summary>
	// Token: 0x020000DE RID: 222
	public class SpecializedEvent : SpecializedMember, IEvent, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x0600084E RID: 2126 RVA: 0x00015D61 File Offset: 0x00014D61
		public SpecializedEvent(IEvent eventDefinition, TypeParameterSubstitution substitution) : base(eventDefinition)
		{
			this.eventDefinition = eventDefinition;
			base.AddSubstitution(substitution);
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x00015D78 File Offset: 0x00014D78
		public bool CanAdd
		{
			get
			{
				return this.eventDefinition.CanAdd;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x00015D85 File Offset: 0x00014D85
		public bool CanRemove
		{
			get
			{
				return this.eventDefinition.CanRemove;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00015D92 File Offset: 0x00014D92
		public bool CanInvoke
		{
			get
			{
				return this.eventDefinition.CanInvoke;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x00015D9F File Offset: 0x00014D9F
		public IMethod AddAccessor
		{
			get
			{
				return base.WrapAccessor(ref this.addAccessor, this.eventDefinition.AddAccessor);
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00015DB8 File Offset: 0x00014DB8
		public IMethod RemoveAccessor
		{
			get
			{
				return base.WrapAccessor(ref this.removeAccessor, this.eventDefinition.RemoveAccessor);
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x00015DD1 File Offset: 0x00014DD1
		public IMethod InvokeAccessor
		{
			get
			{
				return base.WrapAccessor(ref this.invokeAccessor, this.eventDefinition.InvokeAccessor);
			}
		}

		// Token: 0x0400025F RID: 607
		private readonly IEvent eventDefinition;

		// Token: 0x04000260 RID: 608
		private IMethod addAccessor;

		// Token: 0x04000261 RID: 609
		private IMethod removeAccessor;

		// Token: 0x04000262 RID: 610
		private IMethod invokeAccessor;
	}
}

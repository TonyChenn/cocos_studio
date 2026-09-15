using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public interface IEvent : IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		bool CanAdd { get; }

		bool CanRemove { get; }

		bool CanInvoke { get; }

		IMethod AddAccessor { get; }

		IMethod RemoveAccessor { get; }

		IMethod InvokeAccessor { get; }
	}
}

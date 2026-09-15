using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public interface IUnresolvedMethod : IUnresolvedParameterizedMember, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		/// <summary>
		/// Gets the attributes associated with the return type. (e.g. [return: MarshalAs(...)])
		/// </summary>
		IList<IUnresolvedAttribute> ReturnTypeAttributes { get; }

		IList<IUnresolvedTypeParameter> TypeParameters { get; }

		bool IsConstructor { get; }

		bool IsDestructor { get; }

		bool IsOperator { get; }

		/// <summary>
		/// Gets whether the method is a C#-style partial method.
		/// Check <see cref="P:ICSharpCode.NRefactory.TypeSystem.IUnresolvedMethod.HasBody" /> to test if it is a partial method declaration or implementation.
		/// </summary>
		bool IsPartial { get; }

		/// <summary>
		/// Gets whether the method is a C#-style async method.
		/// </summary>
		bool IsAsync { get; }

		[Obsolete("Use IsPartial && !HasBody instead")]
		bool IsPartialMethodDeclaration { get; }

		[Obsolete("Use IsPartial && HasBody instead")]
		bool IsPartialMethodImplementation { get; }

		/// <summary>
		/// Gets whether the method has a body.
		/// This property returns <c>false</c> for <c>abstract</c> or <c>extern</c> methods,
		/// or for <c>partial</c> methods without implementation.
		/// </summary>
		bool HasBody { get; }

		/// <summary>
		/// If this method is an accessor, returns a reference to the corresponding property/event.
		/// Otherwise, returns null.
		/// </summary>
		IUnresolvedMember AccessorOwner { get; }

		/// <summary>
		/// Resolves the member.
		/// </summary>
		/// <param name="context">
		/// Context for looking up the member. The context must specify the current assembly.
		/// A <see cref="T:ICSharpCode.NRefactory.TypeSystem.SimpleTypeResolveContext" /> that specifies the current assembly is sufficient.
		/// </param>
		/// <returns>
		/// Returns the resolved member, or <c>null</c> if the member could not be found.
		/// </returns>
		IMethod Resolve(ITypeResolveContext context);
	}
}

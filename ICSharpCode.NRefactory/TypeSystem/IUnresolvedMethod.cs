using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200009D RID: 157
	public interface IUnresolvedMethod : IUnresolvedParameterizedMember, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		/// <summary>
		/// Gets the attributes associated with the return type. (e.g. [return: MarshalAs(...)])
		/// </summary>
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060004E2 RID: 1250
		IList<IUnresolvedAttribute> ReturnTypeAttributes { get; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060004E3 RID: 1251
		IList<IUnresolvedTypeParameter> TypeParameters { get; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060004E4 RID: 1252
		bool IsConstructor { get; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060004E5 RID: 1253
		bool IsDestructor { get; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060004E6 RID: 1254
		bool IsOperator { get; }

		/// <summary>
		/// Gets whether the method is a C#-style partial method.
		/// Check <see cref="P:ICSharpCode.NRefactory.TypeSystem.IUnresolvedMethod.HasBody" /> to test if it is a partial method declaration or implementation.
		/// </summary>
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060004E7 RID: 1255
		bool IsPartial { get; }

		/// <summary>
		/// Gets whether the method is a C#-style async method.
		/// </summary>
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060004E8 RID: 1256
		bool IsAsync { get; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060004E9 RID: 1257
		[Obsolete("Use IsPartial && !HasBody instead")]
		bool IsPartialMethodDeclaration { get; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060004EA RID: 1258
		[Obsolete("Use IsPartial && HasBody instead")]
		bool IsPartialMethodImplementation { get; }

		/// <summary>
		/// Gets whether the method has a body.
		/// This property returns <c>false</c> for <c>abstract</c> or <c>extern</c> methods,
		/// or for <c>partial</c> methods without implementation.
		/// </summary>
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060004EB RID: 1259
		bool HasBody { get; }

		/// <summary>
		/// If this method is an accessor, returns a reference to the corresponding property/event.
		/// Otherwise, returns null.
		/// </summary>
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060004EC RID: 1260
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
		// Token: 0x060004ED RID: 1261
		IMethod Resolve(ITypeResolveContext context);
	}
}

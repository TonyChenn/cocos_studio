using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents a reference to a type.
	/// Must be resolved before it can be used as type.
	/// </summary>
	// Token: 0x02000008 RID: 8
	public interface ITypeReference
	{
		/// <summary>
		/// Resolves this type reference.
		/// </summary>
		/// <param name="context">
		/// Context to use for resolving this type reference.
		/// Which kind of context is required depends on the which kind of type reference this is;
		/// please consult the documentation of the method that was used to create this type reference,
		/// or that of the class implementing this method.
		/// </param>
		/// <returns>
		/// Returns the resolved type.
		/// In case of an error, returns an unknown type (<see cref="F:ICSharpCode.NRefactory.TypeSystem.TypeKind.Unknown" />).
		/// Never returns null.
		/// </returns>
		// Token: 0x06000020 RID: 32
		IType Resolve(ITypeResolveContext context);
	}
}

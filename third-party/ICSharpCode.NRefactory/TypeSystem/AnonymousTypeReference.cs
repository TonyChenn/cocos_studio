using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Anonymous type reference.
	/// </summary>
	[Serializable]
	public class AnonymousTypeReference : ITypeReference
	{
		public AnonymousTypeReference(IUnresolvedProperty[] properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			this.unresolvedProperties = properties;
		}

		public IType Resolve(ITypeResolveContext context)
		{
			return new AnonymousType(context.Compilation, this.unresolvedProperties);
		}

		private readonly IUnresolvedProperty[] unresolvedProperties;
	}
}

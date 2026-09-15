using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public sealed class OwnedTypeParameterReference : ISymbolReference
	{
		public OwnedTypeParameterReference(ISymbolReference owner, int index)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.owner = owner;
			this.index = index;
		}

		public ISymbol Resolve(ITypeResolveContext context)
		{
			IEntity entity = this.owner.Resolve(context) as IEntity;
			if (entity is ITypeDefinition)
			{
				return ((ITypeDefinition)entity).TypeParameters[this.index];
			}
			if (entity is IMethod)
			{
				return ((IMethod)entity).TypeParameters[this.index];
			}
			return null;
		}

		private ISymbolReference owner;

		private int index;
	}
}

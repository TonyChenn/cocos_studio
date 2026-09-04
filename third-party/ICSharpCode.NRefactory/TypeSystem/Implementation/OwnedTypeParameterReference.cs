using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000A2 RID: 162
	public sealed class OwnedTypeParameterReference : ISymbolReference
	{
		// Token: 0x06000538 RID: 1336 RVA: 0x0000C721 File Offset: 0x0000B721
		public OwnedTypeParameterReference(ISymbolReference owner, int index)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.owner = owner;
			this.index = index;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000C748 File Offset: 0x0000B748
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

		// Token: 0x0400015C RID: 348
		private ISymbolReference owner;

		// Token: 0x0400015D RID: 349
		private int index;
	}
}

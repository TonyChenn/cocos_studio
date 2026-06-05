using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Anonymous type reference.
	/// </summary>
	// Token: 0x02000068 RID: 104
	[Serializable]
	public class AnonymousTypeReference : ITypeReference
	{
		// Token: 0x0600035F RID: 863 RVA: 0x000084A0 File Offset: 0x000074A0
		public AnonymousTypeReference(IUnresolvedProperty[] properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			this.unresolvedProperties = properties;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000084BD File Offset: 0x000074BD
		public IType Resolve(ITypeResolveContext context)
		{
			return new AnonymousType(context.Compilation, this.unresolvedProperties);
		}

		// Token: 0x040000D7 RID: 215
		private readonly IUnresolvedProperty[] unresolvedProperties;
	}
}

using System;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Cache for KnownTypeReferences.
	/// </summary>
	// Token: 0x020000D4 RID: 212
	internal sealed class KnownTypeCache
	{
		// Token: 0x060007D6 RID: 2006 RVA: 0x00014BCB File Offset: 0x00013BCB
		public KnownTypeCache(ICompilation compilation)
		{
			this.compilation = compilation;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00014BE8 File Offset: 0x00013BE8
		public IType FindType(KnownTypeCode typeCode)
		{
			IType type = LazyInit.VolatileRead<IType>(ref this.knownTypes[(int)typeCode]);
			if (type != null)
			{
				return type;
			}
			return LazyInit.GetOrSet<IType>(ref this.knownTypes[(int)typeCode], this.SearchType(typeCode));
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00014C24 File Offset: 0x00013C24
		private IType SearchType(KnownTypeCode typeCode)
		{
			KnownTypeReference knownTypeReference = KnownTypeReference.Get(typeCode);
			if (knownTypeReference == null)
			{
				return SpecialType.UnknownType;
			}
			TopLevelTypeName topLevelTypeName = new TopLevelTypeName(knownTypeReference.Namespace, knownTypeReference.Name, knownTypeReference.TypeParameterCount);
			foreach (IAssembly assembly in this.compilation.Assemblies)
			{
				ITypeDefinition typeDefinition = assembly.GetTypeDefinition(topLevelTypeName);
				if (typeDefinition != null)
				{
					return typeDefinition;
				}
			}
			return new UnknownType(topLevelTypeName);
		}

		// Token: 0x0400023C RID: 572
		private readonly ICompilation compilation;

		// Token: 0x0400023D RID: 573
		private readonly IType[] knownTypes = new IType[46];
	}
}

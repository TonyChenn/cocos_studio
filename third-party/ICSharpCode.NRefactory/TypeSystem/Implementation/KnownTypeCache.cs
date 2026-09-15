using System;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Cache for KnownTypeReferences.
	/// </summary>
	internal sealed class KnownTypeCache
	{
		public KnownTypeCache(ICompilation compilation)
		{
			this.compilation = compilation;
		}

		public IType FindType(KnownTypeCode typeCode)
		{
			IType type = LazyInit.VolatileRead<IType>(ref this.knownTypes[(int)typeCode]);
			if (type != null)
			{
				return type;
			}
			return LazyInit.GetOrSet<IType>(ref this.knownTypes[(int)typeCode], this.SearchType(typeCode));
		}

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

		private readonly ICompilation compilation;

		private readonly IType[] knownTypes = new IType[46];
	}
}

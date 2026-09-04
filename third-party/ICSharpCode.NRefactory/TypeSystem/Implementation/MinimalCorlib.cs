using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Resolve context represents the minimal mscorlib required for evaluating constants.
	/// This contains all known types (<see cref="T:ICSharpCode.NRefactory.TypeSystem.KnownTypeCode" />) and no other types.
	/// </summary>
	// Token: 0x020000D6 RID: 214
	public sealed class MinimalCorlib : DefaultUnresolvedAssembly
	{
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00014FC2 File Offset: 0x00013FC2
		public static MinimalCorlib Instance
		{
			get
			{
				return MinimalCorlib.instance.Value;
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00014FCE File Offset: 0x00013FCE
		public ICompilation CreateCompilation()
		{
			return new SimpleCompilation(new DefaultSolutionSnapshot(), this, new IAssemblyReference[0]);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00014FE4 File Offset: 0x00013FE4
		private MinimalCorlib() : base("corlib")
		{
			DefaultUnresolvedTypeDefinition[] array = new DefaultUnresolvedTypeDefinition[46];
			for (int i = 0; i < array.Length; i++)
			{
				KnownTypeReference knownTypeReference = KnownTypeReference.Get((KnownTypeCode)i);
				if (knownTypeReference != null)
				{
					array[i] = new DefaultUnresolvedTypeDefinition(knownTypeReference.Namespace, knownTypeReference.Name);
					for (int j = 0; j < knownTypeReference.TypeParameterCount; j++)
					{
						array[i].TypeParameters.Add(new DefaultUnresolvedTypeParameter(SymbolKind.TypeDefinition, j, null));
					}
					base.AddTypeDefinition(array[i]);
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				KnownTypeReference knownTypeReference2 = KnownTypeReference.Get((KnownTypeCode)k);
				if (knownTypeReference2 != null && knownTypeReference2.baseType != KnownTypeCode.None)
				{
					array[k].BaseTypes.Add(array[(int)knownTypeReference2.baseType]);
					if (knownTypeReference2.baseType == KnownTypeCode.ValueType && k != 24)
					{
						array[k].Kind = TypeKind.Struct;
					}
				}
			}
			base.Freeze();
		}

		// Token: 0x04000247 RID: 583
		private static readonly Lazy<MinimalCorlib> instance = new Lazy<MinimalCorlib>(() => new MinimalCorlib());
	}
}

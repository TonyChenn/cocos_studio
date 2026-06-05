using System;
using System.Globalization;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000E5 RID: 229
	[Serializable]
	public sealed class TypeParameterReference : ITypeReference, ISymbolReference
	{
		/// <summary>
		/// Creates a type parameter reference.
		/// For common type parameter references, this method may return a shared instance.
		/// </summary>
		// Token: 0x0600088B RID: 2187 RVA: 0x000166A0 File Offset: 0x000156A0
		public static TypeParameterReference Create(SymbolKind ownerType, int index)
		{
			if (index >= 0 && index < 8 && (ownerType == SymbolKind.TypeDefinition || ownerType == SymbolKind.Method))
			{
				TypeParameterReference[] array = (ownerType == SymbolKind.TypeDefinition) ? TypeParameterReference.classTypeParameterReferences : TypeParameterReference.methodTypeParameterReferences;
				TypeParameterReference typeParameterReference = LazyInit.VolatileRead<TypeParameterReference>(ref array[index]);
				if (typeParameterReference == null)
				{
					typeParameterReference = LazyInit.GetOrSet<TypeParameterReference>(ref array[index], new TypeParameterReference(ownerType, index));
				}
				return typeParameterReference;
			}
			return new TypeParameterReference(ownerType, index);
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x000166FB File Offset: 0x000156FB
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00016703 File Offset: 0x00015703
		public TypeParameterReference(SymbolKind ownerType, int index)
		{
			this.ownerType = ownerType;
			this.index = index;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001671C File Offset: 0x0001571C
		public IType Resolve(ITypeResolveContext context)
		{
			if (this.ownerType == SymbolKind.Method)
			{
				IMethod method = context.CurrentMember as IMethod;
				if (method != null && this.index < method.TypeParameters.Count)
				{
					return method.TypeParameters[this.index];
				}
				return DummyTypeParameter.GetMethodTypeParameter(this.index);
			}
			else
			{
				if (this.ownerType != SymbolKind.TypeDefinition)
				{
					return SpecialType.UnknownType;
				}
				ITypeDefinition currentTypeDefinition = context.CurrentTypeDefinition;
				if (currentTypeDefinition != null && this.index < currentTypeDefinition.TypeParameters.Count)
				{
					return currentTypeDefinition.TypeParameters[this.index];
				}
				return DummyTypeParameter.GetClassTypeParameter(this.index);
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x000167BB File Offset: 0x000157BB
		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return this.Resolve(context) as ISymbol;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x000167CC File Offset: 0x000157CC
		public override string ToString()
		{
			if (this.ownerType == SymbolKind.Method)
			{
				return "!!" + this.index.ToString(CultureInfo.InvariantCulture);
			}
			return "!" + this.index.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x04000272 RID: 626
		private static readonly TypeParameterReference[] classTypeParameterReferences = new TypeParameterReference[8];

		// Token: 0x04000273 RID: 627
		private static readonly TypeParameterReference[] methodTypeParameterReferences = new TypeParameterReference[8];

		// Token: 0x04000274 RID: 628
		private readonly SymbolKind ownerType;

		// Token: 0x04000275 RID: 629
		private readonly int index;
	}
}
